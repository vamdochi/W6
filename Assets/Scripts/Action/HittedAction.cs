using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittedAction : BaseAction {

    private Vector3 hit_direction = Vector3.zero;
    private Vector3 added_location = Vector3.zero;
    private bool is_hitted = false;
    private float elapsed_time = 0.0f;

    public float hitted_move_speed = 15.0f;
    public float hitted_move_time = 0.15f;

    protected override void LoadResource()
    {
        _animInfos.InitAnimMaxSize(1, NormalDir.MAX);

//        _animInfos.RegisterAnimInfo(0, NormalDir.TOP,       GetResourcePath() + "Hitted/Beside" );
        _animInfos.RegisterAnimInfo(0, NormalDir.BESIDE,      GetResourcePath() + "Hitted/Beside" );
//       _animInfos.RegisterAnimInfo(0, NormalDir.DOWN,    GetResourcePath() + "Hitted/Beside" );
    
    }
    public void Update()
    {
        if( is_hitted)
        {
            Vector3 delta = hit_direction * Time.deltaTime * hitted_move_speed;
            added_location += delta;
            transform.position = transform.position + delta;

            Debug.Log(delta);
            elapsed_time += Time.deltaTime;
            if( elapsed_time > hitted_move_time)
            {
                EndHitMove();
            }
        }
    }

    public void OnHitted( BaseObject attacker)
    {
        if( RequestCancelPlayingAction() )
        {
            LockObject();

            // 현재 히트판정은 사이드만 존재
            PlayAnimation(0, NormalDir.BESIDE);
            ResetAnimationTime(0, NormalDir.BESIDE);

            EndHitMove();

            if ( attacker != null )
            {
                is_hitted = true;
                hit_direction = new Vector3(_thisObject.Row - attacker.Row, _thisObject.Col - attacker.Col, 0).normalized;

                if( hit_direction.x < 0.0f )
                {
                    _thisObject.GetSpriteRenderer().flipX = false;
                }
                else
                {
                    _thisObject.GetSpriteRenderer().flipX = true;
                }
            }
        }
    }

    public override void OnAnimationDone()
    {
        UnLockObject();
    }

    private void EndHitMove()
    {
        is_hitted = false;
        elapsed_time = 0.0f;
        transform.position = transform.position - added_location;
        added_location = Vector3.zero;
    }
}
