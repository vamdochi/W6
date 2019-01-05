using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCamera : MonoBehaviour {

    public BaseObject TargetObject = null;

    private bool    _IsUpdateMove = false;
    private bool    _IsShaking = false;
    private float   _moveTotalSec = 0.25f;

    private Vector2 _CameraShakeVector = Vector2.zero;
    private Vector3 _originalPosition;

    public void Update()
    {
        if(_IsUpdateMove)
        {
            transform.position = _originalPosition;
        }

        if( _IsShaking)
        {
            transform.position += new Vector3( _CameraShakeVector.x , _CameraShakeVector.y , 0.0f );
        }
    }

    public void SetTarget( BaseObject Object)
    {
        if (Object != null)
        {
            TargetObject = Object;
        }
    }

    public void ShakeCamera( float power)
    {
        if( !_IsShaking)
        {
            _originalPosition = transform.position;
            _IsShaking = true;
            StartCoroutine(ShakeCameraCorutine( power, 2));
        }
    }

    public void OnUpdateCamera()
    {
        if (_IsUpdateMove)
        {
            StopCoroutine(MoveCamera());
            StartCoroutine(MoveCamera());
        }
        else
        {
            _IsUpdateMove = true;
            StartCoroutine(MoveCamera());
        }
    }

    private IEnumerator ShakeCameraCorutine( float power, int shakeCount)
    {
        float elapsedTime = 0.0f;

        float shakeTime = power * 0.001f;

        Vector2 direction = new Vector2((Random.Range(0, 100) - 50) * 0.02f, (Random.Range(0, 100) - 50) * 0.02f);
        direction.Normalize();

        float sectionTime = shakeTime / shakeCount;

        while( elapsedTime < sectionTime * 0.5f)
        {
            _CameraShakeVector += direction * Time.deltaTime * power;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        int currShakeCount = 0;
        float flip = -1.0f;
        while (currShakeCount < shakeCount )
        {
            elapsedTime = 0.0f;
            while (elapsedTime < sectionTime)
            {
                _CameraShakeVector += direction * flip * Time.deltaTime * power;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            flip *=  -1.0f;
            currShakeCount++;
        }


        while (_CameraShakeVector.magnitude > 0.1f)
        {
            float time = 0.0f;
            time += Time.deltaTime * 5.0f * power;
            _CameraShakeVector = Vector2.Lerp( _CameraShakeVector, Vector2.zero, time);

            yield return null;
        }

        _CameraShakeVector = Vector2.zero;
        _IsShaking = false;
        yield break;
    }

    private IEnumerator MoveCamera()
    {
        if (TargetObject != null)
        {
            Vector3 prevPosition = transform.position - new Vector3( _CameraShakeVector.x ,_CameraShakeVector.y, 0.0f );
            float elapsedTime = -0.5f;

            while (elapsedTime < 1.0f)
            {
                Vector3 endPosition = TargetObject.transform.position;
                endPosition.z = prevPosition.z;

                elapsedTime += Time.deltaTime * (1.0f / _moveTotalSec);
                _originalPosition = Vector3.Lerp(prevPosition, endPosition, Utility.easeOutCirc(0.0f, 1.0f, Mathf.Max( 0.0f, elapsedTime)));
                yield return null;
            }
        }
        yield break;

    }
}
