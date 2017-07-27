using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Photo : MonoBehaviour
{
    private string titel;
    private string infoText;
    private Sprite afbeelding;

    public int number;
    public int maxNumber;
    public float rotation;
    public float moveSpeed = 5;
    public PhotoCollection collection;

    private Vector3 v;
    float hoekOver = 0;
    public bool richting;
    float degreesPerSecond = 90;
    float VerplaatsHoek;

    // Use this for initialization
    void Start()
    {
        rotation = (360 * number / maxNumber + 180) % 360;
        transform.position = new Vector3(-20 * Mathf.Sin(rotation * Mathf.Deg2Rad), 8 * Mathf.Cos(rotation * Mathf.Deg2Rad) + 3, 20 * Mathf.Cos(rotation * Mathf.Deg2Rad));
        v = new Vector3(-20 * Mathf.Sin(rotation * Mathf.Deg2Rad), 8 * Mathf.Cos(rotation * Mathf.Deg2Rad) + 3, 20 * Mathf.Cos(rotation * Mathf.Deg2Rad));

    }

    // Update is called once per frame
    void Update()
    {

        VerplaatsHoek = degreesPerSecond * moveSpeed * Time.deltaTime;
        


        if (hoekOver != 0)
        {
            if (richting == true)
            {
                if (hoekOver - VerplaatsHoek < 0)
                {
                    VerplaatsHoek = hoekOver;
                }
                v = Quaternion.AngleAxis(VerplaatsHoek, Vector3.up) * v;
                hoekOver -= VerplaatsHoek;
                rotation -= VerplaatsHoek;

            }
            else
            {
                if (hoekOver + VerplaatsHoek > 0)
                {
                    VerplaatsHoek = -hoekOver;
                }
                v = Quaternion.AngleAxis(VerplaatsHoek, Vector3.down) * v;
                hoekOver += VerplaatsHoek;
                rotation += VerplaatsHoek;
            }

        }

        transform.position = new Vector3(collection.transform.position.x + v.x, collection.transform.position.z + v.z * 0.4f + 4, collection.transform.position.z + v.z);

        if (collection.rot == true)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 180 - rotation, 0), VerplaatsHoek);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), moveSpeed * Time.deltaTime);
        }
    }
    public void rotateLeft()
    {
        richting = true;
        hoekOver += 360 / maxNumber;
    }
    public void rotateRight()
    {
        richting = false;
        hoekOver -= 360 / maxNumber;
    }
}
