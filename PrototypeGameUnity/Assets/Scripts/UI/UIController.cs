using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game;
using Assets.Scripts.Game.Enums;

public class UIController : MonoBehaviour
{
    public GameObject MoveUIPanel;
    public GameObject PlayerStatsPanel;

    public GameObject BottomPanel;
    public GameObject Board;

    public Text TimeLabel;

    // MonoBehavior methods
    //***********************************
    void Update()
    {
        CheckInput();
        ShowHidePlayerStats();
		RefreshPlayerStats();
        RefreshTimer();
    }

    // Private methods
    //***********************************
    private void RefreshTimer()
    {
        if (GameLogic.Instance.GetTimeLeft() >= 0)
        {
            TimeLabel.text = GetTimeLeftString();
        }
    }

    private void ShowHidePlayerStats()
    {
        if (PlayerStatsPanel != null)
        {
            PlayerStatsPanel.SetActive(GameLogic.Instance.CurrentGameState == GameState.Menu);
        }
        if (BottomPanel != null)
        {
            BottomPanel.SetActive(GameLogic.Instance.CurrentGameState == GameState.Menu);
        }
    }

	private void RefreshPlayerStats()
	{
		print (transform.childCount);
		for(int i=0; i<PlayerStatsPanel.transform.childCount; i++)
		{
			if(i < GameLogic.Instance.GetPlayers().Count)
			{
				PlayerStatistics playerStatistics = PlayerStatsPanel.transform.GetChild(i).gameObject.GetComponent(typeof(PlayerStatistics)) as PlayerStatistics;
				playerStatistics.SetPlayer(GameLogic.Instance.GetPlayers()[i]);
			}
			PlayerStatsPanel.transform.GetChild(i).gameObject.SetActive(i < GameLogic.Instance.GetPlayers().Count);
		}
	}

    private string GetTimeLeftString()
    {
        float timeLeft = GameLogic.Instance.GetTimeLeft();
        int minutes = (int)Mathf.Abs(timeLeft / 60);
        int seconds = (int)Mathf.Abs(timeLeft - minutes * 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void CheckInput()
    {
        
    }

    public void ShowBoard()
    {
        Board.SetActive(!Board.activeSelf);
    }

    public void ShowBoard(bool show)
    {
        Board.SetActive(show);
    }
}
