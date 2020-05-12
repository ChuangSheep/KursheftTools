VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
END
Attribute VB_Name = "Daynote"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit

'''<summary>An Object called "WeeklyNotes" that stores all the information of one course in the week</summary>

'------Class Member------------


Private Day As Date

Private Notes() As String

'------Propertys--------------

Public Property Get Gdays() As Date

    Gweekdays = Day

End Property

Public Property Get Gnotes() As String()

    Gnotes = Notes

End Property

'------Public Functions--------

'''<summary>Pseudo-Class Constructor</summary>
'''<param name="dt">The date of this daynote object. </param>
Public Function Initialize(dt As Date)

Day = dt

End Function

'''<summary>Add a note as string to the notes array</summary>
'''<param name="note">A note as string that needs to be added</param>
'''<return>A boolean value represents whether the note is added successfully or not</return>
Public Function AddNote(note As String) As Boolean

If Not IsDimensioned(Notes) Then
    ReDim Notes(0)
    Notes(0) = note
    AddNote = True
    Exit Function
End If

ReDim Preserve Notes(GetArrayLength(Notes))
Notes(GetArrayLength(Notes) - 1) = note
AddNote = True

End Function

'''<summary>Get the date of this day. </summary>
'''<return>Retruen the date of this day as string. </return>
Public Function GetDate() As String

GetDate = DateGetWeekdayAsString(Day, True) & "  " & Format(Day, "dd-MM-yyyy")

End Function

'''<summary>Get the notes on this day as a string</summary>
'''<return>A string containing all the notes on this day. </return>
Public Function GetNotes() As String

If IsDimensioned(Notes) Then
    Dim nt
    For Each nt In Notes

        GetNotes = GetNotes & CStr(nt) & "; "

    Next nt
Else
    GetNotes = ""
End If


End Function


'''<summary>Return the string form of the daynote object</summary>
'''<return>The string containing the weekday, the date and all the notes</return>
Public Function ToString() As String

Dim cache As String

cache = DateGetWeekdayAsString(Day, True) & "    " & Format(Day, "dd-MM-yyyy") & "    "

If IsDimensioned(Notes) Then
    Dim nt
    For Each nt In Notes

        cache = cache & CStr(nt) & "; "

    Next nt
End If

ToString = cache
End Function
