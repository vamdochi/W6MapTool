using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBaseAction : MonoBehaviour {

    protected static Canvas _mainCanvas;
    protected Image _ImageComponent;
    // Use this for initialization
    void Start()
    {
        _ImageComponent = GetComponent<Image>();
        if (_ImageComponent == null)
            Debug.LogError("Error : This class must have Image Component(" + this.GetType() + ")");

        if (_mainCanvas == null)
            _mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

}
