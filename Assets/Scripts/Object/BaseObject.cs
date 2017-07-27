using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour {

    public short Row { get; set; }
    public short Col { get; set; }

    public delegate void OnMoveStart();
    public delegate void OnMoveEnd();

    public OnMoveStart  OnMoveStartCallback;
    public OnMoveEnd    OnMoveEndCallBack;

    protected float _currJumpHeight = 0.0f;
    protected float _moveLeftTime = 0.0f;
    protected float _velocity = 0.0f;

    protected Vector3 _moveDirection = Vector3.zero;
    protected   const float _moveTimeSec = 0.15f;
    private     const float _jumpGuage = 0.0f;

    protected virtual void Initialize()
    {
        _velocity = TileManager.Get.GetTileDist() / _moveTimeSec;
    }

    protected bool IsCanMove()
    {
        return _moveLeftTime <= 0.0f;
    }
    protected void Move()
    {
        if (_moveDirection != Vector3.zero && IsCanMove())
        {
            short MoveRow = (short)(Row + _moveDirection.x);
            short MoveCol = (short)(Col + _moveDirection.y);

            if (TileManager.Get.RequestObjectMove(this, MoveRow, MoveCol))
            {
                _moveLeftTime = _moveTimeSec;
                if (_moveDirection.x != 0)
                {
                    bool IsNegative = _moveDirection.x < 0;
                    if ( IsNegative != transform.localScale.x < 0.0f)
                    {
                        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    }
                }
            }
        }
    }
    protected virtual void MoveUpdate()
    {
        float distJump = _currJumpHeight / _moveLeftTime;
        Vector3 moveDistance = _moveDirection * _velocity * Time.deltaTime;

        _moveLeftTime -= Time.deltaTime;
        if (_moveLeftTime <= 0.0f)
        {
            moveDistance += _moveDirection * _velocity * _moveLeftTime;
            moveDistance.y -= _currJumpHeight;

            _currJumpHeight = 0.0f;
            _moveLeftTime = 0.0f;
            if( OnMoveEndCallBack != null)
                OnMoveEndCallBack.Invoke();
        }
        else
        {
            float JumpDistance = Mathf.Sin(Mathf.PI + _moveLeftTime * Mathf.PI * 2 / _moveTimeSec) * _velocity * _jumpGuage * Time.deltaTime;
            Debug.Log(Mathf.Sin(Mathf.PI + _moveLeftTime * Mathf.PI * 2 / _moveTimeSec));
            moveDistance.y += JumpDistance;
            _currJumpHeight += JumpDistance;
        }

        this.transform.Translate(moveDistance);
    }


}
