using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITargetFollow : MonoBehaviour {

    RectTransform tr;

    public Transform target;

    [SerializeField]
    float lerpTime = 4f;

    [SerializeField]
    Vector3 margin;

    private void Awake()
    {
        tr = GetComponent<RectTransform>();
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (target != null)
        {
            tr.position = Vector3.Lerp(tr.position, Camera.main.WorldToScreenPoint(target.position) + margin, Time.deltaTime * lerpTime);
        }

    }

   

}
