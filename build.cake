#tool nuget:?package=vswhere
#tool nuget:?package=NUnit.Runners&version=2.6.4

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var solution = "./BeatTheNotes.sln";

var vsLatest  = VSWhereLatest();
var msBuildPath = vsLatest?.CombineWithFilePath("./MSBuild/15.0/Bin/amd64/MSBuild.exe");

//TaskSetup(context => Information($"'{context.Task.Name}'"));
//TaskTeardown(context => Information($"'{context.Task.Name}'"));

Task("Restore")
    .Does(() =>
{
    DotNetCoreRestore(solution);
});

Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
{
    var buildSettings = new DotNetCoreBuildSettings 
    { 
        Configuration = configuration,
        ArgumentCustomization = args => args.Append($"/p:Version={gitVersion.AssemblySemVer}")
    };
    DotNetCoreBuild(solution, buildSettings);
});

Task("Pack")
    .IsDependentOn("Build")
    .Does(() =>
{ 
    var artifactsDirectory = "./artifacts";

    CreateDirectory(artifactsDirectory);
    CleanDirectory(artifactsDirectory);    

    foreach (var project in GetFiles($"./BeatTheNotes*/*.csproj"))
    {
        DotNetCorePack(project.FullPath, new DotNetCorePackSettings 
        {
            Configuration = configuration,
            IncludeSymbols = true,
            OutputDirectory = artifactsDirectory
        });
    }
});

Task("Default")
    .IsDependentOn("Pack");

RunTarget(target);
