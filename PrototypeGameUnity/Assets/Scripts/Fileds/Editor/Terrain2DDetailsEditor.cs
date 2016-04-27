
using Assets.Scripts.Terrain.Enums;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Terrain2DDetails))]
[CanEditMultipleObjects]
public class Terrain2DDetailsEditor : Editor
{
    private Terrain2DDetails terrainDetials;
    private int hoverVerticeId = -1;
    private int selectedVerticeId = -1;

    private readonly Color AddDefaultColor = Color.green;
    private readonly Color DeleteDefaultColor = Color.red;
    private readonly Color MoveDefaultColor = Color.yellow;
    private readonly Color HoverColor = Color.blue;
    private readonly Color SelectedColor = Color.white;

    public new void Repaint()
    {
        if (terrainDetials != null)
        {
            terrainDetials.Setup();
        }

        base.Repaint();
    }

    void OnEnable()
    {
        terrainDetials = target as Terrain2DDetails;

        if (terrainDetials != null)
        {
            terrainDetials.Setup();
        }
    }


    void OnDisable()
    {
        terrainDetials = null;
    }

    void OnSceneGUI()
    {
        if (terrainDetials == null || !terrainDetials.editEnabled)
        {
            return;
        }

        int controlId = GUIUtility.GetControlID(FocusType.Passive);

        if (Event.current.type == EventType.keyUp)
        {
            switch (Event.current.keyCode)
            {
                case KeyCode.A:
                    terrainDetials.mode = EditMode.Add;
                    Event.current.Use();
                    break;
                case KeyCode.S:
                    terrainDetials.mode = EditMode.Move;
                    Event.current.Use();
                    break;
                case KeyCode.D:
                    terrainDetials.mode = EditMode.Delete;
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
    }


    private void OnMouseDown()
    {
        if (selectedVerticeId >= 0)
        {
            return;
        }

        switch (terrainDetials.mode)
        {
            case EditMode.Move:
            case EditMode.Delete:
                selectedVerticeId = terrainDetials.GetKeyVerticeId(GetMousePositionInWold(), terrainDetials.markSize);
                break;

            case EditMode.Add:
                selectedVerticeId = terrainDetials.GetNewVerticeId(GetMousePositionInWold(), terrainDetials.markSize);
                break;
        }
    }


    private void OnMouseDrag()
    {
        if (selectedVerticeId < 0)
        {
            return;
        }

        if (terrainDetials.mode == EditMode.Move)
        {
            terrainDetials.MoveKeyVertex(selectedVerticeId, GetMousePositionInWold());
            terrainDetials.Setup();
        }

        Event.current.Use();
    }


    private void OnMouseUp()
    {
        if (selectedVerticeId < 0)
        {
            return;
        }

        switch (terrainDetials.mode)
        {
            case EditMode.Delete:
                terrainDetials.DeleteKeyVertex(selectedVerticeId);
                terrainDetials.Setup();
                break;

            case EditMode.Add:
                terrainDetials.AddKeyVertex(selectedVerticeId, GetMousePositionInWold());
                terrainDetials.Setup();
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

        switch (terrainDetials.mode)
        {
            case EditMode.Move:
            case EditMode.Delete:
                hoverVerticeId = terrainDetials.GetKeyVerticeId(GetMousePositionInWold(), terrainDetials.markSize);
                break;

            case EditMode.Add:
                hoverVerticeId = terrainDetials.GetNewVerticeId(GetMousePositionInWold(), terrainDetials.markSize);
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

        switch (terrainDetials.mode)
        {
            case EditMode.Move:
                if (i == 0)
                    return Color.gray;
                if (i % 2 == 0)
                    return Color.cyan;
                else
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
        if (terrainDetials == null)
        {
            return;
        }

        for (int i = 0; i < terrainDetials.KeyVertices.Count; ++i)
        {
            Color color = GetMarkColor(i);

            if (terrainDetials.mode == EditMode.Move || terrainDetials.mode == EditMode.Delete)
            {

                var pos = terrainDetials.transform.TransformPoint(terrainDetials.KeyVertices[i]);
                DrawMark(pos, terrainDetials.markSize, color);

                if (i > 0)
                {
                    DrawLine(terrainDetials.transform.TransformPoint(terrainDetials.KeyVertices[i]), terrainDetials.transform.TransformPoint(terrainDetials.KeyVertices[i - 1]), color);
                }
            }

            if (terrainDetials.mode == EditMode.Add)
            {
                if (i == 0)
                {
                    continue;
                }

                var v1 = terrainDetials.transform.TransformPoint(terrainDetials.KeyVertices[i - 1]);
                var v2 = terrainDetials.transform.TransformPoint(terrainDetials.KeyVertices[i]);

                var pos = v1 + (v2 - v1) * 0.5f;
                DrawMark(pos, terrainDetials.markSize, color);
            }
        }
    }

    private void DrawLine(Vector3 p1, Vector3 p2, Color color)
    {
        Color previousHandleColor = Handles.color;

        Handles.DrawLine(p1, p2);

        Handles.color = previousHandleColor;
    }

    private void DrawMark(Vector3 position, float radius, Color color)
    {
        Color previousHandleColor = Handles.color;
        Handles.color = color;
        if (terrainDetials.mode == EditMode.Move)
        {
            Handles.DrawSolidDisc(position, Vector3.forward, radius);
        }

        if (terrainDetials.mode == EditMode.Delete)
        {
            Handles.DrawWireDisc(position, Vector3.right, radius);
            Handles.DrawWireDisc(position, Vector3.forward, radius);
        }

        if (terrainDetials.mode == EditMode.Add)
        {
            Handles.DrawSolidDisc(position, Vector3.forward, radius);
        }

        Handles.color = previousHandleColor;
    }
}
