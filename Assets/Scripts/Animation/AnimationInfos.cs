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
    private CustomAnimationController _animController;

    protected List<EnumArray<AnimationInfo>> _animationInfos = new List<EnumArray<AnimationInfo>>();

    public AnimationInfos( CustomAnimationController controller)
    {
        _animController = controller;
    }

    public void InitAnimMaxSize<TDir>(int Length, TDir eDirection)
    {
        _eDirection = (Enum)Enum.ToObject(eDirection.GetType(), eDirection);

        for( int n =0; n < Length; ++n)
        {
            EnumArray<AnimationInfo> enumArray = new EnumArray<AnimationInfo>();
            enumArray.Allocate(eDirection);

            _animationInfos.Add(enumArray);
        }

    }
    public int GetIndex<TDir>( int index, TDir eDirection)
    {
        if (eDirection.GetType() == _eDirection.GetType())
        {
            var animInfo = _animationInfos[index].Get(eDirection);
            if (animInfo != null)
            {    
                return animInfo.AnimIndex;
            }
        }
        return -1;
    }

    public void RegisterAnimInfo<TDir>( int index, TDir eDirection, AnimationClip clip)
    {
        var animInfo = _animationInfos[index].Get(eDirection);

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
            _animationInfos[index].Set(eDirection, animInfo);
        }
    }

    public void UnregisterAllAnimInfo()
    {
        for (int i = 0; i < _animationInfos.Count; ++i)
        {
            var actionArray = _animationInfos[i];
            if( actionArray != null)
            {
                for ( int di = 0; di < actionArray.Length; ++di)
                {
                    var animInfo = actionArray[di];
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
