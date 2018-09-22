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
            _animInfos.RegisterAnimInfo(i, NormalDir.TOP,       GetResourcePath() + "Attack/Down/" + attackInfos[i].ResourcePath);
            _animInfos.RegisterAnimInfo(i, NormalDir.DOWN,      GetResourcePath() + "Attack/Top/" + attackInfos[i].ResourcePath);
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

        short targetRow = (short)(_thisObject.Row + vAttackDir.x);
        short targetCol = (short)(_thisObject.Col + vAttackDir.y);

        var targetObject = TileManager.Get.GetObject(targetRow, targetCol);
        if (targetObject != null)
        {
            _thisObject.MoveDirection = vAttackDir;
            LockObject();
            AttackOnce();
            return true;
        }
        else
            return false;
    }

    public void OnAttack()
    {
        if (_isCanHandleTriggerAttack)
        {
            _isCanHandleTriggerAttack = false;

            Instantiate(Resources.Load(WeaponInfo._attackInfos[_currentAttackIndex].EffectPath, typeof(GameObject)),
                transform.position + _thisObject.MoveDirection * TileManager.Get.GetTileDist(), Quaternion.identity);
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
            if(_currentAttackIndex >= WeaponInfo._attackInfos.Length)
            {
                EndAttack();
            }
            else
            {
                AttackOnce();
            }
        }
        else
        {
            EndAttack();
        }
        ResetAttackEventEnable();
    }

    private void AttackOnce()
    {
        _isCanHandleTriggerAttack = true;
        NormalDir direction = Utility.VecToDir(_thisObject.MoveDirection);

        PlayAnimation(_currentAttackIndex, direction);
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
