using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game;
using Assets.Scripts.Game.Enums;

public class UIController : MonoBehaviour
{
    public GameObject GameMenuPanel;
	public GameObject MoveUIPanel;

	public Text TimeLabel;

    // MonoBehavior methods
    //***********************************
    void Start ()
    {

	}
	
	void Update ()
    {
        CheckInput();
        RefreshTimer();
    }

    // Private methods
    //***********************************
    private void RefreshTimer()
    {
		if (GameLogic.Instance.GetTimeLeft() >= 0)
        {
			TimeLabel.text = GetTimeLeftString ();
		}
	}
	
	private string GetTimeLeftString()
    {
		float timeLeft = GameLogic.Instance.GetTimeLeft ();
		int minutes = (int) Mathf.Abs(timeLeft / 60);
		int seconds = (int) Mathf.Abs(timeLeft - minutes * 60);
		return string.Format("{0:00}:{1:00}", minutes, seconds);
	}

    private void CheckInput()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SwitchUI();
        }
    }

    private void SwitchUI()
    {
        if(GameLogic.Instance.CurrentGameState == GameState.Menu)
        {
            GameLogic.Instance.SetGameState(GameState.Move);
        }
        else
        {
            GameLogic.Instance.SetGameState(GameState.Menu);
        }

        if (GameMenuPanel != null)
        {
            GameMenuPanel.SetActive(GameLogic.Instance.CurrentGameState == GameState.Menu);
        }

        if (MoveUIPanel != null)
        {
            MoveUIPanel.SetActive(GameLogic.Instance.CurrentGameState == GameState.Move);
        }


    }
}
