using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 붙이는 오브젝트에 Animator 필요
[System.Serializable]
public class CustomAnimationController : MonoBehaviour {

    public string ResourcePath = "";

    private SimpleAnimation             _simpleAnimation;
    private Animator                    _animator;

    void Awake() {
        _animator = GetComponent<Animator>();
        _simpleAnimation = gameObject.AddComponent<SimpleAnimation>();
    }

    // Animation Name으로 받는것보다 Enum으로 타입들 정의한다음
    // 변경 테이블 만들어서 바꾸는게 버그낼 가능성이 줄어드는 형태임
    // 추후 변경해야됨 !! 필수
    public void PlayAnimation( int index, float animTime)
    {
        _simpleAnimation.Play( index, animTime);
    }

    public void RemoveClip( int index )
    {
        _simpleAnimation.RemoveClip(index);
    }

    public int AddClip( AnimationClip clip)
    {
        return _simpleAnimation.AddClip(clip, clip.name);
    }
}
