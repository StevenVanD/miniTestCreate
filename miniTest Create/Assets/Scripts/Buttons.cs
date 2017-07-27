using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.IO;
using System.Xml.Serialization;

public class Buttons : MonoBehaviour {
    Canvas can;
    Animator anim;
    
    GameObject total;
    GameObject bluebox;
    private RectTransform panelRectTransform;
    public PhotoCollection collection;
    // Use this for initialization
    public Texture2D img;
    private string getURL = "https://maps.googleapis.com/maps/api/place/textsearch/xml?query=Affligem&key=AIzaSyC6JEaOJbC9ut-k713kL2btnWYpM-qqPro";
    public string fotoURL = "https://maps.googleapis.com/maps/api/place/photo?maxwidth=400&photoreference=CnRtAAAATLZNl354RwP_9UKbQ_5Psy40texXePv4oAlgP4qNEkdIrkyse7rPXYGd9D_Uj1rVsQdWT4oRz4QrYAJNpFX7rzqqMlZw2h2E2y5IKMUZ7ouD_SlcHxYq1yL4KbKUv3qtWgTK0A6QbGh87GB3sscrHRIQiG2RrmU_jF4tENr9wGS_YxoUSSDrYjWmrNfeEHSGSc3FyhNLlBU&key=AIzaSyC6JEaOJbC9ut-k713kL2btnWYpM-qqPro";
    public Sprite sprite;
    public Texture2D texture;
    public string test;
    XmlDocument xml;

    private IEnumerator loadInfo()
    {
        getURL = "https://maps.googleapis.com/maps/api/place/textsearch/xml?query=" + bluebox.GetComponentInChildren<InputField>().text + "&key=AIzaSyC6JEaOJbC9ut-k713kL2btnWYpM-qqPro";
        xml = new XmlDocument();
        WWW getInfo = new WWW(getURL);
        while (!getInfo.isDone)
        {
            Debug.Log("waiting");
            yield return new WaitForSeconds(0.1f);
        }
        


        yield return getInfo;
        print(getInfo.text);
        test = getInfo.text;


        //XmlSerializer serializer = new XmlSerializer(getInfo);

        //StringReader reader = new StringReader(test);
        //xml.LoadXml(test);
        xml.Load(new StringReader(test));
        string xmlPathPattern = "//PlaceSearchResponse/result";
        XmlNodeList myNodeList = xml.SelectNodes(xmlPathPattern);
        string refe = "";
        foreach (XmlNode node in myNodeList)
        {
            XmlNode name = node.FirstChild;
            print(name.InnerText);
                XmlNode photo = node.SelectSingleNode("photo");
                XmlNode photoRef = photo.SelectSingleNode("photo_reference");
                print(photoRef.InnerText);
                refe = photoRef.InnerText;
                //print(refe);

        }        //print(xml.FirstChild.FirstChild.FirstChild.InnerText);
        fotoURL = "https://maps.googleapis.com/maps/api/place/photo?maxwidth=400&photoreference="+ refe +"&key=AIzaSyC6JEaOJbC9ut-k713kL2btnWYpM-qqPro";
        print(fotoURL);
        WWW getFoto = new WWW(fotoURL);
        while (!getFoto.isDone)
        {
            Debug.Log("waiting");
            yield return new WaitForSeconds(0.1f);
        }
        yield return getFoto;

        texture = getFoto.texture;
        sprite = Sprite.Create(texture, new Rect(0, 0,texture.width, texture.height), Vector2.one / 2);
        collection.sprite = sprite;
        foreach (Photo o in collection.transform.GetComponentsInChildren<Photo>())
        {
            o.GetComponent<SpriteRenderer>().sprite = sprite;

        }
        //img = textu;
        //print(getInfo.text);*/

    }
    /*void parseXML(string xmlData)
    {
        string xmlPathPattern = "//PlaceSearchResponse/result";
        XmlNodeList myNodeList = xml.SelectNodes(xmlPathPattern);
        foreach (XmlNode node in myNodeList){
            XmlNode name = node.FirstChild;
            print(name.InnerText);
        }
    }*/
    private void OnGUI()
    {
       // GUILayout.Label(texture);
    }
    void Start () {
        can = GameObject.FindObjectOfType<Canvas>();
        
        anim = can.GetComponentInChildren<Animator>();
        total = can.transform.Find("TotalPanel").gameObject;

        bluebox = total.transform.Find("BlueBox").gameObject;
        panelRectTransform = total.transform as RectTransform;

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void changeLeave()
    {
        StartCoroutine("loadInfo");
        if (total.GetComponentInChildren<InputField>().text != "" && total.GetComponentInChildren<InputField>().text != "0" && total.GetComponentInChildren<InputField>().text != "-" && total.GetComponentInChildren<InputField>().text != "+" 
            && bluebox.GetComponentInChildren<InputField>().text != "")
        {

            anim.SetBool("Leave", true);

        }
    }
    
    public void enter()
    {

        anim.SetBool("Leave", false);
            collection.destroyPhotos();
        

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
         //   o.rotateLeft();
        }
        collection.drag = false;
        collection.rotateLeft();
    }
    public void moveRight()
    {
        foreach (Photo o in collection.transform.GetComponentsInChildren<Photo>())
        {
           // o.rotateRight();
        }
        collection.drag = false;

        collection.rotateRight();

    }
}
