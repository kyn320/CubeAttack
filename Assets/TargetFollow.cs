using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFollow : MonoBehaviour {

    Transform tr;

    public Transform target;

    [SerializeField]
    float lerpTime = 4f;

    [SerializeField]
    Vector3 margin;

    private void Awake()
    {
        tr = GetComponent<Transform>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (target != null)
            tr.position = Vector3.Lerp(tr.position, target.position + margin,Time.deltaTime * lerpTime);
	}
}
