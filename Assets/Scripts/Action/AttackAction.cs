using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AttackAction : BaseAction {

    public WeaponInfo WeaponInfo;

    private bool    _isCanContinueAttack;
    private bool    _isPressContinueAttack;
    private bool    _isCanHandleTriggerAttack;

    private int     _currentAttackIndex = 0;

    protected override void LoadResource()
    {
        var attackInfos = WeaponInfo._attackInfos;

        _animInfos.InitAnimMaxSize(attackInfos.Length, NormalDir.MAX);
        
        for (int i = 0; i < attackInfos.Length; ++i)
        {
            _animInfos.RegisterAnimInfo(i, NormalDir.BESIDE,    GetResourcePath() + "Attack/Beside/" + attackInfos[i].ResourcePath);
            _animInfos.RegisterAnimInfo(i, NormalDir.TOP,       GetResourcePath() + "Attack/Top/" + attackInfos[i].ResourcePath);
            _animInfos.RegisterAnimInfo(i, NormalDir.DOWN,      GetResourcePath() + "Attack/Down/" + attackInfos[i].ResourcePath);
        }
    }
    private void Update()
    {
        if( _isCanContinueAttack &&
            _isPressContinueAttack == false) 
        {
            var inputDir = InputManager.VecToDirForInput(_thisObject.MoveDirection);
            if (InputManager.Instance.IsPressKeyOnce(InputManager.InputAction.PLAYER_ATTACK, inputDir) )
            {
                _isPressContinueAttack = true;
            }
        }
    }

    public bool Attack( Vector3 vAttackDir)
    {
        if (!IsCanDoAction())
            return false;

        ResetAttackIndex();
        ResetAttackEventEnable();

        _thisObject.MoveDirection = vAttackDir;
        if( AttackOnce() )
        {
            LockObject();
            return true;
        }
        return false;

    }

    public void OnAttack()
    {
        if (_isCanHandleTriggerAttack)
        {
            _isCanHandleTriggerAttack = false;

            GameObject effect = Instantiate(Resources.Load(WeaponInfo._attackInfos[_currentAttackIndex].EffectPath, typeof(GameObject)),
                transform.position + _thisObject.MoveDirection * TileManager.Get.GetTileDist(), Quaternion.identity) as GameObject;
            if( effect != null )
            {
                float right_dot = Vector3.Dot(Vector3.right, _thisObject.MoveDirection.normalized);
                float up_dot = Vector3.Dot(Vector3.up, _thisObject.MoveDirection.normalized);

                Quaternion q = Quaternion.identity;

                if (up_dot > 0.0f)
                {
                    q.z = Mathf.Acos(right_dot);
                }
                else
                    q.z = -Mathf.Acos(right_dot);

                effect.transform.localRotation = q;
            }
        }
    }

    public void OnEnableSkipAnimation()
    {
        _isCanContinueAttack = true;
    }

    public override void OnAnimationDone()
    {
        if( _isPressContinueAttack)
        {
            ++_currentAttackIndex;
            if(_currentAttackIndex >= WeaponInfo._attackInfos.Length || !AttackOnce())
            {
                EndAttack();
            }
        }
        else
        {
            EndAttack();
        }
        ResetAttackEventEnable();
    }

    private bool AttackOnce()
    {
        int targetRow = (int)(_thisObject.Row + _thisObject.MoveDirection.x);
        int targetCol = (int)(_thisObject.Col + _thisObject.MoveDirection.y);

        var targetObject = TileManager.Get.GetObject(targetRow, targetCol);
        if (targetObject != null)
        {
            _isCanHandleTriggerAttack = true;
            NormalDir direction = Utility.VecToDir(_thisObject.MoveDirection);

            PlayAnimation(_currentAttackIndex, direction);

            bool IsNegative = _thisObject.MoveDirection.x < 0.0f;

            if (IsNegative != transform.localScale.x < 0.0f)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            return true;
        }

        return false;
    }

    private void EndAttack()
    {
        UnLockObject();
        ResetAttackIndex();
    }

    private void ResetAttackIndex()
    {
        _currentAttackIndex = 0;
    }

    private void ResetAttackEventEnable()
    {
        _isPressContinueAttack  = false;
        _isCanContinueAttack    = false;
    }
}
