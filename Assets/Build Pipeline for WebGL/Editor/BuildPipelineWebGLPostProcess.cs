using System;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Text;

namespace LuviKunG.BuildPipeline
{
    public static class BuildPipelineWebGLPostProcess
    {
        private static BuildPipelineWebGLSettings settings;

        [PostProcessBuild(0)]
        public static void OnPostProcessBuild(BuildTarget target, string targetPath)
        {
            if (target == BuildTarget.WebGL)
            {
                settings = BuildPipelineWebGLSettings.Instance;
                if (settings.stripMobileWarning)
                {
                    try
                    {
                        var path = Path.Combine(targetPath, "Build/UnityLoader.js");
                        var sb = new StringBuilder(File.ReadAllText(path));
                        sb = sb.Replace("UnityLoader.SystemInfo.mobile", "false");
                        File.WriteAllText(path, sb.ToString());
                    }
                    catch (Exception e)
                    {
                        UnityEngine.Debug.LogException(e);
                    }
                }
                if (settings.fixMacOSVersionRegex)
                {
                    try
                    {
                        var path = Path.Combine(targetPath, "Build/UnityLoader.js");
                        var sb = new StringBuilder(File.ReadAllText(path));
                        sb = sb.Replace("Mac OS X (10[\\.\\_\\d]+)", "Mac OS X (1[0-9][\\.\\_\\d]+)");
                        File.WriteAllText(path, sb.ToString());
                    }
                    catch (Exception e)
                    {
                        UnityEngine.Debug.LogException(e);
                    }
                }
            }
        }
    }
}