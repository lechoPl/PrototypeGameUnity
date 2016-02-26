using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player;
    public float smoothingX = 5f;
    public float smoothingY = 5f;

    private Vector3 offset;

    // MonoBehavior methods
    //***********************************
    void Start()
    {
        offset = transform.position - player.position;
    }

    void FixedUpdate()
    {
        var targetCamPos = player.position + offset;

        transform.position = new Vector3(
            Mathf.Lerp(transform.position.x, targetCamPos.x, smoothingX),
            Mathf.Lerp(transform.position.y, targetCamPos.y, smoothingY),
            targetCamPos.z);
    }
}