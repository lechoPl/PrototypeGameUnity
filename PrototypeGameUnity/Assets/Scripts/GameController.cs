using Assets.Scripts.Game;
using UnityEngine;

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

    // MonoBehavior methods
    //***********************************
    public void EndRound()
    {
        GameLogic.Instance.EndRound();
    }

    // Private methods
    //***********************************
    private void CheckInput()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
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
