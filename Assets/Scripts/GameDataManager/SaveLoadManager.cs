using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Reapling.SaveLoad
{
    public class SaveLoadManager: MonoBehaviour
    {
        private string SavePath => $"{Application.persistentDataPath}/";

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
            string jsonData = JsonUtility.ToJson(state);
            using (StreamWriter sw = new StreamWriter(SavePath + $"save{saveNumber}.json"))
            {
                sw.Write(jsonData);
            }
        }

        private Dictionary<string, object> LoadFile(int saveNumber)
        {
            if (!File.Exists(SavePath + $"save{saveNumber}.json"))
                return new Dictionary<string, object>();

            using (StreamReader sr = new StreamReader($"SaveGame{saveNumber}.json"))
            {
                string json = sr.ReadToEnd();
                return JsonUtility.FromJson<Dictionary<string, object>>(json);
            }
        }

        private void CaptureState(Dictionary<string, object> state)
        {
            foreach (var saveable in FindObjectsOfType<SaveableEntity>())
            {
                state[saveable.Id] = saveable.CaptureState();
            }
        }

        private void RestoreState(Dictionary<string, object> state)
        {
            foreach (var saveable in FindObjectsOfType<SaveableEntity>())
            {
                if (state.TryGetValue(saveable.Id, out object value))
                    saveable.RestoreState(value);
            }
        }
    }
}
