using UnityEditor;
using UnityEngine;

namespace LuviKunG.BuildPipeline.WebGL
{
    public sealed class BuildPipelineWebGLWindow : EditorWindow
    {
        private const string HELPBOX_NAME_FORMATTING_INFO = @"How to format the file name.

{name} = App Name.
{package} = Android Package Name.
{version} = App Version.
{bundle} = App Bundle.
{date} = Date time. (format)";

        private BuildPipelineWebGLSettings settings;

        public static BuildPipelineWebGLWindow OpenWindow()
        {
            var window = GetWindow<BuildPipelineWebGLWindow>(true, "WebGL Build Pipeline Setting", true);
            window.Show();
            return window;
        }

        private void OnEnable()
        {
            settings = BuildPipelineWebGLSettings.Instance;
        }

        private void OnGUI()
        {
            GUI.enabled = !UnityEditor.BuildPipeline.isBuildingPlayer;
            EditorGUILayout.LabelField("Folder name formatting", EditorStyles.boldLabel);
            using (var changeScope = new EditorGUI.ChangeCheckScope())
            {
                settings.nameFormat = EditorGUILayout.TextField(settings.nameFormat);
                if (changeScope.changed)
                    settings.Save();
            }
            EditorGUILayout.HelpBox(HELPBOX_NAME_FORMATTING_INFO, MessageType.Info, true);
            EditorGUILayout.LabelField("Formatted name", settings.GetFolderName(), EditorStyles.helpBox);
            using (var changeScope = new EditorGUI.ChangeCheckScope())
            {
                settings.dateTimeFormat = EditorGUILayout.TextField("Date time format", settings.dateTimeFormat);
                settings.stripMobileWarning = EditorGUILayout.Toggle("Strip Mobile Warning", settings.stripMobileWarning);
                settings.createNewFolder = EditorGUILayout.Toggle("Create New Folder", settings.createNewFolder);
                if (changeScope.changed)
                    settings.Save();
            }
            using (var verticalScope = new EditorGUILayout.VerticalScope())
            {
                using (var horizontalScope = new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("Build location", settings.buildPath);
                    if (GUILayout.Button("Change...", GUILayout.Width(96.0f)))
                    {
                        var path = OpenBuildSavePanel(settings.buildPath);
                        if (!string.IsNullOrEmpty(path))
                            settings.buildPath = path;
                    }
                }
                using (var horizontalScope = new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.FlexibleSpace();
                    bool cacheEnable = GUI.enabled;
                    GUI.enabled = !string.IsNullOrWhiteSpace(settings.buildPath);
                    if (GUILayout.Button("Open Build Location", GUILayout.MaxWidth(256.0f)))
                    {
                        Application.OpenURL(settings.buildPath);
                    }
                    GUI.enabled = cacheEnable;
                    GUILayout.FlexibleSpace();
                }
            }
        }

        private string OpenBuildSavePanel(string path)
        {
            string newPath = EditorUtility.SaveFolderPanel("Choose Location of Build Game", path, null);
            if (string.IsNullOrEmpty(newPath))
                return null;
            settings.buildPath = newPath;
            settings.Save();
            return newPath;
        }
    }
}