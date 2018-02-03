using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAction : MonoBehaviour {

    public AnimationClip[] clips;
    public CustomAnimationController customAnimController;
    private int[] _animIndices;

    public void RegisterAnimation()
    {
        _animIndices = new int[clips.Length];

        for ( int n =0; n < clips.Length; ++n)
        {
            AnimationClip clip = clips[n];
            if( clips == null)
            {
                Debug.LogError( string.Format( "Couldn't register animation clip, because clip is null : {0}", this.GetType().Name ));
                continue;
            }
            _animIndices[n] = customAnimController.AddClip(clip);
        }
    }
    public void UnregisterAnimation()
    {
        for (int n = 0; n < _animIndices.Length; ++n)
        {
            int index = _animIndices[n];
            if (index == -1)
            {
                Debug.LogError(string.Format("Couldn't unregister animation clip, because index is -1 : {0}", this.GetType().Name));
                continue;
            }

            customAnimController.RemoveClip(index);
        }
    }
}
