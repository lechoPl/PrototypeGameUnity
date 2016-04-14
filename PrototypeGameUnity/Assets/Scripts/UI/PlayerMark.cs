using Assets.Scripts.Game;
using Assets.Scripts.Game.Enums;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMark : MonoBehaviour
{
    public Text PlayerText;
    public Image Background;

    // Public methods
    //***********************************
    public void SetPlayerText(int id)
    {
        if(PlayerText != null)
        {
            PlayerText.text = string.Format("P{0}", id);
        }
    }

    public void SetColor(Material material)
    {
        if(Background != null)
        {
            Background.color = material.color;
        }
    }

    public void CheckActive()
    {
        gameObject.SetActive(GameLogic.Instance.CurrentRound.GameState == GameState.Menu);
    }
}
