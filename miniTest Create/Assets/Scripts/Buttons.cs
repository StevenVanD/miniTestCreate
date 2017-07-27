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

        if (panelRectTransform.anchorMin.y == 1)
        {
            total.SetActive(false);

        }

    }
    public void changeLeave()
    {
        anim.SetBool("Leave", true);
        collection.createPhotos((int)total.transform.GetComponentInChildren<Slider>().value);
        collection.rot = total.transform.GetComponentInChildren<Toggle>().isOn;
    }
    public void enter()
    {
        anim.SetBool("Leave", false);
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
