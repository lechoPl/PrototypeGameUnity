using Assets.Scripts.Game;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    public int NextFieldID;

    void OnCollisionEnter2D(Collision2D coll)
    {
        var playerControlerGameObject = coll.gameObject.GetComponent<PlayerController>();
        if(playerControlerGameObject != null)
        {
            GameLogic.Instance.PlayerToNextField(playerControlerGameObject, NextFieldID);
        }
    }
}
