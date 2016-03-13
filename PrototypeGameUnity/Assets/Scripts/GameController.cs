using Assets.Scripts.Game;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("UI")]
    public GameObject GameMenuPanel;
    public GameObject MoveUI;

    // MonoBehavior methods
    //***********************************
    void Update ()
    {
        CheckInput();
		GameLogic.Instance.ChechRoundFinished ();
	}

    // MonoBehavior methods
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
        if (Input.GetKeyUp(KeyCode.Escape))// || Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchUI();
        }
    }

    private void SwitchUI()
    {
        if(GameMenuPanel != null)
        {
            GameMenuPanel.SetActive(!GameMenuPanel.activeSelf);
        }

        if(MoveUI != null)
        {
            MoveUI.SetActive(!GameMenuPanel.activeSelf);
        }
    }
}
