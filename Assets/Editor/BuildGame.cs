using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public static class BuildGame
{
    const string k_DefaultWindowsBuildPath = "Builds/Windows/MyFPS.exe";

    public static void BuildWindows64()
    {
        string buildPath = GetCommandLineValue("-customBuildPath", k_DefaultWindowsBuildPath);
        Directory.CreateDirectory(Path.GetDirectoryName(buildPath));

        BuildPlayerOptions buildOptions = new BuildPlayerOptions
        {
            scenes = GetEnabledScenes(),
            locationPathName = buildPath,
            target = BuildTarget.StandaloneWindows64,
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(buildOptions);
        BuildSummary summary = report.summary;

        if (summary.result != BuildResult.Succeeded)
            throw new BuildFailedException("Windows build failed with result: " + summary.result);

        UnityEngine.Debug.Log("Windows build created at: " + summary.outputPath);
    }

    static string[] GetEnabledScenes()
    {
        return System.Array.FindAll(EditorBuildSettings.scenes, scene => scene.enabled)
            .Select(scene => scene.path)
            .ToArray();
    }

    static string GetCommandLineValue(string key, string fallback)
    {
        string[] args = System.Environment.GetCommandLineArgs();

        for (int i = 0; i < args.Length - 1; i++)
        {
            if (args[i] == key)
                return args[i + 1];
        }

        return fallback;
    }
}
