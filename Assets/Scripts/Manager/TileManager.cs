using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {

    public int Row;
    public int Col;

    public float BlockSize;

	// Use this for initialization
	void Start () {

        float minusRow = BlockSize * Row * 0.5f;
        float minusCol = BlockSize * Col * 0.5f;

        var createdObject =
            Resources.Load("tile", typeof(GameObject));
        for ( int y =0; y < Row; ++y)
        {
            for (int x = 0; x < Col; ++x)
            {
                var go = Instantiate(createdObject) as GameObject;
                go.transform.position =
                    new Vector3()
                    {
                        x = x * BlockSize - minusCol,
                        y = y * BlockSize - minusRow,
                        z = 0.0f
                    };

            }
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
