using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class MessagesController : MonoBehaviour
{
	public GameObject MessagePanel;

	public Image Thumbnail;
	public Text Title;
	public Text Text;
	public Button PositiveButton;
	public Button NegativeButton;

	void Update()
	{
		ShowHideMessages();
	}

	private void ShowHideMessages()
	{
		Mailbox Mailbox = GameLogic.GetCurrentMailbox();

		MessagePanel.SetActive ( Mailbox != null );
	}

	private void UpdateListView()
	{

	}
}
