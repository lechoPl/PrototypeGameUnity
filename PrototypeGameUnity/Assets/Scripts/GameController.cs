using Assets.Scripts.Game;
using Assets.Scripts.Game.Enums;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //for Debug
    [Header("Debug options")]
    public bool DebugMode = false;
    public GameState startGameState = GameState.Menu;

    // MonoBehavior methods
    //***********************************
    public void Awake()
    {
        GameLogic.Instance.DebugMode = DebugMode;

        if(GameLogic.Instance.DebugMode)
        {
            GameLogic.Instance.CurrentRound.GameState = startGameState;
        }
    }

    void Update()
    {
        CheckInput();

        if (!GameLogic.Instance.DebugMode)
        {
            GameLogic.Instance.CurrentRound.CheckRoundFinished();
        }
    }

    // Private methods
    //***********************************
    private void CheckInput()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (GameLogic.Instance.CurrentRound.GameState == GameState.Move)
            {
                GameLogic.Instance.CurrentRound.GameState = GameState.Menu;
            }
            else
            {
                GameLogic.Instance.CurrentRound.GameState = GameState.Move;
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
