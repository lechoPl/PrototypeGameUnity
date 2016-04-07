using Assets.Scripts.Game;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    public int NextFieldID;

    void OnCollisionEnter2D(Collision2D coll)
    {
        var playerGameObject = coll.gameObject.GetComponent<Player>();
        if(playerGameObject != null)
        {
            GameLogic.Instance.PlayerToNextField(playerGameObject, NextFieldID);
        }
    }
}
