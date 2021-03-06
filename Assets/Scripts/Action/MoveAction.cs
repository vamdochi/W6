﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class MoveAction : BaseAction {

    [SerializeField]
    public AnimationCurve MoveCurve;

    public delegate void OnMoveEnd();
    public OnMoveEnd OnMoveEndCallBack;

    public float speed { get; set; }
    public float MoveTimeSec = 1.0f;
    public float MaxJumpHeight = 0.1f;
    public bool  canTurnInPlace = false;

    private float   _moveLeftTime   = 0.0f;
    private float   _currJumpHeight = 0.0f;
    private bool    _isInPlace  =   false;

    private Vector3 _startMovePosition  = Vector3.zero;
    private Vector3 _endMovePosition    = Vector3.zero;

    protected override void LoadResource()
    {
        _animInfos.InitAnimMaxSize(1, NormalDir.MAX);

        _animInfos.RegisterAnimInfo(0, NormalDir.TOP,       GetResourcePath() + "Move/Top");
        _animInfos.RegisterAnimInfo(0, NormalDir.DOWN,      GetResourcePath() + "Move/Down");
        _animInfos.RegisterAnimInfo(0, NormalDir.BESIDE,    GetResourcePath() + "Move/Beside");
    }

    public void Update()
    {
        if( IsMoving() )
        {
            _moveLeftTime -= Time.deltaTime;

            float fPercentage   = Mathf.Clamp01(1.0f - _moveLeftTime / MoveTimeSec);
            fPercentage = Utility.easeOutCubic(0.0f, 1.0f, fPercentage);

            if ( IsMoving() )
            {
                _currJumpHeight = Mathf.Sin(fPercentage * Mathf.PI) * MaxJumpHeight;
            }
            else
            {
                _currJumpHeight = 0.0f;
                _moveLeftTime = 0.0f;
                fPercentage = 1.0f;
                OnEndMove();

            }

            CalcPosition(fPercentage);
        }
    }

    protected virtual void UpdateAnimDir(ref Vector3 moveDir)
    {
        // 추후에 움직임 애니메이션 생기거나하면 아래 함수 사용해서 구현
        NormalDir direction = Utility.VecToDir(moveDir);

        PlayAnimation(0, direction);

        bool IsNegative = moveDir.x < 0.0f;

        if (IsNegative)
        {
            _thisObject.GetSpriteRenderer().flipX = true;
        }
        else
        {
            _thisObject.GetSpriteRenderer().flipX = false;
        }
    }

    public virtual bool Move(Vector3 moveDir)
    {
        if (!IsCanDoAction())
            return false;

        int moveRow = (int)(_thisObject.Row + moveDir.x);
        int moveCol = (int)(_thisObject.Col + moveDir.y);

        bool is_moving = false;

        if ( IsCanMove( moveRow, moveCol))
        {
            is_moving = true;
        }
        else if( canTurnInPlace && IsCanMove( _thisObject.Row, _thisObject.Col))
        {
            _thisObject.MoveDirection = moveDir;
            is_moving = true;
            _isInPlace = true;

            moveRow = _thisObject.Row;
            moveCol = _thisObject.Col;

            if (Utility.GetMainTargetCamera() != null)
            {
                Utility.GetMainTargetCamera().ShakeCamera(45.0f);
            }
        }

        if ( is_moving)
        {
            LockObject();

            TileManager.Get.MoveObject(_thisObject, moveRow, moveCol);

            InternalOnlyMove(_thisObject.transform.position, TileManager.Get.GetTilePosition(moveRow, moveCol));

            if (OnMoveEndCallBack != null)
            {
                OnMoveEndCallBack.Invoke();
            }
            return true;
        }
        return false;
    }

    protected void InternalOnlyMove( Vector3 startPosition, Vector3 endPosition)
    {
        _moveLeftTime = MoveTimeSec;
        _startMovePosition = startPosition;
        _endMovePosition = endPosition;

        Vector3 direction = endPosition - startPosition;
        direction.Normalize();

        if ( _isInPlace)
        {
            direction = _thisObject.MoveDirection;
        }
        
        UpdateAnimDir(ref direction);
    }

    protected virtual void OnEndMove()
    {
        _isInPlace = false;

        UnLockObject();

        //if (OnMoveEndCallBack != null)
        //{
        //    OnMoveEndCallBack.Invoke();
        //}
    }

    protected virtual void CalcPosition( float fPercentage)
    {
        transform.position = Vector3.Lerp(_startMovePosition, _endMovePosition, MoveCurve.Evaluate(fPercentage)) + new Vector3(0, _currJumpHeight, 0);

        // 끝났으면 무조건 정해진 위치로 갑니다.
        if(fPercentage >= 1.0f)
        {
            transform.position = _endMovePosition;
        }
    }

    private bool IsCanMove( int row, int col)
    {
        return !IsMoving() && TileManager.Get.IsCanMove(row, col, _thisObject);
    }

    private bool IsMoving()
    {
        return _moveLeftTime > 0.0f;
    }

    public override bool IsCanDoAction()
    {
        var currAction = _thisObject.LockingAction;
        if (currAction != null &&
            currAction.GetType() == typeof(AttackAction))
        {
            return true;
        }

        return base.IsCanDoAction();
    }

}