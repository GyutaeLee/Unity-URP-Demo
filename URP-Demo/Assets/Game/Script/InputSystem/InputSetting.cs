using UnityEngine;
using System;
using System.Collections.Generic;

namespace InputSystem
{
    // TODO : [Key] class naming 수정 필요 (충돌 발)
    [Serializable]
    public class Key
    {
        [SerializeField]
        private string name;
        [SerializeField]
        private KeyCode code;

        public string Name { get => this.name; }
        public KeyCode Code { get => this.code; }
    }

    public class InputSetting : ScriptableObject
    {
        [SerializeField]
        private List<InputSystem.Key> keys;

        public List<InputSystem.Key> Keys { get => this.keys; }
    }
}