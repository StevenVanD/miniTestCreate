using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
    public string getURL = "https://maps.googleapis.com/maps/api/place/textsearch/xml?query=123+main&key=AIzaSyC6JEaOJbC9ut-k713kL2btnWYpM-qqPro";
    private IEnumerator loadInfo()
    {
        WWW getInfo = new WWW(getURL);
        yield return getInfo;
        Debug.Log(getInfo);

    }
   /* private IEnumerator saveInfo()
    {/*
        string urlstring = getURL + "?name=" + 
        WWW getInfo = new WWW(getURL);
        yield return getInfo;
        print(getInfo);

    }*/
    public void startCoroutine()
    {
        StartCoroutine("loadInfo");
    }

}
