using Assets.Scripts.Game;
using Assets.Scripts.Game.Enums;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform playerToCalibrateCameraOffset;
    public float smoothingX = 5f;
    public float smoothingY = 5f;
    public float smoothingSize = 5f;

    private Vector3 offset;
    private float originalSize;
    private float targetSiez;

    private GameState _lastGameSate;

    // MonoBehavior methods
    //***********************************
    void Start()
    {
        offset = transform.position - playerToCalibrateCameraOffset.position;
        originalSize = Camera.main.orthographicSize;
        targetSiez = originalSize;

        _lastGameSate = GameLogic.Instance.CurrentGameState;
    }

    void FixedUpdate()
    {
        var currentGameState = GameLogic.Instance.CurrentGameState;

        if(currentGameState != _lastGameSate)
        {
            SwitchCameraMode();
            _lastGameSate = currentGameState;
        }

        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetSiez, smoothingSize);

        if (currentGameState == GameState.Move)
        {
            var currentPlayerTransform = GameLogic.Instance.GetCurrentPlayer().transform;

            var targetCamPos = currentPlayerTransform.position + offset;

            transform.position = new Vector3(
                Mathf.Lerp(transform.position.x, targetCamPos.x, smoothingX),
                Mathf.Lerp(transform.position.y, targetCamPos.y, smoothingY),
                targetCamPos.z);
        }
        else
        {
            transform.position = new Vector3(
                Mathf.Lerp(transform.position.x, 0, smoothingX),
                Mathf.Lerp(transform.position.y, 0, smoothingY),
                transform.position.z);
        }
    }

    // Private methods
    //***********************************
    private void SwitchCameraMode()
    {
        if(GameLogic.Instance.CurrentGameState == GameState.Move )
        {
            targetSiez = originalSize;
        }
        else
        {
            var maxBoardSize = GameLogic.Instance.BoardSize.MaxSize;
            if(maxBoardSize.HasValue)
            {
                targetSiez = maxBoardSize.Value / 2;
            }
            else
            {
                targetSiez = originalSize;
            }
        }
    }
}