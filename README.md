# .NET Core CLI Project Versioning Tool
This is a small dotnet CLI tool to help with embedding the revision number into your project's version number. It currently supports hg and git and generates a C# VersionInfo.cs file in your project's Properties folder.

Note: This tool depends on the presence of hg / git binaries in the system path.

## Usage
1. In your solution directory run ```dotnet tool install --local ProjectVersioning.DotNet.Cli```
2. In your solution folder, create a new file: ```Version.xml``` (you can use any name).
3. Use this snippet as a template:

```xml
<Project>
	<!-- Project version properties (follows semantic versioning 2.0.0 rules) -->
	<PropertyGroup>
		<VersionPrefix>1.0.0</VersionPrefix>
		<VersionSuffix>rc</VersionSuffix>
		<Company>Raif Atef Wasef</Company>
		<Copyright>Copyright © Raif Atef Wasef 2020</Copyright>
		<Product>MyProduct</Product>
	</PropertyGroup>

	<!-- Suppress default attributes created by the compiler to prevent duplicate attribute errors -->
	<PropertyGroup>
		<GenerateAssemblyCompanyAttribute>false</GenerateAssemblyCompanyAttribute>
		<GenerateAssemblyCopyrightAttribute>false</GenerateAssemblyCopyrightAttribute>
		<GenerateAssemblyProductAttribute>false</GenerateAssemblyProductAttribute>
		<GenerateAssemblyVersionAttribute>false</GenerateAssemblyVersionAttribute>
		<GenerateAssemblyFileVersionAttribute>false</GenerateAssemblyFileVersionAttribute>
		<GenerateAssemblyInformationalVersionAttribute>false</GenerateAssemblyInformationalVersionAttribute>
	</PropertyGroup>

	<Target Name="GenerateVersionInfo" BeforeTargets="CoreCompile">
		<Exec Command="dotnet tool restore" />
		<Exec Command="dotnet tool run rw-project-version -s=git -t=cs -v=$(VersionPrefix) -m=$(VersionSuffix)" />

		<ItemGroup>
			<Compile Remove="Properties\VersionInfo.cs" />
			<Compile Include="Properties\VersionInfo.cs" />
		</ItemGroup>
	</Target>
</Project>
```

4. In each of your csproj files, add the following just before the closing ```</Project>``` tag:
```xml
	<!-- Adjust the path to Version.xml relative to the csproj file -->
	<Import Project="..\Version.xml" />
```

Run ```dotnet tool run rw-project-version``` for detailed command arguments.

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
