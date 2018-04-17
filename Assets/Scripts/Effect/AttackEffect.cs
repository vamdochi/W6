using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : MonoBehaviour {

    const uint _maxHitObjectCount = 10;

    private GameObject[] _hitedObjects = new GameObject[_maxHitObjectCount];

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

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
