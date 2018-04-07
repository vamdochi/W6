using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AttackAction : BaseAction {

    public WeaponInfo WeaponInfo;

    private bool    _isCanContinueAttack;
    private bool    _isPressContinueAttack;
    private bool    _isCanSkipAttack;

    private int     _currentAttackIndex = 0;

    protected override void LoadResource()
    {
        var attackInfos = WeaponInfo._attackInfos;

        _animInfos.InitAnimMaxSize(attackInfos.Length, NormalDir.MAX);
        
        for (int i = 0; i < attackInfos.Length; ++i)
        {
            _animInfos.RegisterAnimInfo(i, NormalDir.BESIDE, Resources.Load(GetResourcePath() + "Attack/attackTest/" + attackInfos[i].ResourcePath, typeof(AnimationClip)) as AnimationClip);
    //        _animInfos.RegisterAnimInfo(i, NormalDir.BESIDE, Resources.Load(GetResourcePath() + "Attack/Beside/" + attackInfos[i].ResourcePath, typeof(AnimationClip)) as AnimationClip);
    //        _animInfos.RegisterAnimInfo(i, NormalDir.BESIDE, Resources.Load(GetResourcePath() + "Attack/Beside/" + attackInfos[i].ResourcePath, typeof(AnimationClip)) as AnimationClip);
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

        if(_isCanSkipAttack)
        {
            OnAnimationEnd();
        }
    }

    public bool Attack( Vector3 vAttackDir)
    {
        if (!IsCanDoAction())
            return false;

        ResetAttackIndex();

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

    public void OnAttackEffect()
    {

    }

    public void OnEnableContinueAttack()
    {
        _isCanContinueAttack = true;
    }

    public void OnEnableSkipAnimation()
    {
        _isCanSkipAttack = true;
    }

    public void OnAnimationEnd()
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
        _isCanSkipAttack        = false;
    }
}
