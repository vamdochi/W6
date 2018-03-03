using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollAction : MoveAction
{

    protected override void LoadResource()
    {
        _animationInfos.Allocate(NormalDir.MAX);

        _animationInfos.Get(NormalDir.TOP).Clip       = Resources.Load(GetResourcePath() + "Roll/Top", typeof(AnimationClip)) as AnimationClip;
        _animationInfos.Get(NormalDir.DOWN).Clip     = Resources.Load(GetResourcePath() + "Roll/Down", typeof(AnimationClip)) as AnimationClip;
    }

    protected override void UpdateAnimDir(Vector3 moveDir)
    {
        base.UpdateAnimDir(moveDir);

        if( moveDir.y > 0.0f)
        {
            CustomAnimController.PlayAnimation(_animationInfos.Get(NormalDir.TOP).AnimIndex, ActionAnimTime);
        }
        else
        {
            CustomAnimController.PlayAnimation(_animationInfos.Get(NormalDir.DOWN).AnimIndex, ActionAnimTime);
        }
    }
}
