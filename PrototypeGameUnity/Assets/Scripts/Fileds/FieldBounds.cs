using System.Collections.Generic;
using UnityEngine;

public class FieldBounds : MonoBehaviour
{
    public int Width;
    public int Height;

    private GameObject _bounds;
    private GameObject Bounds {
        get
        {
            if (_bounds == null)
            {
                for (int i = 0; i < transform.childCount; ++i)
                {
                    if (transform.GetChild(i).name == "Bounds")
                    {
                        _bounds = transform.GetChild(i).gameObject;
                    }
                }
            }

            return _bounds;
        }

        set
        {
            _bounds = value;
        }
    }
    private GameObject TopBound;
    private GameObject DownBound;
    private GameObject LeftBound;
    private GameObject RightBound;

    public void SetWidth(int val)
    {
        Width = val;
        SetupBoundsWidth();
    }


    public void SetHeight(int val)
    {
        Height = val;
        SetupBoundsHeight();
    }

    void Awake()
    {
        ClearBounds();
        Setup();
    }

    public void Setup()
    {
        SetupBoundsWidth();
        SetupBoundsHeight();
    }

    public void SetupBoundsWidth()
    {
        CheckBounds();

        TopBound.transform.localScale += (-TopBound.transform.localScale) + new Vector3(Width, 1f, 1f);
        DownBound.transform.localScale += (-DownBound.transform.localScale) + new Vector3(Width, 1f, 1f);
        LeftBound.transform.position += new Vector3(-Width / 2f, 0f, 0f);
        RightBound.transform.position += new Vector3(Width / 2f, 0f, 0f);
    }

    public void SetupBoundsHeight()
    {
        CheckBounds();

        TopBound.transform.position += new Vector3(0f, Height / 2f, 0f);
        DownBound.transform.position += new Vector3(0f, -Height / 2f, 0f);
        LeftBound.transform.localScale += (-LeftBound.transform.localScale) + new Vector3(1f, Height, 1f);
        RightBound.transform.localScale += (-RightBound.transform.localScale) + new Vector3(1f, Height, 1f);
    }

    public void ClearBounds()
    {
        if (Bounds == null)
        {
            return;
        }

        var childsToDestroy = new List<GameObject>();

        for (int i = 0; i < Bounds.transform.childCount; ++i)
        {
            childsToDestroy.Add(Bounds.transform.GetChild(i).gameObject);
        }

        foreach (var child in childsToDestroy)
        {
            DestroyImmediate(child);
        }
    }

    private void CheckBounds()
    {
        if (Bounds == null)
        {
            Bounds = CreateEmpy("Bounds");
        }

        if (TopBound == null)
        {
            TopBound = CreateBound("Bound Top");
        }

        if (DownBound == null)
        {
            DownBound = CreateBound("Bound Down");
        }

        if (LeftBound == null)
        {
            LeftBound = CreateBound("Bound Left");
        }

        if (RightBound == null)
        {
            RightBound = CreateBound("Bound Right");
        }
    }

    private GameObject CreateEmpy(string name)
    {
        var empty = new GameObject(name);

        empty.transform.position = transform.position;
        empty.transform.parent = transform;

        return empty;
    }

    private GameObject CreateBound(string name)
    {
        var bound = GameObject.CreatePrimitive(PrimitiveType.Cube);

        DestroyImmediate(bound.GetComponent<BoxCollider>());
        bound.AddComponent(typeof(BoxCollider2D));
        bound.name = string.Format(name);
        bound.transform.position = transform.position;
        bound.transform.parent = Bounds.transform;

        return bound;
    }
}
