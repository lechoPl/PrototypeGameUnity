using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Scripts.Game;

public class Message
{
	public string Title;
	public string Text;

	public Message(string title, string text)
	{
		Title = title;
		Text = text;
	}

	virtual public void Display(MessageDisplayController messageDisplayController)
	{
		messageDisplayController.Thumbnail.gameObject.SetActive (true);

		messageDisplayController.Title.gameObject.SetActive(true);
		messageDisplayController.Title.text = Title;

		messageDisplayController.Text.gameObject.SetActive(true);
		messageDisplayController.Text.text = Text;

		messageDisplayController.NegativeButton.gameObject.SetActive(true);
		messageDisplayController.NegativeButton.onClick.AddListener(() => { NegativeAction(); }); 

		messageDisplayController.PositiveButton.gameObject.SetActive(false);
		messageDisplayController.PositiveButton.onClick.AddListener(() => { PositiveAction(); }); 
	}

	virtual protected void NegativeAction()
	{
		GameLogic.Instance.CurrentRound.GetCurrentPlayer().Messages.Remove(this);
	}

	virtual protected void PositiveAction()
	{

	}
}

