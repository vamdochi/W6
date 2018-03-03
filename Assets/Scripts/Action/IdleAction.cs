using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAction : BaseAction {

    protected override void LoadResource()
    {
        _animationInfos.Allocate( NormalDir.MAX);

        _animationInfos.Get( NormalDir.TOP ).Clip     = Resources.Load(GetResourcePath() + "Idle/Beside", typeof(AnimationClip)) as AnimationClip;
        _animationInfos.Get( NormalDir.DOWN ).Clip    = Resources.Load(GetResourcePath() + "Idle/Beside", typeof(AnimationClip)) as AnimationClip;
        _animationInfos.Get( NormalDir.BESIDE ).Clip  = Resources.Load(GetResourcePath() + "Idle/Beside", typeof(AnimationClip)) as AnimationClip;
    }

    public void Idle()
    {
        NormalDir direction = Utility.VecToDir(_thisObject.MoveDirection);
        CustomAnimController.PlayAnimation(_animationInfos.Get(direction).AnimIndex, ActionAnimTime);
    }
}
