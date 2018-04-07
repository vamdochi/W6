using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAction : BaseAction {

    protected override void LoadResource()
    {
        _animInfos.InitAnimMaxSize(1, NormalDir.MAX);

        _animInfos.RegisterAnimInfo(0, NormalDir.TOP,       Resources.Load(GetResourcePath() + "Idle/Beside", typeof(AnimationClip)) as AnimationClip);
        _animInfos.RegisterAnimInfo(0, NormalDir.DOWN,      Resources.Load(GetResourcePath() + "Idle/Beside", typeof(AnimationClip)) as AnimationClip);
        _animInfos.RegisterAnimInfo(0, NormalDir.BESIDE,    Resources.Load(GetResourcePath() + "Idle/Beside", typeof(AnimationClip)) as AnimationClip);
    }

    public void Idle()
    {
        NormalDir direction = Utility.VecToDir(_thisObject.MoveDirection);
        PlayAnimation(0, direction);
    }
}
