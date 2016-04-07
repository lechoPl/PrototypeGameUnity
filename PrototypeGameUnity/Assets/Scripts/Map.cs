using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour
{
    private static int FIELDS_AMOUNT = 20; // Should be dividable by 4
    private Field[] fields;

    public GameObject fieldPrefab;

    private float fieldSize = 30;
    private float baseX = 80, baseY = 50;

    private enum Field
    {
        START,
        FIELD1,
        FIELD2,
        FIELD3,
        FIELD4
    }

    // Use this for initialization
    void Start()
    {
        //init ();
    }

    private void init()
    {
        fields = new Field[FIELDS_AMOUNT];
        fields[0] = Field.START;

        baseX = -fieldSize * (FIELDS_AMOUNT / 4 + 1) / 2;
        baseY = baseX;

        for (int i = 0; i < FIELDS_AMOUNT; i++)
        {
            fields[i] = Field.FIELD1;
            //gameObject.AddComponent(field);

            GameObject field = (GameObject)Instantiate(fieldPrefab);
            field.transform.SetParent(this.gameObject.transform);

            int offset;
            if (i < FIELDS_AMOUNT / 4)
            {
                field.transform.localPosition = new Vector3(baseX + fieldSize * i, baseY, 0);
            }
            else if (i < 2 * FIELDS_AMOUNT / 4)
            {
                offset = FIELDS_AMOUNT / 4;
                field.transform.localPosition = new Vector3(baseX + offset * fieldSize, baseY + fieldSize * (i - offset), 0);
            }
            else if (i < 3 * FIELDS_AMOUNT / 4)
            {
                offset = 2 * FIELDS_AMOUNT / 4;
                field.transform.localPosition = new Vector3(baseX + fieldSize * (3 * FIELDS_AMOUNT / 4 - i), baseY + fieldSize * FIELDS_AMOUNT / 4, 0);
            }
            else
            {
                offset = 3 * FIELDS_AMOUNT / 4;
                field.transform.localPosition = new Vector3(baseX, baseY + fieldSize * (FIELDS_AMOUNT - i), 0);
            }
        }


    }

    // Update is called once per frame
    void Update()
    {

    }
}
