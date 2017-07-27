using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Photo : MonoBehaviour
{
    public string titel;
    public string infoText;
    private Sprite afbeelding;

    public int number;
    public int maxNumber;
    public float rotation;
    public float moveSpeed = 5;
    public PhotoCollection collection;

    public float swipeVelocity;
    public float currentSwipe;
    public float swipeEndTime;
    public float swipeStartTime;
    public float secondPressPos;
    public float firstPressPos;
    public bool dragging;
    Animator anim;

    void Start()
    {
        //Camera.main.GetComponent<Buttons>().StartCoroutine("loadInfo");
        rotation = (360 * number / maxNumber +180) % 360;
        transform.position = new Vector3(maxNumber * 5 * Mathf.Sin(rotation * Mathf.Deg2Rad), 0, maxNumber * 5 * Mathf.Cos(rotation * Mathf.Deg2Rad) + maxNumber*5 +20);
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.RotateAround(new Vector3(0, 0, 30), Vector3.up, moveSpeed);
        if (collection.rot == true)
        {
            transform.LookAt(collection.transform.position);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }



        if (Input.GetMouseButtonDown(0) == true)
        {
            swipeStartTime = Time.time;

        }
        if (Input.GetMouseButtonUp(0) == true)
        {

        }
        swipeEndTime = Time.time;
        if (dragging == false)
        {
            collection.rotX = Mathf.Lerp(collection.rotX, 0, 0.5f*Time.deltaTime) ;
        }
        collection.transform.Rotate(Vector3.up, -collection.rotX);
        

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
    public void showInfo()
    {

        print("hey");
        GameObject.Find("Titel").GetComponent<Text>().text = titel;
        GameObject.Find("Info").GetComponent<Text>().text = infoText;
        anim = GameObject.Find("InfoPanel").gameObject.GetComponent<Animator>();
        anim.SetBool("Enter", true);

    }
}
