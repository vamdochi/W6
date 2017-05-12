using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour {

    public delegate void OnMoveStart();
    public delegate void OnMoveEnd();

    public OnMoveStart  OnMoveStartCallback;
    public OnMoveEnd    OnMoveEndCallBack;

}
