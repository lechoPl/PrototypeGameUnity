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
		GameLogic.Instance.CheckRoundFinished ();
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
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if(GameLogic.Instance.CurrentGameState == GameState.Move)
            {
                GameLogic.Instance.SetGameState(GameState.Menu);
            }
            else
            {
                GameLogic.Instance.SetGameState(GameState.Move);

            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Player player = GameLogic.Instance.GetCurrentPlayer();
            Field field = GameLogic.Instance.GetCurrentPlayer().CurrentField;
            GameLogic.Instance.BuyField(player, field);
        }
    }
}
