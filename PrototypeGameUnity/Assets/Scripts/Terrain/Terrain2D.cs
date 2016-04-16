using Assets.Scripts.Terrain.Enums;
using Assets.Scripts.Terrain.Utilities;
using Assets.Scripts.Terrain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Terrain2D : MonoBehaviour
{
    [HideInInspector]
    [SerializeField]
    public List<Vector3> KeyVertices = new List<Vector3>
                        {
                            new Vector2(-1,-1),
                            new Vector2(-1, 1),
                            new Vector2(1, 1),
                            new Vector2(1, -1)
                        };

    [HideInInspector]
    [SerializeField]
    public List<Terrain2DDetails> TerrainDetails = new List<Terrain2DDetails>();

    [HideInInspector]
    public Terrain2DEditorSettings EditorSettings = new Terrain2DEditorSettings();

    public Material material;


    [Range(1, 89)]
    public float maxTopSideAngle = 60f;
    [Range(1, 89)]
    public float maxBottomSideAngle = 60f;
    [Range(1, 179)]
    public float breakTopDetailAngle = 60f;
    [Range(1, 179)]
    public float breakSideDetailAngle = 60f;
    [Range(1, 179)]
    public float breakBottomDetailAngle = 60f;

    public float DetialsHeight = 0.5f;
    public Material topMaterial;
    public Material sideMaterial;
    public Material bottomMaterial;

    public bool CreateCollider;

    [SerializeField]
    private MeshFilter meshFilter
    {
        get
        {
            var result = GetComponent<MeshFilter>();
            if (result == null)
            {
                result = gameObject.AddComponent<MeshFilter>();
            }
            return result;
        }
    }

    private MeshRenderer meshRenderer
    {
        get
        {
            var result = GetComponent<MeshRenderer>();
            if (result == null)
            {
                result = gameObject.AddComponent<MeshRenderer>();
            }
            return result;
        }
    }

    private PolygonCollider2D polygonCollider2D
    {
        get
        {
            var result = GetComponent<PolygonCollider2D>();
            if (result == null)
            {
                result = gameObject.AddComponent<PolygonCollider2D>();
            }
            return result;
        }
    }

    void Awake()
    {
        Setup();
    }

    public int GetKeyVerticeId(Vector3 mousePosition, float radius)
    {
        if (KeyVertices == null || KeyVertices.Count < 3)
        {
            Debug.Log(string.Format("Terrain2D: {0} - don't have any vertices", name));
            return -1;
        }

        var pos = transform.worldToLocalMatrix * (mousePosition - transform.position);
        var selected = KeyVertices.Where(v => (new Vector2 { x = v.x - pos.x, y = v.y - pos.y }).magnitude < radius);

        if (selected.Count() == 0)
        {
            return -1;
        }

        return KeyVertices.IndexOf(selected.First());
    }

    public int GetNewVerticeId(Vector3 mousePosition, float radius)
    {
        if (KeyVertices == null || KeyVertices.Count < 3)
        {
            Debug.Log(string.Format("Terrain2D: {0} - don't have any vertices", name));
            return -1;
        }

        var pos = transform.worldToLocalMatrix * (mousePosition - transform.position);

        for (int i = 0; i < KeyVertices.Count; ++i)
        {
            int prev = i - 1 < 0 ? KeyVertices.Count - 1 : i - 1;

            var v1 = KeyVertices[prev];
            var v2 = KeyVertices[i];

            var mid = v1 + (v2 - v1) * 0.5f;

            if ((new Vector2 { x = mid.x - pos.x, y = mid.y - pos.y }).magnitude < radius)
            {
                return i;
            }
        }

        return -1;
    }

    public void MoveKeyVertex(int id, Vector3 mousePosition)
    {
        if (id < 0 || id >= KeyVertices.Count)
        {
            return;
        }

        var pos = transform.worldToLocalMatrix * (mousePosition - transform.position);


        KeyVertices[id] = new Vector3 { x = pos.x, y = pos.y, z = 0f };
    }

    public void DeleteKeyVertex(int id)
    {
        if (id < 0 || id >= KeyVertices.Count)
        {
            return;
        }

        if (KeyVertices.Count <= 3)
        {
            Debug.LogWarning(string.Format("Terrain2D: {0} - Unable to delete vertex. Minimum number of vertex is 3.", name));
            return;
        }

        KeyVertices.RemoveAt(id);
    }

    public void AddKeyVertex(int id, Vector3 mousePosition)
    {
        var pos = transform.worldToLocalMatrix * (mousePosition - transform.position);

        var newVertex = new Vector3 { x = pos.x, y = pos.y, z = 0f };
        KeyVertices.Insert(id, newVertex);
    }

    public void Setup()
    {
        SetupDataMesh();
        SetupTerrainDetails();
    }

    public void SetupDataMesh()
    {
        var indices = Triangulator.Triangulate(KeyVertices);

        // Create the mesh
        Mesh msh = new Mesh();
        msh.vertices = KeyVertices.ToArray();
        msh.triangles = indices.ToArray();
        msh.uv = GetUVMapping(KeyVertices).ToArray();

        if (CreateCollider)
        {
            polygonCollider2D.points = KeyVertices.Select(v => new Vector2(v.x, v.y)).ToArray();
        }
        else
        {
            DestroyImmediate(polygonCollider2D);
        }

        msh.RecalculateNormals();
        msh.RecalculateBounds();


        meshRenderer.material = material;
        meshFilter.mesh = msh;
    }

    public void SetupTerrainDetails()
    {
        for (int i = 0; i < TerrainDetails.Count; ++i)
        {
            if (TerrainDetails[i] != null && TerrainDetails[i].gameObject != null)
            {
                DestroyImmediate(TerrainDetails[i].gameObject);
            }
        }
        TerrainDetails.Clear();

        DetailType detailType = DetailType.Side;
        List<Vector3> detailKeyVertices = new List<Vector3>();

        for (int i = 0; i < KeyVertices.Count; ++i)
        {
            if (i == 0)
            {
                detailKeyVertices.Add(KeyVertices[i]);

                Vector3 first = (KeyVertices[1] - KeyVertices[0]).normalized;
                var angle = Vector3.Angle(first, Vector3.up);

                if (angle < maxBottomSideAngle || angle > 180 - maxBottomSideAngle)
                {
                    detailType = DetailType.Side;
                }
                else
                {
                    detailType = DetailType.Top;
                }
            }
            else
            {
                detailKeyVertices.Add(KeyVertices[i]);

                int nextId = i < KeyVertices.Count - 1 ? i + 1 : 0;
                Vector3 next = (KeyVertices[nextId] - KeyVertices[i]).normalized;
                Vector3 current = (KeyVertices[i] - KeyVertices[i - 1]).normalized;

                var angle = Vector3.Angle(next, current);
                float breakAngle = GetBreakAngle(detailType);

                if (angle > breakAngle)
                {
                    TerrainDetails.Add(CreateTerrain2DDetails(detailKeyVertices, detailType));
                    detailKeyVertices = new List<Vector3>();
                    detailKeyVertices.Add(KeyVertices[i]);

                    if (TerrainDetails.Count > 0)
                    {
                        detailType = GetDetailType(current, next);
                    }
                }

                if (i == KeyVertices.Count - 1)
                {
                    detailKeyVertices.Add(KeyVertices[nextId]);
                    TerrainDetails.Add(CreateTerrain2DDetails(detailKeyVertices, detailType));
                }
            }
        }
    }

    private float GetBreakAngle(DetailType type)
    {
        switch (type)
        {
            case DetailType.Top:
                return breakTopDetailAngle;

            case DetailType.Side:
                return breakSideDetailAngle;

            case DetailType.Bottom:
                return breakBottomDetailAngle;

            default:
                return 70f;
        }
    }

    private DetailType GetDetailType(Vector3 current, Vector3 next)
    {
        DetailType result;

        var lastType = TerrainDetails.Last().type;

        if (lastType == DetailType.Side)
        {
            if (current.y > 0)
            {
                return next.x < 0 ? DetailType.Bottom : DetailType.Top;
            }
            else
            {
                return next.x < 0 ? DetailType.Bottom : DetailType.Top;
            }
        }

        if (lastType == DetailType.Bottom)
        {
            if (current.x < 0 && next.x < 0)
            {
                return DetailType.Side;
            }

            var angle = Vector3.Angle(next, Vector3.up);

            if (angle < maxBottomSideAngle || angle > 180 - maxBottomSideAngle)
            {
                return DetailType.Side;
            }
            else
            {
                return DetailType.Top;
            }
        }

        if (lastType == DetailType.Top)
        {
            var angle = Vector3.Angle(next, Vector3.up);

            if (angle < maxTopSideAngle || angle > 180 - maxTopSideAngle)
            {
                return DetailType.Side;
            }
            else
            {
                return DetailType.Bottom;
            }
        }

        throw new NotImplementedException();
    }

    private Terrain2DDetails CreateTerrain2DDetails(List<Vector3> detailsKeyVertices, DetailType type)
    {
        var details = new GameObject("Details");

        var pos = transform.position;
        switch (type)
        {
            case DetailType.Top:
                pos.z -= 0.03f;
                break;

            case DetailType.Side:
                pos.z -= 0.01f;
                break;

            case DetailType.Bottom:
                pos.z -= 0.02f;
                break;
        }

        details.transform.position = pos;
        details.transform.rotation = transform.rotation;
        details.transform.localScale = transform.localScale;
        details.transform.parent = transform;

        var detailsComponent = details.AddComponent<Terrain2DDetails>();
        detailsComponent.KeyVertices = detailsKeyVertices;
        detailsComponent.heigth = DetialsHeight;
        detailsComponent.type = type;

        switch (type)
        {
            case DetailType.Top:
                detailsComponent.material = topMaterial;
                break;

            case DetailType.Side:
                detailsComponent.material = sideMaterial;
                break;

            case DetailType.Bottom:
                detailsComponent.material = bottomMaterial;
                break;
        }

        detailsComponent.Setup();

        return detailsComponent;
    }

    private IList<Vector2> GetUVMapping(IList<Vector3> verticies)
    {
        var uvs = new List<Vector2>();
        for (int i = 0; i < verticies.Count; i++)
        {
            var vertex = verticies[i];

            //Our standard uv mapping is just our point in space divided by the width and the height of our texture (assuming an x/y plane)
            float xMapping = vertex.x;
            float yMapping = vertex.y;

            //Finally set the actual uv mapping
            var uv = new Vector2(xMapping, yMapping);
            uvs.Add(uv);
        }

        return uvs;
    }
}
