using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldBounds))]
public class FieldBoundsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUI.changed)
        {
            var fieldsGenerator = target as FieldBounds;

            fieldsGenerator.ClearBounds();
            fieldsGenerator.Setup();
        }
    }
}
