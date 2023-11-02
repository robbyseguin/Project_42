using System;
using System.Globalization;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    public class AutoSave : EditorWindow
    {
        private enum SaveMode
        {
            ON_PLAY = 0,
            ON_DIRTY,
            ON_TIME,
            //MAXIMUM // Commented because the enum is converted to an array of string for display
        }

        private static SaveMode _saveMode = SaveMode.ON_PLAY;
        private static bool _showOnStart = true;
        private static bool _isAutoSaving = false;
        private static bool _isFoldout = false;
        private static int _onTimeLenght = 60;
        private static DateTime _lastSave = DateTime.UnixEpoch;

        #region Initialisation method

        [MenuItem("Tools/Auto save/Options")]
        public static void ShowWindow()
        {
            // Credit for << System.Type.GetType("UnityEditor.InspectorWindow,UnityEditor.dll") >>
            // https://forum.unity.com/threads/add-inspector-to-custom-editorwindow.743858/

            EditorWindow.GetWindow<AutoSave>("Auto save options", true,
                new[] { System.Type.GetType("UnityEditor.InspectorWindow,UnityEditor.dll") });
            SessionState.SetBool("_firstOpening", false);
        }

        [InitializeOnLoadMethod]
        public static void Init()
        {
            LoadPrefs();

            if (_showOnStart && SessionState.GetBool("_firstOpening", true))
                EditorApplication.delayCall += ShowWindow;

            EditorApplication.wantsToQuit += OnApplicationQuitting;
            UpdateSaveMethod();
        }

        private static bool OnApplicationQuitting()
        {
            // Save pref before the application quit
            SavePrefs();
            return true;
        }

        #endregion

        private void OnGUI()
        {
            Rect mainGroup = new Rect(10, 10, position.width - 20, position.height);

            GUI.BeginGroup(mainGroup);
            {
                EditorGUI.BeginChangeCheck();
                {
                    _isAutoSaving = EditorGUILayout.BeginToggleGroup("Auto-save", _isAutoSaving);
                    {
                        OptionsGUILayout();
                    }
                    EditorGUILayout.EndToggleGroup();

                    EditorGUILayout.Space(15);

                    if (GUILayout.Button("Force save active scene"))
                        SaveCurrentScene();

                    _showOnStart = EditorGUILayout.Toggle("Show window on start", _showOnStart);
                }
                if (EditorGUI.EndChangeCheck())
                {
                    SavePrefs();
                    UpdateSaveMethod();
                }

                EditorGUILayout.Space(10);

                InfoGUILayout();
            }
            GUI.EndGroup();
        }
        
        #region Saving scene related method

        private static void UpdateSaveMethod()
        {
            // For some reason do not raise an error
            EditorApplication.playModeStateChanged -= SaveOnPlayMode;
            EditorSceneManager.sceneDirtied -= SaveOnDirtied;
            EditorApplication.update -= SaveOnTime;

            if (!_isAutoSaving)
                return;

            switch (_saveMode)
            {
                case SaveMode.ON_PLAY:
                    EditorApplication.playModeStateChanged += SaveOnPlayMode;
                    break;
                case SaveMode.ON_DIRTY:
                    if (SceneManager.GetActiveScene().isDirty) SaveCurrentScene();
                    EditorSceneManager.sceneDirtied += SaveOnDirtied;
                    break;
                case SaveMode.ON_TIME:
                    EditorApplication.update += SaveOnTime;
                    break;
            }
        }

        private static void SaveCurrentScene()
        {
            if (EditorApplication.isPlaying)
                return;

            _lastSave = DateTime.Now;
            SessionState.SetString("_lastSave", DateTime.Now.ToString(CultureInfo.CurrentCulture));
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        }

        private static void SaveOnPlayMode(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
                SaveCurrentScene();
        }

        private static void SaveOnDirtied(Scene s)
        {
            SaveCurrentScene();
        }

        private static void SaveOnTime()
        {
            if (_lastSave.AddSeconds(_onTimeLenght) < DateTime.Now)
                SaveCurrentScene();
        }

        #endregion

        #region Editor preference method

        private static void SavePrefs()
        {
            EditorPrefs.SetBool("_showOnStart", _showOnStart);
            EditorPrefs.SetBool("_isAutoSaving", _isAutoSaving);
            EditorPrefs.SetBool("_isFoldout", _isFoldout);

            EditorPrefs.SetInt("_saveMode", (int)_saveMode);
            EditorPrefs.SetInt("_onTimeLenght", _onTimeLenght);
        }

        private static void LoadPrefs()
        {
            _showOnStart = EditorPrefs.GetBool("_showOnStart", _showOnStart);
            _isAutoSaving = EditorPrefs.GetBool("_isAutoSaving", _isAutoSaving);
            _isFoldout = EditorPrefs.GetBool("_isFoldout", _isFoldout);

            _saveMode = (SaveMode)EditorPrefs.GetInt("_saveMode", (int)_saveMode);
            _onTimeLenght = EditorPrefs.GetInt("_onTimeLenght", _onTimeLenght);
            _lastSave = DateTime.Parse(SessionState.GetString("_lastSave",
                DateTime.Now.ToString(CultureInfo.CurrentCulture)));
        }

        #endregion

        #region GUI method

        private void OptionsGUILayout()
        {
            // Styling
            Rect borderBox = new Rect(0, 23, position.width - 20, 55);
            Color borderColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            Color fillColor = new Color(0.2f, 0.2f, 0.2f, _isAutoSaving ? 0.7f : 0.9f);

            Rect optionsGroup = new Rect(10, 5, position.width - 40, position.height);

            // Elements
            GUIEditorUtility.DrawGUIRectBorder(borderBox, 1, borderColor, fillColor);
            GUI.BeginGroup(optionsGroup);
            {
                _saveMode = (SaveMode)GUILayout.SelectionGrid((int)_saveMode, Enum.GetNames(typeof(SaveMode)), 3,
                    "toggle");
                EditorGUI.BeginDisabledGroup(!_isAutoSaving || _saveMode != SaveMode.ON_TIME);
                {
                    EditorGUILayout.Separator();
                    _onTimeLenght = EditorGUILayout.IntField("Time in between save:", _onTimeLenght);
                }
                EditorGUI.EndDisabledGroup();
            }
            GUI.EndGroup();
        }

        private void InfoGUILayout()
        {
            GUIStyle infoStyle = GUI.skin.label;
            infoStyle.richText = true; // Make me able to html style node to modify text

            int timeSince = (int)(DateTime.Now.Subtract(_lastSave).TotalMinutes);
            string elapsedTimeMsg;
            string sceneSavingMsg = "<color=green>" + SceneManager.GetActiveScene().name + "</color>";
            string sceneStatusMsg = "<color=green>Unchanged</color>";

            if (!_isAutoSaving)
                sceneSavingMsg = "<color=red>NOTHING</color>";

            if (SceneManager.GetActiveScene().isDirty)
                sceneStatusMsg = "<color=yellow>Unsaved change</color>";

            // Did not know you could use a switch that way with operator as case.
            switch (SceneManager.GetActiveScene().isDirty ? timeSince : 0)
            {
                case < 5:
                    elapsedTimeMsg = "<color=green>" + timeSince + " minutes ago</color>";
                    break;

                case < 15:
                    elapsedTimeMsg = "<color=yellow>" + timeSince + " minutes ago</color>";
                    break;

                default:
                    elapsedTimeMsg = "<color=red>" + timeSince + " minutes ago</color>";
                    break;
            }

            EditorGUILayout.LabelField("Currently auto-saving:", sceneSavingMsg, infoStyle);
            EditorGUILayout.LabelField("Active scene status:", sceneStatusMsg, infoStyle);
            EditorGUILayout.LabelField("Session last auto-save:", elapsedTimeMsg, infoStyle);
            EditorGUILayout.LabelField(" ", _lastSave.ToString(CultureInfo.CurrentCulture));
        }

        #endregion
    }
}