using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoCollection : MonoBehaviour {
    private Photo[] photolijst;
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
        //Destroy(gameObject);
        foreach (Photo o in Object.FindObjectsOfType<Photo>())
        {
            Destroy(o);
        }
        for (int i = 0; i < aantal; i++)
        {
            foto.number = i;
            foto.maxNumber = aantal;
            foto.collection = this;
            Photo fot =  Instantiate(foto);
            fot.transform.parent = transform;
        }
    }
    public void rotateLeft()
    {
        rotation -= 360 / aantalBlok;
        if (rotation <= 0)
        {
            rotation = rotation + 360;
        }
       //rotating = false;

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
