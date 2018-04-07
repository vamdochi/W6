using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AttackInfo
{
    [SerializeField]
    public string ResourcePath;
}

[Serializable]
public class WeaponInfo  {

    [SerializeField]
    public AttackInfo[] _attackInfos;
}
