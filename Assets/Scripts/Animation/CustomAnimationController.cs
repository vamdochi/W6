using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationClipOverrides : List<KeyValuePair<AnimationClip, AnimationClip>>
{
    public AnimationClipOverrides( int capacity) : base(capacity) { }

    public AnimationClip this[string name]
    {
        get { return this.Find(pair => pair.Key.name.Equals(name)).Value; }
        set
        {
            int index = this.FindIndex(x => x.Key.name.Equals(name));
            if (index != -1)
                this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
        }
    }
}
// 붙이는 오브젝트에 Animator 필요
public class CustomAnimationController : MonoBehaviour {

    private SimpleAnimation             _simpleAnimation;
    private Animator                    _animator;

    void Awake() {
        _animator = GetComponent<Animator>();
        _simpleAnimation = this.gameObject.AddComponent<SimpleAnimation>();
    }

    // Animation Name으로 받는것보다 Enum으로 타입들 정의한다음
    // 변경 테이블 만들어서 바꾸는게 버그낼 가능성이 줄어드는 형태임
    // 추후 변경해야됨 !! 필수
    public void PlayAnimation( int index, float animTime)
    {
        _simpleAnimation.Play( index );
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
