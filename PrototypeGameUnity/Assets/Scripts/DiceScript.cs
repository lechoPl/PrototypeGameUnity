using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using Assets.Scripts.Game;

public class DiceScript : MonoBehaviour {
	private const float RotationTime = 2;
	
	private float rollStartTime = -RotationTime;
	private Vector3 rollRotation;

	private int RollNumber;
	private bool IsRolling;

	// Use this for initialization
	void Start () {
		RollNumber = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if (IsRolling) {
			if (rollStartTime + RotationTime >= Time.time) {
				UpdateDice ();
			} else {
				EndRoll();
			}
		}
	}

	private void UpdateDice() {
		float progress = (Time.time - rollStartTime) / RotationTime;
		this.gameObject.transform.rotation = Quaternion.Euler (progress * rollRotation);
	}

	public int GetRollNumber() {
		return RollNumber;
	}

	public void PrepareRoll() {
		int x = Random.Range (5, 15);
		int y = Random.Range (5, 15);
		int z = Random.Range (5, 15);
		
		rollRotation = new Vector3 (90 * z, 90 * x, 90 * y); // For Quaternion.Euler(Vector3(z, x, y))
		print (rollRotation);
		rollStartTime = Time.time;

		IsRolling = true;
	}

	private void EndRoll() {
		IsRolling = false;

		print (this.gameObject.transform.forward);
		RollNumber = CalculateRoll ();

		print ("Rolled " + RollNumber);
		
		GameLogic.Instance.EndRound ();
		GameLogic.Instance.StartRound (RollNumber);
	}

	private int CalculateRoll() {
		Vector3 up = this.gameObject.transform.forward;

		if (up.x > 0.5) {
			return 1;
		} else if (up.x < -0.5) {
			return 2;
		} else if (up.y > 0.5) {
			return 3;
		} else if (up.y < -0.5) {
			return 4;
		} else if (up.z > 0.5) {
			return 5;
		} else {
			return 6;
		}
	}

	private void OnMouseDown() {
		RollDice ();
	}

	public void RollDice() {
		if (!IsRolling) {
			//RollNumber = (int)Mathf.Floor (Random.Range (1, 6)) + 1;
			//print (RollNumber);

			PrepareRoll ();
		}
	}

}
