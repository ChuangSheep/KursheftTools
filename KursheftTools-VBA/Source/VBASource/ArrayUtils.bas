Attribute VB_Name = "ArrayUtils"
'
' Author: Chuyang W. (https://github.com/ChuangSheep/KursheftTools)
' License: Apache 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
' Document: https://github.com/ChuangSheep/KursheftTools/blob/master/README.md
' Copyright 2020 (c)
'
' ----------------------------------------


Option Explicit


'------Utils-----------------------


'''<summary>Get the Length of an array</summary>
'''<param name="ary">As Variant: the array</param>
'''<return>The length of that array in Integer</return>
Public Function GetArrayLength(ByVal ary) As Integer

If Not IsDimensioned(ary) Then
    GetArrayLength = 0
    Exit Function
End If

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

