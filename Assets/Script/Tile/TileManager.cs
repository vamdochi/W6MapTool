using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using W6;
using System.IO;

public enum BrushType
{
    Pencil,
    Erase
}
public class TileManager : MonoBehaviour {

    public int Row;
    public int Col;

    public int BlockSize;

    private Tiles _tiles;
    private OutLineTile _outLineTile;
    private Sprite _sellectedSprite;
    private SellectTileSystem _sellectTileSystem;
    private HistoryManager _historyManager;
    private BrushType _brushType;

    void Start()
    {
        _historyManager = GetComponent<HistoryManager>();
        _sellectTileSystem = gameObject.AddComponent<SellectTileSystem>();

        InitalizeTiles();
    }

    void Update()
    {
        UpdateOutLineTile();
    }

    public byte[] GetTilesBinary() { return BinaryConverter.TilesToBinary(_tiles); }
    public void LoadTiles(byte[] tilesBinary) { _tiles = BinaryConverter.BinaryToTiles(tilesBinary, _tiles); }
    public void ChangeSellectTile( Sprite sprite) { _sellectedSprite = sprite; }
    public void ChangeBrushType( string typeName) {
        object brushType = Enum.Parse(typeof(BrushType), typeName);

        if( brushType != null)
            _brushType = (BrushType)brushType;
    }

    private void InitalizeTiles()
    {
        const string tilePath = "Prefab/Tile/tile";
        const string outLineTilePath = "Prefab/Tile/testOutLine";

        _outLineTile = (Instantiate(Resources.Load( outLineTilePath )) as GameObject).GetComponent<OutLineTile>();

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

        if (targetTiles.IsEmpty())
            _outLineTile.CoverTile(0, 0, Vector3.zero);
        else
        {
            Vector3 position = new Vector3(0, 0, 0);
            targetTiles.Foreach(t => position += t.transform.position);
            position /= targetTiles.MaxCol * targetTiles.MaxRow;

            _outLineTile.CoverTile(targetTiles.MaxRow, targetTiles.MaxCol, position);

            if (InputSystem.Instance.GetKeyCode(CustomKeyCode.AllocateTile) &&
                !EventSystem.current.IsPointerOverGameObject())
            {

                Sprite newSprite = _sellectedSprite;
                if (_brushType == BrushType.Erase)
                    newSprite = null;

                var history = new ChangedResourceHistory();
                targetTiles.Foreach(
                    new Action<Tile>((Tile t) =>
                   {
                       history.TargetRenderers.Add(t.SpriteRenderer);
                       history.PrevSprites.Add(t.GetSprite());
                       history.ChangedSprites.Add(newSprite);
                       t.ChangeSprite(newSprite);
                   }));
                _historyManager.PushHistory(history);
            }
        }
    }
}
