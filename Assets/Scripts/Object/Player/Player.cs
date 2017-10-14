using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseObject{

    public GameObject _shadow = null;
    public AnimationClip[] _clips;


    private KeyCode _lastInputKey = KeyCode.None;
    private const float _oriMoveTimeSec = 0.22f;
    private const float _rollTimeSec    = 0.5f;

	// Use this for initialization
	void Start () {
        base.Initialize();

        Camera.main.GetComponent<TargetCamera>().SetTarget(this);
        OnMoveEndCallBack += () =>
        {
            _animator.SetBool("Rolling", false);
        };
    }
	
	// Update is called once per frame
	void Update () {
        MoveUpdate();

        if( Input.GetKeyDown(KeyCode.Space))
        {
            _animator.SetTrigger("Attack");
        }
    }
    private void InputUpdate()
    {
        if ( Input.GetKeyDown( KeyCode.W))
        {
            _lastInputKey = KeyCode.W;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            _lastInputKey = KeyCode.S;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            _lastInputKey = KeyCode.A;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            _lastInputKey = KeyCode.D;
        }
    }
    protected override void AttackEnd()
    {
        short targetRow = (short)(Row + _moveDirection.x);
        short targetCol = (short)(Col + _moveDirection.y);

        var TargetObject = TileManager.Get.GetObject(targetRow, targetCol);
        if( TargetObject != null)
        {
            TargetObject.OnHit(1.0f);
        }
        if( _lastInputKey != KeyCode.None)
        {
            KeyCode prevKeyCode = KeyCode.None;
            if( _moveDirection.x == 0.0f)
            {
                if (_moveDirection.y > 0.0f)
                {
                    prevKeyCode = KeyCode.W;
                }
                else
                    prevKeyCode = KeyCode.S;
            }
            else if (_moveDirection.y == 0.0f)
            {
                if (_moveDirection.x > 0.0f)
                {
                    prevKeyCode = KeyCode.D;
                }
                else
                    prevKeyCode = KeyCode.A;
            }

            if(prevKeyCode == _lastInputKey)
            {
                return;
            }
        }
        base.AttackEnd();
    }
    protected override void MoveUpdate()
    {
        if (IsCanMove())
            MoveInput();
        else
            base.MoveUpdate();
    }
    private void MoveInput()
    {
        Vector3 direction = Vector3.zero;
        SetMoveTime(_oriMoveTimeSec);
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            direction.y += 1.0f;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            direction.y -= 1.0f;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            direction.x -= 1.0f;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            direction.x += 1.0f;
            if (transform.localScale.x < 0.0f)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            SetMoveTime(_rollTimeSec);
            direction.x = -1.0f;
            direction.y = -1.0f;
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            SetMoveTime(_rollTimeSec);
            direction.x = -1.0f;
            direction.y = 1.0f;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            SetMoveTime(_rollTimeSec);
            direction.x = 1.0f;
            direction.y = -1.0f;
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            SetMoveTime(_rollTimeSec);
            direction.x = 1.0f;
            direction.y = 1.0f;
        }

        if ( direction  != Vector3.zero)
        {
            _animator.SetInteger("Direction", (int)direction.y);
            if( direction.x != 0.0f && 
                direction.y != 0.0f)
            {
                _animator.SetBool("Rolling", true);
            }
            _moveDirection = direction;

            if (!Move())
            {
                Attack();
                _lastInputKey = KeyCode.None;
            }
        }
    }

}
