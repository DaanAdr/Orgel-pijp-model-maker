# TODO List
- Get the horizontal spacing between object correct for nest CAD files.
- Allow the application to work for pipe corpus.

<br>

# Application architecture
This application doesn't follow the MVVM architecture that us usually considered best practise for WPF applications. It instead borrows ideas from Multitier architecture. 

- Presentation Layer. The presentation layer for this application is the folder named: Views. This folder holds the UserControls that serve as the pages of the application. These UserControls should hold very little logic, only what is necessary to render the view.
- Business/Logic layer. The logic layer for this application is the folder named: Logic. This is where the bulk of code that provides functionality to the application resides. Though it should be noted that there is some limited functionality in the models this application uses.
- Entities. There is a folder named: Entities. This folder holds the models the application uses. These are not view- or datamodels however. These are objects that make it easier to read Excel files or function as a CAD model in code. Because these models aren't intended to render a view, or communicate with a database, there is some logic in these models. The logic in these models pretains exclusively to the object itself.

<br>

# NuGet Packages
This application uses the following NuGet packages to function:
- [ACadSharp](https://www.nuget.org/packages/ACadSharp/) for creating DXF files.
- [CSVHelper](https://www.nuget.org/packages/CsvHelper) for reading CSV files.