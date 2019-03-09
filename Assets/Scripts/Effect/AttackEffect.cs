using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : MonoBehaviour {

    const uint _maxHitObjectCount = 10;

    public BaseObject Owner;

    private Vector3 _origin_location;
    private Vector3 _origin_scale;
    private Quaternion _origin_quat;

    private GameObject[] _hitedObjects = new GameObject[_maxHitObjectCount];
    private Animator _animator = null;
    
	// Use this for initialization
	void Start () {
        _animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {

        transform.SetPositionAndRotation(_origin_location, _origin_quat);
        transform.localScale = _origin_scale;

        if (_animator == null)
        {
            OnAnimationEnd();
            return;
        }

        if(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= ( 1.0f - float.Epsilon) )
        {
            OnAnimationEnd();
        }
    }

    public void OnAnimationEnd()
    {
        Destroy(gameObject);
    }

    public void SetOriginTransform( Vector3 location, Quaternion quat, Vector3 scale)
    {
        _origin_location = location;
        _origin_quat = quat;
        _origin_scale = scale;

        transform.SetPositionAndRotation(_origin_location, _origin_quat);
        transform.localScale = _origin_scale;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject otherObject = collision.gameObject;
        int i = 0;
        for (; i < _maxHitObjectCount; ++i)
        {
            if (_hitedObjects[i] == null)
                break;
            else if( _hitedObjects[i] == otherObject)
            {
                return;
            }
        }


        _hitedObjects[i] = otherObject;

        BaseObject hitted_object = otherObject.GetComponent<BaseObject>();
        if(hitted_object != null)
        {
            hitted_object.OnHitted(Owner, 1.0f);
        }

        if ( Owner != null)
            Owner.OnHit();

        if (_animator != null)
        {
            _animator.enabled = false;
            Invoke("OnEnableAnimator", 0.1f);
        }
    }

    private void OnEnableAnimator()
    {
        if( _animator != null)
        {
            _animator.enabled = true;
        }
    }
}
