using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CustomKeyCode
{
    CameraZoomIn,
    CameraZoomOut,
    CameraMove,
    AllocateTile,
    RestoreHistory,
    FrontRestoreHistory
}


public class InputSystem : SingleTon<InputSystem> {

    public bool GetKeyCode( CustomKeyCode keyCode )
    {
        switch(keyCode)
        {
            case CustomKeyCode.CameraZoomIn:
                return Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.Z) && !Input.GetKey(KeyCode.LeftAlt);
            case CustomKeyCode.CameraZoomOut:
                return Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.Z) && Input.GetKey(KeyCode.LeftAlt);
            case CustomKeyCode.AllocateTile:
                return Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.Z);
            case CustomKeyCode.CameraMove:
                return Input.GetMouseButton(0) && Input.GetKey(KeyCode.Space);
            case CustomKeyCode.RestoreHistory:
                return Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Z) && !Input.GetKey(KeyCode.LeftAlt);
            case CustomKeyCode.FrontRestoreHistory:
                return Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Z) && Input.GetKey(KeyCode.LeftAlt);

        }
        return false;
    }
}
