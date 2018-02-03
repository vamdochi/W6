using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using System.IO;

public class TileManager : MonoBehaviour {

    private static TileManager _Instance = null;
    public static TileManager Get { get { return _Instance; } }

    public int Row;
    public int Col;

    public int BlockSize;

    private Tiles _tiles;

    public TileManager()
    {
        if (_Instance == null)
        {
            _Instance = this;
        }
        else
            Destroy(this);
    }
    void Awake()
    {
        InitalizeTiles();
    }

    void Update()
    {
    }
    
    public bool RequestObjectMove(BaseObject Object, short row, short col )
    {
        if( IsCanMove( row, col))
        {
            MoveObject( Object, row, col);
            return true;
        }
        return false;
    }
    public BaseObject GetObject( short row, short col)
    {
        return _tiles[row, col].Object;
    }
    public void MoveObject(BaseObject Object, short row, short col)
    {
        _tiles[Object.Row, Object.Col].Object = null;
        _tiles[row, col].Object = Object;

        Object.Col = col;
        Object.Row = row;
        Object.TargetMovePosition = _tiles[row, col].transform.position + new Vector3(0, 0.1f, 0);
        Object.PrevMovePosition = Object.transform.position;
    }
    public bool IsCanMove( int row, int col)
    {
        if (row >= _tiles.MaxRow || row < 0 ||
            col >= _tiles.MaxCol || col < 0) return false; // Array 넘어갔을시 짤라부려!

        int position = col * _tiles.MaxRow + row; // 작은 퍼포먼스를 위하여 랜덤접근~_~
        if (_tiles[position] != null)
        {
            return _tiles[position].IsCanMove();
        }
        return false;
    }
    public Vector3 GetTilePosition( int row, int col) { return _tiles[col * _tiles.MaxRow + row].transform.position; }
    public float GetTileDist() { return _tiles.BlockDistance; }

    private void InitalizeTiles()
    {
        const string tilePath = "Prefab/Tile/tile";

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
}
