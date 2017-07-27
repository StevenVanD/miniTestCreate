using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoCollection : MonoBehaviour {
    public bool rot = false;
    public Photo foto;
    public float rotation;
    public int aantalBlok;
    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        aantalBlok = Object.FindObjectsOfType<Photo>().Length;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, rotation, 0), 5 * Time.deltaTime);
    }
    public void createPhotos(int aantal)
    {
        
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

       // rotating = true;
    }
}
