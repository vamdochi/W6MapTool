using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRotateAction : UIBaseAction {


    // Lerp 필요시 추가
    
    public void RotateActionZ(float z)
    {
        transform.rotation = Quaternion.Euler(0, 0, z) * transform.rotation;
    }

}
