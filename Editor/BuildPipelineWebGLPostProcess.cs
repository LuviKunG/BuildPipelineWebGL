using System;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;

namespace LuviKunG.BuildPipeline
{
    public static class BuildPipelineWebGLPostProcess
    {
        private static BuildPipelineWebGLSettings settings;

        [PostProcessBuild(0)]
        public static void OnPostProcessBuild(BuildTarget target, string targetPath)
        {
            settings = BuildPipelineWebGLSettings.Instance;
            if (settings.stripMobileWarning && target == BuildTarget.WebGL)
            {
                try
                {
                    var path = Path.Combine(targetPath, "Build/UnityLoader.js");
                    var text = File.ReadAllText(path);
                    text = text.Replace("UnityLoader.SystemInfo.mobile", "false");
                    File.WriteAllText(path, text);
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogException(e);
                }
            }
        }
    }
}