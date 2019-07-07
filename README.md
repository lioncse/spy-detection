# spy detection



A spy might be hiding in any given message and we need a way to find them.
A message is formed of an array of integers, and the spy’s code name must appear, in that
order, for the spy to present.

# Example

|Message | Code Name | Spy | Contains spy? |
-------- | --------- | --- | ------------- |
| [1,2,4,0,0,7,5] | [0,0,7] | James Bond | true |
| [0,2,2,0,4,7,0] | [0,0,7] | James Bond | true |
| [1,2,0,7,4,4,0] | [0,0,7] | James Bond | false |
| [3,6,0,1,2,6,4] | [3,1,4] | Ethan Hunt | true |
| [3,3,1,5,1,4,4] | [3,1,4] | Ethan Hunt | true |
| [4,1,3,8,4,3,1] | [3,1,4] | Ethan Hunt | false |

# How to install and run

1. install latest ms sql express server. [download link](https://www.microsoft.com/en-us/download/details.aspx?id=55994)
2. install .net core 2.2. [download link](https://dotnet.microsoft.com/download)
3. clone this repository, in a command-prompt terminal run "git clone &lt;repository url&gt;".
4. goto "spy-detection\spy-detection" folder from the repository root folder.
5. open "appsettings.json" inside this folder in your favourite text editor. it's a plain JSON file.
6. under "ConnectionStrings" section in that file there is property "DefaultConnection", updates it's value with a valid SQL sevrer connection string. it should look like *"Server=(local);Database=spy-detection;User ID=&lt;user&gt;;Password=&lt;password&gt;;MultipleActiveResultSets=true"*. replace *&lt;user&gt;* and *&lt;password&gt;* with your ones.
4. in the terminal navigate to "spy-detection\spy-detection" folder from the repository root.
5. in the terminal run "dotnet build".
6. then run "dotnet ef database update".
7. finally run "dotnet run". it will start the application at http://localhost:5000. now browse it in your browser.
  

# How to run unit test
1. in a terminal goto "spy-detection\spy-detection.tests" from your repository root folder.
2. now run command "dotnet build"
3. then run "dotnet test". it will run the tests show you the result in the console.
