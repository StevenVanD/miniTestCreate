using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour {
    Canvas can;
    Animator anim;
    GameObject total;
    private RectTransform panelRectTransform;

    // Use this for initialization
    void Start () {
        can = GameObject.FindObjectOfType<Canvas>();
        //can = GetComponent<Canvas>();

        anim = can.GetComponentInChildren<Animator>();
        //can = GetComponent<Canvas>();
        total = can.transform.GetChild(0).gameObject;
        panelRectTransform = total.transform as RectTransform;

    }

    // Update is called once per frame
    void Update()
    {

        if (panelRectTransform.anchorMin.y == 1)
        {
            total.SetActive(false);

        }

    }
    public void changeLeave()
    {
        anim.SetBool("Leave", true);
    }
}
