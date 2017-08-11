using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseObject{

    public GameObject _shadow = null;
    public AnimationClip[] _clips;


    private const float _oriMoveTimeSec = 0.22f;
    private const float _rollTimeSec    = 0.56f;
    private Animator _animator          = null;

	// Use this for initialization
	void Start () {
        base.Initialize();

        _animator       = GetComponent<Animator>();
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
            _animator.SetInteger("Direction", 1);
            direction.y += 1.0f;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            _animator.SetInteger("Direction", -1);
            direction.y -= 1.0f;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            _animator.SetInteger("Direction", 0);
            direction.x -= 1.0f;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            _animator.SetInteger("Direction", 0);
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
            _animator.SetBool("Rolling", true);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            SetMoveTime(_rollTimeSec);
            direction.x = 1.0f;
            direction.y = -1.0f;
            _animator.SetBool("Rolling", true);
        }

        if ( direction  != Vector3.zero)
        {
            _moveDirection = direction;
            Move();
        }
    }

}
