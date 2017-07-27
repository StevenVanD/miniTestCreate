using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Photo : MonoBehaviour {
    private string titel;
    private string infoText;
    private Sprite afbeelding;

    public int number;
    public int rotation;
    public float moveSpeed = 5f;
    public PhotoCollection collection;

    // Use this for initialization
    void Start () {
        collection = transform.parent.gameObject.GetComponent<PhotoCollection>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            rotation += 45;
            if (rotation == 360)
            {
                rotation = 0;
            }
        }
        transform.position = new Vector3(Mathf.Lerp(transform.position.x, -15 * Mathf.Sin(rotation * Mathf.Deg2Rad), moveSpeed * Time.deltaTime), Mathf.Lerp(transform.position.y, 8 * Mathf.Cos(rotation * Mathf.Deg2Rad)+3, moveSpeed * Time.deltaTime), Mathf.Lerp(transform.position.z, 5 * Mathf.Cos(rotation * Mathf.Deg2Rad), moveSpeed * Time.deltaTime));
        if (collection.rot == true)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 180-rotation, 0), moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), moveSpeed * Time.deltaTime);
        }
    }
}
