using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Klasse om de photo objecten the beheren + groep manipulatie
public class PhotoCollection : MonoBehaviour {
    public bool roteren = false;
    public bool drag;
    private int aantalBlok;
    private float rotationAngle;
    public float rotSpeed;
    private float closestRot;
    public Photo foto;

    void Update () {
        closestRot = 0;

        //Als men niet dragt, dan roteert de collectie naar een bepaalde hoek
        if (drag == false)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, rotationAngle, 0), 5 * Time.deltaTime);
        }
        //Anders wordt de snelheid bijgehouden
        else
        {
            for (int i = 0; i < aantalBlok; i++)
            {
                float offset = 360*1.5f/aantalBlok;

                if (transform.eulerAngles.y > -offset + 360 * i / aantalBlok + 360 / aantalBlok
                    && transform.eulerAngles.y < -offset + 360 * (i + 1) / aantalBlok + 360 / aantalBlok)
                {
                    closestRot =  i *360 / aantalBlok;
                }
            }
            rotationAngle = closestRot;
        }
        
        //Als de rotatie snelheid te traag is stopt het drag-gedrag
        if (rotSpeed < 0.5f*3/aantalBlok && rotSpeed > -0.5f*3/aantalBlok)
        {
            rotSpeed = 0;
            drag = false;
        }
    }

    //Aanmaken van photo objecten
    public void createPhotos(int aantal)
    {
        transform.position = new Vector3(0, 0, (aantal * 5 + 20));
        rotationAngle = 0;
        transform.rotation = Quaternion.Euler(0, 0, 0);

        for (int i = 0; i < aantal; i++)
        {
            foto.number = i;
            foto.maxNumber = aantal;
            foto.collection = this;
            Photo fot =  Instantiate(foto);
            fot.transform.parent = transform;
        }
        aantalBlok = Object.FindObjectsOfType<Photo>().Length;
    }

    //Vernietigen van photo objecten
    public void destroyPhotos()
    {
        foreach (Photo o in transform.GetComponentsInChildren<Photo>())
        {
            Destroy(o.gameObject);
        }
    }

    //Links roteren
    public void rotateLeft()
    {
        rotationAngle -= 360 / aantalBlok;
        if (rotationAngle <= 0)
        {
            rotationAngle = rotationAngle + 360;
        }
    }

    //Rects roteren
    public void rotateRight()
    {
        rotationAngle += 360 / aantalBlok;
        if (rotationAngle >= 360)
        {
            rotationAngle = rotationAngle - 360;
        }
    }
}
