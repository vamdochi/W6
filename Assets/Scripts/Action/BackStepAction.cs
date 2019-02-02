using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BackStepAction : MoveAction
{
    protected override void LoadResource()
    {
        _animInfos.InitAnimMaxSize(1, NormalDir.MAX);

        _animInfos.RegisterAnimInfo(0, NormalDir.TOP, GetResourcePath() + "BackStep/Top");
        _animInfos.RegisterAnimInfo(0, NormalDir.DOWN, GetResourcePath() + "BackStep/Down");
        _animInfos.RegisterAnimInfo(0, NormalDir.BESIDE, GetResourcePath() + "BackStep/Beside");
    }

    protected override void UpdateAnimDir(ref Vector3 moveDir)
    {
        // 추후에 움직임 애니메이션 생기거나하면 아래 함수 사용해서 구현
        NormalDir direction = Utility.VecToDir(moveDir);

        PlayAnimation(0, direction);

    }
}
