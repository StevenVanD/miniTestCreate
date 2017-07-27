using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buttons : MonoBehaviour {
    Canvas can;
    Animator anim;
    GameObject total;

    private RectTransform panelRectTransform;
    public PhotoCollection collection;
    // Use this for initialization
    void Start () {
        can = GameObject.FindObjectOfType<Canvas>();

        anim = can.GetComponentInChildren<Animator>();
        total = can.transform.GetChild(0).gameObject;
        panelRectTransform = total.transform as RectTransform;

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void changeLeave()
    {
        anim.SetBool("Leave", true);
        

    }
    
    public void enter()
    {
        anim.SetBool("Leave", false);
        collection.destroyPhotos();

    }
    public void showSettings()
    {

    }
    public void changeOrientation()
    {

    }
    public void closeSettings()
    {

    }
    public void showInfo()
    {

    }
    public void closeInfo()
    {

    }
    public void moveLeft()
    {
        foreach (Photo o in collection.transform.GetComponentsInChildren<Photo>())
        {
            o.rotateLeft();
        }

        //collection.rotateLeft();
    }
    public void moveRight()
    {
        foreach (Photo o in collection.transform.GetComponentsInChildren<Photo>())
        {
            o.rotateRight();
        }
        //collection.rotateRight();

    }
    public void amountOfScreens(float amount)
    {
    }
}
