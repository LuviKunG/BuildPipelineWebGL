﻿using System;
using UnityEngine;
using UnityEditor;
using System.Text;

namespace LuviKunG.BuildPipeline
{
    public sealed class BuildPipelineWebGLSettings
    {
        private const string ALIAS = "unity.editor.luvikung.buildpipeline.webgl.";
        private static readonly string PREFS_SETTINGS_BUILD_PATH = ALIAS + "buildpath";
        private static readonly string PREFS_SETTINGS_NAME_FORMAT = ALIAS + "nameformat";
        private static readonly string PREFS_SETTINGS_DATE_TIME_FORMAT = ALIAS + "datetimeformat";
        private static readonly string PREFS_SETTINGS_STRIP_MOBILE_WARNING = ALIAS + "stripMobileWarning";
        private static readonly string PREFS_SETTINGS_FIX_MACOS_VERSION_REGEX = ALIAS + "fixMacOSVersionRegex";
        private static readonly string PREFS_SETTINGS_CREATE_NEW_FOLDER = ALIAS + "createNewFolder";
        private static readonly string PREFS_SETTINGS_BUILD_OPTIONS = ALIAS + "buildOptions";
        private static readonly string PREFS_SETTINGS_MEMORY_SIZE = ALIAS + "memorySize";
        private static readonly string PREFS_SETTINGS_COMPRESSION_FORMAT = ALIAS + "compressionFormat";
        private static readonly string PREFS_SETTINGS_WASM_STREAMING = ALIAS + "wasmStreaming";
        private static readonly string PREFS_SETTINGS_LINKER_TARGET = ALIAS + "linkerTarget";

        public string buildPath;
        public string nameFormat;
        public string dateTimeFormat;
        public bool stripMobileWarning;
        public bool fixMacOSVersionRegex;
        public bool createNewFolder;
        public WebGLLinkerTarget linkerTarget;
        public int memorySize;
        public WebGLCompressionFormat compressionFormat;
        public bool wasmStreaming;
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
            fixMacOSVersionRegex = PlayerPrefs.GetString(PREFS_SETTINGS_FIX_MACOS_VERSION_REGEX, bool.FalseString) == bool.TrueString;
            createNewFolder = PlayerPrefs.GetString(PREFS_SETTINGS_CREATE_NEW_FOLDER, bool.TrueString) == bool.TrueString;
            linkerTarget = (WebGLLinkerTarget)PlayerPrefs.GetInt(PREFS_SETTINGS_LINKER_TARGET, (int)PlayerSettings.WebGL.linkerTarget);
            memorySize = PlayerPrefs.GetInt(PREFS_SETTINGS_MEMORY_SIZE, PlayerSettings.WebGL.memorySize);
            compressionFormat = (WebGLCompressionFormat)PlayerPrefs.GetInt(PREFS_SETTINGS_COMPRESSION_FORMAT, (int)PlayerSettings.WebGL.compressionFormat);
            wasmStreaming = PlayerPrefs.GetString(PREFS_SETTINGS_WASM_STREAMING, PlayerSettings.WebGL.wasmStreaming ? bool.TrueString : bool.FalseString) == bool.TrueString;
            buildOptions = (BuildOptions)PlayerPrefs.GetInt(PREFS_SETTINGS_BUILD_OPTIONS, 0);
        }

        public void Save()
        {
            PlayerPrefs.SetString(PREFS_SETTINGS_BUILD_PATH, buildPath);
            PlayerPrefs.SetString(PREFS_SETTINGS_NAME_FORMAT, nameFormat);
            PlayerPrefs.SetString(PREFS_SETTINGS_DATE_TIME_FORMAT, dateTimeFormat);
            PlayerPrefs.SetString(PREFS_SETTINGS_STRIP_MOBILE_WARNING, stripMobileWarning ? bool.TrueString : bool.FalseString);
            PlayerPrefs.SetString(PREFS_SETTINGS_FIX_MACOS_VERSION_REGEX, fixMacOSVersionRegex ? bool.TrueString : bool.FalseString);
            PlayerPrefs.SetString(PREFS_SETTINGS_CREATE_NEW_FOLDER, createNewFolder ? bool.TrueString : bool.FalseString);
            PlayerPrefs.SetInt(PREFS_SETTINGS_LINKER_TARGET, (int)linkerTarget);
            PlayerPrefs.SetInt(PREFS_SETTINGS_MEMORY_SIZE, memorySize);
            PlayerPrefs.SetInt(PREFS_SETTINGS_COMPRESSION_FORMAT, (int)compressionFormat);
            PlayerPrefs.SetString(PREFS_SETTINGS_WASM_STREAMING, wasmStreaming ? bool.TrueString : bool.FalseString);
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