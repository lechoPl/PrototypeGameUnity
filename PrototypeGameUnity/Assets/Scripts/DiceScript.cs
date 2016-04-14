using UnityEngine;
using System.Collections;

using Assets.Scripts.Game;

public class DiceScript : MonoBehaviour
{
    private static Vector3 RotationVector = new Vector3(720, 720, 0);

    private const float RotationTime = 2;

    private float rollStartTime = -RotationTime;
    private Vector3 rollRotation;

    private Vector3[] Rolls = new Vector3[]{
        new Vector3(280, -10, 0),
        new Vector3(10, -10, 0),
        new Vector3(100, -10, 0),
        new Vector3(-190, -10, 0),
        new Vector3(0, -100, -10),
        new Vector3(0, 80, 10)
    };

    private int RollNumber;
    private bool IsRolling;

    // Use this for initialization
    void Start()
    {
        RollNumber = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsRolling)
        {
            if (rollStartTime + RotationTime >= Time.time)
            {
                UpdateDice();
            }
            else
            {
                EndRoll();
            }
        }
    }

    private void UpdateDice()
    {
        float progress = (Time.time - rollStartTime) / RotationTime;
        progress = Mathf.Min(progress, 1);

        this.gameObject.transform.eulerAngles = progress * rollRotation;
    }

    public int GetRollNumber()
    {
        return RollNumber;
    }

    public void PrepareRoll()
    {

        float roll = Mathf.Floor(Random.Range(1, 7));
        RollNumber = (int)Mathf.Min(roll, 6);
        rollRotation = RotationVector + Rolls[RollNumber - 1];

        print(rollRotation);
        rollStartTime = Time.time;

        IsRolling = true;
    }

    private void EndRoll()
    {
        IsRolling = false;

        UpdateDice();

        print(this.gameObject.transform.forward);
        print("Rolled " + RollNumber);

		StartCoroutine(DelayStart(2));
    }

	private IEnumerator DelayStart(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		GameLogic.Instance.StartRound(RollNumber);
	}

    private void OnMouseDown()
    {
        RollDice();
    }

    public void RollDice()
    {
        if (!IsRolling)
        {
            GameLogic.Instance.EndRound();

            PrepareRoll();
        }
    }

}
