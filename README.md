You will need to download the Newtonsoft .NET Json Package in Unity for these scripts to work:
1. Package Manager
  - Open Package Manager
  - Install Package by Name: `com.unity.nuget-newtonsoft-json`
or
2. manifest.json
  - Find in `whatever_path_to_unity_default_project_folders/UnityProjectFolder/Packages/manifest.json`
  - add under dependancies: `"com.unity.nuget.newtonsoft-json": "3.2.1",`

Used [this tool](https://json2csharp.com/) to create the classes needed to deserialize the json response to C# classes.
