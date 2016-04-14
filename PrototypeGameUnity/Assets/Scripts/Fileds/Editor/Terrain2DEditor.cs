using Assets.Scripts.Terrain.Enums;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Terrain2D))]
public class Terrain2DEditor : Editor
{
    SerializedProperty keyVertices;
    SerializedProperty editorSettings;
    SerializedProperty editModeEnabled;
    SerializedProperty mode;
    SerializedProperty markSize;

    private Terrain2D terrain;
    private int hoverVerticeId = -1;
    private int selectedVerticeId = -1;

    private readonly Color AddDefaultColor = Color.green;
    private readonly Color DeleteDefaultColor = Color.red;
    private readonly Color MoveDefaultColor = Color.yellow;
    private readonly Color HoverColor = Color.blue;
    private readonly Color SelectedColor = Color.white;


    void OnEnable()
    {
        terrain = target as Terrain2D;

        if (terrain != null)
        {
            terrain.SetupDataMesh();
        }

        keyVertices = serializedObject.FindProperty("KeyVertices");
        editorSettings = serializedObject.FindProperty("EditorSettings");
        editModeEnabled = editorSettings.FindPropertyRelative("editModeEnabled");
        mode = editorSettings.FindPropertyRelative("mode");
        markSize = editorSettings.FindPropertyRelative("markSize");
    }


    void OnDisable()
    {
        terrain = null;
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Editor", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(editModeEnabled, new GUIContent("Edit Mode"));

        if (editModeEnabled.boolValue)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();

            EditorGUILayout.HelpBox("Shortcuts:\n s - select mode\n d - delete mode\n a - add mode", MessageType.None);

            EditorGUILayout.PropertyField(mode);
            EditorGUILayout.PropertyField(markSize);

            EditorGUILayout.PropertyField(keyVertices);

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        serializedObject.ApplyModifiedProperties();

        if(GUI.changed)
        {
            if (terrain != null)
            {
                terrain.SetupDataMesh();
            }
        }
    }


    void OnSceneGUI()
    {
        if (terrain == null || !terrain.EditorSettings.editModeEnabled)
        {
            return;
        }

        int controlId = GUIUtility.GetControlID(FocusType.Passive);

        if (Event.current.type == EventType.keyUp)
        {
            switch (Event.current.keyCode)
            {
                case KeyCode.A:
                    terrain.EditorSettings.mode = EditMode.Add;
                    Event.current.Use();
                    break;
                case KeyCode.S:
                    terrain.EditorSettings.mode = EditMode.Move;
                    Event.current.Use();
                    break;
                case KeyCode.D:
                    terrain.EditorSettings.mode = EditMode.Delete;
                    Event.current.Use();
                    break;
            }

            selectedVerticeId = -1;
        }

        if (Event.current.button == 0)
        {
            switch (Event.current.GetTypeForControl(controlId))
            {
                case EventType.MouseUp:
                    OnMouseUp();
                    break;

                case EventType.MouseDrag:
                    OnMouseDrag();
                    break;

                case EventType.MouseDown:
                    OnMouseDown();
                    break;
            }
        }

        CheckHover();

        MarkKeyVertices();
        terrain.SetupDataMesh();
    }


    private void OnMouseDown()
    {
        if (selectedVerticeId >= 0)
        {
            return;
        }

        switch(terrain.EditorSettings.mode)
        {
            case EditMode.Move:
            case EditMode.Delete:
                selectedVerticeId = terrain.GetKeyVerticeId(GetMousePositionInWold(), terrain.EditorSettings.markSize);
                break;

            case EditMode.Add:
                selectedVerticeId = terrain.GetNewVerticeId(GetMousePositionInWold(), terrain.EditorSettings.markSize);
                break;
        }
    }


    private void OnMouseDrag()
    {
        if (selectedVerticeId < 0)
        {
            return;
        }

        if (terrain.EditorSettings.mode == EditMode.Move)
        {
            terrain.MoveKeyVertex(selectedVerticeId, GetMousePositionInWold());
        }

        Event.current.Use();
    }


    private void OnMouseUp()
    {
        if (selectedVerticeId < 0)
        {
            return;
        }

        switch (terrain.EditorSettings.mode)
        {
            case EditMode.Delete:
                terrain.DeleteKeyVertex(selectedVerticeId);
                break;

            case EditMode.Add:
                terrain.AddKeyVertex(selectedVerticeId, GetMousePositionInWold());
                break;
        }

        selectedVerticeId = -1;
        Event.current.Use();
    }


    private Vector3 GetMousePositionInWold()
    {
        return HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
    }


    private void CheckHover()
    {
        if (selectedVerticeId >= 0)
        {
            hoverVerticeId = -1;
            return;
        }

        switch (terrain.EditorSettings.mode)
        {
            case EditMode.Move:
            case EditMode.Delete:
                hoverVerticeId = terrain.GetKeyVerticeId(GetMousePositionInWold(), terrain.EditorSettings.markSize);
                break;

            case EditMode.Add:
                hoverVerticeId = terrain.GetNewVerticeId(GetMousePositionInWold(), terrain.EditorSettings.markSize);
                break;
        }
    }


    private Color GetMarkColor(int i)
    {
        if (i == selectedVerticeId)
        {
            return SelectedColor;
        }

        if (i == hoverVerticeId)
        {
            return HoverColor;
        }

        switch(terrain.EditorSettings.mode)
        {
            case EditMode.Move:
                return MoveDefaultColor;

            case EditMode.Add:
                return AddDefaultColor;

            case EditMode.Delete:
                return DeleteDefaultColor;
        }

        return Color.black;
    }


    private void MarkKeyVertices()
    {
        if (terrain == null)
        {
            return;
        }

        for (int i = 0; i < terrain.KeyVertices.Count; ++i)
        {
            Color color = GetMarkColor(i);

            if (terrain.EditorSettings.mode == EditMode.Move || terrain.EditorSettings.mode == EditMode.Delete)
            {

                var pos = terrain.transform.TransformPoint(terrain.KeyVertices[i]);
                DrawMark(pos, terrain.EditorSettings.markSize, color);
            }

            if (terrain.EditorSettings.mode == EditMode.Add)
            {
                int prev = i - 1 < 0  ? terrain.KeyVertices.Count - 1 : i - 1;

                var v1 = terrain.transform.TransformPoint(terrain.KeyVertices[prev]);
                var v2 = terrain.transform.TransformPoint(terrain.KeyVertices[i]);

                var pos = v1 + (v2 - v1) * 0.5f;
                DrawMark(pos, terrain.EditorSettings.markSize, color);
            }
        }
    }

    private void DrawMark(Vector3 position, float radius, Color color)
    {
        Color previousHandleColor = Handles.color;
        Handles.color = color;
        if(terrain.EditorSettings.mode == EditMode.Move)
        {
            Handles.DrawSolidDisc(position, Vector3.forward, radius);
        }

        if (terrain.EditorSettings.mode == EditMode.Delete)
        {
            Handles.DrawWireDisc(position, Vector3.right, radius);
            Handles.DrawWireDisc(position, Vector3.forward, radius);
        }

        if(terrain.EditorSettings.mode == EditMode.Add)
        {
            Handles.DrawSolidDisc(position, Vector3.forward, radius);
        }

        Handles.color = previousHandleColor;
    }
}
