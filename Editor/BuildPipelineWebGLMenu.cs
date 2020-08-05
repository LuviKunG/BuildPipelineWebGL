using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace LuviKunG.BuildPipeline
{
    public static class BuildPipelineWebGLMenu
    {
        private static BuildPipelineWebGLSettings settings;

        [MenuItem("Build/WebGL", false, 0)]
        public static void Build()
        {
            if (UnityEditor.BuildPipeline.isBuildingPlayer)
                return;
            settings = BuildPipelineWebGLSettings.Instance;
            string path;
            if (string.IsNullOrEmpty(settings.buildPath))
                path = OpenBuildSavePanel(settings.buildPath);
            else
                path = settings.buildPath;
            var scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
            for (int i = 0; i < scenes.Count; i++)
                if (!scenes[i].enabled)
                    scenes.RemoveAt(i--);
            if (!(scenes.Count > 0))
                return;
            string buildPath;
            if (settings.createNewFolder)
                buildPath = Path.Combine(path, settings.GetFolderName());
            else
                buildPath = path;
            PlayerSettings.WebGL.linkerTarget = settings.linkerTarget;
            PlayerSettings.WebGL.memorySize = settings.memorySize;
            PlayerSettings.WebGL.compressionFormat = settings.compressionFormat;
            PlayerSettings.WebGL.wasmStreaming = settings.wasmStreaming;
            BuildReport report = UnityEditor.BuildPipeline.BuildPlayer(scenes.ToArray(), buildPath, BuildTarget.WebGL, settings.buildOptions);
            BuildSummary summary = report.summary;
            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"Build succeeded at '{buildPath}' using {summary.totalTime.TotalSeconds.ToString("N2")} seconds with size of {summary.totalSize} bytes.");
#if UNITY_EDITOR_OSX
                Application.OpenURL("file:/" + buildPath);
#else
                Application.OpenURL(buildPath);
#endif
            }
            if (summary.result == BuildResult.Failed)
            {
                Debug.LogError($"Build failed...");
            }
        }

        [MenuItem("Build/Settings/WebGL", false, 0)]
        public static void OpenBuildSetting()
        {
            _ = BuildPipelineWebGLWindow.OpenWindow();
        }

        private static string OpenBuildSavePanel(string path)
        {
            string newPath = EditorUtility.SaveFolderPanel("Choose location to build a game", path, null);
            if (string.IsNullOrEmpty(newPath))
                return null;
            settings.buildPath = newPath;
            settings.Save();
            return newPath;
        }
    }
}