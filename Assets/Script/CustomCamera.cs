using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCamera : MonoBehaviour {

    private const float _moveVelocity = 0.01f;
    private const float _oriZoomSize = 500.0f;
    private const int _cameraZoomVelocity = 50;
    private const int _minExpandPer = 50;

    private Camera _mainCamera;

    private Vector3 _prevMousePosition;
    private int _expandPer = 100;
	// Use this for initialization
	void Start () {
        _mainCamera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        UpdateCameraMove();
        UpdateCameraZoom();
    }


    private void UpdateCameraZoom()
    {
        if(InputSystem.Instance.GetKeyCode(CustomKeyCode.CameraZoomIn))
        {
            _expandPer += _cameraZoomVelocity;
        }
        else if(InputSystem.Instance.GetKeyCode(CustomKeyCode.CameraZoomOut))
        {
            _expandPer -= _cameraZoomVelocity;
            if (_expandPer < _minExpandPer)
                _expandPer = _minExpandPer;
        }
        _mainCamera.orthographicSize = _oriZoomSize / _expandPer;
    }

    private void UpdateCameraMove()
    {
        if (InputSystem.Instance.GetKeyCode(CustomKeyCode.CameraMove))
        {
            Vector3 dist = (_prevMousePosition - Input.mousePosition) * _moveVelocity / ( _expandPer * 0.01f );
            CameraMove(dist);
        }
        _prevMousePosition = Input.mousePosition;
    }

    private void CameraMove( Vector3 dist)
    {
        transform.Translate(dist);
    }
}
