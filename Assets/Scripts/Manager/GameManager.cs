using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        const string playerPath = "Prefab/Character/Player/Player";
        const string rabbitPath = "Prefab/Character/Monster/rabbit";
        
        CreateObject(Resources.Load(playerPath), 13, 13);
        short gen = 14;
        for (int n = 0; n < 7; ++n)
        {
            ++gen;
            CreateObject(Resources.Load(rabbitPath), gen, gen);
        }

    }

    // Update is called once per frame
    void Update () {
		
	}

    public bool CreateObject( Object original, short row, short col)
    {
        if( TileManager.Get.IsCanMove( row, col ))
        {
            GameObject go = Instantiate(original,
                TileManager.Get.GetTilePosition(row, col) + new Vector3(0, 0.1f, 0),
                Quaternion.identity) as GameObject;

            if( TileManager.Get.RequestObjectMove(go.GetComponent<BaseObject>(), row, col) )
                return true;                

            Destroy(go); // 미래에 오브젝트들은.. 풀로 관리.. 해줘야함 ㅡㅡ;
        }
        return false;
    }

}
