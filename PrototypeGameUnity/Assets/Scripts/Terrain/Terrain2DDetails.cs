using Assets.Scripts.Terrain.Enums;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Terrain2DDetails : MonoBehaviour
{
    public float heigth = 0.5f;

    public DetailType type;

    public List<Vector3> KeyVertices = new List<Vector3>();

    public Material material;

    public bool editEnabled;

    public EditMode mode;

    public float markSize = 0.5f;

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


    public void Setup()
    {
        SetupDataMesh();
    }

    public void SetupDataMesh()
    {
        if (KeyVertices.Count < 2)
        {
            return;
        }

        var vertices = CreateVertices();

        // Create the mesh
        Mesh msh = new Mesh();
        msh.vertices = vertices.ToArray();
        msh.triangles = GetIndices(vertices).ToArray();
        msh.uv = GetUVMapping(vertices).ToArray();

        msh.RecalculateNormals();
        msh.RecalculateBounds();

        meshRenderer.material = material;
        meshFilter.mesh = msh;
    }

    private IList<int> GetIndices(List<Vector3> vertices)
    {
        var result = new List<int>();

        int n = vertices.Count;
        for (int i = 0; i < n / 2; ++i)
        {
            result.Add(i);
            result.Add(i + 1);
            result.Add(n - i - 1);

            result.Add(i + 1);
            result.Add(n - i - 2);
            result.Add(n - i - 1);
        }

        return result;
    }

    private List<Vector3> CreateVertices()
    {
        var result = CreateTopVertices();
        result.AddRange(CreateBottomVertices());

        return result;
    }

    private List<Vector3> CreateTopVertices()
    {
        var result = new List<Vector3>();

        var halfHeight = heigth / 2;

        for (int i = 0; i < KeyVertices.Count; ++i)
        {
            Vector3 diff;

            if (i == 0)
            {
                diff = KeyVertices[i + 1] - KeyVertices[i];
                diff = Vector3.Cross(diff.normalized, Vector3.forward).normalized * halfHeight;
            }
            else if (i == KeyVertices.Count - 1)
            {
                diff = KeyVertices[i] - KeyVertices[i - 1];
                diff = Vector3.Cross(diff.normalized, Vector3.forward).normalized * halfHeight;
            }
            else
            {
                Vector3 v1 = KeyVertices[i + 1] - KeyVertices[i];
                Vector3 cross1 = Vector3.Cross(v1, Vector3.forward).normalized * halfHeight;

                Vector3 v2 = KeyVertices[i] - KeyVertices[i - 1];
                Vector3 cross2 = Vector3.Cross(v2, Vector3.forward).normalized * halfHeight;


                diff = (cross1 + cross2).normalized;

                var angle = Vector3.Angle(v2.normalized, diff);

                if (angle != 0)
                {
                    diff *= halfHeight / Mathf.Sin(angle * Mathf.Deg2Rad);
                }
            }

            result.Add(KeyVertices[i] - diff);
        }

        return result;
    }

    private List<Vector3> CreateBottomVertices()
    {
        var result = new List<Vector3>();

        var halfHeight = heigth / 2;

        for (int i = KeyVertices.Count - 1; i >= 0; --i)
        {
            Vector3 diff;

            if (i == 0)
            {
                diff = KeyVertices[i + 1] - KeyVertices[i];
                diff = Vector3.Cross(diff.normalized, Vector3.forward).normalized * halfHeight;
            }
            else if (i == KeyVertices.Count - 1)
            {
                diff = KeyVertices[i] - KeyVertices[i - 1];
                diff = Vector3.Cross(diff.normalized, Vector3.forward).normalized * halfHeight;
            }
            else
            {
                Vector3 v1 = KeyVertices[i - 1] - KeyVertices[i];
                Vector3 cross1 = Vector3.Cross(v1, Vector3.forward).normalized * halfHeight;

                Vector3 v2 = KeyVertices[i] - KeyVertices[i + 1];
                Vector3 cross2 = Vector3.Cross(v2, Vector3.forward).normalized * halfHeight;


                diff = (cross1 + cross2).normalized;

                var angle = Vector3.Angle(v2.normalized, diff);

                if (angle != 0)
                {
                    diff *= -halfHeight / Mathf.Sin(angle * Mathf.Deg2Rad);
                }
            }

            result.Add(KeyVertices[i] + diff);
        }

        return result;
    }

    private IList<Vector2> GetUVMapping(IList<Vector3> verticies)
    {
        var uvs = new List<Vector2>();

        float xMapping = 0f;

        var uvs_temp = new List<Vector2>();

        for (int i = 0; i < verticies.Count / 2; ++i)
        {
            xMapping = i == 0 ? 0f : xMapping + (verticies[i] - verticies[i - 1]).magnitude;

            uvs.Add(new Vector2(xMapping, 1f));
        }

        xMapping = 0f;
        for (int i = verticies.Count - 1; i >= verticies.Count / 2; --i)
        {
            xMapping = i == verticies.Count - 1 ? 0f : xMapping + (verticies[i] - verticies[i + 1]).magnitude;
            uvs_temp.Add(new Vector2(xMapping, 0f));
        }

        uvs_temp.Reverse();
        uvs.AddRange(uvs_temp);

        return uvs;
    }

    public int GetKeyVerticeId(Vector3 mousePosition, float radius)
    {
        if (KeyVertices == null || KeyVertices.Count < 2)
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
        if (KeyVertices == null || KeyVertices.Count < 2)
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

        if (KeyVertices.Count <= 2)
        {
            Debug.LogWarning(string.Format("Terrain2DDetails: {0} - Unable to delete vertex. Minimum number of vertex is 2.", name));
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
}
