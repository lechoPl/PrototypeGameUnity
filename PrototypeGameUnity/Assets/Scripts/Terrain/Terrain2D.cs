using Assets.Scripts.Terrain.Utilities;
using Assets.Scripts.Terrain.ValueObjects;
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
    public Terrain2DEditorSettings EditorSettings = new Terrain2DEditorSettings();

    public Material material;

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
        Debug.Log("test");
        SetupDataMesh();
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

        msh.RecalculateNormals();
        msh.RecalculateBounds();


        meshRenderer.material = material;
        meshFilter.mesh = msh;
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
