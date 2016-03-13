using Assets.Scripts.Game;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // MonoBehavior methods
    //***********************************
    void Update ()
    {
        CheckInput();
		GameLogic.Instance.ChechRoundFinished ();
	}

    // Public methods
    //***********************************
    public void EndRound()
    {
        GameLogic.Instance.EndRound();
    }

	public void StartRound() {

	}

    // Private methods
    //***********************************
    private void CheckInput()
    {
        //nothing
    }
}
