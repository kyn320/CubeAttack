using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public int playerNumber;

    [SerializeField]
    float lerpTime = 3f, power = 100f;

    [SerializeField]
    Vector3 oldPos, movePos, dir;

    Transform tr;

    private void Awake()
    {
        tr = GetComponent<Transform>();
    }

    public void OnMouseDown()
    {
        if (playerNumber != GameManager.instance.InputSelect)
            return;

        MoveSelector.instance.SetButtonAction(transform, ForwardAction, BackAction, LeftAction, RightAction);
        GameManager.instance.cameraFollow.target = transform;

    }

    public bool CheckAttack(Vector3 dir, float dist)
    {
        RaycastHit hit;
        Vector3 pos = Vector3.zero;
        if (Physics.Raycast(transform.position, dir, out hit, dist))
        {
            movePos = hit.transform.position;
            return true;
        }

        return false;
    }

    private void FixedUpdate()
    {
        if (GetComponent<Rigidbody>().velocity.y < -10f)
        {
            GameManager.instance.SetCameraTarget(transform);
        }
    }

    public void Attack(Vector3 dir, float dist, float power)
    {
        RaycastHit hit;
        Vector3 pos = Vector3.zero;
        Debug.DrawRay(transform.position, dir * dist, Color.red);
        if (Physics.Raycast(transform.position, dir, out hit, dist, LayerMask.GetMask("Player")))
        {
            if (hit.collider.GetComponent<PlayerController>().playerNumber != playerNumber)
                hit.rigidbody.AddForce((dir + Vector3.up) * power);
        }

    }

    public void ForwardAction()
    {
        if (!CheckAttack(transform.forward, Mathf.Infinity))
        {
            movePos = tr.position + Vector3.forward * 10f;
        }
        dir = Vector3.forward;
        StartCoroutine("Move");
        GameManager.instance.InputSelect += 1;
    }

    public void BackAction()
    {
        if (!CheckAttack(-transform.forward, Mathf.Infinity))
        {
            movePos = tr.position + Vector3.back * 10f;
        }
        dir = Vector3.back;
        StartCoroutine("Move");
        GameManager.instance.InputSelect += 1;
    }

    public void RightAction()
    {
        if (!CheckAttack(transform.right, Mathf.Infinity))
        {
            movePos = tr.position + Vector3.right * 10f;
        }
        dir = Vector3.right;
        StartCoroutine("Move");
        GameManager.instance.InputSelect += 1;
    }

    public void LeftAction()
    {
        if (!CheckAttack(-transform.right, Mathf.Infinity))
        {
            movePos = tr.position +  Vector3.left * 10f;
        }
        dir = Vector3.left;
        StartCoroutine("Move");
        GameManager.instance.InputSelect += 1;
    }

    IEnumerator Move()
    {
        while (oldPos != movePos)
        {
            oldPos = tr.position = Vector3.MoveTowards(tr.position, movePos, Time.deltaTime * lerpTime);
            yield return null;
        }
        Attack(dir, 1f, power);
    }

}
