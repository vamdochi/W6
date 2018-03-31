using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollAction : MoveAction
{

    protected override void LoadResource()
    {
        _animInfos.InitAnimMaxSize(NormalDir.MAX, NormalAct.MAX);

        _animInfos.RegisterAnimInfo(NormalDir.TOP, NormalAct.Act, Resources.Load(GetResourcePath() + "Roll/Top", typeof(AnimationClip)) as AnimationClip);
        _animInfos.RegisterAnimInfo(NormalDir.DOWN, NormalAct.Act, Resources.Load(GetResourcePath() + "Roll/Down", typeof(AnimationClip)) as AnimationClip);

    }

    protected override void UpdateAnimDir(Vector3 moveDir)
    {
        base.UpdateAnimDir(moveDir);

        if( moveDir.y > 0.0f)
        {
            PlayAnimation(NormalDir.TOP, NormalAct.Act);
        }
        else
        {
            PlayAnimation(NormalDir.DOWN, NormalAct.Act);
        }
    }
}
