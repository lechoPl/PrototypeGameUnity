using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldsGenerator))]
public class FieldsGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUI.changed)
        {
            var fieldsGenerator = target as FieldsGenerator;

            fieldsGenerator.ClearFields();
            fieldsGenerator.CreateFields();
        }
    }
}
