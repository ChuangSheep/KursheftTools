VERSION 5.00
Begin {C62A69F0-16DC-11CE-9E98-00AA00574A4F} FormForCreatingBoard 
   Caption         =   "Bemerkungsbogen erstellen"
   ClientHeight    =   3336
   ClientLeft      =   108
   ClientTop       =   456
   ClientWidth     =   6240
   OleObjectBlob   =   "FormForCreatingBoard.frx":0000
   StartUpPosition =   1  'Fenstermitte
End
Attribute VB_Name = "FormForCreatingBoard"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'''<file name="FormForInputDates">
'''This file contains the callback functions for the form "FormForInputDates"
'''Also, It contains some functions that help to indicate the validity of the dates
'''</file>

'---------Form Callback Functions----------------

'''<summary>The callback function for the button "Aufheben"</summary>
Private Sub BtnCancel_Click()

DialogResult1 = False
Unload FormForCreatingBoard

End Sub

'''<summary>The callback function for the button "Fertig"</summary>
Private Sub BtnFinish_Click()

Dim datesFromTB(2) As String
datesFromTB(0) = StartPeriod1.text
datesFromTB(1) = StartPeriod2.text
datesFromTB(2) = EndHY.text

'write the text boxes into an array
Dim TextBoxes
TextBoxes = Array(StartPeriod1, StartPeriod2, EndHY)

Dim exiting As Boolean
'test the data validity
'if they are not valid
'change the background color of that textbox and exit
Dim i As Integer
For i = 0 To 2 Step 1
    If Not DateIsValid(datesFromTB(i)) Then
        TextBoxes(i).BackColor = RGB(255, 192, 192)
        exiting = True
    Else
        TextBoxes(i).BackColor = RGB(255, 255, 255)
    End If
Next i

If Not exiting Then
    For i = 0 To 2 Step 1
        If weekday(DateValue(TextBoxes(i).text), vbMonday) = 6 Or weekday(DateValue(TextBoxes(i).text), vbMonday) = 7 Then
            TextBoxes(i).BackColor = RGB(255, 192, 192)
            exiting = True
        End If
    Next i
End If
'Test if the dates are in incorrect order or the intervals are too short
If Not exiting Then
    Dim cache1 As Date
    Dim cache2 As Date
    For i = 0 To 1 Step 1
        cache1 = DateValue(TextBoxes(i).text)
        cache2 = DateAdd("d", -17, DateValue(TextBoxes(i + 1).text))
        If cache1 > cache2 Then
            TextBoxes(i).BackColor = RGB(255, 192, 192)
            TextBoxes(i + 1).BackColor = RGB(255, 192, 192)
            exiting = True
        End If
    Next i
End If


If Not exiting Then
    'pass the parameter to the main sub
    dates = Array(ConvertStringToDate(datesFromTB(0)), ConvertStringToDate(datesFromTB(1)), ConvertStringToDate(datesFromTB(2)))

    'close the Form
    Unload FormForCreatingBoard

    'Change the dialogResult1 to true since the dateS is written correctly
    DialogResult1 = True

End If

End Sub

