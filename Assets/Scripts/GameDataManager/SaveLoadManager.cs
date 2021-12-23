using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Reapling.SaveLoad
{
    public class SaveLoadManager: MonoBehaviour
    {
        public static SaveLoadManager Instance
        {
            get
            {
                return _instance ?? (_instance = new GameObject("SaveLoadManager").AddComponent<SaveLoadManager>());
            }

            private set
            {
                _instance = value;
            }
        }

        private static SaveLoadManager _instance;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private string SavePath => $"";

        public void Save(int saveNumber)
        {
            var state = LoadFile(saveNumber);
            CaptureState(state);
            SaveFile(saveNumber, state);
        }

        public void Load(int saveNumber)
        {
            var state = LoadFile(saveNumber);
            RestoreState(state);
        }

        private void SaveFile(int saveNumber, object state)
        {
            using (StreamWriter sw = new StreamWriter(SavePath + $"save{saveNumber}.json"))
            {
                sw.Write(JsonSerializer.Serialize(state));
            }
        }

        private Dictionary<string, object> LoadFile(int saveNumber)
        {
            if (!File.Exists(SavePath + $"save{saveNumber}.json"))
                return new Dictionary<string, object>();

            using (StreamReader sr = new StreamReader(SavePath + $"save{saveNumber}.json"))
            {
                string json = sr.ReadToEnd();
                return JsonSerializer.Deserialize<Dictionary<string, object>>(json);
            }
        }

        private void CaptureState(Dictionary<string, object> state)
        {
            foreach (var saveable in FindObjectsOfType<SaveableEntity>(true))
            {
                if (saveable.Id == string.Empty) continue;
                state[saveable.Id] = saveable.CaptureState();
            }
        }

        private void RestoreState(Dictionary<string, object> state)
        {
            foreach (var saveable in FindObjectsOfType<SaveableEntity>(true))
            {
                if (state.TryGetValue(saveable.Id, out object value))
                {
                    saveable.RestoreState(value);
                }
            }
        }
    }
}
