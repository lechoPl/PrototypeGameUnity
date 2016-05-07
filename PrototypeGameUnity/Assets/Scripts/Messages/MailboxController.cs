using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Game;

public class MailboxController : MonoBehaviour
{
	public MessageDisplayController MessageDisplayController;
	public MessageListController MessageListController;

	public void ShowHide()
	{
		gameObject.SetActive(!gameObject.activeSelf);
	}
}
