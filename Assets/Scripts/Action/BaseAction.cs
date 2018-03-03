using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


[System.Serializable]
public abstract class BaseAction : MonoBehaviour{
    public enum NormalDir : int { TOP = 0, DOWN, BESIDE, MAX }

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

    public CustomAnimationController CustomAnimController;
    public float ActionAnimTime = 1.0f;

    protected EnumArray<AnimationInfo> _animationInfos = null;

    protected BaseObject _thisObject;

    protected abstract void LoadResource();

    private void Awake()
    {
        _thisObject = GetComponent<BaseObject>();
        CustomAnimController = GetComponent<CustomAnimationController>();
        RegisterAnimation();
    }
    protected string GetResourcePath()
    {
        return CustomAnimController.ResourcePath;
    }

    public void RegisterAnimation()
    {
        if (_animationInfos == null)
        {
            _animationInfos = new EnumArray<AnimationInfo>();
            LoadResource();
            if (_animationInfos == null)
            {
                Debug.LogError(string.Format("Couldn't register animation info, because animation info is null: {0}", this.GetType().Name));
                return;
            }
        }

        for ( int n =0; n < _animationInfos.Length; ++n)
        {
            AnimationClip clip = _animationInfos[n].Clip;
            if( clip == null)
            {
                Debug.LogError( string.Format( "Couldn't register animation clip, because clip is null : {0}", this.GetType().Name ));
                continue;
            }
            _animationInfos[n].AnimIndex = CustomAnimController.AddClip(clip);
        }
    }

    public void UnregisterAnimation()
    {
        for (int n = 0; n < _animationInfos.Length; ++n)
        {
            int index = _animationInfos[n].AnimIndex;
            if (index == -1)
            {
                Debug.LogError(string.Format("Couldn't unregister animation clip, because index is -1 : {0}", this.GetType().Name));
                continue;
            }

            CustomAnimController.RemoveClip(index);
        }
    }
}
