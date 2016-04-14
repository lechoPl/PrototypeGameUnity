using UnityEngine;
using UnityEngine.EventSystems;

public class ScreenInput : MonoBehaviour
{
    public CameraController CameraController;
    public float ZoomVal = 5f;
    public float Sensivity = 0.1f;

    private Vector3 _lastPosition;
    private readonly int _mouseButton = 2;

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            return;
        }

        var SensivityBySize = Camera.main.orthographicSize / CameraController.maxSize;

        if (Input.GetMouseButtonDown(_mouseButton))
        {
            _lastPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(_mouseButton) || Input.GetMouseButtonUp(_mouseButton))
        {
            Debug.Log("test");
            var diff = Input.mousePosition - _lastPosition;

            CameraController.MoveByScreen(diff * Sensivity * SensivityBySize);
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0) // forward
        {
            CameraController.ZoomIn(ZoomVal * SensivityBySize);
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0) // back
        {
            CameraController.ZoomOut(ZoomVal * SensivityBySize);
        }

        ///TODO: touchInput implemnetation 

        //if (Input.touchCount > 0)
        //{
        //    foreach (var t in Input.touches)
        //    {
        //        var ray = MainCamera.ScreenPointToRay(t.position);
        //        RaycastHit hit;

        //        if (Physics.Raycast(ray, out hit, touchInputMask))
        //        {
        //            if (t.phase == TouchPhase.Ended)
        //            {
        //                Shoot(hit.point);
        //            }

        //            if (t.phase == TouchPhase.Stationary || t.phase == TouchPhase.Moved)
        //            {
        //                Shoot(hit.point);
        //            }
        //        }
        //    }
        //}
    }
}
