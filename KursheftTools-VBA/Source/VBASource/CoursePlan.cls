VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
END
Attribute VB_Name = "CoursePlan"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit
'''<summary>An Object called "CoursePlan" that stores all the information of one course in this half year</summary>

'------Class Member------------


Private Lines() As Daynote

Private CourseName As String

Private ClassName As String

Private Teacher As String


'------Propertys--------------

Public Property Get GCourseName() As String

    GCourseName = CourseName

End Property

Public Property Get GClassName() As String

    GClassName = ClassName

End Property

Public Property Get GPlan() As Daynote()

    GPlan = Lines

End Property

Public Property Get GTeacher() As String

    GTeacher = Teacher
    
End Property

'------Public Functions--------

'''<summary>Pseudo-Class Constructor</summary>
Public Function Initialize(PcourseName As String, PclassName As String, PTeacher As String)

CourseName = PcourseName
ClassName = PclassName
Teacher = PTeacher

End Function

'''<summary>Add a Daynote object to the CoursePlan object</summary>
'''<param name="plan">The Daynote object that needs to be added</param>
'''<return>A boolean value represents whether the line is added successfully or not</return>
Public Function AddLine(plan As Daynote) As Boolean

If Not IsDimensioned(Lines) Then
    ReDim Lines(0)
    Set Lines(0) = plan
    AddLine = True
    Exit Function
End If

ReDim Preserve Lines(GetArrayLength(Lines))
Set Lines(GetArrayLength(Lines) - 1) = plan
AddLine = True

End Function

'''<summary>Get the dates of this course as an array for export. </summary>
'''<return>Return an array of string containing all the dates on them the course will be held chronologically. </return>
Public Function GetDatesForExport() As String()
Dim rtr() As String
Dim cache
For Each cache In Lines
    If IsDimensioned(rtr) Then
        ReDim Preserve rtr(GetArrayLength(rtr))
        rtr(GetArrayLength(rtr) - 1) = cache.GetDate()
    Else
        ReDim rtr(0)
        rtr(0) = cache.GetDate()
    End If
Next cache

GetDatesForExport = rtr
End Function

'''<summary>Get the notes of this course as an array for export. </summary>
'''<return>Return an array of notes containing all the notes chronologically. The order of this array will be the same as the dates array from "GetDatesForExport()". </return>
Public Function GetNotesForExport() As String()
Dim rtr() As String
Dim cache
For Each cache In Lines
    If IsDimensioned(rtr) Then
        ReDim Preserve rtr(GetArrayLength(rtr))
        rtr(GetArrayLength(rtr) - 1) = cache.GetNotes()
    Else
        ReDim rtr(0)
        rtr(0) = cache.GetNotes()
    End If
Next cache

GetNotesForExport = rtr


End Function

'''<summary>ONLY FOR DEBUG
'''         Print all the daynotes of this course</summary>
Public Function PrintString()

If Not IsDimensioned(Lines) Then
    Debug.Print "Lines not initialized"
    Exit Function
End If

Dim s
For Each s In Lines
    Debug.Print s.ToString
Next s

End Function
