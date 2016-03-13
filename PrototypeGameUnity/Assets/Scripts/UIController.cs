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

	}
	
	// Update is called once per frame
	void Update () {
		RefreshTimer ();
	}

	
	private void RefreshTimer() {
		if (GameLogic.Instance.GetTimeLeft() >= 0) {
			TimeLabel.text = GetTimeLeftString ();
		}
	}

	
	private string GetTimeLeftString() {
		float timeLeft = GameLogic.Instance.GetTimeLeft ();
		int minutes = (int) Mathf.Abs(timeLeft / 60);
		int seconds = (int) Mathf.Abs(timeLeft - minutes * 60);
		return string.Format("{0:00}:{1:00}", minutes, seconds);
	}
}
