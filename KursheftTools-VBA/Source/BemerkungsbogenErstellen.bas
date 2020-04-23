Attribute VB_Name = "BemerkungsbogenErstellen"
Option Explicit

'------Global Variables-------------------

'''<summary>Stores the start date, the start of the second period and the end of the year </summary>
Public Dates()
'''<summary>Stores the result of the FormForInputDates
'''True represents that the form is closed with correct data
'''False represent that the form is closed without writing the correct data </summary>
Public DialogResult1 As Boolean

'------Main Function----------------------


'''<summary>The main function that creates a note board based on the user inputed dates</summary>
Public Sub BemerkungsbogenErstellen()

'Set the dialog result for the FormForInputDates
DialogResult = False

'Show the window that allows the user to input the dates
FormForCreatingBoard.Show

'If the right dates are stored
If DialogResult1 Then

    Dim sheet As Excel.Worksheet
    Set sheet = CreateNewSheet("")
    
    If Not (sheet Is Nothing) Then
        'Create the real board
        CreateBoard sheet, Dates

        Call MsgBox("Die Bemerkungsbogen wurde erfolgreich generiert", vbOKOnly, "Erfolg")
    Else
        Debug.Print "Unexpected Error At BemerkungsbogenGenerieren.Main"
    End If

End If


End Sub

'------Private Functions and Subs-------------------


'''<summary>Create a new sheet in the current workbook with the given name</summary>
'''<param name="name">The name for the generated worksheet
'''                 If the name is an empty string, then generate a worksheet with the default name</param>
'''<return>An Excel Worksheet object represents the generated worksheet
'''         Return nothing if the sheet exists</return>
Private Function CreateNewSheet(name As String) As Excel.Worksheet
On Error GoTo ErrorHandle

'Test if there is a workbook already opened
'If not, add one
If (Workbooks.Count = 0) Then
    Dim BBWorkBook As Excel.Workbook
    Set BBWorkBook = Workbooks.Add
    BBWorkBook.Activate
    Set CreateNewSheet = BBWorkBook.Sheets(1)
    If name <> "" Then
        BBWorkBook.Sheets(1).name = name
    End If
    Exit Function
End If

'If a sheet with the given name exists, then return False and exit the function
If SheetExists(name) Then

    Set CreateNewSheet = Nothing
    Debug.Print "Information: BemerkungsbogenGenerieren.CreateNewSheet: sheet already exists"
    Exit Function

'If the sheet with given name does not exist
'Add a new empty sheet to the active workbook with the given name
Else
    Dim addedSheet As Excel.Worksheet
    Set addedSheet = Sheets.Add
    'If the name is given, set it to the generated sheet
    If name <> "" Then
        addedSheet.name = name
    End If
    Set CreateNewSheet = addedSheet
End If

'Exit the subsquence
CleanUp:
    Exit Function

'Error Handler: show a message box and clean up
ErrorHandle:
    Debug.Print "BemerkungsbogenGenerieren.CreateNewSheet: " & Err.Number & Err.Source & Err.Description
    Set CreateNewSheet = Nothing
    Err.Clear
Resume CleanUp

End Function


'''<summary>Create a note board on the given sheet based on the given dates</summary>
'''<param name="sheet">A Excel sheet object indicating the board sheet</param>
'''<param name="dates">An array containing 3 dates' object which represent the start of the
''' first period, the start of the second period and the end of the year</param>
Private Sub CreateBoard(sheet As Object, Dates())

Const STARTT1 As String = "Anfang d. 1. Abschnitts"
Const STARTT2 As String = "Anfang d. 2. Abschnitts"
Const ENDT As String = "Ende des Schulhalbjahres"
Dim titlesStart(2) As String
titlesStart(0) = STARTT1
titlesStart(1) = STARTT2
titlesStart(2) = ENDT

'Get the days between the dates
Dim dateIntervals(2) As Integer
dateIntervals(0) = 0
dateIntervals(1) = BusinessDaysUntil(CDate(Dates(0)), DateAdd("d", -17, CDate(Dates(1))))
dateIntervals(2) = BusinessDaysUntil(CDate(Dates(0)), DateAdd("d", -17, CDate(Dates(1)))) + BusinessDaysUntil(CDate(Dates(1)), CDate(Dates(2))) - 1


'Set the format of the whole worksheet
sheet.Range("A:A").EntireColumn.EntireRow.ColumnWidth = 30
sheet.Range("A:A").EntireColumn.EntireRow.RowHeight = 30


'Set the titles row its format
sheet.Range("A1").value = "Wochentage"
sheet.Range("B1").value = "Datum"
sheet.Range("I1").value = getDateS(Dates(0)) & "~" & getDateS(Dates(1)) & "~" & getDateS(Dates(2))

Dim i As Integer

For i = 1 To 3 Step 1
sheet.Cells(1, (i + 1) * 2).EntireColumn.ColumnWidth = 10
sheet.Cells(1, (i + 1) * 2).EntireColumn.WrapText = True
sheet.Cells(1, (i + 1) * 2).EntireColumn.NumberFormat = "@"
sheet.Cells(1, (i + 1) * 2) = "Stufe"
sheet.Cells(1, (i + 1) * 2 - 1) = "Besonderheit " & CStr(i)
Next i

sheet.Range("A1").EntireRow.Font.Bold = True
sheet.Range("A1").EntireRow.Font.Size = 14
sheet.Application.ActiveWindow.SplitRow = 1
sheet.Application.ActiveWindow.FreezePanes = True


'Set the dates and the weekdays

'Dates Format
sheet.Range("B:B").NumberFormat = "dd-MM-yyyy"

For i = 0 To 2 Step 1
    'Set the first three date and weekday
    sheet.Cells(dateIntervals(i) + 3 + i, 1).value = DecimalGetWeekdayAsString(weekday(Dates(i), vbMonday))
    sheet.Cells(dateIntervals(i) + 3 + i, 2).value = Dates(i)
    
    If i < 2 Then
        'Set the title lines and its Format (Anfang d. Abschnitts etc. )
        sheet.Cells(dateIntervals(i) + 2 + i, 2).value = titlesStart(i)
        sheet.Cells(dateIntervals(i) + 2 + i, 2).EntireRow.Interior.Color = RGB(146, 208, 80)
        sheet.Cells(dateIntervals(i) + 2 + i, 2).Font.Size = 14
        
        'Autofill the dates and weekdays
        sheet.Cells(dateIntervals(i) + 3 + i, 1).AutoFill _
        sheet.Range(sheet.Cells(dateIntervals(i) + 3 + i, 1), sheet.Cells(dateIntervals(i + 1) + 2 + i, 1)), Excel.XlAutoFillType.xlFillWeekdays
        sheet.Cells(dateIntervals(i) + 3 + i, 2).AutoFill _
        sheet.Range(sheet.Cells(dateIntervals(i) + 3 + i, 2), sheet.Cells(dateIntervals(i + 1) + 2 + i, 2)), Excel.XlAutoFillType.xlFillWeekdays
    End If
Next i

'Set the last two lines
sheet.Cells(dateIntervals(2) + 3 + 1, 1).value = DecimalGetWeekdayAsString(weekday(Dates(2), vbMonday))
sheet.Cells(dateIntervals(2) + 3 + 1, 2).value = Dates(2)

sheet.Cells(dateIntervals(2) + 3 + 2, 1).value = ""
sheet.Cells(dateIntervals(2) + 3 + 2, 2).value = titlesStart(2)
'Format of the titil line
sheet.Cells(dateIntervals(2) + 3 + 2, 1).EntireRow.Interior.Color = RGB(146, 208, 80)
sheet.Cells(dateIntervals(2) + 3 + 2, 2).Font.Size = 14


'Auto fit the columns
sheet.Cells(1, 1).EntireColumn.AutoFit
sheet.Activate


End Sub


'''<summary>Test if a sheet in the current workbook exist</summary>
'''<param name="sheetName">A string represents the name of the worksheet
'''             If it is an empty string, then always return false</param>
'''<return>A boolean value represents whether that sheet exists</return>
Private Function SheetExists(sheetName As String) As Boolean

Dim x As Object

On Error Resume Next

'If there is no workbook open, then obviously, the sheet does not exist
If (Workbooks.Count = 0) Then
    SheetExists = False
    Exit Function
End If

'If the given name is a empty string, we return always false
If sheetName = "" Then

SheetExists = False

End If
'Try to set x to the worksheet with the given name
Set x = ActiveWorkbook.Sheets(sheetName)

'If no error occured, then it exists
If Err = 0 Then
    SheetExists = True
'Otherwise, a sheet with the given name does not exist
Else
    SheetExists = False
End If

Err.Clear
End Function
