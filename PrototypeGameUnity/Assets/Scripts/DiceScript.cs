﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DiceScript : MonoBehaviour {
	public Image Dice;

	private int RollNumber;

	public Sprite[] diceTextures;

	// Use this for initialization
	void Start () {
		RollDice ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public int GetRollNumber() {
		return RollNumber;
	}

	public void RollDice() {
		RollNumber = (int) Mathf.Floor(Random.Range (1, diceTextures.Length)) + 1;
		print (RollNumber);
		Dice.sprite = diceTextures [RollNumber-1];
	}

}
