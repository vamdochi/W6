using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


[System.Serializable]
public abstract class BaseAction : MonoBehaviour{
    public enum NormalDir : int { TOP = 0, DOWN, BESIDE, MAX }

    public CustomAnimationController CustomAnimController;
    public float ActionAnimTime = 1.0f;

    protected AnimationInfos _animInfos;
    protected BaseObject _thisObject;

    protected abstract void LoadResource();

    private void Awake()
    {
        CustomAnimController = GetComponent<CustomAnimationController>();
        _animInfos = new AnimationInfos(CustomAnimController);
        _thisObject          = GetComponent<BaseObject>();
        LoadResource();
    }
    protected string GetResourcePath()
    {
        return CustomAnimController.ResourcePath;
    }

    protected void PlayAnimation<TDir>( int index, TDir eDir)
    {
        CustomAnimController.PlayAnimation(this, _animInfos.GetIndex(index, eDir), ActionAnimTime);
    }

    public void LockObject()
    {
        _thisObject.LockingAction = this;
    }
    
    public void UnLockObject()
    {
        _thisObject.LockingAction = null;
    }

    public virtual void OnAnimationDone()
    {

    }

    public virtual bool IsCanDoAction()
    {
        return !_thisObject.IsLockAction();
    }
}
