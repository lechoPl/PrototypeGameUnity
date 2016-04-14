using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldsGenerator))]
public class FieldsGeneratorEditor : Editor
{
    private SerializedObject fieldsGeneratorSO;
    private SerializedProperty fieldsList;
    private SerializedProperty prefabsExpended;

    void OnEnable()
    {
        fieldsGeneratorSO = new SerializedObject(target);

        prefabsExpended = fieldsGeneratorSO.FindProperty("prefabsExpended");
        fieldsList = fieldsGeneratorSO.FindProperty("PrefabLists");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // Prefabs
        // ****************************** //
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        prefabsExpended.boolValue = EditorGUILayout.Foldout(prefabsExpended.boolValue, string.Format("Prefabs"));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        if (prefabsExpended.boolValue)
        {
            if (GUILayout.Button("Add prefab", GUILayout.Width(150)))
            {
                fieldsList.arraySize += 1;
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical();

            PrefabListEditor();

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        fieldsGeneratorSO.ApplyModifiedProperties();

        if (GUI.changed)
        {
            var fieldsGenerator = target as FieldsGenerator;

            fieldsGenerator.ClearFields();
            fieldsGenerator.CreateFields();

            SceneView.RepaintAll();
        }
    }

    private void PrefabListEditor()
    {
        for (int i = 0; i < fieldsList.arraySize; i++)
        {
            var fieldPrefab = fieldsList.GetArrayElementAtIndex(i);

            var fieldIsExpended = fieldPrefab.FindPropertyRelative("IsExpended");
            var prefab = fieldPrefab.FindPropertyRelative("Prefab");

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();

            fieldIsExpended.boolValue = EditorGUILayout.Foldout(fieldIsExpended.boolValue, string.Format("Prefab {0} - {1}", i + 1, prefab.objectReferenceValue != null ? prefab.objectReferenceValue.name : "null"));
            var deletePrefab = GUILayout.Button("Delete", GUILayout.Width(50));

            EditorGUILayout.EndHorizontal();

            if (deletePrefab)
            {
                fieldsList.DeleteArrayElementAtIndex(i);
            }
            else if (fieldIsExpended.boolValue)
            {
                EditorGUILayout.PropertyField(prefab);
            }

            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
    }
}
