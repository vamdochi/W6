using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollAction : MoveAction
{

    protected override void LoadResource()
    {
        _animInfos.InitAnimMaxSize(1, NormalDir.MAX);

        _animInfos.RegisterAnimInfo(0, NormalDir.TOP, Resources.Load(GetResourcePath() + "Roll/Top", typeof(AnimationClip)) as AnimationClip);
        _animInfos.RegisterAnimInfo(0, NormalDir.DOWN, Resources.Load(GetResourcePath() + "Roll/Down", typeof(AnimationClip)) as AnimationClip);

    }

    protected override void UpdateAnimDir(Vector3 moveDir)
    {
        base.UpdateAnimDir(moveDir);

        if( moveDir.y > 0.0f)
        {
            PlayAnimation(0, NormalDir.TOP);
        }
        else
        {
            PlayAnimation(0, NormalDir.DOWN);
        }
    }

    public override bool IsCanDoAction()
    {
        var currAction = _thisObject.LockingAction;
        if (currAction != null &&
            currAction.GetType() == typeof(AttackAction))
        {
            return true;
        }

        return base.IsCanDoAction();
    }

}
