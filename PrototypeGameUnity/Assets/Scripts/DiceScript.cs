using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using Assets.Scripts.Game;

public class DiceScript : MonoBehaviour {
	private static Vector2 RotationOffset = new Vector2 (10, -10);
	private static Vector3 RotationVector = new Vector3 (720 + RotationOffset.x, 720 + RotationOffset.y, 0);

	private const float RotationTime = 2;
	
	private float rollStartTime = -RotationTime;
	private Vector3 rollRotation;

	private Vector3[] Rolls = new Vector3[]{
		new Vector3(270, 0, 0),
		new Vector3(0, 0, 0),
		new Vector3(90, 0, 0),
		new Vector3(-180, 0, 0),
		new Vector3(0, -90, 0),
		new Vector3(0, 90, 0)
	};

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
		progress = Mathf.Min (progress, 1);

		this.gameObject.transform.rotation = Quaternion.Euler (progress * rollRotation);
	}

	public int GetRollNumber() {
		return RollNumber;
	}

	public void PrepareRoll() {

		float roll = Mathf.Floor(Random.Range (1, 7));
		RollNumber = (int) Mathf.Min (roll, 6);
		rollRotation = RotationVector + Rolls[RollNumber-1];

		print (rollRotation);
		rollStartTime = Time.time;

		IsRolling = true;
	}

	private void EndRoll() {
		IsRolling = false;

		UpdateDice ();

		print (this.gameObject.transform.forward);
		print ("Rolled " + RollNumber);

		GameLogic.Instance.StartRound (RollNumber);
	}

	private void OnMouseDown() {
		RollDice ();
	}

	public void RollDice() {
		if (!IsRolling) {
			GameLogic.Instance.EndRound ();

			PrepareRoll ();
		}
	}

}
