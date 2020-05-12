VERSION 5.00
Begin {C62A69F0-16DC-11CE-9E98-00AA00574A4F} FormForGeneratingCoursebook 
   Caption         =   "Kurshefte generieren"
   ClientHeight    =   3588
   ClientLeft      =   108
   ClientTop       =   456
   ClientWidth     =   7944
   OleObjectBlob   =   "FormForGeneratingCoursebook.frx":0000
   StartUpPosition =   1  'Fenstermitte
End
Attribute VB_Name = "FormForGeneratingCoursebook"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

'---------Form Callback Functions--------------

'When the button "Durchsuchen" for the course list is clicked
Private Sub BtnCourseListSearch_Click()

'The Path contains also the file name
Dim path As String
path = GetFilePath("Kursliste", "*.csv")

If path <> "" Then
    courseListFrom.text = path
End If

End Sub

'When the button "Durchsuchen" for the stored place for the Kursheft is clicked
Private Sub BtnStoredInSearch_Click()

Dim path As String
path = GetFolderPath()

If path <> "" Then
    storedIn.text = path
End If

End Sub

'When the button "Fertig und Generieren" is clicked
Private Sub BtnFinish_Click()


'Preset constant colors
Dim REDCOLOR
REDCOLOR = RGB(255, 192, 192)
Dim WHITECOLOR
WHITECOLOR = RGB(255, 255, 255)

'an array contains the valid grades
Dim GRADESC() As Variant
GRADESC = Array("05", "06", "07", "08", "09", "EF", "Q1", "Q2")

'Reset the backcolors
courseListFrom.BackColor = WHITECOLOR
storedIn.BackColor = WHITECOLOR
grade.BackColor = WHITECOLOR

'A variable represents whether all the inputed data are correct
Dim allRight As Boolean
allRight = True

allRight = PathExists(courseListFrom.text) And PathExists(storedIn.text)

'Split the user inputed grades into an array of string
Dim gradesArr As Variant
gradesArr = Split(grade.text, ";")

'If the array is too long or too short
If GetArrayLength(gradesArr) < 1 Or GetArrayLength(gradesArr) > 8 Then
    allRight = False
    grade.BackColor = REDCOLOR
End If

Dim s
For Each s In gradesArr
    If Not ItemExistsInArr(GRADESC, CStr(s)) Then
        allRight = False
        grade.BackColor = REDCOLOR
        Exit For
    End If
Next s


'If all the data are correct, then store the data to the global variables and close the form
If allRight Then

    KursheftGenerieren.CourseListPath = courseListFrom.text
    KursheftGenerieren.StoredPath = storedIn.text
    KursheftGenerieren.Grades = gradesArr

    'Set the dialog result to true
    KursheftGenerieren.DialogResult = True
    Unload FormForGeneratingCoursebook

'If there is someting wrong, then
Else
    'Set the dialog result to false
    KursheftGenerieren.DialogResult = False

    'Find the wrong path and mark it red
    If Not PathExists(courseListFrom.text) Then
        courseListFrom.BackColor = REDCOLOR
    End If
    If Not PathExists(storedIn.text) Then
        storedIn.BackColor = REDCOLOR
    End If
End If

End Sub

'When the button "Aufheben" is clicked
Private Sub BtnCancel_Click()

KursheftGenerieren.DialogResult = False
Unload FormForGeneratingCoursebook

End Sub


'------Private Functions------------


'''<summary>Open a dialog window asks the user to select a specific type of file</summary>
'''<param name="filterName">The name describes the file that needed to be selected</param>
'''<param name="filterType>The type of the file that needed to be selected</param>
'''<return>A string Value represents the full path of the selected file,
'''     if user canceled, then return a empty string</return>
Private Function GetFilePath(filterName As String, filterType As String) As String

Dim filePath As String
Dim fileExplorer As FileDialog
Set fileExplorer = Application.FileDialog(msoFileDialogFilePicker)

'To allow or disable to multi select
fileExplorer.AllowMultiSelect = False

With fileExplorer
    .Filters.Add filterName, filterType

    If .Show = -1 Then 'Any file is selected
        filePath = .SelectedItems.Item(1)
    Else ' else dialog is cancelled
        filePath = "" ' when cancelled set blank as file path.
    End If
End With

GetFilePath = filePath
End Function

'''<summary>Open a dialog window asks the user to select a folder</summary>
'''<return>A string Value represents the full path of the selected file,
'''     if user canceled, then return a empty string</return>
Private Function GetFolderPath() As String

Dim folderPath As String
Dim folderExplorer As FileDialog
Set folderExplorer = Application.FileDialog(msoFileDialogFolderPicker)

With folderExplorer
    If .Show = -1 Then 'Any folder is selected
        folderPath = .SelectedItems.Item(1)
    Else ' else dialog is cancelled
        folderPath = "" ' when cancelled set blank as file path.
    End If
End With

GetFolderPath = folderPath

End Function

'''<summary>Test if the given path exists</summary>
'''<param name="path">The full path that needs to be tested</param>
'''<return>A boolean Value represents whether the path exists or not</return>
Private Function PathExists(path As String) As Boolean

If path = "" Then
    PathExists = False
    Exit Function
End If

'If the return of the dir function is not a null string, then the path exists
PathExists = Not (Dir(path, vbDirectory) = "")

End Function


'''<summary>Check if the given string is in the given array</summary>
'''<param name="astrItems">The array that needs to be searched</param>
'''<param name="strSearch">The string</param>
'''<return>A boolean value represents whether that given string exists in the given array or not</return>
'See Also: https://docs.microsoft.com/en-us/previous-versions/office/developer/office2000/aa164525(v=office.10)
Private Function ItemExistsInArr(astrItems, _
                          strSearch As String) As Boolean
                  

Dim astrFilter() As String
Dim astrTemp() As String
Dim lngUpper As Long
Dim lngLower As Long
Dim lngIndex As Long
   
' Filter array for search string.
astrFilter = Filter(astrItems, strSearch)
   
' Store upper and lower bounds of resulting array.
lngUpper = UBound(astrFilter)
lngLower = LBound(astrFilter)
   
'If nothing fit to the originary filter
If lngUpper < lngLower Then
    ItemExistsInArr = False
    Exit Function
End If
   
' Resize temporary array to be same size.
ReDim astrTemp(lngLower To lngUpper)
   
' Loop through each element in filtered array.
For lngIndex = lngLower To lngUpper
    ' Check that element matches search string exactly.
    If astrFilter(lngIndex) = strSearch Then
        ' Store elements that match exactly in another array.
        ItemExistsInArr = True
        Exit Function
    End If
Next lngIndex

ItemExistsInArr = False

End Function
