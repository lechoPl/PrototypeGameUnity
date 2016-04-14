using Assets.Scripts.Game;
using Assets.Scripts.Game.Enums;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerToCalibrateCameraOffset;
    public float smoothingX = 5f;
    public float smoothingY = 5f;
    public float smoothingSize = 5f;

    public float minSize = 5f;
    public float maxSize = 100f;

    private Vector3 offset;
    private float originalSize;
    private float targetSiez;
    private Vector3 _targetPos;

    private GameState _lastGameSate;


    // MonoBehavior methods
    //***********************************
    void Start()
    {
        offset = transform.position - playerToCalibrateCameraOffset.position;
        originalSize = Camera.main.orthographicSize;
        targetSiez = originalSize;

		_lastGameSate = GameLogic.Instance.CurrentRound.GameState;

        _targetPos = transform.position;
    }

    void FixedUpdate()
    {
		var currentGameState = GameLogic.Instance.CurrentRound.GameState;

        if (currentGameState != _lastGameSate)
        {
            SwitchCameraMode();
            _lastGameSate = currentGameState;
        }

        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetSiez, smoothingSize);

        if (currentGameState == GameState.Move)
        {
            UpdateMove();
        }
        else
        {
            UpdateMenu();
        }
    }

    public void MoveByScreen(Vector3 diff)
    {
		if (GameLogic.Instance.CurrentRound.GameState != GameState.Menu)
        {
            return;
        }

        diff.z = 0f;
        _targetPos += diff;
    }

    public void ZoomIn(float val)
    {
		if (GameLogic.Instance.CurrentRound.GameState != GameState.Menu)
        {
            return;
        }

        if (targetSiez < maxSize)
        {
            targetSiez += val;
        }
    }

    public void ZoomOut(float val)
    {
		if (GameLogic.Instance.CurrentRound.GameState != GameState.Menu)
        {
            return;
        }

        if (targetSiez > minSize)
        {
            targetSiez -= val;
        }
    }

    // Private methods
    //***********************************
    private void UpdateMove()
    {
        var currentPlayerTransform = GameLogic.Instance.CurrentRound.GetCurrentPlayer().transform;

        var targetCamPos = currentPlayerTransform.position + offset;

        transform.position = new Vector3(
            Mathf.Lerp(transform.position.x, targetCamPos.x, smoothingX),
            Mathf.Lerp(transform.position.y, targetCamPos.y, smoothingY),
            targetCamPos.z);
    }

    private void UpdateMenu()
    {
        transform.position = new Vector3(
                Mathf.Lerp(transform.position.x, _targetPos.x, smoothingX),
                Mathf.Lerp(transform.position.y, _targetPos.y, smoothingY),
                transform.position.z);
    }

    private void SwitchCameraMode()
    {
		if (GameLogic.Instance.CurrentRound.GameState == GameState.Move)
        {
            targetSiez = originalSize;
        }
        else
        {
            var maxBoardSize = GameLogic.Instance.BoardSize.MaxSize;
            if (maxBoardSize.HasValue)
            {
                targetSiez = maxBoardSize.Value / 2;
                maxSize = targetSiez;
            }
            else
            {
                targetSiez = originalSize;
            }

            _targetPos = new Vector3(0, 0, _targetPos.z);
        }
    }
}