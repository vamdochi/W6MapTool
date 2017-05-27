using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[HideInInspector]
public class SellectTileSystem : MonoBehaviour {

    private Vector2 _startDragPosition;
    private Vector2 _endDragPosition;
    private Camera _mainCamera;

    void Start()
    {
        _mainCamera = Camera.main;
    }
    void Update()
    {
        Vector2 position = _mainCamera.ScreenToWorldPoint(Input.mousePosition);

        if ( Input.GetKeyDown(KeyCode.LeftShift))
            _startDragPosition = position;
        else if( Input.GetKey(KeyCode.LeftShift))
            _endDragPosition = position;
        else
        {
            _startDragPosition = position;
            _endDragPosition = position;
        }

        //if( Input.GetKey(KeyCode.LeftControl))
        //{
        //    var _endDragPosition.x - _startDragPosition.x;
        //}
    }

    public Tiles GetSellectedTiles( Tiles tiles)
    {
        return tiles[_startDragPosition, _endDragPosition];
    }
}
