Attribute VB_Name = "RibbonUI"
'
' Author: Chuyang W. (https://github.com/ChuangSheep/KursheftTools)
' License: Apache 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
' Document: https://github.com/ChuangSheep/KursheftTools/blob/master/README.md
' Copyright 2020 (c)
'
' ----------------------------------------
'Version 1.0.2.0


Option Explicit

' -----Ribbon callback functions------------------

Public Sub HideAddin_Click(ctrl As IRibbonControl)

Dim ae
ae = ChrW(&HE4)
Dim oe
oe = ChrW(&HF6)
Dim ue
ue = ChrW(&HFC)
Dim ss
ss = ChrW(&HDF)

Dim result As Integer
result = MsgBox("Wollen Sie dieses Addin wirklich deinstallieren?", vbYesNo)

If result = vbYes Then
Application.AddIns("KursheftTools").Installed = False
Call MsgBox("Addin deinstalliert. " & Chr(10) & Chr(13) & "Sie m" & ue & "ssen evtl. Excel neu" & oe & "ffenen, um den Effekt zu sehen. ", vbOKOnly)
Else
    Call MsgBox("Deinstallation abgebrochen", vbOKOnly)
End If

End Sub

Public Sub CreateNoteBoard_Click(ctrl As IRibbonControl)

BemerkungsbogenErstellen.BemerkungsbogenErstellen

End Sub


Public Sub CreateCoursePlan_Click(ctrl As IRibbonControl)

KursheftGenerieren.KursheftGenerieren

End Sub
