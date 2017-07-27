using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : BaseObject {

    public float MoveDelaySec = 2.5f;
    // Use this for initialization
    void Start () {
        _moveDirection.x = 1;
        base.Initialize();
        StartCoroutine(MoveCorutine());
	}
	
	// Update is called once per frame
	void Update () {
        MoveUpdate();
    }
    public IEnumerator MoveCorutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 1.0f) * MoveDelaySec);

            if (IsCanMove())
            {
                if (Random.Range(0, 11) > 0)
                {
                    _moveDirection = Vector3.zero;
                    if (Random.Range(0, 2) > 0)
                    {
                        _moveDirection.x = Random.Range(0, 2) * 2 - 1;
                    }
                    else
                    {
                        _moveDirection.y = Random.Range(0, 2) * 2 - 1;
                    }
                }
                Move();
            }
        }
    }
}
