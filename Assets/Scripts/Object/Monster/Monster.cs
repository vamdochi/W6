using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : BaseObject {

    public float MoveDelaySec = 10.0f;
    public MoveAction MoveAction = null;

    // Use this for initialization
    void Start () {
        // Load Action Func으로 뺴세요
        if (MoveAction == null)
            MoveAction = GetComponent<MoveAction>();

        base.Initialize();
        StartCoroutine(MoveCorutine());
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

            if (Random.Range(0, 11) > 0)
            {
                MoveDirection = Vector3.zero;

                float random = Random.Range(0, 2) * 2 - 1;
                Vector3 prevMoveDirection = MoveDirection;
                
                if (Random.Range(0, 2) > 0)
                {
                    MoveDirection = new Vector3( random, prevMoveDirection.y, prevMoveDirection.z);
                }
                else
                {
                    MoveDirection = new Vector3(prevMoveDirection.x , random , prevMoveDirection.z);
                }
                MoveAction.Move(MoveDirection);
            }
        }
    }
}
