using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game;

public class UIController : MonoBehaviour
{
	public GameObject MoveUIPanel;
	public GameObject PlayerStatsPanel;

	public GameObject BottomPanel;
	public GameObject Board;
	
	public Text TimeLabel;

    // MonoBehavior methods
    //***********************************
    void Start ()
    {

	}
	
	void Update ()
    {
        CheckInput();
		ShowHidePlayerStats ();
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

	private void ShowHidePlayerStats()
	{
		if (PlayerStatsPanel != null)
		{
			PlayerStatsPanel.SetActive(GameLogic.Instance.CheckRoundFinished());
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

	public void ShowBoard() {
		Board.SetActive (!Board.activeSelf);
	}

	public void ShowBoard(bool show) {
		Board.SetActive (show);
	}

    private void SwitchUI()
    {
        if (MoveUIPanel != null)
        {
            //MoveUIPanel.SetActive(!GameMenuPanel.activeSelf);
        }
    }
}
