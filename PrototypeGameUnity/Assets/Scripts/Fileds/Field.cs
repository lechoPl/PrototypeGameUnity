using Assets.Scripts.Game;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public int Width;
    public void SetWidth(int val)
    {
        Width = val;
        SetupBoundsWidth();
    }
    public int Height;
    public void SetHeight(int val)
    {
        Height = val;
        SetupBoundsHeight();
    }
    public Vector3 SartPointPosition
    {
        get
        {
            return _startPoint.transform.position;
        }
    }

    private GameObject _startPoint;
    private EndPoint _endPoint;
    private GameObject TopBound;
    private GameObject DownBound;
    private GameObject LeftBound;
    private GameObject RightBound;

    // MonoBehaviour methods
    //***********************************
    void Awake ()
    {
        Clear();
        SetupBoundsWidth();
        SetupBoundsHeight();
        GameLogic.Instance.RegisterFiled(this);
	}

    void OnDestroy()
    {
        GameLogic.Instance.RemoveField(this);
    }

    // Public methods
    //***********************************
    public void SetFieldName(string name)
    {
        this.name = name;
    }

    public void AddStartPoint(GameObject sp, Vector3 pos)
    {
        _startPoint = GameObject.Instantiate(sp);
        _startPoint.transform.position = pos;
        _startPoint.transform.parent = transform;
    }

    public void AddEndPoint(EndPoint ep, Vector3 pos, int nextFieldId)
    {
        if(ep == null)
        {
            Debug.Log("Wrong EndPoint");
            return;
        }

        _endPoint = GameObject.Instantiate(ep);
        _endPoint.transform.position = pos;
        _endPoint.transform.parent = transform;
        _endPoint.NextFieldID = nextFieldId;
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

    public void Clear()
    {

        var childsToDestroy = new List<GameObject>();

        for (int i = 0; i < transform.childCount; ++i)
        {
            childsToDestroy.Add(transform.GetChild(i).gameObject);
        }

        foreach (var child in childsToDestroy)
        {
            DestroyImmediate(child);
        }
    }

    // Private methods
    //***********************************
    private void CheckBounds()
    {
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

    private GameObject CreateBound(string name)
    {
        var bound = GameObject.CreatePrimitive(PrimitiveType.Cube);

        DestroyImmediate(bound.GetComponent<BoxCollider>());
        bound.AddComponent(typeof(BoxCollider2D));
        bound.name = string.Format(name);
        bound.transform.position = transform.position;
        bound.transform.parent = transform;

        return bound;
    }
}
