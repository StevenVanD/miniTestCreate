using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Klasse om de foto's elk hun eigen rotatie, etc te geven.
public class Photo : MonoBehaviour
{
    private bool dragging;
    public int number;
    public int maxNumber;
    private float rotationAngle;
    public string naam;
    public string infoText;
    public PhotoCollection collection;
    private Animator animInfo;

    void Start()
    {
        rotationAngle = (360 * number / maxNumber +180) % 360;
        transform.position = new Vector3(maxNumber * 5 * Mathf.Sin(rotationAngle * Mathf.Deg2Rad), 0, maxNumber * 5 * Mathf.Cos(rotationAngle * Mathf.Deg2Rad) + maxNumber*5 +20);
    }

    void Update()
    {
        //Roteren naar het centerpunt
        if (collection.roteren == true)
        {
            transform.LookAt(collection.transform.position);
        }
        //Roteren naar het scherm
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        //Tijdens het draggen de foto's roteren
        if (dragging == false)
        {
            collection.rotSpeed = Mathf.Lerp(collection.rotSpeed, 0, 0.5f*Time.deltaTime) ;
        }

        collection.transform.Rotate(Vector3.up, -collection.rotSpeed);
    }

    //Info overzetten
    public void showInfo()
    {
        GameObject.Find("Naam").GetComponent<Text>().text = naam;
        GameObject.Find("Info").GetComponent<Text>().text = infoText;
        animInfo = GameObject.Find("InfoPanel").gameObject.GetComponent<Animator>();
        animInfo.SetBool("Enter", true);
    }

    //Tijdens het verplaatsen waardes wijzigen
    void OnMouseDrag()
    {
        if(collection.rotSpeed > 0.1f || collection.rotSpeed < -0.1f)
        {
            dragging = true;
        }

        collection.drag = true;
        collection.rotSpeed = Input.GetAxis("Mouse X") * 15/maxNumber ;
    }

    //Drag deactiveren
    private void OnMouseUp()
    {
        if (dragging == false)
        {
            showInfo();
        }

        dragging = false;
    }
}
