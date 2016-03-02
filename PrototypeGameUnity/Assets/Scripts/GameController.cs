using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    [Header("UI")]
    public GameObject GameMenuPanel;
    public GameObject MoveUI;


    // MonoBehavior methods
    //***********************************
    void Update ()
    {
        CheckInput();
	}

    // Private methods
    //***********************************
    private void CheckInput()
    {
        if (Input.GetKeyUp(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchUI();
        }
    }

    private void SwitchUI()
    {
        if(GameMenuPanel != null)
        {
            GameMenuPanel.SetActive(!GameMenuPanel.activeSelf);
        }

        if(MoveUI != null)
        {
            MoveUI.SetActive(!GameMenuPanel.activeSelf);
        }
    }
}
