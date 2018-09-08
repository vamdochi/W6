using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackAction : MoveAction
{
    protected override void LoadResource()
    {
        _animInfos.InitAnimMaxSize(1, NormalDir.MAX);

        // _animInfos.RegisterAnimInfo(NormalDir.TOP,  NormalAct.Act,   Resources.Load(GetResourcePath() + "Move/Top", typeof(AnimationClip)) as AnimationClip);
        // _animInfos.RegisterAnimInfo(NormalDir.DOWN, NormalAct.Act,   Resources.Load(GetResourcePath() + "Move/Down", typeof(AnimationClip)) as AnimationClip);
        // _animInfos.RegisterAnimInfo(NormalDir.BESIDE, NormalAct.Act, Resources.Load(GetResourcePath() + "Move/Down", typeof(AnimationClip)) as AnimationClip);

    }
}
