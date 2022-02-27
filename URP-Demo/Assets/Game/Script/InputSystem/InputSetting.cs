using UnityEngine;
using System;
using System.Collections.Generic;

namespace Demo.InputSystem
{
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