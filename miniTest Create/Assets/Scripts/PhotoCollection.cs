using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoCollection : MonoBehaviour {
    public bool rot = false;
    public bool drag;
    private int aantalBlok;
    private float rotation;
    public float rotX;
    private float closestRot;
    public Photo foto;

    void Update () {
        aantalBlok = Object.FindObjectsOfType<Photo>().Length;
        closestRot = 0;

        if (drag == false)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, rotation, 0), 5 * Time.deltaTime);
        }
        else
        {
            for (int i = 0; i < aantalBlok; i++)
            {
                float offset = 810;

                for (int j = 0; j < aantalBlok ; j++)
                {
                    offset -= 360 / Mathf.Pow(2, j);
                }

                if (transform.eulerAngles.y > -offset + 360 * i / aantalBlok + 360 / aantalBlok
                    && transform.eulerAngles.y < -offset + 360 * (i + 1) / aantalBlok + 360 / aantalBlok)
                {
                    closestRot =  i *360 / aantalBlok;
                }
            }
            rotation = closestRot;
        }

        if (rotX < 0.5f*3/aantalBlok && rotX > -0.5f*3/aantalBlok)
        {
            rotX = 0;
            drag = false;
        }
    }

    public void createPhotos(int aantal)
    {
        transform.position = new Vector3(0, 0, (aantal * 5 + 20));
        rotation = 0;
        transform.rotation = Quaternion.Euler(0, 0, 0);

        for (int i = 0; i < aantal; i++)
        {
            foto.number = i;
            foto.maxNumber = aantal;
            foto.collection = this;
            Photo fot =  Instantiate(foto);
            fot.transform.parent = transform;
        }
    }

    public void destroyPhotos()
    {
        foreach (Photo o in transform.GetComponentsInChildren<Photo>())
        {
            Destroy(o.gameObject);
        }
    }

    public void rotateLeft()
    {
        rotation -= 360 / aantalBlok;
        if (rotation <= 0)
        {
            rotation = rotation + 360;
        }
    }

    public void rotateRight()
    {
        rotation += 360 / aantalBlok;
        if (rotation >= 360)
        {
            rotation = rotation - 360;
        }
    }
}
