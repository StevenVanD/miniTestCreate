﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Photo : MonoBehaviour
{
    private bool dragging;
    public int number;
    public int maxNumber;
    private float rotation;
    public string titel;
    public string infoText;
    public PhotoCollection collection;
    private Animator anim;

    void Start()
    {
        rotation = (360 * number / maxNumber +180) % 360;
        transform.position = new Vector3(maxNumber * 5 * Mathf.Sin(rotation * Mathf.Deg2Rad), 0, maxNumber * 5 * Mathf.Cos(rotation * Mathf.Deg2Rad) + maxNumber*5 +20);
    }

    void Update()
    {
        if (collection.rot == true)
        {
            transform.LookAt(collection.transform.position);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (dragging == false)
        {
            collection.rotX = Mathf.Lerp(collection.rotX, 0, 0.5f*Time.deltaTime) ;
        }

        collection.transform.Rotate(Vector3.up, -collection.rotX);
    }

    public void showInfo()
    {
        GameObject.Find("Titel").GetComponent<Text>().text = titel;
        GameObject.Find("Info").GetComponent<Text>().text = infoText;
        anim = GameObject.Find("InfoPanel").gameObject.GetComponent<Animator>();
        anim.SetBool("Enter", true);
    }

    void OnMouseDrag()
    {
        if(collection.rotX >0.1f || collection.rotX < -0.1f)
        {
            dragging = true;
        }

        collection.drag = true;
        collection.rotX = Input.GetAxis("Mouse X") * 15/maxNumber ;
    }

    private void OnMouseUp()
    {
        if (dragging == false)
        {
            showInfo();
        }

        dragging = false;
    }
}
