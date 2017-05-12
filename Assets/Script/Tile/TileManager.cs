using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {

    public int Row;
    public int Col;

    public int BlockSize;

    private Camera _mainCamera;
    private Tiles _tiles;
    private float _blockDistance;
    private GameObject _outLineTile;

	// Use this for initialization
	void Start () {
        _mainCamera = Camera.main;
        LoadTile();
    }
	
	// Update is called once per frame
	void Update () {
        UpdateOutLineTile();
    }

    private void LoadTile()
    {
        const string tilePath = "Prefab/Tile/tile";
        const string outLineTilePath = "Prefab/Tile/testOutLine";

        _outLineTile = Instantiate(Resources.Load( outLineTilePath )) as GameObject;
        _outLineTile.SetActive(false);

        _tiles = new Tiles(Row, Col);
        _blockDistance = BlockSize * 0.01f;

        for (int y = 0; y < Col; ++y)
        {
            for (int x = 0; x < Row; ++x)
            {
                GameObject go = Instantiate(
                    Resources.Load(tilePath),
                    new Vector3(x * _blockDistance, y * _blockDistance, 0.0f),
                    Quaternion.identity) as GameObject;
                _tiles[x ,y] = go.GetComponent<Tile>();
            }
        }
    }

    private void UpdateOutLineTile()
    {
        Vector2 position = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        int row = (int)( ( position.x + _blockDistance * 0.5f) / _blockDistance  );
        int col = (int)( ( position.y + _blockDistance * 0.5f) / _blockDistance  );

        var targetTile = _tiles[row, col];

        if ( targetTile == null)
            _outLineTile.SetActive(false);
        else
        {
            _outLineTile.SetActive(true);
            _outLineTile.transform.position = targetTile.transform.position;

            if (Input.GetMouseButton(0))
            {
                //targetTile.ChangeSprite( );
            }
        }
    }
}
