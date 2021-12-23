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
        public void GenerateId() => id = Guid.NewGuid().ToString();

        public object CaptureState()
        {
            if (id == string.Empty) return null;
            Dictionary<string, object> state = new Dictionary<string, object>();

            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                state[saveable.GetType().ToString()] = saveable.CaptureState();
            }

            return state;
        }

        public void RestoreState(object state)
        {
            if (state.ToString() == "{}") return;
            Dictionary<string, object> stateDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(state);
            
            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                string typeName = saveable.GetType().ToString();

                if (stateDictionary.TryGetValue(typeName, out object value))
                    saveable.RestoreState(value);
            }
        }
    }
}
