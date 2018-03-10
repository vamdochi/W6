using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAction : BaseAction {

    protected override void LoadResource()
    {
        _animInfos.InitAnimMaxSize(NormalDir.MAX, NormalAct.MAX);

        _animInfos.RegisterAnimInfo(NormalDir.TOP,      NormalAct.Act,   Resources.Load(GetResourcePath() + "Idle/Beside", typeof(AnimationClip)) as AnimationClip);
        _animInfos.RegisterAnimInfo(NormalDir.DOWN,     NormalAct.Act,   Resources.Load(GetResourcePath() + "Idle/Beside", typeof(AnimationClip)) as AnimationClip);
        _animInfos.RegisterAnimInfo(NormalDir.BESIDE,   NormalAct.Act,   Resources.Load(GetResourcePath() + "Idle/Beside", typeof(AnimationClip)) as AnimationClip);
    }

    public void Idle()
    {
        NormalDir direction = Utility.VecToDir(_thisObject.MoveDirection);
        PlayAnimation(direction, NormalAct.Act);
    }
}
