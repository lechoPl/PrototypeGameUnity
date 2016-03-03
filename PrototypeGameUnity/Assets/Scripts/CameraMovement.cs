using Assets.Scripts.Game;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform playerToCalibrateCameraOffset;
    public float smoothingX = 5f;
    public float smoothingY = 5f;

    private Vector3 offset;

    // MonoBehavior methods
    //***********************************
    void Start()
    {
        offset = transform.position - playerToCalibrateCameraOffset.position;
    }

    void FixedUpdate()
    {
        var currentPlayerTransform = GameLogic.Instance.GetCurrentPlayer().transform;

        var targetCamPos = currentPlayerTransform.position + offset;

        transform.position = new Vector3(
            Mathf.Lerp(transform.position.x, targetCamPos.x, smoothingX),
            Mathf.Lerp(transform.position.y, targetCamPos.y, smoothingY),
            targetCamPos.z);
    }
}