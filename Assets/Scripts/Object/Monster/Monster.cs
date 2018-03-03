using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : BaseObject {

    public float MoveDelaySec = 2.5f;
    // Use this for initialization
    void Start () {
//        _moveDirection.x = 1;
        base.Initialize();
//        StartCoroutine(MoveCorutine());
	}
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
//        MoveUpdate();
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
                    MoveDirection = Vector3.zero;
                    if (Random.Range(0, 2) > 0)
                    {
                        MoveDirection.ChangeProperty(Random.Range(0, 2) * 2 - 1, float.NaN, float.NaN);
                    }
                    else
                    {
                        MoveDirection.ChangeProperty(float.NaN, Random.Range(0, 2) * 2 - 1, float.NaN);
                    }
                }
                Move();
            }
        }
    }
}
