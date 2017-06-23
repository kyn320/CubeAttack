using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceController : MonoBehaviour {

    public static DiceController instance;

    public Transform centerTr;
    public Vector3 marign;

    Transform tr;
    Rigidbody ri;

    [SerializeField]
    float power = 2f, radius= 1f;

    public int number = 1;

    private void Awake()
    {
        instance = this;
        tr = GetComponent<Transform>();
        ri = GetComponent<Rigidbody>();
    }

    public void ThrowDice() {
        tr.position = centerTr.position + marign;
        GameManager.instance.SetCamTarget(tr);
        ri.AddExplosionForce(power,tr.position,radius);
        ri.AddTorque(Random.Range(30f,360f), Random.Range(30f, 360f),Random.Range(30f, 360f));
        StartCoroutine("CheckNumber");
    }

    IEnumerator CheckNumber() {
        Vector3 old = Vector3.up * 100f;

        while (old != ri.velocity) {
            old = ri.velocity;
            yield return new WaitForSeconds(0.3f);
        }

        print("finish");

        //어떻게 체크 할까
        //음...?\
        if (Physics.Raycast(tr.position, tr.up, 1f))
        {
            number = 1;
        }
        else if (Physics.Raycast(tr.position, -tr.up, 1f))
        {
            number = 6;
        }
        else if(Physics.Raycast(tr.position, tr.right, 1f))
        {
            number = 5;
        }
        else if(Physics.Raycast(tr.position, -tr.right, 1f))
        {
            number = 2;
        }
        else if(Physics.Raycast(tr.position, tr.forward, 1f))
        {
            number = 3;
        }
        else if(Physics.Raycast(tr.position, -tr.forward, 1f))
        {
            number = 4;
        }
        GameManager.instance.isInputed = true;
        GameManager.instance.SetCamTarget();
    }


}
