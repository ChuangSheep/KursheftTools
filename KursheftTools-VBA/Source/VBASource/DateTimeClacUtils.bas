Attribute VB_Name = "DateTimeClacUtils"
'
' Author: Chuyang W. (https://github.com/ChuangSheep/KursheftTools)
' License: Apache 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
' Document: https://github.com/ChuangSheep/KursheftTools/blob/master/README.md
' Copyright 2020 (c)
'
' ----------------------------------------


Option Explicit
Option Private Module

'-----------Functions for date and time calculations--------

'-----------Public Functions--------------------------------

'''<summary>Calculate the business days between the given two dates</summary>
'''<param name="firstDay">First day in the time interval</param>
'''<param name="lastDay">Last day in the time interval</param>
'''<return>An Integer value representing the days between the given two dates</return>
'''<exception cref="vbObjectError + 3001">AugumentException: If the last day is earlier than the first day</exception>
Public Function BusinessDaysUntil(ByVal firstDay As Date, ByVal lastDay As Date) As Integer

'If the first day is later than the last day, raise an error
If firstDay > lastDay Then
    Err.Raise vbObjectError + 3001, "KursheftTools.StaticCalculations.BusinessDaysUntil", "ArgumentException in StaticCalculation.BusinessDaysUntil: The first day must be earlier than the last day"
End If

Dim businessDaysBetween As Integer
businessDaysBetween = VBA.DateDiff("d", firstDay, lastDay) + 1

Dim fullWeekCount As Integer
fullWeekCount = Application.WorksheetFunction.RoundDown(businessDaysBetween / 7, 0)

If (businessDaysBetween > 7 * fullWeekCount) Then

    'Find whether there is 1-day or 2-day weekend
    Dim firstDayOfWeek As Integer
    Dim lastDayOfWeek As Integer

    firstDayOfWeek = IIf(weekday(firstDay, vbMonday) = 7, 7, weekday(firstDay, vbMonday))
    lastDayOfWeek = IIf(weekday(lastDay, vbMonday) = 7, 7, weekday(lastDay, vbMonday))
    

    If (lastDayOfWeek < firstDayOfWeek) Then
        lastDayOfWeek = lastDayOfWeek + 7
    End If
    
    If (firstDayOfWeek <= 6) Then
        'The whole weekend is in the remaining interval
        If (lastDayOfWeek >= 7) Then
            businessDaysBetween = businessDaysBetween - 2
        'Only Saturday is in the remaining interval
        ElseIf (lastDayOfWeek >= 6) Then
            businessDaysBetween = businessDaysBetween - 1
        End If
    'Only Sunday is in the remaining interval
    ElseIf (firstDayOfWeek <= 7 And lastDayOfWeek >= 7) Then
        businessDaysBetween = businessDaysBetween - 1
    End If
End If

'Substract weekends during the full weeks in the time interval
businessDaysBetween = businessDaysBetween - 2 * fullWeekCount

'Return the value
BusinessDaysUntil = businessDaysBetween

End Function

'I would let the following two functions overload
'However, VBA does not support that

'''<summary>Get the weekday in long or short form of a given integer representing a weekday</summary>
'''<param name="weekday">An Integer value represents the weekday. Monday is seen as 1</param>
'''<param name="isShortForm" optional=True>A boolean Value represents whether the return is in short form or full form</param>
'''<return>A string represents the weekday in short form or in long form in German. ex. Mo, Di/ Montag, Dienstag </return>
'''<exception cref="vbObjectError + 3001">ArgumentException: The given "Pweekday" is not valid</exception>
Public Function DecimalGetWeekdayAsString(weekday As Integer, Optional isShortForm As Boolean = False) As String

Select Case weekday
    Case 1
        DecimalGetWeekdayAsString = IIf(isShortForm, "Mo", "Montag")
        Exit Function
    Case 2
        DecimalGetWeekdayAsString = IIf(isShortForm, "Di", "Dienstag")
        Exit Function
    Case 3
        DecimalGetWeekdayAsString = IIf(isShortForm, "Mi", "Mittwoch")
        Exit Function
    Case 4
        DecimalGetWeekdayAsString = IIf(isShortForm, "Do", "Donnerstag")
        Exit Function
    Case 5
        DecimalGetWeekdayAsString = IIf(isShortForm, "Fr", "Freitag")
        Exit Function
    Case 6
        DecimalGetWeekdayAsString = IIf(isShortForm, "Sa", "Samstag")
        Exit Function
    Case 7
        DecimalGetWeekdayAsString = IIf(isShortForm, "So", "Sonntag")
        Exit Function
    Case Else
        Err.Raise vbObjectError + 3001, "KursheftTools.StaticCalculations.DecimalGetWeekdaysAsString", "ArgumentException in StaticCalculations.DecimalGetWeekdaysAsString: The given Integer is not between 1 - 7"
End Select
End Function

'''<summary>Get the weekday in long or short form of a given Date Object representing a weekday</summary>
'''<param name="dt">A date object value represents the weekday. Monday is seen as 1</param>
'''<param name="isShortForm" optional=True>A boolean Value represents whether the return is in short form or full form</param>
'''<return>A string represents the weekday in short form or in long form in German. ex. Mo, Di/ Montag, Dienstag etc. </return>
'''<exception cref="vbObjectError + 3001">ArgumentException: The given "dt" is not valid</exception>
Public Function DateGetWeekdayAsString(dt As Date, Optional isShortForm As Boolean = False) As String

Dim weekday_ As Integer
weekday_ = weekday(dt, vbMonday)

Select Case weekday_
    Case 1
        DateGetWeekdayAsString = IIf(isShortForm, "Mo", "Montag")
        Exit Function
    Case 2
        DateGetWeekdayAsString = IIf(isShortForm, "Di", "Dienstag")
        Exit Function
    Case 3
        DateGetWeekdayAsString = IIf(isShortForm, "Mi", "Mittwoch")
        Exit Function
    Case 4
        DateGetWeekdayAsString = IIf(isShortForm, "Do", "Donnerstag")
        Exit Function
    Case 5
        DateGetWeekdayAsString = IIf(isShortForm, "Fr", "Freitag")
        Exit Function
    Case 6
        DateGetWeekdayAsString = IIf(isShortForm, "Sa", "Samstag")
        Exit Function
    Case 7
        DateGetWeekdayAsString = IIf(isShortForm, "So", "Sonntag")
        Exit Function
    Case Else
        Err.Raise vbObjectError + 3001, "KursheftTools.StaticCalculations.DateGetWeekdayAsString", "ArgumentException in StaticCalculations.DateGetWeekdayAsString: The given Integer is not between 1 - 7"
End Select
End Function


'''<summary>Convert a Date object to a string in the form of dd-MM-yyyy</summary>
'''<param name="dt">The date in Date object</param>
'''<param name="format" optional=True>The Format of the returned string</param>
'''<return>A string represents the given date</return>
Public Function getDateS(dt, Optional Pformat As String = "dd-MM-yyyy") As String

getDateS = format(dt, Pformat, vbMonday, vbFirstJan1)

End Function

'''<summary>Get a date object that represents the nearest date of the weekday after the given start date</summary>
'''<param name="weekdayI">The weekday</param>
'''<param name="startDate">The first day</param>
'''<return>A date object represents the nearest weekday after the given start date</return>
'''<exception cref="vbObjectError + 3001">ArgumentException: The given "weekdayI" is not valid</exception>
Public Function getNearestDateForWeekday(ByVal weekdayI As Integer, ByVal startDate As Date) As Date

If (weekday(startDate, vbMonday) = weekdayI) Then

    getNearestDateForWeekday = startDate
    Exit Function

Dim i As Integer
Else
    For i = 0 To 7 Step 1
    
    startDate = DateAdd("d", 1, startDate)
    
    If (weekday(startDate, vbMonday) = weekdayI) Then
        getNearestDateForWeekday = startDate
        Exit Function
        
    End If
    
    Next i
End If

Err.Raise vbObjectError + 3001, "KursheftTools.StaticCalculations.getNearestDateForWeekday", "ArgumentException in StaticCalculations.getNearestDateForWeekday: The given weekdayI is not valid"

End Function

'''<summary>Test if the given string is valid</summary>
'''<param name="dateS">A string value represents the date needed to be tested
'''We assume the valid string would be:
'''dd-MM-yyyy</param>
'''<return>a boolean value represents whether the given string is valid or not</return>
Public Function dateIsValid(ByVal Dates As String) As Boolean

dateIsValid = True
'Change this constant if the seperator is changed
Const SEPERATOR As String = "-"

'The different months that contain different days
Dim LONGMONTH As Variant
LONGMONTH = Array(1, 3, 5, 7, 8, 10, 12)
Dim SHORTMONTH As Variant
SHORTMONTH = Array(4, 6, 9, 11)

'If the String is not long enouth, then not valid
If Not (Len(Dates) = 10) Then
dateIsValid = False
End If

'If the string is not right(have less or more than 2 "-"
If Not (GetArrayLength(Split(Dates, SEPERATOR)) = 3) Then
dateIsValid = False
Exit Function
End If


'spilt the string
Dim spiltedStringDate
spiltedStringDate = Split(Dates, SEPERATOR)


'If the Year is valid
Dim year As Integer
year = CInt(spiltedStringDate(2))
If year < 1 Or year > 2100 Then
dateIsValid = False
Exit Function
End If

'If the Month is valid
If CInt(spiltedStringDate(1)) > 13 Or 1 > CInt(spiltedStringDate(0)) Then
dateIsValid = False
Exit Function
End If


'Check the days

'For the months that have 31 days
Dim Month1
For Each Month1 In LONGMONTH
    If CInt(Month1) = CInt(spiltedStringDate(1)) Then
        If CInt(spiltedStringDate(0)) > 31 Or CInt(spiltedStringDate(0)) < 1 Then
            dateIsValid = False
            Exit Function
        End If
    End If
Next Month1
 
'For the months that have 30 days
Dim Month2
For Each Month2 In SHORTMONTH
    If CInt(Month2) = CInt(spiltedStringDate(1)) Then
        If CInt(spiltedStringDate(0)) > 30 Or CInt(spiltedStringDate(0)) < 1 Then
            dateIsValid = False
            Exit Function
        End If
    End If
Next Month2


'For February
'If it is leap year
If CInt(spiltedStringDate(1)) = 2 Then
    If IsLeapYear(year) Then
        If CInt(spiltedStringDate(0)) > 29 Or CInt(spiltedStringDate(0)) < 1 Then
        dateIsValid = False
        Exit Function
    End If
    Else
        If CInt(spiltedStringDate(0)) > 28 Or CInt(spiltedStringDate(0)) < 1 Then
            dateIsValid = False
            Exit Function
        End If
    End If
End If


End Function



'''<summary>Check the given year, if it's leap year, return True, else return False</summary>
'''<param name="year">The year to be checked</param>
'''<return>A boolean value that represents whether that year is leap year or not</return>
Public Function IsLeapYear(year As Integer) As Boolean

If year Mod 100 = 0 Then
    If year Mod 400 = 0 Then
        IsLeapYear = True
    Else
        IsLeapYear = False
    End If
Else
    If year Mod 4 = 0 Then
        IsLeapYear = True
    End If
End If
    
End Function


'''<summary> Convert the given string to a date object
'''IMPORTANT: WE ASSUME ALL THE GIVEN STRINGS ARE VALID
'''CHECK BEFORE USE THIS FUNCTION </summary>
'''<param name="DateStr"> a string represents a date
'''We assume the valid string would be:
'''dd-MM-yyyy </param>
'''<return>A Date object represents the date of the given string</return>
Public Function convertStringToDate(dateStr As String) As Date

'Change this constant if the seperator is changed
Const SEPERATOR As String = "-"

'spilt the string
Dim spiltedSDate As Variant
spiltedSDate = Split(dateStr, SEPERATOR)

'convert the date from string to date object
convertStringToDate = DateSerial(spiltedSDate(2), spiltedSDate(1), spiltedSDate(0))
End Function

'''<summary>Sort the dates in the given array from early to late</summary>
'''<param name="datesArr">The array needs to be sorted</param>
'''<param name="arr" Optional=True>2-Dimensioned array: If there are other arrays relating to the given date array, this function will also keep the order of the index same</param>
'''<return>2-Dimensioned array: The changed array with the other arrays that relate to the dates array
'''         The array of the date will be returned as the first item of the returned array
'''         The other arrays will be returned as the same order when they were given</return>
Public Function SortDates(datesArr, Optional arr) As Variant()


    Dim cacheArr

    Dim i As Integer
    Dim j As Integer
    Dim n As Integer
    For i = 0 To GetArrayLength(datesArr) - 2
        For j = 0 To GetArrayLength(datesArr) - 2
            If datesArr(j) > datesArr(j + 1) Then
                Call SwapArr(datesArr, j, j + 1)
                
                If IsDimensioned(arr) Then
                    For n = 0 To GetArrayLength(arr) - 1 Step 1
                        Call SwapArr(arr(n), j, j + 1)
                    Next n
                End If
                
            End If
        Next j
    Next i
    

Dim rtr()
ReDim rtr(0)
rtr(0) = datesArr
For Each cacheArr In arr
    ReDim Preserve rtr(GetArrayLength(rtr))
    rtr(GetArrayLength(rtr) - 1) = cacheArr
Next cacheArr

SortDates = rtr
End Function

'''<summary>Get to know which half year is the given date in</smmary>
'''<param name="dt">The date object</param>
'''<return>A integer of 1 or 2 represents whether the given date is in the first half year or the second</return>
Public Function GetHalfYearNum(dt As Date) As Integer

    If CInt(format(dt, "ww")) < 30 Then
        GetHalfYearNum = 1
    ElseIf CInt(format(dt, "ww")) > 30 Then
        GetHalfYearNum = 2
    End If


End Function

'''<summary>Get the school year of the given date and return a string of it</summary>
'''<param name="dt">The date object</param>
'''<return>A string represents the half year of the given date</return>
Public Function GetSchoolYear(dt As Date) As String

    If GetHalfYearNum(dt) = 1 Then
        GetSchoolYear = "2. Halbjahr " & format(DateAdd("d", -365, dt), "yyyy") & "/" & format(dt, "yy")
    Else
        GetSchoolYear = "1. Halbjahr " & format(dt, "yyyy") & "/" & format(DateAdd("d", 365, dt), "yy")
    End If

End Function

'''<summary>Indicate whether the give date is in a even week or in an odd week</summary>
'''<param name="dt">The date</param>
'''<return>A boolean value represents whether the given date is in a even week or not</return>
Public Function isEvenWeek(dt As Date) As Boolean

    isEvenWeek = ((CInt(format(dt, "ww", vbMonday)) Mod 2) = 0)

End Function

'------Private Functions----------------------


'''<summary>Get the Length of an array</summary>
'''<param name="ary">As Variant: the array</param>
'''<return>The length of that array in Integer</return>
Private Function GetArrayLength(ByVal ary) As Integer

If Not IsDimensioned(ary) Then
    GetArrayLength = 0
    Exit Function
End If

GetArrayLength = UBound(ary) - LBound(ary) + 1

End Function


'''<summary>Change the items of the given array on the two indexs
'''         Important: The param arr is byref</summary>
'''<param name="arr" ByRef=True>As Variant: the array</param>
'''<param name="index1">The first index</param>
'''<param name="arr">The second index</param>
Private Function SwapArr(ByRef arr, index1 As Integer, index2 As Integer)

If Not IsDimensioned(arr) Then
    Err.Raise vbObjectError + 3001, "DateTimeCalcUtils.SwapArr", "ArgumentException: The given array is not initialized"
    Exit Function
End If

If GetArrayLength(arr) <= index1 Or GetArrayLength(arr) <= index2 Then
    Err.Raise vbObjectError + 3001, "DateTimeCalcUtils.SwapArr", "ArgumentException: The given index is out of the bound"
    Exit Function
End If

Dim cache
cache = arr(index1)
arr(index1) = arr(index2)
arr(index2) = cache

End Function



