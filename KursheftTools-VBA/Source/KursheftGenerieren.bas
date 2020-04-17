Attribute VB_Name = "KursheftGenerieren"
Option Explicit


'------Global Variables---------------------------
'''<summary>The path of the course list</summary>
Public courseListPath As String
'''<summary>The path where the pdfs will be stored</summary>
Public storedPath As String
'''<summary>An array contains the grades for exporting As String</summary>
Public grades As Variant
'''<summary>The dialog result for "FormForGeneratingCoursebook</summary>
Public dialogResult As Boolean
'''<summary>The path where the logo is stored</summary>
Public Const LogoPath As String = "F:\repos\LehrplanTools\LehrplanTools\img\LOGO.png"




'------Main Function------------------------------

Public Sub KursheftGenerieren()

Dim cache As Variant
Dim index As Integer

'Test if there is any workbook already opened
If (Workbooks.Count = 0) Then
    Call MsgBox("Kein Workbook is zurzeit geoeffnet. Gehen Sie auf der Bemerkungsbogen und versuchen Sie nochmal.", vbOKOnly, "Error")
    Exit Sub
End If

'An array contains the start of the year, the start of the second period and the end of the year
Dim datePeriods(2) As Date

Dim dtsStr As String
'Test if on a valid note board
Dim noteBoard As Excel.Worksheet

Set noteBoard = Excel.ActiveSheet

dtsStr = noteBoard.Range("I1").value
'If the String is not long enouth, then not valid
If Not (Len(dtsStr) = 32) Then
    Call MsgBox("Die Bemerkungsbogen ist beschaedigt oder existiert nicht. Gehen Sie auf dem Sheet und versuchen Sie nochmal", vbOKOnly, "Error")
    Exit Sub
End If

'If the string is not right(have less or more than 2 "~"
If Not (GetArrayLength(Split(dtsStr, "~")) = 3) Then
    Call MsgBox("Die Bemerkungsbogen ist beschaedigt oder existiert nicht. Gehen Sie auf dem Sheet und versuchen Sie nochmal", vbOKOnly, "Error")
    Exit Sub
End If


'spilt the string
Dim splitedStringDate
splitedStringDate = Split(dtsStr, "~")

'Test the validity of each date
index = 0
For Each cache In splitedStringDate
    If Not dateIsValid(CStr(cache)) Then
        Call MsgBox("Die Bemerkungsbogen ist beschaedigt oder existiert nicht. Gehen Sie auf dem Sheet und versuchen Sie nochmal", vbOKOnly, "Error")
        Exit Sub
    Else
        datePeriods(index) = convertStringToDate(CStr(cache))
    End If
    index = index + 1
Next cache
index = 0

'Show the form allowing the user to input the paths and the grades that need to be exported
FormForGeneratingCoursebook.Show

'If got the right values from the form
If dialogResult Then
    Application.StatusBar = "Export beginnt"
    
    'Set the name for the csv sheet
    Dim csvSheetName As String
    'Set the name with the current system time
    'sothat it won't be the same name as the possible previous imported sheet
    csvSheetName = "courseListCSV" & format(Now, "MMddhhnnss")
    
    'Load the course list (.csv) into excel and set the sheet to not visible
    'If success, then
    If ImportCourseList(courseListPath, csvSheetName) Then
        
        'Store how many courses are exported
        Dim exportCounter As Integer
        exportCounter = 0
        
        'Store the imported data sheet as a worksheet object into "courseListS"
        Dim courseListS As Excel.Worksheet
        Set courseListS = Worksheets(csvSheetName)
        
        'The rows of the course list
        Dim rows As Long
        rows = courseListS.UsedRange.rows.Count
        
        'The rows of the note board
        Dim NoteboardRows As Long
        NoteboardRows = noteBoard.UsedRange.rows.Count
        
        'The course that is handled now
        Dim currentCourseNum As String
        currentCourseNum = -1
        'The class that is handled now since the course number could be the same
        Dim currentCourseClass As String
        currentCourseClass = ""
        
        Dim currentCourseName As String
        currentCourseName = ""
        
        'Initialize the plan for the courseplan
        Dim currentPlan As CoursePlan
        
        'The array contains whether the course is regular on this day or not
        'The index of this array should be the same order as dts()
        Dim isRegular() As String
        
        'The array contains the dates of the first of the week
        Dim dts() As Date
        
        'Initialize the variable that stores the sorted dts() array
        Dim sortedDts
        
        'Initizlize the template sheet for exporting the plans
        Dim exportingSheet As Excel.Worksheet
        Set exportingSheet = PresetExportSheet(datePeriods, LogoPath)
        
        Application.ScreenUpdating = False
        
        'For loop variables
        Dim i As Long, k As Integer, n As Byte, gradeL As Variant
        
        For i = 2 To rows Step 1
            'Get the course number
            For Each gradeL In grades
                'If the grade is Q1 or Q2, or the grade between 05-09
                If gradeL = courseListS.Cells(i, 2) Or gradeL = Left(courseListS.Cells(i, 2), 2) Then
                    If (courseListS.Cells(i, 1) = "" Or courseListS.Cells(i, 2) = "") Then
                        'jump the line without a course number or without a class
                        Debug.Print "course list csv empty course number: line: " & CStr(i)
                    ElseIf (currentCourseNum = courseListS.Cells(i, 1) And (currentCourseClass = courseListS.Cells(i, 2)) And (currentCourseName <> courseListS.Cells(i + 1, 2))) Then
                        'If the current course number and so as the same class is also for this line
                        
                        'Then If the weekday is not added, add it to the array
                        If Not ((dts(GetArrayLength(dts) - 1)) = getNearestDateForWeekday(courseListS.Cells(i, 6), datePeriods(0))) Then
                            ReDim Preserve dts(GetArrayLength(dts))
                            dts(GetArrayLength(dts) - 1) = getNearestDateForWeekday(courseListS.Cells(i, 6), datePeriods(0))
                            ReDim Preserve isRegular(GetArrayLength(isRegular))
                            isRegular(GetArrayLength(isRegular) - 1) = courseListS.Cells(i, 8).value
                        ElseIf (courseListS.Cells(i, 8).value <> isRegular(GetArrayLength(isRegular) - 1)) And (isRegular(GetArrayLength(isRegular) - 1) <> "") Then
                            isRegular(GetArrayLength(isRegular) - 1) = courseListS.Cells(i, 8).value
                        End If
                    Else
                        'Handle the new course number now
                        currentCourseNum = courseListS.Cells(i, 1).value
                        currentCourseClass = courseListS.Cells(i, 2).value
                        currentCourseName = courseListS.Cells(i, 4).value
                
                        ReDim dts(0)
                        dts(0) = getNearestDateForWeekday(courseListS.Cells(i, 6), datePeriods(0))
                        ReDim isRegular(0)
                        isRegular(0) = courseListS.Cells(i, 8).value
                    End If
        
        
        
                    'If all the lines for this course number with the class is processed, summary them up and export as pdf
                    If (currentCourseNum <> courseListS.Cells(i + 1, 1) Or (currentCourseClass <> courseListS.Cells(i + 1, 2)) Or (currentCourseName <> courseListS.Cells(i + 1, 2))) Then
                        Set currentPlan = New CoursePlan
                        Call currentPlan.Initialize(currentCourseName, currentCourseClass, courseListS.Cells(i, 3))
                
                        'sort the dts() array
                        Dim cac
                        cac = SortDates(dts, Array(isRegular))
                        sortedDts = cac(0)
                        isRegular = cac(1)
                
                        Dim j As Long
                        Dim lineDate As String
                        
                        
                        k = 0
                        For j = 3 To NoteboardRows Step 1
                        
                            lineDate = CStr(noteBoard.Cells(j, 2).value)
                        
                            'If on even weeks but the current date is odd
                            If isRegular(k) = "g" And Not isEvenWeek(CDate(sortedDts(k))) Then
                            'don't process this date
                            'If on odd weeks but the current date is even
                            ElseIf isRegular(k) = "u" And isEvenWeek(CDate(sortedDts(k))) Then
                                'don't process this date
                            'Else when it is on the right week
                            Else
                                'Add 14 days for the dates because of the holiday
                                If lineDate = "Anfang d. 2. Abschnitts" Then
                                    Dim c As Byte
                                    For c = 0 To GetArrayLength(sortedDts) - 1 Step 1
                                        sortedDts(c) = DateAdd("ww", 2, sortedDts(c))
                                    Next c
                                ElseIf lineDate = "Ende des Schulhalbjahres" Or lineDate = "" Then
                                    'Jump the title lines
                                Else
                                    'if the date are the same
                                    If CDate(sortedDts(k)) = DateValue(lineDate) Then
                                        'Initialize the current daynote
                                        Dim currentDayNt As Daynote
                                        Set currentDayNt = New Daynote
                                        currentDayNt.Initialize (sortedDts(k))
                                        
                                        'Get the notes
                                        For n = 3 To 7 Step 2
                                            Dim currentNote As String
                                            currentNote = noteBoard.Cells(j, n).value
                                            'If in this cell there is note
                                            If currentNote <> "" Then
                                                Dim currentGrade As String
                                                currentGrade = noteBoard.Cells(j, n + 1).value
                                                
                                                'If the grade fits to the current course
                                                If (currentGrade = currentCourseClass) Or (currentGrade = Left(currentCourseClass, 2)) Or (currentGrade = "") Then
                                                    'If the class or grade is the same as the current class, then add the note to the daynote
                                                    Call currentDayNt.AddNote(currentNote)
                                                End If
                                            End If
                                        Next n
                                
                                        'Add the plan for this day to the course plan
                                        Call currentPlan.AddLine(currentDayNt)
                                    End If
                                End If
                            End If
                            
                            'If one week passed, the add one week to the sortedDts()
                            If (Not (lineDate = "Anfang d. 2. Abschnitts" Or lineDate = "Ende des Schulhalbjahres" Or lineDate = "")) Then
                                If (CDate(sortedDts(k)) = DateValue(lineDate)) Then
                                    'No matter whether the date is processed or not
                                    'We have to add 7 days to it
                                    'Add 7 days to the processed date
                                    sortedDts(k) = DateAdd("ww", 1, sortedDts(k))
                                    'So as also need to move the counter
                                    'Add the counter or change it to 0
                                    'This counter is to let the dates move further
                                    If (k < (GetArrayLength(sortedDts) - 1)) Then
                                        k = k + 1
                                    Else
                                    k = 0
                                    End If
                            End If
                        End If
                        Next j
                    
                    
                        'When the whole noteBoard is processed
                        'Export the plan as pdf
                        Dim tit As String
                        tit = currentPlan.GCourseName & "-" & currentPlan.GTeacher & "-" & currentPlan.GClassName & ".pdf"
                    
                        'If the pdf is not exported successfully, print the debug info
                        If Not ExportPDF(currentPlan, storedPath & "\" & tit, exportingSheet) Then
                            Debug.Print "The pdf export is not successful"
                            Debug.Print currentPlan.GCourseName & " " & currentPlan.GClassName & " " & currentCourseNum
                        Else
                            exportCounter = exportCounter + 1
                        End If
                    
                    End If
                    
                Exit For
                End If
                
            Next gradeL
        Next i
        
        
    
    Else
        Debug.Print "The csv file is not imported properly"
        Application.StatusBar = False
        MsgBox "An unexpected error is occured at KursheftGenerieren:KursheftGenerieren The csv file is not imported properly", vbOKOnly, "Fatal Error"
        Exit Sub
    End If
    
    'Disable the alert
    Application.DisplayAlerts = False
    'Delete the exported sheet
    exportingSheet.Delete
    Application.DisplayAlerts = True
    Application.ScreenUpdating = True

    'Show a message box telling the user that the files are exported successfully
    Application.StatusBar = "Export fertig"
    Call MsgBox(CStr(exportCounter) & " PDF-Datei sind erfolgreich exportiert unter: " & Chr(10) & Chr(13) & storedPath, vbOKOnly, "Erfolg")
    
    'Delete the query
    ActiveWorkbook.Queries(csvSheetName).Delete
    'Delete the imported csv sheet
    'The delete of the sheet causes an error 1004
    'No sulution found
   ' ActiveWorkbook.Sheets(csvSheetName).Delete
    End If
    
Application.StatusBar = False

End Sub


'------Private Functions--------------------------

'''<summary>Import the course list as csv from the given path to the excel as a non-visible worksheet and a query</summary>
'''<param name="path">The valid path of the course list</param>
'''<param name=csvName">Import the csv into the excel with the name csvName
'''             The string should not contain any space or sepcial characters</param>
'''<return>A boolean value represents whether this function is processed successfully or not</return>
Private Function ImportCourseList(ByVal path As String, ByVal csvNameP As String) As Boolean

On Error GoTo ErrorHandle


ActiveWorkbook.Queries.Add name:=csvNameP, Formula:= _
    "let" & Chr(13) & "" & Chr(10) & "    Quelle = Csv.Document(File.Contents(" & """" & path & """" & "),[Delimiter="";"", Columns=9, Encoding=1252, QuoteStyle=QuoteStyle.None])," & Chr(13) & "" & Chr(10) & "    #""Geaenderter Typ"" = Table.TransformColumnTypes(Quelle,{{""Column1"", type text}, {""Column2"", type text}, {""Column3"", type text}, {""Column4"", type text}, {""Column5"", type text}, " & _
    "{""Column6"", Int64.Type}, {""Column7"", Int64.Type}, {""Column8"", type text}, {""Column9"", type text}})" & Chr(13) & "" & Chr(10) & "in" & Chr(13) & "" & Chr(10) & "    #""Geaenderter Typ"""
'Create a new sheet using the name with the variable csvNameP
ActiveWorkbook.Worksheets.Add.name = csvNameP
With ActiveSheet.ListObjects.Add(SourceType:=0, Source:= _
    "OLEDB;Provider=Microsoft.Mashup.OleDb.1;Data Source=$Workbook$;Location=" & csvNameP & ";Extended Properties=""""" _
    , Destination:=Range("$A$1")).QueryTable
    .CommandType = xlCmdSql
    .CommandText = Array("SELECT * FROM [" & csvNameP & "]")
    .RowNumbers = False
    .FillAdjacentFormulas = False
    .PreserveFormatting = True
    .RefreshOnFileOpen = False
    .BackgroundQuery = True
    .RefreshStyle = xlInsertDeleteCells
    .SaveData = True
    .AdjustColumnWidth = True
    .RefreshPeriod = 0
    .PreserveColumnInfo = True
    .ListObject.DisplayName = csvNameP
    .Refresh BackgroundQuery:=False
End With

Worksheets(csvNameP).Visible = Excel.xlSheetVeryHidden

ImportCourseList = True

CleanUp:

Err.Clear
Exit Function


ErrorHandle:

'Name exists
If Err.Number = 1004 Then
    MsgBox "Err 1004: Unexpected Error At ProgressWindow.ImportCourseList", vbOKOnly, "Fatal Error"
    Debug.Print Err.Number & Err.Source & Err.Description
    End

Else
    MsgBox "General Error: " & Err.Number & " : Unexpected Error At KursheftGenerieren.ImportCourseList", vbOKOnly, "Fatal Error"
    Debug.Print Err.Number & Err.Source & Err.Description
End If

ImportCourseList = False
Resume CleanUp


End Function

'''<summary>Export the given course plan in the preset form as pdf file storing in the given path</summary>
'''<param name="PcurrentPlan">The course plan object represents the course that needs to be exported</param>
'''<param name="Ppath">The path where the pdf data will be stored</param.
'''<param name="PpresetSheet">The preset sheet generated before</param>
'''<return>A boolean value represents whether the course is exported successfully or not</return>
Private Function ExportPDF(PcurrentPlan As CoursePlan, Ppath As String, PpresetSheet As Excel.Worksheet) As Boolean

Dim GREY1
GREY1 = RGB(245, 245, 245)
Dim GREY2
GREY2 = RGB(230, 230, 230)

Dim sheet As Excel.Worksheet
Set sheet = PpresetSheet


'Set the title
Dim title As String
title = PcurrentPlan.GCourseName & "-" & PcurrentPlan.GTeacher & "-" & PcurrentPlan.GClassName
sheet.name = title

If Len(title) < 12 Then
    With sheet.Range("C1")
        .HorizontalAlignment = Excel.xlCenter
        .VerticalAlignment = Excel.xlCenter
        .value = title
        .Font.Bold = True
        .Font.Size = 26
    End With
Else
    With sheet.Range("C1:D1")
        .Merge
        .HorizontalAlignment = Excel.xlCenter
        .VerticalAlignment = Excel.xlCenter
        .value = title
        .Font.Bold = True
        .Font.Size = 23
        .MergeCells = True
    End With
End If



'Set the main dates and notes
Dim CurrentDatesFromPlan
CurrentDatesFromPlan = PcurrentPlan.GetDatesForExport()
Dim CurrentNotesFromPlan
CurrentNotesFromPlan = PcurrentPlan.GetNotesForExport()

'Test if the notes are too much
If GetArrayLength(CurrentDatesFromPlan) > 89 Then
    Err.Raise vbObjectError + 3011, "KursheftTools.KursheftGenerieren.ExportPDF", "The notes are too much(more than 89): " & CStr(GetArrayLength(CurrentNotesFromPlan))
End If

Dim i As Integer
'The first column
For i = 3 To 55 Step 1
    'Test if all the notes are processed
    If ((i - 3) = GetArrayLength(CurrentNotesFromPlan)) Then
        Exit For
    End If
    
    With sheet.Range(sheet.Cells(i, 2), sheet.Cells(i, 3)).Interior
        If i Mod 2 = 0 Then
            .Color = GREY1
        Else
            .Color = GREY2
        End If
    End With
    
    sheet.Cells(i, 2) = CurrentDatesFromPlan(i - 3)
    sheet.Cells(i, 3) = CurrentNotesFromPlan(i - 3)
Next i
'If possible, the second column
If GetArrayLength(CurrentNotesFromPlan) > 53 Then
    For i = 3 To 38 Step 1
    'Test if all the notes are processed
    If ((i - 3) = (GetArrayLength(CurrentNotesFromPlan) - 53)) Then
        Exit For
    End If
    
    With sheet.Range(sheet.Cells(i, 5), sheet.Cells(i, 6)).Interior
        If i Mod 2 = 0 Then
            .Color = GREY1
        Else
            .Color = GREY2
        End If
    End With
    
    sheet.Cells(i, 5) = CurrentDatesFromPlan(i + 50)
    sheet.Cells(i, 6) = CurrentNotesFromPlan(i + 50)
    Next i
End If

'Export the page
sheet.ExportAsFixedFormat xlTypePDF, Ppath

ExportPDF = True

'If the title cell is merged, unmerge it
With sheet.Range("C1:D1")
    If (.MergeCells) Then
        .UnMerge
    End If
End With

End Function

'''<summary>Generate a excel worksheet as the preset for the later export as pdf</summary>
'''<param name="Pdts">An Array with 3 date object represents the start of the year, of the first period and the end of the year</param>
'''<param name="PLogoPath" Optional=True>The Path of the logo image, if not given, then the pdf data will not have any logo on the left top edge</param>
'''<return>An excel worksheet object represents the generated preset sheet</return>
Private Function PresetExportSheet(Pdts, Optional PLogoPath As String = "") As Excel.Worksheet

Dim ae
ae = ChrW(&HE4)
Dim oe
oe = ChrW(&HF6)
Dim ue
ue = ChrW(&HFC)
Dim ss
ss = ChrW(&HDF)

'Pause the screen update
Application.ScreenUpdating = False

Dim startDt As Date
startDt = CDate(Pdts(0))

Dim footNote As String
footNote = "Alle Termine dieser Liste m" & ue & "ssen in der Kursmappe eingetragen sein, auch die unterrichtsfreien Tage. Alle Termine(au" & ss & "er den Ferien) m" & ue & "ssen durch ihre Paraphe best" & ae & "tigt werden. Tragen Sie bitte auch die Fehlstundenzahl sowie die Soll - Ist - Stunden(Kursheft S.5) ein." & _
            Chr(10) & Chr(13) & "Hinweis: Sch" & ue & "ler, die aus schulischen Gr" & ue & "nden den Unterricht vers" & ae & "umt haben(Klausur, schul.Veranstaltung usw.) m" & ue & "ssen im Kursheft aufgef" & ue & "hrt werden. Diese Stunden d" & ue & "rfen auf dem Zeugnis aber nicht als Fehlstunden vermerkt werden."


'Create a new sheet
Dim sheet As Excel.Worksheet
Set sheet = ActiveWorkbook.Worksheets.Add
sheet.name = "UninnitializedPresetSheet"

'Set the export format
With sheet.PageSetup
        .PaperSize = xlPaperA4
        .Orientation = xlPortrait
        .PrintArea = "A1:F55"
        .LeftMargin = 0
        .RightMargin = 0
        .TopMargin = 0
        .BottomMargin = 0
        .HeaderMargin = 0
        .FooterMargin = 0
End With

'Set the page format
'rows
sheet.Range("1:1").RowHeight = 70
sheet.Range("2:2").RowHeight = 20
sheet.Range("3:55").RowHeight = 13.5

'columns
sheet.Range("A:A").ColumnWidth = 5
sheet.Range("B:B").ColumnWidth = 15
sheet.Range("C:C").ColumnWidth = 27.5
sheet.Range("D:D").ColumnWidth = 5
sheet.Range("E:E").ColumnWidth = 15
sheet.Range("F:F").ColumnWidth = 27.5

'Set the logo picture
'If the logo path is given
If Not PLogoPath = "" Then
    With sheet.Range("A1:B1")
        Dim img As Excel.Shape
        Set img = ActiveSheet.Shapes.AddShape(msoShapeRectangle, .Left, .Top, .Width - 10, .Height)
        img.Fill.UserPicture PLogoPath
        With img
            .LockAspectRatio = msoTrue
            .Rotation = 0#
            .Placement = xlMoveAndSize
            .Line.Visible = msoFalse
        End With
    End With
End If

'Set the stand date and the half year
With sheet.Range("F1")
    .HorizontalAlignment = Excel.xlRight
    .VerticalAlignment = Excel.xlTop
    .MergeCells = True
    .value = GetSchoolYear(startDt) & Chr(10) & Chr(13) & "Stand: " & format(Now, "dd-MM-yyyy")
    .Font.Size = 10
    .Font.Color = RGB(120, 120, 120)
End With

'Set the periods
sheet.Range("E41").value = "1. Kursabschnitt: "
sheet.Range("F41").value = format(Pdts(0), "dd.MM.yyyy") & " - " & format(DateAdd("d", -17, Pdts(1)), "dd.MM.yyyy")
sheet.Range("E42").value = "2. Kursabschnitt: "
sheet.Range("F42").value = format(Pdts(1), "dd.MM.yyyy") & " - " & format(Pdts(2), "dd.MM.yyyy")

'Set the foot note
With sheet.Range("E43:F55")
    .Merge
    .WrapText = True
    .Font.Size = 11
    .value = footNote
    .MergeCells = True
End With

Set PresetExportSheet = sheet

Application.ScreenUpdating = True

End Function



'------Utils-----------------------

'''<summary>Get the Length of an array</summary>
'''<param name="ary">As Variant: the array</param>
'''<return>The length of that array as Integer</return>
Public Function GetArrayLength(ByVal ary) As Integer

GetArrayLength = UBound(ary) - LBound(ary) + 1

End Function

'''<summary>Indicate whether the array is allocated</summary>
'''<param name="vValue">As Variant: the array</param>
'''<return>A boolean value represents whether the given array is allocated or not</return>
Public Function IsDimensioned(vValue As Variant) As Boolean
    On Error Resume Next
    If Not IsArray(vValue) Then Exit Function
    Dim i As Integer
    i = UBound(vValue)
    IsDimensioned = Err.Number = 0
    Err.Clear
End Function
