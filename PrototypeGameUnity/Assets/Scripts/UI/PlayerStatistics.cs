using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerStatistics : MonoBehaviour {
	private Player Player;

	public Text MoneyText;
	public Text PlayerNameText;
	public Image PlayerAvatar;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Player != null) 
		{
			PlayerNameText.text = Player.name;
			MoneyText.text = Player.Money.ToString();
		}
	}

	public void SetPlayer(Player player) {
		Player = player;
	}
}
