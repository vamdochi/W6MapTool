using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Tiles{

    public float BlockDistance { get; private set; }
    public int MaxRow { get; private set; }
    public int MaxCol { get; private set; }
    private Tile[] _tiles;

    public Tiles( int row, int col, int blockSize)
    {
        MaxRow = row;
        MaxCol = col;
        BlockDistance = blockSize * 0.01f;


        _tiles = new Tile[row * col];
    }
    public bool IsEmpty() { return MaxRow <= 0 || MaxCol <= 0; }

    public IEnumerable<Tile> GetEnumerable()
    {
        for(int y =0; y < MaxCol; ++y)
        {
            for (int x = 0; x < MaxRow; ++x)
            {
                yield return this[x, y];
            }
        }
    }
    public void Foreach( Action<Tile> action)
    {
        foreach (var tile in _tiles)
        {
            if (tile == null) continue;
            action(tile);
        }
    }
    public Tiles this[Vector2 startPosition, Vector2 endPosition]
    {
        get
        {
            int startRow = (int)((startPosition.x + BlockDistance * 0.5f) / BlockDistance);
            int startCol = (int)((startPosition.y + BlockDistance * 0.5f) / BlockDistance);
            int endRow = (int)((endPosition.x + BlockDistance * 0.5f) / BlockDistance);
            int endCol = (int)((endPosition.y + BlockDistance * 0.5f) / BlockDistance);

            if (startRow > endRow)
            {
                startRow.Swap(ref startRow, ref endRow);
            }
            if (startCol > endCol)
                startCol.Swap(ref startCol, ref endCol);

            Tiles tiles = new Tiles( endRow - startRow + 1, endCol - startCol + 1, 0);

            for (int y = startCol; y <= endCol; ++y)
            {
                for (int x = startRow; x <= endRow; ++x)
                {
                    tiles[ x - startRow, y -startCol ] = this[x, y];
                }
            }


            return tiles;
        }
        private set { }
    }
    public Tile this[Vector2 position]
    {
        get
        {
            int row = (int)((position.x + BlockDistance * 0.5f) / BlockDistance);
            int col = (int)((position.y + BlockDistance * 0.5f) / BlockDistance);

            return this[ row, col];
        }
        private set { }
    }
    public Tile this[int row, int col]
    {
        get {
            if( row < 0 || row >= MaxRow ||
                col < 0 || col >= MaxCol)
                return null;
                return _tiles[col * MaxRow + row];
        }
        set
        {
            if (row < 0 || row >= MaxRow ||
                col < 0 || col >= MaxCol)
                return;

            _tiles[col * MaxRow + row] = value;
        }
    }
}
