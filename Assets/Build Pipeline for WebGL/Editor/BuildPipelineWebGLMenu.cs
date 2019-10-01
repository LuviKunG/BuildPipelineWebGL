﻿using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace LuviKunG.BuildPipeline.WebGL
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
            string buildPath = Path.Combine(path, settings.GetFolderName());
            BuildReport report = UnityEditor.BuildPipeline.BuildPlayer(scenes.ToArray(), buildPath, BuildTarget.WebGL, BuildOptions.None);
            BuildSummary summary = report.summary;
            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"Build succeeded at '{buildPath}' using {summary.totalTime.TotalSeconds.ToString("N2")} seconds with size of {summary.totalSize} bytes.");
                Application.OpenURL(buildPath);
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