using UnityEngine;
using UnityEditor;
using System.IO;

public static class BuildScript
{
    public static void BuildWindows()
    {
        string[] scenes = { "Assets/Scenes/Main.unity" };
        string buildPath = Path.Combine(Application.dataPath, "../Builds/Windows");
        Directory.CreateDirectory(buildPath);

        BuildPipeline.BuildPlayer(scenes, Path.Combine(buildPath, "Game.exe"), BuildTarget.StandaloneWindows64, BuildOptions.None);
        Debug.Log("Build completado en " + buildPath);
    }
}