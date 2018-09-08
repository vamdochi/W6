using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAction : BaseAction {

    protected override void LoadResource()
    {
        _animInfos.InitAnimMaxSize(1, NormalDir.MAX);

        _animInfos.RegisterAnimInfo(0, NormalDir.TOP,    GetResourcePath() + "Idle/Top");
        _animInfos.RegisterAnimInfo(0, NormalDir.DOWN,   GetResourcePath() + "Idle/Down");
        _animInfos.RegisterAnimInfo(0, NormalDir.BESIDE, GetResourcePath() + "Idle/Beside");
    }

    public void Idle()
    {
        NormalDir direction = Utility.VecToDir(_thisObject.MoveDirection);
        PlayAnimation(0, direction);
    }
}
