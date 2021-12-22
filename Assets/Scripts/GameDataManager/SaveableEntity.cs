using System;
using System.Collections.Generic;
using UnityEngine;

namespace Reapling.SaveLoad
{
    public class SaveableEntity: MonoBehaviour
    {
        public string Id => id;
        [SerializeField]
        private string id = string.Empty;

        [ContextMenu("Generate ID")]
        private void GenerateId() => id = Guid.NewGuid().ToString();

        public object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();

            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                state[saveable.GetType().ToString()] = saveable.CaptureState();
            }

            return state;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> stateDictionary = (Dictionary<string, object>)state;

            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                string typeName = saveable.GetType().ToString();

                if (stateDictionary.TryGetValue(typeName, out object value))
                    saveable.RestoreState(value);
            }
        }
    }
}
