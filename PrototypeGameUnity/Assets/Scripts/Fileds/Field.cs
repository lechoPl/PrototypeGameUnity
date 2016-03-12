﻿using Assets.Scripts.Game;
using UnityEngine;

public class Field : MonoBehaviour
{
    private int _width;
    public int Width
    {
        get
        {
            return _width;
        }

        set
        {
            _width = value;
            setupBoundsWidth();
        }
    }
    private int _height;
    public int Height
    {
        get
        {
            return _height;
        }
        
        set
        {
            _height = value;
            setupBoundsHeight();
        }
    }

    private GameObject TopBound;
    private GameObject DownBound;
    private GameObject LeftBound;
    private GameObject RightBound;

    // MonoBehaviour methods
    //***********************************
    void Awake ()
    {
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

    // Private methods
    //***********************************
    private void setupBoundsWidth()
    {
        CheckBounds();

        TopBound.transform.localScale += (-TopBound.transform.localScale) + new Vector3(Width, 1f, 1f);
        DownBound.transform.localScale += (-DownBound.transform.localScale) + new Vector3(Width, 1f, 1f);
        LeftBound.transform.position = new Vector3(-Width / 2f, 0f, 0f);
        RightBound.transform.position = new Vector3(Width / 2f, 0f, 0f);
    }

    private void setupBoundsHeight()
    {
        CheckBounds();

        TopBound.transform.position = new Vector3(0f, Height / 2f, 0f);
        DownBound.transform.position = new Vector3(0f, -Height / 2f, 0f);
        LeftBound.transform.localScale += (-LeftBound.transform.localScale) + new Vector3(1f, Height, 1f);
        RightBound.transform.localScale += (-RightBound.transform.localScale) + new Vector3(1f, Height, 1f);
    }

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
        bound.transform.parent = this.transform;

        return bound;
    }
}