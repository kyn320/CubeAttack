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

    bool isAttack = false;

    Transform tr;
    Rigidbody ri;

    private void Awake()
    {
        tr = GetComponent<Transform>();
        ri = GetComponent<Rigidbody>();
    }

    public void OnMouseDown()
    {
        if (!GameManager.instance.isInputed || MoveSelector.instance.isOpen || playerNumber != GameManager.instance.InputSelect)
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
            if (hit.collider.CompareTag("Wall") || hit.collider.GetComponent<PlayerController>().playerNumber == playerNumber)
            {
                movePos -= dir;
            }
            return true;
        }

        return false;
    }

    private void FixedUpdate()
    {
        if (isAttack)
            Attack(dir, 1f, power);

        
    }

    public void Attack(Vector3 dir, float dist, float power)
    {
        RaycastHit hit;
        Vector3 pos = Vector3.zero;
        Debug.DrawRay(transform.position, dir * dist, Color.red);
        if (Physics.Raycast(transform.position, dir, out hit, dist, LayerMask.GetMask("Player")))
        {
            if (hit.collider.GetComponent<PlayerController>().playerNumber != playerNumber)
            {
                hit.collider.GetComponent<PlayerController>().Damage(dir, DiceController.instance.number);
            }
        }

    }

    public void ForwardAction()
    {
        GameManager.instance.isInputed = false;
        if (!CheckAttack(transform.forward, DiceController.instance.number))
        {
            movePos = tr.position + Vector3.forward * DiceController.instance.number;
        }
        dir = Vector3.forward;
        StartCoroutine("Move", true);
    }

    public void BackAction()
    {
        GameManager.instance.isInputed = false;
        if (!CheckAttack(-transform.forward, DiceController.instance.number))
        {
            movePos = tr.position + Vector3.back * DiceController.instance.number;
        }
        dir = Vector3.back;
        StartCoroutine("Move", true);
    }

    public void RightAction()
    {
        GameManager.instance.isInputed = false;
        if (!CheckAttack(transform.right, DiceController.instance.number))
        {
            movePos = tr.position + Vector3.right * DiceController.instance.number;
        }
        dir = Vector3.right;
        StartCoroutine("Move", true);
    }

    public void LeftAction()
    {
        GameManager.instance.isInputed = false;
        if (!CheckAttack(-transform.right, DiceController.instance.number))
        {
            movePos = tr.position + Vector3.left * DiceController.instance.number;
        }
        dir = Vector3.left;
        moveCoroutine = StartCoroutine("Move", true);
    }

    public void Damage(Vector3 dir, int dist)
    {
        if (moveCoroutine != null)
            StopCoroutine("Move");

        movePos = tr.position + (dir * dist);

        if (Random.Range(0f, 100f) < 20f || GameManager.instance.CheckBoardOut(movePos))
            ri.AddForce((dir + Vector3.up) * power, ForceMode.Impulse);
        else
        {
            moveCoroutine = StartCoroutine("Move", false);
        }
    }

    Coroutine moveCoroutine = null;

    IEnumerator Move(bool isDamage)
    {

        isAttack = isDamage;
        while (oldPos != movePos)
        {
            oldPos = tr.position = Vector3.Lerp(tr.position, movePos, Time.deltaTime * lerpTime);
            if (ri.velocity.y < -5f)
            {
                GameManager.instance.RemovePlayer(playerNumber, transform);
                break;
            }
            yield return null;
        }
        tr.rotation = Quaternion.identity;
        if (isAttack)
            isAttack = false;

        moveCoroutine = null;
        if (isDamage)
            GameManager.instance.InputSelect += 1;
    }

}
