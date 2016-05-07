using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Notification
{
	public string Title;
	public string NotificationText;

	public Notification(string title, string notificationText)
	{
		Title = title;
		NotificationText = notificationText;
	}
	
	public void Dismiss() 
	{

	}

	public void Build(GameObject gameObject)
	{
		GameObject newCanvas = new GameObject("Canvas");
		Canvas c = newCanvas.AddComponent<Canvas>();
		Button dismissButton = new Button();
		RectTransform buttonRectTransform = (RectTransform) dismissButton.transform;
		buttonRectTransform.anc
	}
}

