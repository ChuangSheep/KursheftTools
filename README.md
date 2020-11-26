**This description might be outdated.**

*Lang En |* [*De*](#Unavaliable) <br>

# Kursheft Tools

An Excel Addin for schools to quickly create its own course plans. <br>
Avaliable as VBA Addin and also as VSTO Addin(c#). 

## Table of Contents

* [Getting Started](#GettingStarted) 
	* [For c#](#StartCS) 
	* [For VBA](#StartVBA) 
	
* [Technologies](#Technologies)

* [Examples of use](#ExamplesOfUse) 
	* [How to create a new note board](#CreateNewNoteboard)
	* [How to import the course list (Only for c# version)](#ImportCourseList)
	* [How to export the course plans](#ExportCoursePlan)
	* [How to export the note board (Only for c# version)](#ExportNoteboard)
	* [How to import a note board (Only for c# version)](#ImportCourseList)
	
* [Performance](#Performance)

* [Format of files](#Format)
	* [Format of course list](#FormatCourseList)
	* [Format of note board](#FormatNoteBoard)

* [Deployment](#Deployment)
	* [For c#](#DeploymentCSharp)
	* [For VBA](#DeploymentVBA)
	
* [Info for collaboration](#Collaboration)
	* [Code style](#CodeStyle)

* [Built With](#BuiltWith)

* [License](#License)

<a name="GettingStarted"></a>
## Getting Started

The following instructions will get you a copy of the project and running on your local machine for *development and testing purposes*. **See** [**Deployment**](#Deployment) **or** [**Release**](https://github.com/ChuangSheep/KursheftTools/releases) **to get the newest stable version for deployment on a live system.** <br>

<a name="StartCS"></a>
### For C#

#### Prerequisites

To compile and run the source code of the VSTO Addin, you need to have **Microsoft Visual Studio 2017 or a newer** version on your computer. <br>
As this project was created on Visual Studio 2019, we also *highly recommend you using the newest version of the Visual Studio IDE* (Dev 16.5). <br> 

To Get Microsoft Visual Studio, [download it from Microsoft](https://visualstudio.microsoft.com/downloads/). <br>

The Microsoft Office developer tools will be installed by default. If you already have Visual Studio on your computer but don't have the Office developer tools, make sure that **Microsoft Office Developer Tools** is selected during setup. If not, open **Visual Studio Installer**, click **Modify**, and select this functionality called *Office/Sharepoint development*. <br>
Even though the *Visual Studio Tools for Office runtime redistributable* will normally be installed while the *Office Developer Tools* being installed, however, if you don't have it on your local machine, have a look of [this Microsoft documentation about how to enable it](https://docs.microsoft.com/en-us/visualstudio/vsto/how-to-install-the-visual-studio-tools-for-office-runtime-redistributable?view=vs-2019). <br>

Also, you need to have **Microsoft Office 2016, Office 365 (Microsoft 365), or a newer** version installed on your local machine. <br>

#### Installing

As the next step, download the whole folder in the root called `KursheftTools-CSharp` or the newest release, where you are going to download the source code of csharp as a .zip file. <br>

**Open the solution file** (*KursheftTools.sln*) using Visual Studio.

After everything is loaded and ready to use, **click Start** with the green triangle on the top of your Visual Studio IDE. Make sure that your configuration is set to `Debug`. <br>

Your Excel will be opened, open or create a random workbook, you will find a tab called **Kursheft Tools** after the tab **help**. <br>

**Congratulations!** The Addin is successfully installed and ready to use. <br>

___

<a name="StartVBA"></a>
### For VBA

#### Prerequisites

To use the VBA Addin, you need to have **Microsoft Office 2013, Office 365 (Microsoft 365), or newer** installed on your local machine. <br>

We also recommend you to set the **macro security level** to allow the downloaded macro to run properly. You can find [a support page of Office here](https://support.office.com/en-us/article/change-macro-security-settings-in-excel-a97c09d2-c082-46b8-b19f-e8621e8fe373)<br>

#### Installing

At first, you have to download either the KursheftTools.xlam in the folder `KursheftTools/KursheftTools-VBA` or download the whole folder called `VBASource` at `KursheftTools/KursheftTools-VBA`. <br>

Then **open Excel**, go to the tab called **Developer**. Then click the button called **Visual Basic**. If you don't see this tab, [have a look of this office support page explaining how to show it](https://support.office.com/en-us/article/show-the-developer-tab-e1192344-5e56-4d45-931b-e5fd9bea2d45). <br>

*Depending on what you have downloaded*, **double click KursheftTools.xlam** or **import the source files** into your Visual Basic editor. <br>

**Congratulations!** The Addin is successfully loaded into your excel and ready to use.  <br>

<a name="Technologies"></a>
## Technologies

For C# VSTO Addin: 

* C# 5.0
* .Net Framework 4.5
* VSTO Runtime library

For VBA Addin: 

* Visual Basic for Application 7.1


<a name="ExamplesOfUse"></a>
## Examples of use

Depending on what you have chosen, there could be some small differences in the user interface between the versions as also the way how to run the Addin. <br>

For the C# version, after installing, this Addin will be available every time you open Excel. <br>
For the VBA version, if you have used the `.xlam` file, the addin will show up every time you open Excel unless you uninstalled it. <br>
After uninstalled the VBA Addin by **itself**, you may have to re-enable it manually from *Developer* -> *Excel-Addin*. 


<a name="CreateNewNoteboard"></a>
### Create a new note board

To test how this Addin creates a new note board, click the button called **Neue Bemerkungsbogen erstellen** at tab **Kursheft Tools**. <br>

After that, a dialog window will open and asks you to input the periods, which are supposed to be the start of the year, the start of the second period, and the end of the half-year. <br>

Input them, and an empty note board will be generated on a new worksheet in your opened workbook. 

*Please note that all of the inputted dates should be weekdays.* <br>
*Also, the intervals between every inputted date should be more than two weeks.* <br>
*Certainly, the date for the start of the year should not be later than the start of the second period or the end of the year.* <br>


If you have made a mistake mentioned here, the window will not close and *the wrong field will be marked red*. So you don't have to worry if you inputted the dates wrong. <br>

___

<a name="ImportCourseList"></a>
### Import the course list (C#)

***Process the course list using fixCourseList.py to delete the duplicate courses and generate a new text file for use.***

***If there is not python interpreter installed, you can still use fixCourseList.html as alternative.***

To import the course list from a `.csv` or `.txt` file, click the button called **Kursliste importieren**. A dialog window will be opened and asks you to choose the `.csv`/`.txt` file that has been processed by `fixCourseList.py/html`. <br>

After excel has successfully imported the course list, a message box will show up that the import was completed. 

___

<a name="ExportCoursePlan"></a>
### Export the course plans

Before exporting the course plans, please go to the note board worksheet which you want to use for the export. <br>

To export the course plans as pdf, click the button called **Kurshefte generieren** at tab **Kursheft Tools**. <br>

A dialog window will show up. If you are using the C# VSTO Addin, you will be asked to input the path to store the exported course plans, the grades need to be exported and the path of the logo file (*optional*). <br>

**For C#, you have to import a course list at first.** <br>

For VBA version, you will be extra asked for the path of the course list. However, the field for the path of the logo file does not exist here. <br>

**If you want to add a logo for the VBA version, go to the macro method called** `KursheftGenerieren` 
**and change the constant called** `LogoPath` 
**on the top of this file. Add the path of your logo file here.** <br>

```VBA
Public Const LogoPath As String = "YOUR LOGO PICTURE FILE PATH HERE"
```
___


<a name="ExportNoteboard"></a>
### Export the note board (C#)

To export your current note board, go to the note board which you want to export. Then click the **Bemerkungsbogen exportieren** button. A dialog window will show up asking you to choose where to store the exported note board. <br>

Or, for the *VBA* version, you can save the note board as `.xlsx` file yourself and open it using excel next time when you need it. It works the same as this functionality. <br>

___

<a name="ImportNoteboard"></a>
### Import the note board (C#)

To import a note board, click the **Bemerkungsbogen importieren** button. A dialog window will show up asking you to choose a `.xlsx` file which is supposed to be a note board. <br>

Or, for the *VBA* version, you can easily open the `.xlsx` file using excel itself. 


<a name="Performance"></a>
## Performance

```
The values are an average  of at least 3 tests.

The note board has a length of a normal half year (23 weeks). 

The logo picture is in .png, 26.6 Kb, and 897*573 pixel. 

Tested on Windows 10 Pro (R) Version 1909
Ram 16 Gb, CPU Intel i7-8750H @ 2.20 Ghz (3.90 Ghz) 6 Cores / 12

Big data set shall mean more than 1500 files;
Small data set shall mean about several hundred files. 
```

### C#
	
```
While running, the program (including the excel itself) takes:
ca. 200 Mb Ram (50Mb more) and
ca. 10% CPU usage (of all processors)
```
	
#### Import a course list
	
* ca. 3600 lines take less than 1 second.
	
#### Export with logo
	
* Big data set: **Per file 167.73 ms.** <br>
* Small data set: **Per file 157.36 ms.**<br>

#### Export without logo
	
* Big data set: **Per file 101.19 ms.** <br>
* Small data set: **Per file 99.72 ms.**<br>

___


### VBA

```
While running, the program (including the excel itself) takes:
ca. 200 Mb Ram (60Mb more) and
ca. 8-12% CPU usage (of all processors)
```
	
**The time cost of the VBA Addin may vary in a big space.** <br>

#### Export with logo
	
* Big data set: **Per file 382.46 ms.** <br>
* Small data set: **Per file 360.95 ms.**<br>

#### Export without logo
	
* Big data set: **Per file 375.60 ms.** <br>
* Small data set: **Per file 407.70 ms.**<br>

	

<a name="Format"></a>
## Format of files

To get the example files, go to the folder called [*ExampleDataForTest*](ExampleDataForTest). <br>
*Note that these example data are made up and do not contain any personal information.* <br>

<a name="FormatCourseList"></a>
### Format of the course list 

The `.csv` file as course list should have the following format: <br>

```
[Course Number] | [Class] | [Teacher] | [Subject] | [Room] | [Weekday] | [Hour] | [Unused] | [Unused]
```

**Please note that the csv file itself does NOT contain any title line.** <br>
**However, if needed, importing a csv file with title is supported on the code level.** <br>
**See CSVUtils.cs method ImportCSVasDT for more details.** <br>


#### Course Number

A course number is a number used to identify the different courses. <br>
*This will NOT be used at all for our project since one course could have different course numbers for different days.* <br>

#### Class

The class of this course. A course with the same subject but different classes will be identified as two different courses. <br>

#### Teacher

The name (or in short form) of the teacher. <br>

#### Subject

The subject of this course (in short form). <br>

#### Room

The room where this course will be held. <br>
*This property will NOT be used for this project currently.* <br>

#### Weekday

The weekday as a number on that this course will be held. <br>

#### Hour

The hour which this course will be held. This value should be 1 to 9. <br>

If this property is between `1` to `7`, then this course will be held **every week**. <br>

Otherwise, if this shows `8`, then this course will only be held on **even weeks**. <br>
If this shows `9`, then this course will only be held on **odd weeks**. <br> 

**If 8 or 9 shows up together, then this course will be held every week.** <br>


For example: <br>

```CSV
1002;"06b";"Dvd";"ChB";"10.07";3;8;;
7020;"06b";"Dvd";"ChB";"10.07";5;5;;
```

This example shows, the course named `ChB` for class `06b` will be held **every even week** on the *eighth and ninth hour* of **Wednesday**. <br>
However, this course will also be held **every week** on the fifth hour of **Friday**. <br>


#### Unused

This column does not contain any useful information and will not be used at all. <br>

___

#### Examples

*A part of the course list* <br>

```csv
46;"05c";"Frk";"Eng";"7.06";1;6;;
```

`46` : This course number will not be used at all. <br>
`05c` : The class of this course. <br>
`Frk` : The name of the teacher (in the short form). <br>
`Eng` : The subject of this course (in the short form). <br>
`7.06` : The room where this course will be held. <br>
`1` : This course will be held on Monday. <br>
`6` : This course will be held in the sixth hour of the day in every week since it is not `8` or `9`. <br>


*And multiple lines of such record shows how the course looks like:* <br>

```csv
46;"05c";"Frk";"Eng";"7.06";1;6;;
46;"05c";"Frk";"Eng";"7.06";2;8;;
47;"05c";"Frk";"Eng";"7.06";3;6;;
```

Here we can see, the course number for the same course could be different. This does not affect this program. <br>
Also, it is ok that only part of the course will be held on the specific weeks and the other part on every week. <br>


Thousands of such lines make up the course list: <br>

```csv
...
46;"05c";"Frk";"Eng";"7.06";1;6;;
46;"05c";"Frk";"Eng";"7.06";2;8;;
46;"05c";"Frk";"Eng";"7.06";3;6;;
28;"08a";"Gar";"Deu";"5.01";1;1;;
28;"08a";"Gar";"Deu";"5.01";1;2;;
28;"08b";"Gar";"Deu";"5.02";2;3;;
28;"08b";"Gar";"Deu";"5.02";3;8;;
28;"08b";"Gar";"Deu";"5.02";3;9;;
...
```

___

<a name="FormatNoteBoard"></a>
### Format of the note board

The note board should be generated by the program itself. The format will be set automatically and should not be changed. <br>

The first column shows the weekday, the second the date, the third, fifth, and seventh the note inputted by the user and the fourth, sixth, and eighth the grade for the note on its left side. <br>


<a name="Collaboration"></a>
## Info for Collaboration

To make it possible for us to work on this project together, we have set a few guidelines below. 


<a name="CodeStyle"></a>
### Code style / Naming conventions

All of the variables should be named with **English** words. <br>
When you are using the abbreviation, make sure the most people could understand what you mean by it. <br>

---

We use the style as following: <br>

|Kind|Rule|
|:---|:---|
|Private class member (C#)|_leadingUnderscore|
|Private class member (VBA)|PascalCase|
|Public class member|PascalCase|
|Local variable|camelCase|
|Parameter|camelCase|
|Class|PascalCase|
|Method|PascalCase|

---

The controls of a windows form should start with its prefix. <br>
For example: <br>

```
btnOK
```

---

The callback functions should start with the caller name, then with an underscore `_` and then the event. <br>
For example: <br>

```csharp
private void btnSearch2_Click(object sender, EventArgs e)
```

---

Also: <br>
Use static typing as much as possible. <br>

Use: <br>

```csharp
int myVariable = 0;
```

```VBA
Dim myVariable As Integer
myVariable = 0
```

Use less: <br>

```csharp
var myVariable = 0;
dynamic myVariable = 0;
```

```VBA
Dim myVariable As Variant
myVariable = 0

Dim mySecondVariable
mySecondVariable = ""
```

---

Use Indentation of 4 spaces for `loop`, `condition` and `class or method` sentences: <br>

```csharp
public void ExampleMethod(int exampleParameter)
{
    int exampleLocalVariable = 0;
	
	if (exampleLocalVariable > exampleParameter) 
	{
	    return exampleLocalVariable;
	}
	else
	{
	    return exampleParameter;
	}
}
```

Or: <br>

```VBA
Sub TestProcedure(testParameter As Integer)

Dim testVariable As Integer
testVariable = 0

If testVariable > testParameter Then
    Debug.Print testVariable
Else
    Debug.Print testParameter
End If

End Sub
```

---

#### In addition, for VBA:

Force to use an explicit declaration for every variables by adding the option at the top of the file: <br>

```VBA
Option Explicit
```

___

<a name="Deployment"></a>
## Deployment

To deploy this Excel Addin on a live system, you need to decide at first, which version you want to use.  <br>

You can find the install files at [**"Release"**](https://github.com/ChuangSheep/KursheftTools/releases). <br>

It is **highly recommend to use the VSTO version** if possible since it is **much quicker, safer and more user-friendly**. <br>

___

<a name="DeploymentCSharp"></a>
### For C#

For the VSTO Addin written with C#, you need the *administrator privilege* of the system to allow you to install the Addin for Excel. <br>

**We recommend you to use the EXE installer**, which has ***already bundles all of the dependencies (Internet connection required).*** Therefore, you will not have to install them manually. Double click the Exe file, read and agree with the agreement, and you are done!<br>

If you choose to use the `.vsto` or the `.msi` installer, you have to install the *Visual Studio Tools for Office runtime redistributable* manually. Also, .Net 4.5 or newer version is required. These should be installed at first before you install the VSTO Addin. <br>
Here is [a Microsoft documentation explaining how to install these runtime libraries on your computer](https://docs.microsoft.com/en-us/visualstudio/vsto/how-to-install-the-visual-studio-tools-for-office-runtime-redistributable). <br>

If you choose to compile the source code yourself, you have to install **Microsoft Visual Studio 2017 or newer** version. You can find [a detailed instruction here](#StartCS). 

___

<a name="DeploymentVBA"></a>
### For VBA

For the VBA Addin written with Visual Basic for Application, you only need the permission for running macros in your excel. This is useful if you are in a **restricted environment**. <br>

Double click the downloaded `KursheftTools.xlam`, and the Addin will be loaded into your system. <br>

Or, if you have downloaded the source code, import them to your Visual Basic editor and run the method you want.  <br>
*If you are using the source code, the method* `ribbonUI` *will not be used since it is for the user interface*. <br>

You can find [a detailed instruction by Microsoft of how to run a macro](https://support.office.com/en-us/article/run-a-macro-5e855fd2-02d1-45f5-90a3-50e645fe3155) here. <br>

___

<a name="BuiltWith"></a>
## Built With

### VSTO Addin:

* [PDFsharp](https://github.com/empira/PDFsharp) - A .Net library for processing PDF

---

### VBA Addin:

* None

___

<a name="License"></a>
## License

This project is licensed under the [*Apache 2.0 License*](https://www.apache.org/licenses/LICENSE-2.0). <br>
See the [LICENSE](LICENSE) file for details. <br>

Apache 2.0 <br>
Copyright (c) 2020 Chuyang W. <br>

