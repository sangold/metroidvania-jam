using Reapling.SaveLoad;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SaveableEntity))]
[CanEditMultipleObjects]
public class SaveableEntityEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Generate Id"))
        {
            
            foreach (SaveableEntity target in targets)
            {
                target.GenerateId();
                PrefabUtility.RecordPrefabInstancePropertyModifications(target);
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(target.gameObject.scene);
            }


        }
    }
}
