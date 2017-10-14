using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionStatus
{
    IDLE,
    MOVE,
    ATTACK,
    MAX
}
public class BaseObject : MonoBehaviour {

    public short Row { get; set; }
    public short Col { get; set; }
    public Vector3 TargetMovePosition { get; set; }
    public Vector3 PrevMovePosition { get; set; }
    public ActionStatus Status { get; private set; }

    public delegate void OnMoveStart();
    public delegate void OnMoveEnd();
    public delegate void AnimationEndHandler();

    public OnMoveStart  OnMoveStartCallback;
    public OnMoveEnd    OnMoveEndCallBack;
    private AnimationEndHandler _animationEndHandler;

    protected float _currJumpHeight = 0.0f;
    protected float _moveLeftTime = 0.0f;
    protected float _velocity = 0.0f;

    protected Vector3 _moveDirection = Vector3.zero;
    protected   float _moveTimeSec = 0.0f;
    protected Animator _animator = null;

    private SpriteRenderer _spriteRenderer = null;
    private const float _jumpGuage = 0.0f;

    public void OnHit( float fDamage )
    {
        _spriteRenderer.color = new Color(1.0f, 0, 0);
    }
    protected virtual void Initialize()
    {
        Status = ActionStatus.IDLE;
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        SetMoveTime(0.22f);
    }
    protected virtual void Update()
    {
        Color color = _spriteRenderer.color;

        if( color.g <= 1.0f - float.Epsilon)
        {
            color.g += 1.0f * Time.deltaTime;
            color.b += 1.0f * Time.deltaTime;
            _spriteRenderer.color = color;
        }

    }

    protected bool SetMoveTime( float moveTimeSec)
    {
        if (IsCanMove())
        {
            if (_moveTimeSec != moveTimeSec)
            {
                _moveTimeSec = moveTimeSec;
                _velocity = TileManager.Get.GetTileDist() / _moveTimeSec;
            }
            return true;
        }
        return false;
    }
    protected bool IsCanMove()
    {
        return _moveLeftTime <= 0.0f;
    }
    protected bool ChangeStatus( ActionStatus destStatus)
    {
        if( Status != destStatus)
        {
            bool isCanChange = false;
            switch(destStatus)
            {
                case ActionStatus.ATTACK:
                    isCanChange = Status == ActionStatus.IDLE;
                    break;
                case ActionStatus.MOVE:
                    isCanChange = Status == ActionStatus.IDLE;
                    break;
                default:
                    isCanChange = true;
                    break;
            }

            if( isCanChange)
            {
                Status = destStatus;
                _animator.SetInteger("Status", (int)Status);
                return true;
            }
        }
        return false;
    }

    IEnumerator WaitAnimationEnd( )
    {
        yield return null; // Animation 적용까지 기다림..

        var stateInfo = _animator.GetCurrentAnimatorClipInfo(0);
        var test = stateInfo[0].clip;

        float f = test.apparentSpeed * 2.0f;
        //float time = stateInfo.length / stateInfo.speed;
        //yield return new WaitForSeconds( time );

        if (_animationEndHandler != null)
        {
            _animationEndHandler.Invoke();
        }
        _animationEndHandler = null;
        yield break;
    }
    protected bool Attack()
    {
        if( ChangeStatus(ActionStatus.ATTACK) )
        {
            _animationEndHandler = AttackEnd;
            StartCoroutine(WaitAnimationEnd());
            return true;
        }
        return false;
    }
    protected virtual void AttackEnd()
    {
        ChangeStatus(ActionStatus.IDLE);
    }
    protected bool Move()
    {
        if (_moveDirection != Vector3.zero && IsCanMove())
        {
            short MoveRow = (short)(Row + _moveDirection.x);
            short MoveCol = (short)(Col + _moveDirection.y);

            if (TileManager.Get.IsCanMove( MoveRow, MoveCol ) && ChangeStatus(ActionStatus.MOVE) )
            {
                TileManager.Get.MoveObject(this, MoveRow, MoveCol);
                _animator.SetInteger("AnimPer", 0);
                _moveLeftTime = _moveTimeSec;
                if (_moveDirection.x != 0)
                {
                    bool IsNegative = _moveDirection.x < 0;
                    if ( IsNegative != transform.localScale.x < 0.0f)
                    {
                        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    }
                }
                return true;
            }
        }
        return false;
    }
    protected virtual void MoveUpdate()
    {
        float distJump = _currJumpHeight / _moveLeftTime;
        _moveLeftTime -= Time.deltaTime;

        if (_moveLeftTime <= 0.0f)
        {
            _currJumpHeight = 0.0f;
            _moveLeftTime = 0.0f;
            if (OnMoveEndCallBack != null)
            {
                OnMoveEndCallBack.Invoke();
            }
            ChangeStatus(ActionStatus.IDLE);
        }
        else
        {
            float JumpDistance = Mathf.Sin(Mathf.PI + _moveLeftTime * Mathf.PI * 2 / _moveTimeSec) * _velocity * _jumpGuage * Time.deltaTime;
            _currJumpHeight += JumpDistance;
        }
        CalcPosition();
    }

    protected virtual void CalcPosition()
    {
        float fPer = Mathf.Clamp01( 1.0f -_moveLeftTime / _moveTimeSec);
        fPer = Utility.easeOutCubic(0.0f, 1.0f, fPer);
        transform.position = Vector3.Lerp(PrevMovePosition, TargetMovePosition, fPer) + new Vector3(0, _currJumpHeight);
        _animator.SetInteger("AnimPer", (int)( 100 * fPer));
    }

}
