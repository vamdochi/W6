using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseAction : MonoBehaviour {
    
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

    public string ResourcePath { get; set; }
    public CustomAnimationController customAnimController;
    protected EnumArray<AnimationInfo> _animationInfos;

    protected abstract void LoadResource();


    public void RegisterAnimation()
    {
        if (_animationInfos == null)
        {
            Debug.LogError(string.Format("Couldn't register animation info, because animation info is null: {0}", this.GetType().Name));
            return;
        }

        for ( int n =0; n < _animationInfos.Length; ++n)
        {
            AnimationClip clip = _animationInfos[n].Clip;
            if( clip == null)
            {
                Debug.LogError( string.Format( "Couldn't register animation clip, because clip is null : {0}", this.GetType().Name ));
                continue;
            }
            _animationInfos[n].AnimIndex = customAnimController.AddClip(clip);
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

            customAnimController.RemoveClip(index);
        }
    }
}
