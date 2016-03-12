using System.Collections.Generic;
using UnityEngine;

public class FieldsGenerator : MonoBehaviour
{
    [Range(1, 50)]
    public int NumberOfFields = 1;
    public int Width;
    public int Height;

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
        for (int i = 0; i < NumberOfFields; i++)
        {
            var newField = new GameObject(string.Format("Field {0} - not registered", transform.childCount));
            newField.AddComponent<Field>();
            var field = newField.GetComponent<Field>();

            field.Width = Width;
            field.Height = Height;
            field.transform.position = this.transform.position + new Vector3(i * Width, 0f, 0f);
            field.transform.parent = this.transform;
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
