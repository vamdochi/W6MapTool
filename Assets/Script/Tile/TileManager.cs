using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TileManager : MonoBehaviour {

    public int Row;
    public int Col;

    public int BlockSize;

    private Tiles _tiles;
    private GameObject _outLineTile;
    private Sprite _sellectedSprite;
    private SellectTileSystem _sellectTileSystem;
    private HistoryManager _historyManager;

	// Use this for initialization
	void Start () {
        _historyManager = GetComponent<HistoryManager>();
        _sellectedSprite = Resources.Load("Tile/base_tile/tail1_3", typeof( Sprite )) as Sprite;
        _sellectTileSystem = gameObject.AddComponent<SellectTileSystem>();

        LoadTile();
    }
	
	// Update is called once per frame
	void Update () {
        UpdateInput();
        UpdateOutLineTile();
    }
    private void UpdateInput()
    {
        
    }

    private void LoadTile()
    {
        const string tilePath = "Prefab/Tile/tile";
        const string outLineTilePath = "Prefab/Tile/testOutLine";

        _outLineTile = Instantiate(Resources.Load( outLineTilePath )) as GameObject;
        _outLineTile.SetActive(false);

        _tiles = new Tiles(Row, Col, BlockSize);
        
        for (int y = 0; y < Col; ++y)
        {
            for (int x = 0; x < Row; ++x)
            {
                GameObject go = Instantiate(
                    Resources.Load(tilePath),
                    new Vector3(x * _tiles.BlockDistance, y * _tiles.BlockDistance, 0.0f),
                    Quaternion.identity) as GameObject;
                _tiles[x ,y] = go.GetComponent<Tile>();
            }
        }
    }

    private void UpdateOutLineTile()
    {
        var targetTiles = _sellectTileSystem.GetSellectedTiles(_tiles);
        
        if ( targetTiles.IsEmpty())
            _outLineTile.SetActive(false);
        else
        {
            _outLineTile.SetActive(true);

            _outLineTile.transform.localScale = new Vector3(targetTiles.MaxRow, targetTiles.MaxCol, 0);

            Vector3 position = new Vector3( 0, 0, 0 );
            targetTiles.Foreach(t => position += t.transform.position);
            position /= targetTiles.MaxCol * targetTiles.MaxRow;
            _outLineTile.transform.position = position;

            if (InputSystem.Instance.GetKeyCode(CustomKeyCode.AllocateTile))
            {
                var history = new ChangedResourceHistory();
                targetTiles.Foreach(
                    new Action<Tile>( (Tile t) =>
                    {
                        history.TargetRenderers.Add(t.SpriteRenderer);
                        history.PrevSprites.Add(t.GetSprite());
                        history.ChangedSprites.Add(_sellectedSprite);
                        t.ChangeSprite(_sellectedSprite);
                    }));
                _historyManager.PushHistory(history);
            }
        }
    }
}
