using System.Collections.Generic;
using UnityEngine;

namespace Demo.InputSystem
{
    public class InputManager : Singleton<InputManager>
    {
        private InputSetting inputSetting;

        public Dictionary<KeyName, KeyInput> KeyInputDictionary { get; private set; }
        public MouseInput MouseInput { get; private set; }

        private void Awake()
        {
            this.inputSetting = Resources.Load<InputSetting>(InputSystemSetting.Path);
            if (this.inputSetting == null)
            {
                Debug.Log("InputSystem을 설정하지 않았습니다.");
                return;
            }

            BindKey();
            this.MouseInput = new MouseInput();
        }

        private void Update()
        {
            foreach (var keyInput in this.KeyInputDictionary.Values)
            {
                keyInput.IsKeyPressed = Input.GetKey(keyInput.Code);
                keyInput.IsKeyDown = Input.GetKeyDown(keyInput.Code);
                keyInput.IsKeyUp = Input.GetKeyUp(keyInput.Code);
            }
            this.MouseInput.Run();
        }

        private void BindKey()
        {
            this.KeyInputDictionary = new Dictionary<KeyName, KeyInput>();
            foreach (var keyButton in this.inputSetting.Keys)
            {
                KeyName keyName = Util.EnumMapper.GetEnumType<KeyName>(keyButton.Name);
                KeyCode keyCode = GetUserSettingKeyCode(keyName);
                if (keyCode == KeyCode.None)
                {
                    keyCode = keyButton.Code;
                }

                this.KeyInputDictionary[keyName] = new KeyInput(keyCode);
            }
        }

        private KeyCode GetUserSettingKeyCode(KeyName keyName)
        {
            string userSettingKeyName = PlayerPrefs.GetString(keyName.ToString());
            if (string.IsNullOrEmpty(userSettingKeyName) == true)
            {
                return KeyCode.None;
            }
            else
            {
                return Util.EnumMapper.GetEnumType<KeyCode>(userSettingKeyName);
            }
        }

        public KeyInput GetKeyInput(KeyName keyName)
        {
            return this.KeyInputDictionary[keyName];
        }

        public void ChangeKeyInput(KeyName targetKeyName, KeyCode changeKeyCode)
        {
            this.KeyInputDictionary[targetKeyName].Code = changeKeyCode;
            PlayerPrefs.SetString(targetKeyName.ToString(), changeKeyCode.ToString());
        }

        private void OnGUI()
        {
            int height = 0;
            foreach (var keyInput in this.KeyInputDictionary)
            {
                GUI.Label(new Rect(100, 40 + height, 80, 20), keyInput.Key.ToString());
                GUI.Label(new Rect(20, 40 + height, 80, 20), keyInput.Value.Code.ToString());
                height += 20;
            }
        }
    }
}
