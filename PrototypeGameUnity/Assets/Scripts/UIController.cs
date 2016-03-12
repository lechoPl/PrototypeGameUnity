using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using Assets.Scripts.Game;

public class UIController : MonoBehaviour {
	public GameObject GameMenuPanel;
	public GameObject MoveUIPanel;

	public Text TimeLabel;

	// Use this for initialization
	void Start () {
		GameLogic.Instance.StartRound ();
	}
	
	// Update is called once per frame
	void Update () {
		RefreshTimer ();
	}

	
	private void RefreshTimer() {
		GameLogic.Instance.ChechRoundFinished ();	
		TimeLabel.text = GetTimeLeftString ();
	}

	
	private string GetTimeLeftString() {
		float timeLeft = GameLogic.Instance.GetTimeLeft ();
		int minutes = (int) Mathf.Abs(timeLeft / 60);
		int seconds = (int) Mathf.Abs(timeLeft - minutes * 60);
		return string.Format("{0:00}:{1:00}", minutes, seconds);
	}
}
