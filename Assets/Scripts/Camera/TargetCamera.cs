using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCamera : MonoBehaviour {

    public BaseObject TargetObject = null;

    private bool    _IsUpdate = false;
    private float   _moveTotalSec = 0.1f;

    public void SetTarget( BaseObject Object)
    {
        if (Object != null)
        {
            TargetObject = Object;
        }
    }

    public void OnUpdateCamera()
    {
        if (_IsUpdate)
        {
            StopCoroutine(MoveCamera());
            StartCoroutine(MoveCamera());
        }
        else
        {
            _IsUpdate = true;
            StartCoroutine(MoveCamera());
        }
    }

    private IEnumerator MoveCamera()
    {
        if (TargetObject != null)
        {
            Vector3 prevPosition = transform.position.Clone();
            Vector3 endPosition = TargetObject.transform.position.Clone();
            float elapsedTime = 0.0f;

            endPosition.z = prevPosition.z;
            while (elapsedTime < 1.0f)
            {
                elapsedTime += Time.deltaTime * (1.0f / _moveTotalSec);
                transform.position =
                    Vector3.Lerp(prevPosition, endPosition, Utility.easeInBack(0.0f, 1.0f, elapsedTime));
                yield return null;
            }
        }
    }
}
