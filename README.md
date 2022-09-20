# Kursheft Tools

*A newer version as webapp is avaliable. For detailed information see branch [web/main](https://github.com/ChuangSheep/KursheftTools/tree/web/main). A demo is avaliable [here](https://kht.chuangsheep.com/).*

---

An Excel Addin for school to quickly create its own course plans. 

## Table of Contents

* [Getting Started](#GettingStarted) 
	* [For c#](#StartCS) 
	
* [Technologies](#Technologies)

* [Examples of use](#ExamplesOfUse) 
	* [How to create a new note board](#CreateNewNoteboard)
	* [How to import the course list](#ImportCourseList)
	* [How to export the course plans](#ExportCoursePlan)

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

The Microsoft Office developer tools will be installed by default. If you already have Visual Studio on your computer but don't have the Office developer tools, make sure that **Microsoft Office Developer Tools** is selected during setup. If not, open **Visual Studio Installer**, click **Modify**, and select the functionality called *Office/Sharepoint development*. <br>
Even though the *Visual Studio Tools for Office runtime redistributable* will normally be installed while the *Office Developer Tools* being installed, however, if you don't have it on your local machine, have a look of [this Microsoft documentation about how to enable it](https://docs.microsoft.com/en-us/visualstudio/vsto/how-to-install-the-visual-studio-tools-for-office-runtime-redistributable?view=vs-2019). <br>

Also, you need to have **Microsoft Office 2016, Office 365 (Microsoft 365), or a newer** version installed on your local machine. <br>

#### Installing

As the next step, download the whole folder in the root called `KursheftTools-CSharp` or the newest release, where you are going to download the source code of csharp as a .zip file. <br>

**Open the solution file** (*KursheftTools.sln*) using Visual Studio.

After everything is loaded and ready to use, **click Start** with the green triangle on the top of your Visual Studio IDE. Make sure that your configuration is set to `Debug`. <br>

Your Excel will be opened, open or create a random workbook, you will find a tab called **Kursheft Tools** after the tab **help**. <br>

**Congratulations!** The Addin is successfully installed and ready to use. <br>


<a name="Technologies"></a>
## Technologies

For C# VSTO Addin: 

* C# 5.0
* .Net Framework 4.5
* VSTO Runtime


<a name="ExamplesOfUse"></a>
## Examples of use


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
### Import the course list

To import the course list from a `.csv` or `.txt` file, click the button called **Kursliste importieren**. A dialog window will be opened and asks you to choose the `.csv`/`.txt` file. <br>

After excel has successfully imported the course list, a message box will show up that the import was completed. 

___

<a name="ExportCoursePlan"></a>
### Export the course plans (Kursheft)

Before exporting the course plans, please go to the note board worksheet which you want to use for the export. <br>

To export the course plans as pdf, click the button called **Kurshefte generieren** at tab **Kursheft Tools**. <br>

A dialog window will show up. You will be asked to input the path to store the exported course plans, the grades need to be exported. <br>

___


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

**Note that the csv file itself does NOT contain any title line.** <br>
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

___

<a name="FormatNoteBoard"></a>
### Format of the note board

The note board should be generated by the program itself. The format will be set automatically and should not be changed. <br>

The first column shows the weekday, the second the date, the third, fifth, and seventh the note inputted by the user and the fourth, sixth, and eighth the grade for the note on its left side. <br>

___

<a name="Deployment"></a>
## Deployment

To deploy this Excel Addin on a live system, you need to decide at first, which version you want to use.  <br>

You can find the installation files at [**"Release"**](https://github.com/ChuangSheep/KursheftTools/releases). <br>

For the VSTO Addin written with C#, you need the *administrator privilege* of the system to allow you to install the Addin for Excel. <br>

**We recommend you to use the EXE installer**, which has ***already bundles all of the dependencies (Internet connection required).*** Therefore, you will not have to install them manually. Double click the Exe file, read and agree with the agreement, and you are done!<br>

If you choose to use the `.vsto` or the `.msi` installer, you have to install the *Visual Studio Tools for Office runtime redistributable* manually. Also, .Net 4.5 or newer version is required. These should be installed at first before you install the VSTO Addin. <br>
Here is [a Microsoft documentation explaining how to install these runtime libraries on your computer](https://docs.microsoft.com/en-us/visualstudio/vsto/how-to-install-the-visual-studio-tools-for-office-runtime-redistributable). <br>

If you choose to compile the source code yourself, you have to install **Microsoft Visual Studio 2017 or newer** version. You can find [a detailed instruction here](#StartCS). 

___

<a name="BuiltWith"></a>
## Built With

* [PDFsharp](https://github.com/empira/PDFsharp) - A .Net library for processing PDF

___

<a name="License"></a>
## License

This project is licensed under the [*Apache 2.0 License*](https://www.apache.org/licenses/LICENSE-2.0). <br>
See the [LICENSE](LICENSE) file for details. <br>

Apache 2.0 <br>
Copyright (c) 2020 Chuyang W. <br>

