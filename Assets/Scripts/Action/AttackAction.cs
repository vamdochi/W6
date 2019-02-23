using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AttackAction : BaseAction {

    public WeaponInfo WeaponInfo;
    public float    EnableContinueAttackTime = 0.1f;

    private bool    _isCanContinueAttack;
    private bool    _isPressContinueAttack;
    private bool    _isCanHandleTriggerAttack = true;

    private int     _currentAttackIndex = 0;

    protected override void LoadResource()
    {
        if (WeaponInfo.AttackCount > 0)
        {
            _animInfos.InitAnimMaxSize(WeaponInfo.AttackCount, NormalDir.MAX);

            for (int i = 0; i < WeaponInfo.AttackCount; ++i)
            {
                _animInfos.RegisterAnimInfo(i, NormalDir.BESIDE, GetResourcePath() + "Attack/" + WeaponInfo.ResourcePath + "Beside/" + i.ToString() + "/" + i.ToString());
                _animInfos.RegisterAnimInfo(i, NormalDir.TOP, GetResourcePath() + "Attack/" + WeaponInfo.ResourcePath + "Top/" + i.ToString() + "/" + i.ToString());
                _animInfos.RegisterAnimInfo(i, NormalDir.DOWN, GetResourcePath() + "Attack/" + WeaponInfo.ResourcePath + "Down/" + i.ToString() + "/" + i.ToString());
            }
        }
    }

    private void Update()
    {
        if( _isCanContinueAttack &&
            _isPressContinueAttack == false) 
        {
            var inputDir = InputManager.VecToDirForInput(_thisObject.MoveDirection);
            if (_thisObject as Player && InputManager.Instance.IsPressKeyOnce(InputManager.InputAction.PLAYER_ATTACK, inputDir) )
            {
                _thisObject.MoveDirection = Utility.GetMouseWorldPosition() - transform.position;
                _thisObject.MoveDirection = Utility.NormalizeIntVector(_thisObject.MoveDirection);

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

            string effectPath = WeaponInfo.EffectPath + Utility.VecToDirString(_thisObject.MoveDirection) + "/" + (_currentAttackIndex).ToString()
                + "/" + (_currentAttackIndex).ToString();

            GameObject effect = Instantiate(Resources.Load(effectPath, typeof(GameObject)), transform.position, Quaternion.identity) as GameObject;
            if( effect != null )
            {
                var attack_effect = effect.GetComponent<AttackEffect>();
                if (attack_effect != null)
                {
                    attack_effect.SetOriginTransform( transform.position, Quaternion.identity, new Vector3(_thisObject.GetSpriteRenderer().flipX ? -1 : 1, 1, 1));
                    attack_effect.Owner = _thisObject;
                }
            //    float right_dot = Vector3.Dot(Vector3.right, _thisObject.MoveDirection.normalized);
            //    float up_dot = Vector3.Dot(Vector3.up, _thisObject.MoveDirection.normalized);

            //    Quaternion q = Quaternion.identity;

            //    if (up_dot > 0.0f)
            //    {
            //        q.z = Mathf.Acos(right_dot);
            //    }
            //    else
            //        q.z = -Mathf.Acos(right_dot);

            //    effect.transform.localRotation = q;
            }
        }
    }

    public void OnEnableSkipAnimation()
    {
        _isCanContinueAttack = true;
    }

    public override void OnAnimationDone()
    {
        if ( _isPressContinueAttack)
        {
            ++_currentAttackIndex;
            if(_currentAttackIndex >= WeaponInfo.AttackCount || !AttackOnce())
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

    public void EnableTriggerAttack()
    {
        _isCanHandleTriggerAttack = true;
    }

    private bool AttackOnce()
    {
        int targetRow = (int)(_thisObject.Row + _thisObject.MoveDirection.x);
        int targetCol = (int)(_thisObject.Col + _thisObject.MoveDirection.y);

        NormalDir direction = Utility.VecToDir(_thisObject.MoveDirection);

        PlayAnimation(_currentAttackIndex, direction);
        Invoke("EnableTriggerAttack", 0.05f);
        Invoke("OnEnableSkipAnimation", EnableContinueAttackTime);

        bool IsNegative = _thisObject.MoveDirection.x < 0.0f;

        if (IsNegative)
        {
            _thisObject.GetSpriteRenderer().flipX = true;
        }
        else
        {
            _thisObject.GetSpriteRenderer().flipX = false;
        }
        return true;

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
