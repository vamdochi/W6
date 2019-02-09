using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockAction : BaseAction {

    protected override void LoadResource()
    {
        _animInfos.InitAnimMaxSize(1, NormalDir.MAX);

        _animInfos.RegisterAnimInfo(0, NormalDir.TOP, GetResourcePath() + "Block/Top");
        _animInfos.RegisterAnimInfo(0, NormalDir.DOWN, GetResourcePath() + "Block/Down");
        _animInfos.RegisterAnimInfo(0, NormalDir.BESIDE, GetResourcePath() + "Block/Beside");
    }

    public void Block()
    {
        if( IsCanDoAction() )
        {
            NormalDir direction = Utility.VecToDir(_thisObject.MoveDirection);
            PlayAnimation(0, direction);
            LockObject();
        }
    }

    public override void OnAnimationDone()
    {
        base.OnAnimationDone();
        UnLockObject();
    }
}
