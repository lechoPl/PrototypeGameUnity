using System.Collections.Generic;
using UnityEngine;

public class FieldsGenerator : MonoBehaviour
{
    [Range(0, 50)]
    public int NumberOfFields = 1;
    public int Width;
    public int Height;
    private float Radius;

    public GameObject StartPoint;
    public float StartPointX;
    public float StartPointY;
    public EndPoint EndPoint;
    public float EndPointX;
    public float EndPointY;

    void Start ()
    {
        ClearFields();
        CreateFields();
    }
	
	void Update ()
    {
	
	}

    public void CreateFields()
    {
        if (NumberOfFields == 0)
        {
            return;
        }

        Radius = Mathf.Sqrt(2) * Mathf.Max(Width, Height) * (NumberOfFields - 1) / 4f;

        int id = 0;
        for (float i = 0; i < 360.0f; i = i + 360.0f/NumberOfFields)
        {
            var newField = new GameObject(string.Format("Field {0} - not registered", id));
            newField.AddComponent<Field>();
            var field = newField.GetComponent<Field>();

            field.Width = Width;
            field.Height = Height;
            field.AddStartPoint(StartPoint, new Vector3(StartPointX, StartPointY, 0f));
            field.AddEndPoint(this.EndPoint, new Vector3(EndPointX, EndPointY, 0f), id == NumberOfFields - 1 ? 0 : id + 1);
            newField.transform.position = this.transform.position + new Vector3(Radius * Mathf.Cos(i * Mathf.Deg2Rad), Radius * Mathf.Sin(i * Mathf.Deg2Rad), 0f);
            field.transform.parent = this.transform;

            id++;
        }
    }

    public void ClearFields()
    {

        var childsToDestroy = new List<GameObject>();

        for(int i=0; i<transform.childCount; ++i)
        {
            childsToDestroy.Add(transform.GetChild(i).gameObject);
        }

        foreach(var child in childsToDestroy)
        {
            DestroyImmediate(child);
        }
    }
}
