using UnityEngine;

public class TestScript : MonoBehaviour
{
    public float Speed = 6f;

	void Start ()
    {
	
	}
	
	void Update ()
    {
        transform.Rotate(new Vector3(Speed * Time.deltaTime, 0, 0));
	}
}
