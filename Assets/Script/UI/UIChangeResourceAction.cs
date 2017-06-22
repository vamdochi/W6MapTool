using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChangeResourceAction : UIBaseAction {

    public void ChangeResourceAction( Sprite sprite)
    {
        _ImageComponent.sprite = sprite;
    }
}
