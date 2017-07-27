using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotalPanel : MonoBehaviour {
    public PhotoCollection collection;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Create()
    {
        collection.createPhotos(int.Parse (transform.GetComponentInChildren<InputField>().text));
        collection.rot = transform.GetComponentInChildren<Toggle>().isOn;
    }

}
