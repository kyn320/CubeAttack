using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MoveSelector : MonoBehaviour
{

    public static MoveSelector instance;

    public Button forwardButton, backButton, leftButton, rightButton;
    public UITargetFollow targetFollow;

    Animator ani;
    RectTransform tr;
    

    private void Awake()
    {

        instance = this;
        tr = GetComponent<RectTransform>();
        ani = GetComponent<Animator>();
    }


    public void SetButtonAction(Transform _tr, UnityAction forward, UnityAction back, UnityAction left, UnityAction right)
    {
        if (targetFollow.target != null)
        {
            CloseButton();
        }
        
        targetFollow.target = _tr;
        ani.SetTrigger("Open");
        forwardButton.onClick.AddListener(forward);
        backButton.onClick.AddListener(back);
        leftButton.onClick.AddListener(left);
        rightButton.onClick.AddListener(right);

        forwardButton.onClick.AddListener(CloseButton);
        backButton.onClick.AddListener(CloseButton);
        leftButton.onClick.AddListener(CloseButton);
        rightButton.onClick.AddListener(CloseButton);

    }

    public void CloseButton()
    {
        targetFollow.target = null;
        ani.SetTrigger("Close");
        forwardButton.onClick.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();
        leftButton.onClick.RemoveAllListeners();
        rightButton.onClick.RemoveAllListeners();
    }
    

}
