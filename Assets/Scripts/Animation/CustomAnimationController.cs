using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 붙이는 오브젝트에 Animator 필요
[System.Serializable]
public class CustomAnimationController : MonoBehaviour {

    public string ResourcePath = string.Empty;

    private BaseAction                  _driveAction        =   null;
    private SimpleAnimation             _simpleAnimation    =   null;
    private Animator                    _animator           =   null;

    void Awake() {
        _animator = GetComponent<Animator>();
        _simpleAnimation = gameObject.AddComponent<SimpleAnimation>();
        _simpleAnimation.AnimationDoneHandler += OnAnimationDone;
    }

    public void OnAnimationDone()
    {
        if(_driveAction != null)
        {
            _driveAction.OnAnimationDone();
        }
    }

    // Animation Name으로 받는것보다 Enum으로 타입들 정의한다음
    // 변경 테이블 만들어서 바꾸는게 버그낼 가능성이 줄어드는 형태임
    // 추후 변경해야됨 !! 필수
    public void PlayAnimation(BaseAction action, int index, float animTime)
    {
        _simpleAnimation.Play( index, animTime);

        _driveAction = action;
    }
    public void ResetAnimationTime(int index)
    {
        _simpleAnimation.Rewind(index);
    }

    public void RemoveClip( int index )
    {
        _simpleAnimation.RemoveClip(index);
    }

    public int AddClip( out AnimationClip clip, string animationPath )
    {
        clip = Resources.Load(animationPath, typeof(AnimationClip)) as AnimationClip;

        if(clip == null)
        {
            Debug.LogError("AddClip() : animationClip Path is wrong Path");
            return -1;
        }
        return _simpleAnimation.AddClip(clip, animationPath);
    }
}
