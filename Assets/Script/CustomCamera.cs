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
        if(IsOnExpandCameraZoom() )
        {
            _expandPer += _cameraZoomVelocity;
        }
        else if(IsOnReduceCameraZoom())
        {
            _expandPer -= _cameraZoomVelocity;
            if (_expandPer < _minExpandPer)
                _expandPer = _minExpandPer;
        }
        _mainCamera.orthographicSize = _oriZoomSize / _expandPer;
    }

    private void UpdateCameraMove()
    {
        if (IsOnCameraMove())
        {
            Vector3 dist = (_prevMousePosition - Input.mousePosition) * _moveVelocity;
            CameraMove(dist);
        }
        _prevMousePosition = Input.mousePosition;
    }
    private bool IsOnCameraMove() { return Input.GetKey(KeyCode.Space) && Input.GetMouseButton(0); }
    private bool IsOnReduceCameraZoom() { return Input.GetKey(KeyCode.Z) && Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButtonDown(0); }
    private bool IsOnExpandCameraZoom() { return Input.GetKey(KeyCode.Z) && !Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButtonDown(0); }

    private void CameraMove( Vector3 dist)
    {
        transform.Translate(dist);
    }
}
