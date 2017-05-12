using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseObject{

    public GameObject _shadow = null;
    public AnimationClip[] _clips;

    private float   _moveLeftTime   = 0.0f;
    private float   _velocity       = 0.0f;
    private float   _currJumpHeight = 0.0f;
    private Vector3 _moveDirection  = Vector3.zero;
    private Animator _animator      = null;

    const float _moveTime = 0.2f;
	// Use this for initialization
	void Start () {
        _animator       = GetComponent<Animator>();
        _velocity       = 0.8f;
    }
	
	// Update is called once per frame
	void Update () {
        UpdateMove();

        if( Input.GetKeyDown(KeyCode.Space))
        {
            _animator.SetTrigger("Attack");
        }
    }
    private void UpdateMove()
    {
        _moveLeftTime -= Time.deltaTime;
        if (IsCanMove())
            Move();
        else
            MoveInput();
    }
    private void Move()
    {
        float distJump = 1.5f * _velocity * Time.deltaTime * GetGravity();
        Vector3 moveDistance = _moveDirection * _velocity * Time.deltaTime;

        this.   transform.Translate(moveDistance);
        _shadow.transform.Translate(moveDistance);

        if (_currJumpHeight + distJump > 0.0f)
            _currJumpHeight += distJump;
        else
        {
            _moveLeftTime = 0.0f;
        }
        if(!IsCanMove())
        {
            distJump = -_currJumpHeight;
            _currJumpHeight = 0.0f;
            OnMoveEndCallBack();
        }
        transform.Translate(Vector3.up * distJump);
    }
    private void MoveInput()
    {
        Vector3 direction = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.W))
        {
            _animator.SetInteger("Direction", 1);
            direction.y += 1.0f;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            _animator.SetInteger("Direction", -1);
            direction.y -= 1.0f;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            _animator.SetInteger("Direction", 0);
            direction.x -= 1.0f;
            transform.localScale =
                new Vector3(-1.0f,
                transform.localScale.y, transform.localScale.z);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            _animator.SetInteger("Direction", 0);
            direction.x += 1.0f;

            transform.localScale =
                new Vector3(1.0f,
                transform.localScale.y, transform.localScale.z);
        }
        if( direction != Vector3.zero)
        {
            _moveLeftTime = _moveTime;
        }
        _moveDirection = direction;
    }

    private float GetGravity()
    {
        return _moveLeftTime > _moveTime * 0.5f ? 1.0f : -1.1f;
    }

    private bool IsCanMove()
    {
        return _moveLeftTime > 0.0f;
    }
}
