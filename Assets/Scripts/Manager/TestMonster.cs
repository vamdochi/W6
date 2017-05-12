using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMonster : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Invoke("Test", Random.Range(0.1f, 3.0f));
	}
	
	// Update is called once per frame
	void Update () {
    }
    void Test()
    {
        GetComponent<Animator>().SetBool("Test", true);
    }
}
