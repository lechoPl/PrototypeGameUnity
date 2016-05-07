using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Assets.Scripts.Game;

public class MessageDisplayController : MonoBehaviour 
{
	public Image Thumbnail;
	public Text Title;
	public Text Text;
	public Button PositiveButton;
	public Button NegativeButton;

	public Message DisplayedMessage;

	void Update () 
	{
		Refresh ();
	}

	private void Refresh()
	{
		HideComponents ();
		UpdateContentView ();
	}

	private void UpdateContentView()
	{
		if(DisplayedMessage != null)
		{
			DisplayedMessage.Display(this);
		}
	}
	
	private void HideComponents()
	{
		Thumbnail.gameObject.SetActive(false);
		Title.gameObject.SetActive(false);
		Text.gameObject.SetActive(false);
		PositiveButton.gameObject.SetActive(false);
		NegativeButton.gameObject.SetActive(false);
	}
}
