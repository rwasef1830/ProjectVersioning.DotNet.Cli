# .NET Core CLI Project Versioning Tool
This is a small dotnet CLI tool to help with embedding the revision number into your project's version number. It currently supports hg and git and generates a C# VersionInfo.cs file in your project's Properties folder.

Note: This tool depends on the presence of hg / git binaries in the system path.

## Usage
Add the nuget reference to the ```tools``` section of your ```project.json``` file:
```
"tools": {
    "ProjectVersioning.DotNet.Cli": "2.0.0"
}
```

Add a script to the ```precompile``` section of your ```project.json``` (assuming hg project and generating c# ```VersionInfo.cs``` file, taking the version from the project.json version field):
```
"scripts": {
    "precompile": [
        "dotnet project-version -s=hg -t=cs -v=%project:Version% -m=modifier",
    ]
}
```

Run ```dotnet project-version``` for detailed command arguments.

## Version handling for C# projects
The major and minor parts of the numeric version are taken from the passed version number. The build and revision fields will be the high 16-bit and low l6-bit parts of the local 32-bit repository revision number. In case the build is dirty, the highest bit of the high portion will be set.

The full version number, revision hash and version marker and will be emitted in the assembly informational version attribute for C# projects. If the tree is dirty, "-dirty" will be appended to the version string.

### Example scenario
- project.json version set to ```1.2.3.4```
- Local revision hash is ```abcdefabcdef```, local revision number is ```131071```.
- Marker is ```alpha```.

If the working copy is clean: 
- Numerical version: ```1.2.1.65535``` 
- Informational version: ```1.2.3.131071-alpha+abcdefabcdef```

If the working copy is dirty:
- Numerical version: ```1.2.32769.65535``` 
- Informational version: ```1.2.3.131071-alpha+abcdefabcdef-dirty```

### Bug reports and feature suggestions welcome!