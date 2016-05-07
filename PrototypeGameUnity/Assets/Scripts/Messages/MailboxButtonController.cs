using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Scripts.Game;

public class MailboxButtonController : MonoBehaviour {
	public Image MailboxImage;

	public Sprite MailboxEmptySprite;
	public Sprite MailboxNewMessageSprite;

	void Update () {
		if(GameLogic.Instance.CurrentRound.GetCurrentPlayer().Messages.Count > 0)
		{
			MailboxImage.sprite = MailboxNewMessageSprite;
		}
		else
		{
			MailboxImage.sprite = MailboxEmptySprite;
		}
	}
}
