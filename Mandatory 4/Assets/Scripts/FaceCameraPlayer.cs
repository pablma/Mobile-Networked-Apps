﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCameraPlayer : MonoBehaviour {
    public Transform target;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (target != null)
        {
            transform.LookAt(target.transform);
        }
    }
}
