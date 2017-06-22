using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SwitchOffEvent : UnityEvent
{

}
[System.Serializable]
public class SwitchOnEvent : UnityEvent
{

}

public class SwitchSystem : MonoBehaviour {

    [SerializeField]
    public SwitchOffEvent SwitchOffHandler  = new SwitchOffEvent();
    [SerializeField]
    public SwitchOnEvent SwitchOnHandler    = new SwitchOnEvent();

    private bool _IsSwitchOn;

    public void OnSwitch() {
        _IsSwitchOn = !_IsSwitchOn;

        if (_IsSwitchOn)
            SwitchOnHandler.Invoke();
        else
            SwitchOffHandler.Invoke();
    }

}
