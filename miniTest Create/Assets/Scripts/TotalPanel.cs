using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotalPanel : MonoBehaviour {
    public PhotoCollection collection;

    //Wordt geactiveerd tijdens een animatie.
    public void create()
    {
        collection.createPhotos(int.Parse(transform.GetComponentInChildren<InputField>().text));
        collection.roteren = transform.GetComponentInChildren<Toggle>().isOn;
    }
}
