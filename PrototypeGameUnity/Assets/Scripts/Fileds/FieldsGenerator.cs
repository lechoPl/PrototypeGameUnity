using Assets.Scripts.Common;
using Assets.Scripts.Fileds.ValueObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FieldsGenerator : MonoBehaviour
{
    [Range(0, 50)]
    public int NumberOfFields = 1;
    public bool refreshOnly;

    [HideInInspector()]
    public bool prefabsExpended = false;
    [HideInInspector()]
    public List<FieldPrefab> PrefabLists = new List<FieldPrefab>();


    private float RadiusX;
    private float RadiusY;

    // MonoBehaviour methods
    //***********************************
    void Start ()
    {
        ClearFields();
        CreateFields();
    }
	
    // Public methods
    //***********************************
    public void CreateFields()
    {
        if (NumberOfFields == 0 || PrefabLists == null)
        {
            return;
        }

        var fieldMaxWidth = PrefabLists.Max(f => f.Prefab.Width);
        var fieldMaxHeight = PrefabLists.Max(f => f.Prefab.Height);
        RadiusX = fieldMaxWidth * (NumberOfFields - 1) / 2f;
        RadiusY = fieldMaxHeight * (NumberOfFields - 1) / 2f;

        int id = 0;
        for (float i = 0; i < 360.0f; i = i + 360.0f / NumberOfFields)
        {
            var fieldPrefab = PrefabLists[Utilities.rand.Next(PrefabLists.Count)].Prefab;
            var fieldPosition = this.transform.position + new Vector3(RadiusX * Mathf.Cos(i * Mathf.Deg2Rad), RadiusY * Mathf.Sin(i * Mathf.Deg2Rad), 0f);

            var field = Instantiate(fieldPrefab, fieldPosition, Quaternion.Euler(0, 0, 0)) as Field;
            field.gameObject.name = string.Format("Field {0} - not registered - ({1})", id, fieldPrefab.gameObject.name);
            field.SetEndPointNextFieldId(id == NumberOfFields - 1 ? 0 : id + 1);
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
