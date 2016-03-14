using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Field))]
public class FieldEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUI.changed)
        {
            var fieldsGenerator = target as Field;

            fieldsGenerator.Clear();
            fieldsGenerator.SetupBoundsWidth();
            fieldsGenerator.SetupBoundsHeight();
        }
    }
}
