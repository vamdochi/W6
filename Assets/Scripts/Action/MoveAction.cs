using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class MoveAction : BaseAction {
    private enum Move : int { Start = 0, TOP = 0, DOWN, BESIDE, MAX }

    protected override void LoadResource()
    {
        _animationInfos = new EnumArray<AnimationInfo>( Move.MAX );

        for( Move n = Move.Start; n != Move.MAX; ++n)
        {
            _animationInfos[n] = new AnimationInfo();
        }

        _animationInfos[Move.TOP].Clip      = Resources.Load(ResourcePath + "Move/Top", typeof(AnimationClip)) as AnimationClip;
        _animationInfos[Move.DOWN].Clip     = Resources.Load(ResourcePath + "Move/Down", typeof(AnimationClip)) as AnimationClip;
        _animationInfos[Move.BESIDE].Clip   = Resources.Load(ResourcePath + "Move/Beside", typeof(AnimationClip)) as AnimationClip;
    }

}