using Assets.Scripts.Game;
using Assets.Scripts.Game.Enums;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // MonoBehavior methods
    //***********************************
    void Update ()
    {
        CheckInput();
		GameLogic.Instance.CurrentRound.CheckRoundFinished ();
	}

    // Public methods
    //***********************************
    public void EndRound()
    {
		GameLogic.Instance.CurrentRound.EndRound();
    }

	public void StartRound() {

	}

    // Private methods
    //***********************************
    private void CheckInput()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
			if(GameLogic.Instance.CurrentRound.GameState == GameState.Move)
            {
				GameLogic.Instance.CurrentRound.SetGameState(GameState.Menu);
            }
            else
            {
				GameLogic.Instance.CurrentRound.SetGameState(GameState.Move);

            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
			Player player = GameLogic.Instance.CurrentRound.GetCurrentPlayer();
			Field field = GameLogic.Instance.CurrentRound.GetCurrentPlayer().CurrentField;
            GameLogic.Instance.BuyField(player, field);
        }
    }
}
