using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Assets.Scripts.Game;

public class MessageListController : MonoBehaviour 
{
	public Toggle ListItem;
	public GameObject ScrollViewContent;

	public MessageDisplayController MessageDisplayController;
	
	public void Update()
	{
		Refresh();
	}

	private void Refresh()
	{
		MessageDisplayController.DisplayedMessage = null;

		int max = Mathf.Max (GetMessages().Count, ScrollViewContent.transform.childCount);

		for(int i=0; i < max; i++)
		{
			if(i >= GetMessages().Count)
			{
				Destroy(ScrollViewContent.transform.GetChild(i).gameObject);
			} else {
				Message m = GetMessages()[i];
				Toggle listItem;

				if(i >= ScrollViewContent.transform.childCount)
				{
					// If there's no view of index i create one
					listItem = Instantiate(ListItem) as Toggle;
					listItem.transform.SetParent(ScrollViewContent.transform, false);
				} else {
					// Otherwise get handle to this component
					listItem = ScrollViewContent.transform.GetChild(i).GetComponent<Toggle>();
				}

				listItem.name = i.ToString();
				
				Text title = listItem.transform.Find("Title").gameObject.GetComponent<Text>();
				title.text = m.Title;
				Text message = listItem.transform.Find("Message").gameObject.GetComponent<Text>();
				message.text = m.Text;
				listItem.gameObject.SetActive(true);

				if(listItem.isOn)
				{
					MessageDisplayController.DisplayedMessage = m;
				}
			}
		}
	}

	private IList<Message> GetMessages()
	{
		return GameLogic.Instance.CurrentRound.GetCurrentPlayer().Messages;
	}
}
