using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tiles{

    private int _maxRow;
    private int _maxCol;
    private Tile[] _tiles;

    public Tiles( int row, int col)
    {
        _maxRow = row;
        _maxCol = col;

        _tiles = new Tile[row * col];
    }
    public Tile this[int row, int col]
    {
        get {
            try
            {
                return _tiles[col * _maxRow + row];
            }
            catch(IndexOutOfRangeException ex)
            {
                Debug.Log(ex.Message);
                return null;
            }
        }
        set
        {
            _tiles[col * _maxRow + row] = value;
        }
    }
}
