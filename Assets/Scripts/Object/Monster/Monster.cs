using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Monster : BaseObject {

    public float MoveDelaySec = 10.0f;
    public IdleAction IdleAction = null;
    public MoveAction MoveAction = null;
    public AttackAction AttackAction = null;

    // Use this for initialization
    void Start () {
        // Load Action Func으로 뺴세요
        if (MoveAction == null)
            MoveAction = GetComponent<MoveAction>();

        if (IdleAction == null)
            IdleAction = GetComponent<IdleAction>();

        if (AttackAction == null)
            AttackAction = GetComponent<AttackAction>();

        base.Initialize();
        StartCoroutine(MoveCorutine());
	}
	
	// Update is called once per frame
	protected override void Update () {
        if (!IsLockAction())
        {
            IdleAction.Idle();
        }
        base.Update();
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

                if( !MoveAction.Move(MoveDirection) )
                {
                    AttackAction.Attack(MoveDirection);
                }
            }
        }
    }
}
