using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMoveAction : UIBaseAction {

    // Lerp 필요시 추가
    public void MoveActionX( float x)
    {
        transform.position = new Vector3(
            transform.position.x + x  * _mainCanvas.transform.localScale.x, 
            transform.position.y, 
            transform.position.z);
    }
}
