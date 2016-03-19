using UnityEngine;
using System.Collections;

public class Dice3dScript : MonoBehaviour {
	private const float RotationTime = 2;

	private float rollStartTime = -RotationTime;
	private Vector3 rollRotation;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (rollStartTime + RotationTime >= Time.time) {
			UpdateDice();
		}
	}

	public void RollDice() {
		int x = Random.Range (5, 15);
		int y = Random.Range (5, 15);
		int z = Random.Range (5, 15);

		rollRotation = new Vector3 (90 * z, 90 * x, 90 * y); // For Quaternion.Euler(Vector3(z, x, y))
		print (rollRotation);
		rollStartTime = Time.time;
	}

	private void UpdateDice() {
		float progress = (Time.time - rollStartTime) / RotationTime;
		this.gameObject.transform.rotation = Quaternion.Euler (progress * rollRotation);
	}

	private void OnMouseDown() {
		RollDice ();
	}
}
