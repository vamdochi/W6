using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AttackInfo
{
    public float    AnimTime;
    public int      AttackFrame;
}

public class AttackAction : BaseAction {

    [SerializeField]
    public AttackInfo[] AttackInfos = new AttackInfo[(int)AttackAct.MAX];

    enum AttackAct : int // 추후에 Count로 그냥 뺴는게 나을꺼같지 않냐?
    {
        ONE, TWO, THREE, MAX
    }

    protected override void LoadResource()
    {
        _animInfos.InitAnimMaxSize(NormalDir.MAX, AttackAct.MAX);

        _animInfos.RegisterAnimInfo(NormalDir.BESIDE,   AttackAct.ONE,   Resources.Load(GetResourcePath() + "Attack/Beside/_1", typeof(AnimationClip)) as AnimationClip);
        _animInfos.RegisterAnimInfo(NormalDir.BESIDE,   AttackAct.TWO,   Resources.Load(GetResourcePath() + "Attack/Beside/_2", typeof(AnimationClip)) as AnimationClip);
        _animInfos.RegisterAnimInfo(NormalDir.BESIDE,   AttackAct.THREE, Resources.Load(GetResourcePath() + "Attack/Beside/_3", typeof(AnimationClip)) as AnimationClip);
    }

    public void Attack( Vector3 vAttackDir)
    {
        short targetRow = (short)(_thisObject.Row + vAttackDir.x);
        short targetCol = (short)(_thisObject.Col + vAttackDir.y);

        var targetObject = TileManager.Get.GetObject(targetRow, targetCol);
        if (targetObject != null)
        {
            _thisObject.IsActing = true;
        }
    }

    public void OnAttack()
    {

    }

}
