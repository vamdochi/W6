using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationInfos {

    [SerializeField]
    protected class AnimationInfo
    {
        public int AnimIndex { get; set; }
        public AnimationClip Clip { get; set; }

        public AnimationInfo()
        {
            AnimIndex = -1;
            Clip = null;
        }
    }

    private Enum _eDirection;
    private Enum _eAction;
    private CustomAnimationController _animController;

    protected EnumArray<EnumArray<AnimationInfo>> _animationInfos = new EnumArray<EnumArray<AnimationInfo>>();

    public AnimationInfos( CustomAnimationController controller)
    {
        _animController = controller;
    }

    public void InitAnimMaxSize<TDir, TAct>(TDir eDirection, TAct eAction)
    {
        _eDirection = (Enum)Enum.ToObject(eDirection.GetType(), eDirection);
        _eAction    = (Enum)Enum.ToObject(eAction.GetType(), eAction);

        _animationInfos.Allocate(eDirection);
    }
    public int GetIndex<TDir, TAct>( TDir eDirection, TAct eAction)
    {
        if (eDirection.GetType() == _eDirection.GetType() &&
            eAction.GetType() == _eAction.GetType())
        {
            var actionArray = _animationInfos.Get(eDirection);
            if (actionArray != null)
            {
                var animInfo = actionArray.Get(eAction);
                if (animInfo != null)
                {
                    return animInfo.AnimIndex;
                }
            }
        }
        return -1;
    }

    public void RegisterAnimInfo<TDir, TAct>( TDir eDirection, TAct eAction, AnimationClip clip)
    {
        var actionArray = _animationInfos.Get(eDirection);

        if( actionArray == null)
        {
            actionArray = new EnumArray<AnimationInfo>();
            actionArray.Allocate((TAct)(object)_eAction);
            _animationInfos.Set(eDirection, actionArray);
        }

        AnimationInfo animInfo = actionArray.Get(eAction);
        if( animInfo != null)
        {
            _animController.RemoveClip(animInfo.AnimIndex);
            animInfo = null;
        }

        animInfo = new AnimationInfo();
        animInfo.Clip = clip;
        animInfo.AnimIndex = _animController.AddClip(clip);

        if( animInfo.AnimIndex != -1)
        {
            actionArray.Set(eAction, animInfo);
        }
    }

    public void UnregisterAllAnimInfo()
    {
        for (int di = 0; di < _animationInfos.Length; ++di)
        {
            var actionArray = _animationInfos[di];
            if( actionArray != null)
            {
                for ( int ai = 0; ai < actionArray.Length; ++ai)
                {
                    var animInfo = actionArray[ai];
                    if (animInfo == null || animInfo.AnimIndex == -1)
                    {
                        Debug.LogError(string.Format("Couldn't unregister animation clip, because index is -1 : {0}", this.GetType().Name));
                        continue;
                    }

                    _animController.RemoveClip(animInfo.AnimIndex);
                }
            }
        }
    }
}
