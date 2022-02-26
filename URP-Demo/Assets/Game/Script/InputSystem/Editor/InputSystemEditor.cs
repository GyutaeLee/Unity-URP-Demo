using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace InputSystem
{
    public class InputSystemEditor : EditorWindow
    {
        private class Constants
        {
            public const string ResourcesPath = "Assets/Resources/";
            public const string ExtensionPath = "asset";
        }

        private InputSetting inputSetting;
        private SerializedObject serializedInputSetting;

        [SerializeField]
        private string rootDirectoryName = "InputSystem";
        [SerializeField]
        private string settingDirectoryName = "InputSetting";

        private GUIStyle titleStyle = new GUIStyle();
        private GUIStyle subLabelStyle = new GUIStyle();

        private int textFieldSize = 250;
        private int buttonSize = 150;
        private int spaceSize = 20;

        [MenuItem("InputSystem/Set Input System")]
        private static void OpenEditor()
        {
            GetWindow<InputSystemEditor>("InputSystem");
        }

        private void OnEnable()
        {
            SetTitleStyle();
            SetSubLabelStyle();
        }

        private void SetTitleStyle()
        {
            this.titleStyle.alignment = TextAnchor.MiddleCenter;
            this.titleStyle.fontStyle = FontStyle.Bold;
            this.titleStyle.normal.textColor = new Color(0.0f, 1.0f, 0.6f);
            this.titleStyle.fontSize = 24;
        }

        private void SetSubLabelStyle()
        {
            this.subLabelStyle.alignment = TextAnchor.MiddleRight;
            this.subLabelStyle.normal.textColor = Color.gray;
        }

        private void OnGUI()
        {
            ShowTopMenu();
            ShowMiddleMenu();
            ShowBottomMenu();
        }

        private void ShowTopMenu()
        {
            GUILayout.Label("InputSystem", this.titleStyle);
            GUILayout.Space(this.spaceSize);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Directory Path : ");
            GUILayout.Label(Constants.ResourcesPath, this.subLabelStyle);
            this.rootDirectoryName = EditorGUILayout.TextField(this.rootDirectoryName, GUILayout.Width(this.textFieldSize));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("File Name : ");
            this.settingDirectoryName = EditorGUILayout.TextField(this.settingDirectoryName, GUILayout.Width(this.textFieldSize));
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Refresh", GUILayout.Width(this.buttonSize)) == true)
            {
                RefreshButton();
            }
            GUILayout.Space(this.spaceSize);
        }

        private void RefreshButton()
        {
            if (string.IsNullOrEmpty(this.rootDirectoryName) == true
                || string.IsNullOrEmpty(this.settingDirectoryName) == true)
            {
                EditorUtility.DisplayDialog("", "잘못된 경로입니다.\n폴더경로 또는 파일이름을 다시 입력해주세요. ", "확인");
                return;
            }

            string rootDirectoryPath = string.Format($"{Path.Combine(Constants.ResourcesPath, this.rootDirectoryName)}");
            if (Directory.Exists(rootDirectoryPath) == false)
            {
                Directory.CreateDirectory(rootDirectoryPath);
            }

            string settingDirectoryPath = string.Format($"{Path.Combine(rootDirectoryPath, this.settingDirectoryName)}.{Constants.ExtensionPath}");
            this.inputSetting = AssetDatabase.LoadAssetAtPath(settingDirectoryPath, typeof(InputSetting)) as InputSetting;
            if (this.inputSetting == null)
            {
                this.inputSetting = CreateInstance<InputSetting>();
                AssetDatabase.CreateAsset(this.inputSetting, settingDirectoryPath);
                AssetDatabase.ImportAsset(settingDirectoryPath);
            }
            this.serializedInputSetting = new SerializedObject(this.inputSetting);
        }

        private void ShowMiddleMenu()
        {
            if (this.inputSetting == null || this.serializedInputSetting == null)
                return;

            SerializedProperty keyButtonProperty = this.serializedInputSetting.FindProperty("keys");
            EditorGUILayout.PropertyField(keyButtonProperty, true);
            this.serializedInputSetting.ApplyModifiedProperties();
        }

        private void ShowBottomMenu()
        {
            if (this.inputSetting == null || this.serializedInputSetting == null)
                return;

            GUI.color = Color.cyan;
            if (GUILayout.Button("Apply Input System", GUILayout.Width(this.buttonSize)) == true)
            {
                // TODO : 왜 여기서 저장하는지 확인 필요
                CreateEnumClass(this.inputSetting.Keys);
                AssetDatabase.SaveAssets();
            }
        }

        // TODO : 함수명 고민하기
        public void CreateEnumClass(List<Key> inputButtons)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("// auto generated Enum By InputSystem");
            stringBuilder.AppendLine("namespace InputSystem");
            stringBuilder.AppendLine("{");

            stringBuilder.AppendLine("    public static class InputSystemSetting");
            stringBuilder.AppendLine("    {");
            stringBuilder.AppendLine($"        public const string Path = \"{this.rootDirectoryName}/{this.settingDirectoryName}\";");
            stringBuilder.AppendLine("    }");
            stringBuilder.AppendLine();

            if (inputButtons.Count > 0)
            {
                stringBuilder.AppendLine("    public enum KeyName");
                stringBuilder.AppendLine("    {");
                foreach (Key inputButton in inputButtons)
                {
                    stringBuilder.AppendLine($"        {inputButton.Name},");
                }
                stringBuilder.AppendLine("    }");
            }
            stringBuilder.AppendLine("}");
            string writePath = Application.dataPath + "/InputSystem/InputNames.cs";
            File.WriteAllText(writePath, stringBuilder.ToString());
            AssetDatabase.Refresh();
        }
    }
}