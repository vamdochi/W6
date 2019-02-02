using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Tiles{

    public float BlockDistance { get; private set; }
    public int BlockSize { get { return (int)(BlockDistance * 100); } }
    public int MaxRow { get; private set; }
    public int MaxCol { get; private set; }
    public int Length { get { return _tiles.Length; } }

    private Tile[] _tiles;

    public Tiles( int row, int col, int blockSize)
    {
        MaxRow = row;
        MaxCol = col;
        BlockDistance = blockSize * 0.01f;

        _tiles = new Tile[row * col];
    }
    public bool IsEmpty() { return MaxRow <= 0 || MaxCol <= 0; }
    public int GetCount() { return MaxRow * MaxCol; }
    public IEnumerator<Tile> GetEnumerator()
    {
        for(int y =0; y < MaxCol; ++y)
        {
            for (int x = 0; x < MaxRow; ++x)
            {
                yield return this[x, y];
            }
        }
    }
    public List<Tile> ToList()
    {
        List<Tile> list = new List<Tile>();
        for (int n =0; n < MaxCol * MaxRow; ++n)
        {
            list.Add(_tiles[n]);
        }
        return list;
    }
    public void Foreach( Action<Tile> action)
    {
        for (int n =0; n < MaxCol * MaxRow; ++n)
        {
            var tile = _tiles[n];
            if (tile == null) continue;
            action(tile);
        }
    }
    public Tiles this[Vector2 startPosition, Vector2 endPosition]
    {
        get
        {
            int startRow    = (int)((startPosition.x + BlockDistance * 0.5f) / BlockDistance);
            int startCol    = (int)((startPosition.y + BlockDistance * 0.5f) / BlockDistance);
            int endRow      = (int)((endPosition.x + BlockDistance * 0.5f) / BlockDistance);
            int endCol      = (int)((endPosition.y + BlockDistance * 0.5f) / BlockDistance);

            if (startRow > endRow)
                startRow.Swap(ref startRow, ref endRow);
            if (startCol > endCol)
                startCol.Swap(ref startCol, ref endCol);

            startRow = Math.Min(Math.Max(0, startRow), MaxRow - 1);
            startCol = Math.Min(Math.Max(0, startCol), MaxCol - 1);
            endRow = Math.Min(Math.Max(0, endRow), MaxRow - 1);
            endCol = Math.Min(Math.Max(0, endCol), MaxCol - 1);


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
    public Tile this[Vector3 position]
    {
        get
        {
            int row = (int)((position.x + BlockDistance * 0.5f) / BlockDistance);
            int col = (int)((position.y + BlockDistance * 0.5f) / BlockDistance);

            if (row < 0 || row >= MaxRow || col < 0 || col >= MaxCol)
                return null;

            return this[row, col];
        }
        private set { }
    }
    public Tile this[int index] {  get { return _tiles[index]; } }
    public Tile this[int row, int col]
    {
        get
        {
            return _tiles[col * MaxRow + row];
        }
        set
        {
            _tiles[col * MaxRow + row] = value;
        }
    }
}
