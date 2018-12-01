using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : MonoBehaviour {

    const uint _maxHitObjectCount = 10;

    private GameObject[] _hitedObjects = new GameObject[_maxHitObjectCount];
    private Animator _animator = null;
    
	// Use this for initialization
	void Start () {
        _animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if(_animator == null)
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
        otherObject.GetComponent<BaseObject>().OnHit(1.0f);
    }
}
