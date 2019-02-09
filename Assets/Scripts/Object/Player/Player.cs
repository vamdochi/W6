using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseObject{

    public MoveAction MoveAction = null;
    public IdleAction IdleAction = null;
    public RollAction RollAction = null;
    public BlockAction BlockAction = null;
    public AttackAction AttackAction = null;
    public KnockBackAction KnockBackAction = null;
    public BackStepAction BackStepAction = null;

    public GameObject _shadow = null;

    private KeyCode _lastInputKey = KeyCode.None;
    private const float _rollTimeSec    = 0.5f;

	// Use this for initialization
	void Start () {
        // Time.timeScale = 0.5f;
        base.Initialize();

        // Load Action Func으로 뺴세요
        if (MoveAction == null)
            MoveAction = GetComponent<MoveAction>();

        if (IdleAction == null)
            IdleAction = GetComponent<IdleAction>();

        if (RollAction == null)
            RollAction = GetComponent<RollAction>();

        if (AttackAction == null)
            AttackAction = GetComponent<AttackAction>();

        if (KnockBackAction == null)
            KnockBackAction = GetComponent<KnockBackAction>();

        if (BackStepAction == null)
            BackStepAction = GetComponent<BackStepAction>();

        if (BlockAction == null)
            BlockAction = GetComponent<BlockAction>();

        var mainCamera = Camera.main.GetComponent<TargetCamera>();

        MoveAction.OnMoveEndCallBack += mainCamera.OnUpdateCamera;
        RollAction.OnMoveEndCallBack += mainCamera.OnUpdateCamera;
        mainCamera.SetTarget(this);
    }
	
	// Update is called once per frame
	protected override void Update () {
        if( !IsLockAction() )
        {
            IdleAction.Idle();
        }
        MoveInput();

        if ( Input.GetKeyDown(KeyCode.Space))
        {
            BlockAction.Block();
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
        short targetRow = (short)(Row + MoveDirection.x);
        short targetCol = (short)(Col + MoveDirection.y);

        var TargetObject = TileManager.Get.GetObject(targetRow, targetCol);
        if( TargetObject != null)
        {
            TargetObject.OnHitted( this, 1.0f);
        }
        if( _lastInputKey != KeyCode.None)
        {
            KeyCode prevKeyCode = KeyCode.None;
            if( MoveDirection.x == 0.0f)
            {
                if (MoveDirection.y > 0.0f)
                {
                    prevKeyCode = KeyCode.W;
                }
                else
                    prevKeyCode = KeyCode.S;
            }
            else if (MoveDirection.y == 0.0f)
            {
                if (MoveDirection.x > 0.0f)
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
                GetComponent<Animator>().
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            SetMoveTime(_rollTimeSec);
            direction.x = -1.0f;
            direction.y = 1.0f;
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            SetMoveTime(_rollTimeSec);
            direction.x = -1.0f;
            direction.y = -1.0f;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            SetMoveTime(_rollTimeSec);
            direction.x = 1.0f;
            direction.y = 1.0f;
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            SetMoveTime(_rollTimeSec);
            direction.x = 1.0f;
            direction.y = -1.0f;
        }

        if ( direction  != Vector3.zero)
        {
            bool moveResult = false;

            bool do_backstep = false;

            if( IsLockAction() && LockingAction.GetType() == typeof(AttackAction) )
            {
                switch (Utility.VecToDir(MoveDirection))
                {
                    case BaseAction.NormalDir.BESIDE:
                        if (GetSpriteRenderer().flipX)
                        {
                            if (direction.x > 0.0f)
                            {
                                do_backstep = true;
                            }
                        }
                        else
                        {
                            if (direction.x < 0.0f)
                            {
                                do_backstep = true;
                            }
                        }
                        break;

                    case BaseAction.NormalDir.TOP:
                        if (direction.y < 0.0f)
                        {
                            do_backstep = true;
                        }
                        break;
                    case BaseAction.NormalDir.DOWN:
                        if (direction.y > 0.0f)
                        {
                            do_backstep = true;
                        }
                        break;
                }
            }

            if( direction.x !=0 && direction.y != 0)
            {
                moveResult = RollAction.Move(direction);
            }
            else
            {
                if (do_backstep)
                {
                    moveResult = BackStepAction.Move(direction);
                }
                else
                {
                    moveResult = MoveAction.Move(direction);
                }
            }

        }

        if( Input.GetMouseButtonDown(0))
        {
            direction = Utility.GetMouseWorldPosition() - transform.position;
            direction = Utility.NormalizeIntVector(direction);

            AttackAction.Attack(direction);
        }
    }

}
