using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Publish;
using Cake.Common.Tools.DotNetCore.Restore;
using Cake.Core;
using Cake.Frosting;
using Cake.Npm;
using Cake.Npm.Install;
using Cake.Npm.RunScript;
using DirectoryPath = Cake.Core.IO.DirectoryPath;

new CakeHost()
    .UseContext<BuildContext>()
    .Run(args);

public class BuildContext : FrostingContext
{
    public BuildContext(ICakeContext context) : base(context)
    {
    }
}

[TaskName("clean-output")]
public class CleanOutputTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        if (Directory.Exists(WorkingDirectories.Output)) Directory.Delete(WorkingDirectories.Output, true);
        Directory.CreateDirectory(WorkingDirectories.Output);
        Directory.CreateDirectory(WorkingDirectories.ApiOutput);
    }
}

[TaskName("default")]
[IsDependentOn(typeof(CleanOutputTask))]
[IsDependentOn(typeof(DevOps.Frontend.FrontendPublishTask))]
[IsDependentOn(typeof(DevOps.Backend.BackendPublishTask))]
public class BuildTask : FrostingTask<BuildContext> { }

namespace DevOps.Frontend
{
    [TaskName("npm-clean")]
    public class NpmCleanTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            var node_modules = Path.Combine(WorkingDirectories.Angular, "node_modules");
            if (Directory.Exists(node_modules))
            {
                Console.WriteLine("deleting 'node_modules'...");
                Directory.Delete(node_modules, true);
            }

            var packageLock = Path.Combine(WorkingDirectories.Angular, "package-lock.json");
            if (File.Exists(packageLock))
            {
                Console.WriteLine("deleting 'package-lock.json'...");
                File.Delete(packageLock);
            }
        }
    }

    [TaskName("npm-install")]
    public class NpmInstallTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            var install_settings = new NpmInstallSettings()
            {
                LogLevel = NpmLogLevel.Silent,
                WorkingDirectory = new DirectoryPath(WorkingDirectories.Angular)
            };
            context.NpmInstall(install_settings);
        }
    }

    [TaskName("npm-deploy")]
    public class NpmDeployTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            var build_settings = new NpmRunScriptSettings()
            {
                ScriptName = "deploy",
                LogLevel = NpmLogLevel.Silent,
                WorkingDirectory = new DirectoryPath(WorkingDirectories.Angular)
            };
            context.NpmRunScript(build_settings);
        }
    }

    [TaskName("frontend-publish")]
    [IsDependentOn(typeof(NpmCleanTask))]
    [IsDependentOn(typeof(NpmInstallTask))]
    [IsDependentOn(typeof(NpmDeployTask))]
    public class FrontendPublishTask : FrostingTask<BuildContext>
    {
    }
}

namespace DevOps.Backend
{
    [TaskName("dotnet-restore")]
    public class DotNetRestoreTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            var restore_settings = new DotNetCoreRestoreSettings()
            {
                Verbosity = DotNetCoreVerbosity.Quiet,
                Runtime = "win-x64",
            };
            context.DotNetCoreRestore(WorkingDirectories.WorkingDir, restore_settings);
        }
    }

    [TaskName("dotnet-publish")]
    public class DotNetPublisTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            var publish_settings = new DotNetCorePublishSettings()
            {
                Verbosity = DotNetCoreVerbosity.Quiet,
                Configuration = "release",
                Framework = "netcoreapp2.2",
                Runtime = "win-x64",
                SelfContained = true,
                OutputDirectory = new DirectoryPath(WorkingDirectories.ApiOutput)
            };
            context.DotNetCorePublish(WorkingDirectories.DotNetWebApi, publish_settings);
            base.Run(context);
        }

        public override void Finally(BuildContext context)
        {
            foreach (var fileName in Directory.EnumerateFiles(WorkingDirectories.ApiOutput, "*.pdb"))
            {
                File.Delete(fileName);
            }
            var appsettings = Path.Combine("", "appsettings.json");
            if (File.Exists(appsettings))
            {
                File.Delete(appsettings);
            }
            base.Finally(context);
        }
    }

    [TaskName("backend-publish")]
    [IsDependentOn(typeof(DotNetRestoreTask))]
    [IsDependentOn(typeof(DotNetPublisTask))]
    public class BackendPublishTask : FrostingTask<BuildContext>
    {
    }
}

static class WorkingDirectories
{
    public static string WorkingDir => Path.Combine(Directory.GetCurrentDirectory(), "..");
    public static string Angular => Path.Combine(WorkingDir, "Frontend");
    public static string DotNetWebApi => Path.Combine(WorkingDir, "Backend", "Perlink.Oi.Juridico.WebApi");
    public static string Output => Path.Combine(WorkingDir, "Dist");
    public static string ApiOutput => Path.Combine(Output, "api");
}
