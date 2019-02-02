using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using System.IO;
using System.Text;

public class TileManager : MonoBehaviour {

    private static TileManager _Instance = null;
    public static TileManager Get { get { return _Instance; } }

    public int Row;
    public int Col;

    public int BlockSize;

    private Tiles _tiles;
    private GameObject _mouse_tile_border;

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
        if (_mouse_tile_border != null)
        {
            Vector3 mousePosition = Utility.GetMouseWorldPosition();

            Tile tile = _tiles[mousePosition];

            if (tile != null)
            {
                if(!_mouse_tile_border.activeSelf)
                {
                    _mouse_tile_border.SetActive(true);
                }
                _mouse_tile_border.transform.position = tile.transform.position;
            }
            else if( _mouse_tile_border.activeSelf)
            {
                _mouse_tile_border.SetActive(false);
            }
        }
    }
    
    public bool RequestObjectMove(BaseObject Object, int row, int col )
    {
        if( IsCanMove( row, col, Object))
        {
            MoveObject( Object, row, col);
            return true;
        }
        return false;
    }
    public BaseObject GetObject(int row, int col)
    {
        return _tiles[row, col].Object;
    }
    public Vector3 GetTilePosition(int row, int col)
    {
        return _tiles[row, col].transform.position;
    }

    public void MoveObject(BaseObject Object, int row, int col)
    {
        _tiles[Object.Row, Object.Col].Object = null;
        _tiles[row, col].Object = Object;

        Object.Col = col;
        Object.Row = row;
    }

    public bool IsCanMove( int row, int col, BaseObject self)
    {
        if (row >= _tiles.MaxRow || row < 0 ||
            col >= _tiles.MaxCol || col < 0) return false; // Array 넘어갔을시 짤라부려!

        int position = col * _tiles.MaxRow + row; // 작은 퍼포먼스를 위하여 랜덤접근~_~
        Tile tile = _tiles[position];
        if (tile != null)
        {
            if( tile.IsCanMove() )
            {
                return true;
            }

            // 제자리 이동도 가능합니다.
            if( tile.Object == self )
            {
                return true;
            }
        }
        return false;
    }
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

                go.transform.SetParent(transform);
                var name = new StringBuilder();
                name.AppendFormat("tile {0} / {1}", y, x);
                go.name = name.ToString();
            }
        }

        const string tile_attack_border_path = "Prefab/Tile/tile_attack_border";
        GameObject tile_attack_border = Instantiate( Resources.Load(tile_attack_border_path),new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;

        if(tile_attack_border != null)
        {
            tile_attack_border.SetActive(false);
            _mouse_tile_border = tile_attack_border;
        }
    }
}
