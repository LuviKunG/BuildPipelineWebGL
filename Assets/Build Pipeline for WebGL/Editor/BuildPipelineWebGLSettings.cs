using System;
using UnityEngine;
using UnityEditor;
using System.Text;

namespace LuviKunG.BuildPipeline.WebGL
{
    public sealed class BuildPipelineWebGLSettings
    {
        private const string ALIAS = "unity.editor.luvikung.buildpipeline.webgl.";
        private static readonly string PREFS_SETTINGS_BUILD_PATH = ALIAS + "buildpath";
        private static readonly string PREFS_SETTINGS_NAME_FORMAT = ALIAS + "nameformat";
        private static readonly string PREFS_SETTINGS_DATE_TIME_FORMAT = ALIAS + "datetimeformat";
        private static readonly string PREFS_SETTINGS_STRIP_MOBILE_WARNING = ALIAS + "stripMobileWarning";
        private static readonly string PREFS_SETTINGS_CREATE_NEW_FOLDER = ALIAS + "createNewFolder";
        private static readonly string PREFS_SETTINGS_BUILD_OPTIONS = ALIAS + "buildOptions";

        public string buildPath;
        public string nameFormat;
        public string dateTimeFormat;
        public bool stripMobileWarning;
        public bool createNewFolder;
        public BuildOptions buildOptions;

        private static BuildPipelineWebGLSettings instance;
        public static BuildPipelineWebGLSettings Instance
        {
            get
            {
                if (instance == null)
                    instance = new BuildPipelineWebGLSettings();
                return instance;
            }
        }

        public BuildPipelineWebGLSettings()
        {
            Load();
        }
        public void Load()
        {
            // Define 'BUILD_PIPELINE_WEBGL_UNITY_DEFAULT' if you want to set a build location as same as default of Unity Editor
            // But it's buggy because the native doesn't generate the new folder to put a build.
#if BUILD_PIPELINE_WEBGL_UNITY_DEFAULT
            buildPath = PlayerPrefs.GetString(PREFS_SETTINGS_BUILD_PATH, EditorUserBuildSettings.GetBuildLocation(BuildTarget.WebGL));
#else
            buildPath = PlayerPrefs.GetString(PREFS_SETTINGS_BUILD_PATH, string.Empty);
#endif
            nameFormat = PlayerPrefs.GetString(PREFS_SETTINGS_NAME_FORMAT, "{package}_{date}");
            dateTimeFormat = PlayerPrefs.GetString(PREFS_SETTINGS_DATE_TIME_FORMAT, "yyyyMMddHHmmss");
            stripMobileWarning = PlayerPrefs.GetString(PREFS_SETTINGS_STRIP_MOBILE_WARNING, bool.FalseString) == bool.TrueString;
            createNewFolder = PlayerPrefs.GetString(PREFS_SETTINGS_CREATE_NEW_FOLDER, bool.TrueString) == bool.TrueString;
            buildOptions = (BuildOptions)PlayerPrefs.GetInt(PREFS_SETTINGS_BUILD_OPTIONS, 0);
        }

        public void Save()
        {
            PlayerPrefs.SetString(PREFS_SETTINGS_BUILD_PATH, buildPath);
            PlayerPrefs.SetString(PREFS_SETTINGS_NAME_FORMAT, nameFormat);
            PlayerPrefs.SetString(PREFS_SETTINGS_DATE_TIME_FORMAT, dateTimeFormat);
            PlayerPrefs.SetString(PREFS_SETTINGS_STRIP_MOBILE_WARNING, stripMobileWarning ? bool.TrueString : bool.FalseString);
            PlayerPrefs.SetString(PREFS_SETTINGS_CREATE_NEW_FOLDER, createNewFolder ? bool.TrueString : bool.FalseString);
            PlayerPrefs.SetInt(PREFS_SETTINGS_BUILD_OPTIONS, (int)buildOptions);
        }

        public string GetFolderName()
        {
            StringBuilder s = new StringBuilder(nameFormat);
            s.Replace("{name}", Application.productName);
            s.Replace("{package}", PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.WebGL));
            s.Replace("{version}", Application.version);
            s.Replace("{bundle}", PlayerSettings.Android.bundleVersionCode.ToString());
            s.Replace("{date}", DateTime.Now.ToString(dateTimeFormat));
            return s.ToString();
        }
    }
}