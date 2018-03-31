using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class MoveAction : BaseAction {

    public delegate void OnMoveEnd();
    public OnMoveEnd OnMoveEndCallBack;

    public float speed { get; set; }
    public float MoveTimeSec = 1.0f;
    public float MaxJumpHeight = 0.1f;


    private float   _moveLeftTime   = 0.0f;
    private float   _currJumpHeight = 0.0f;

    private Vector3 _startMovePosition  = Vector3.zero;
    private Vector3 _endMovePosition    = Vector3.zero;

    protected override void LoadResource()
    {
        _animInfos.InitAnimMaxSize(NormalDir.MAX, NormalAct.MAX);

        // _animInfos.RegisterAnimInfo(NormalDir.TOP,  NormalAct.Act,   Resources.Load(GetResourcePath() + "Move/Top", typeof(AnimationClip)) as AnimationClip);
        // _animInfos.RegisterAnimInfo(NormalDir.DOWN, NormalAct.Act,   Resources.Load(GetResourcePath() + "Move/Down", typeof(AnimationClip)) as AnimationClip);
        // _animInfos.RegisterAnimInfo(NormalDir.BESIDE, NormalAct.Act, Resources.Load(GetResourcePath() + "Move/Down", typeof(AnimationClip)) as AnimationClip);

    }

    public void Update()
    {
        if( IsMoving() )
        {
            _moveLeftTime -= Time.deltaTime;

            float fPercentage   = Mathf.Clamp01(1.0f - _moveLeftTime / MoveTimeSec);
            fPercentage = Utility.easeOutCubic(0.0f, 1.0f, fPercentage);

            // 아직 이동이 안끝났을떄
            if ( IsMoving() )
            {
                _currJumpHeight = Mathf.Sin(fPercentage * Mathf.PI) * MaxJumpHeight;
            }
            else // 이동이 끝났을때
            {
                _currJumpHeight = 0.0f;
                _moveLeftTime = 0.0f;
                _thisObject.IsActing = false;
                if (OnMoveEndCallBack != null)
                {
                    OnMoveEndCallBack.Invoke();
                }
            }

            CalcPosition(fPercentage);
        }
    }

    protected virtual void UpdateAnimDir(Vector3 moveDir)
    {
        // 추후에 움직임 애니메이션 생기거나하면 아래 함수 사용해서 구현
        // NormalDir direction = Utility.VecToDir(moveDir);
        // 일단 사용안하니 주석걸어놈

        {
            bool IsNegative = moveDir.x < 0.0f;

            if (IsNegative != transform.localScale.x < 0.0f)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
    }

    public bool Move(Vector3 moveDir)
    {
        short moveRow = (short)(_thisObject.Row + moveDir.x);
        short moveCol = (short)(_thisObject.Col + moveDir.y);

        if ( IsCanMove( moveRow, moveCol))
        {
            _thisObject.IsActing = true;

            TileManager.Get.MoveObject(_thisObject, moveRow, moveCol);

            _startMovePosition = _thisObject.transform.position;
            _endMovePosition   = TileManager.Get.GetTilePosition( moveRow, moveCol);
            _moveLeftTime = MoveTimeSec;

            UpdateAnimDir(moveDir);
            return true;
        }
        return false;
    }

    protected virtual void CalcPosition( float fPercentage)
    {
        transform.position = Vector3.Lerp(_startMovePosition, _endMovePosition, fPercentage) + new Vector3(0, _currJumpHeight, 0);
    }

    private bool IsCanMove( short row, short col)
    {
        return !IsMoving() && TileManager.Get.IsCanMove(row, col);
    }

    private bool IsMoving()
    {
        return _moveLeftTime > 0.0f;
    }

}