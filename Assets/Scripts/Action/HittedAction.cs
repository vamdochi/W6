using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittedAction : BaseAction {

    protected override void LoadResource()
    {
        _animInfos.InitAnimMaxSize(1, NormalDir.MAX);

//        _animInfos.RegisterAnimInfo(0, NormalDir.TOP,       GetResourcePath() + "Hitted/Beside" );
        _animInfos.RegisterAnimInfo(0, NormalDir.BESIDE,      GetResourcePath() + "Hitted/Beside" );
//       _animInfos.RegisterAnimInfo(0, NormalDir.DOWN,    GetResourcePath() + "Hitted/Beside" );

    }

    public void OnHit()
    {
        if( RequestCancelPlayingAction() )
        {
            LockObject();

            // 현재 히트판정은 사이드만 존재
            PlayAnimation(0, NormalDir.BESIDE);
            ResetAnimationTime(0, NormalDir.BESIDE);
        }
    }

    public override void OnAnimationDone()
    {
        UnLockObject();
    }
}
