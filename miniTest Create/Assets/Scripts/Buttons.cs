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
    Animator anim2;
    string key1 = "AIzaSyC6JEaOJbC9ut-k713kL2btnWYpM-qqPro";
    string key2 = "AIzaSyCFi0PGjjJ0CJqWOEry3ZoqZ1due2EdIW0";
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
    string refe = "";
    string titel = "";
    string info = "";
    //string refe = "";
    public TextAsset infile;
    private IEnumerator loadInfo()
    {
        refe = "";
        
       /* getURL = "https://maps.googleapis.com/maps/api/place/textsearch/xml?query=" + bluebox.GetComponentInChildren<InputField>().text.Replace(" ", "+") + "&key=" + key2;
        xml = new XmlDocument();
        WWW getInfo = new WWW(getURL);
        while (!getInfo.isDone)
        {
            Debug.Log("waiting");
            yield return new WaitForSeconds(0.1f);
        }


        
        yield return getInfo;
        test = getInfo.text;

        xml.Load(new StringReader(test));
        
        xml.Save(Path.Combine(Application.dataPath, "xml.xml"));
        */
        
        
        //Offline loading, problemen
        if (File.Exists("xml.xml"))
        {
            xml = new XmlDocument();

            TextAsset xmlData = new TextAsset();
            xmlData = (TextAsset)Resources.Load("xml.xml", typeof(TextAsset));
            test = infile.text;
            print(test);


            xml.Load(new StringReader(test));
        }
        else
        {
            print("no xml");
        }
        

        string xmlPathPattern = "//PlaceSearchResponse/result";
        print(xml);
        XmlNodeList myNodeList = xml.SelectNodes(xmlPathPattern);
        string type = "";
        string adres = "";
        string locatie = "";
        
        foreach (XmlNode node in myNodeList)
        {
            XmlNode name = node.FirstChild;
            type = node.SelectSingleNode("type").InnerText;
            adres = node.SelectSingleNode("formatted_address").InnerText;
            locatie = "\n    Longitude: " + node.SelectSingleNode("geometry").SelectSingleNode("location").SelectSingleNode("lng").InnerText + "\n    Latitude: " + node.SelectSingleNode("geometry").SelectSingleNode("location").SelectSingleNode("lat").InnerText;
            XmlNode photo = node.SelectSingleNode("photo");
            XmlNode photoRef = photo.SelectSingleNode("photo_reference");
            titel = name.InnerText;
            if (refe == "")
            {
                refe = photoRef.InnerText;
            }

            
        }
        foreach (Photo o in collection.transform.GetComponentsInChildren<Photo>())
        {
            o.titel = titel;

            o.infoText = "Type: " + type + "\n\nAdres: " + adres + "\n\nLocatie: " + locatie;
        }

        print(test);

        fotoURL = "https://maps.googleapis.com/maps/api/place/photo?maxwidth=400&photoreference="+ refe +"&key=" + key2;
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

    }
    /*void parseXML(string xmlData)
    {
        string xmlPathPattern = "//PlaceSearchResponse/result";
        XmlNodeList myNodeList = xml.SelectNodes(xmlPathPattern);
        foreach (XmlNode node in myNodeList){
            XmlNode name = node.FirstChild;
        }
    }*/
    private void OnGUI()
    {
       // GUILayout.Label(texture);
    }
    void Start () {
        can = GameObject.FindObjectOfType<Canvas>();
        total = can.transform.Find("TotalPanel").gameObject;

        anim = can.GetComponentInChildren<Animator>();
        anim2 = total.gameObject.transform.Find("ResultPanel").gameObject.GetComponentInChildren<Animator>();

        bluebox = total.transform.Find("BlueBox").gameObject;
        panelRectTransform = total.transform as RectTransform;

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void changeLeave()
    {
        if (total.GetComponentInChildren<InputField>().text != "" && total.GetComponentInChildren<InputField>().text != "0" && total.GetComponentInChildren<InputField>().text != "-" && total.GetComponentInChildren<InputField>().text != "+" 
            && bluebox.GetComponentInChildren<InputField>().text != "")
        {

            anim.SetBool("Leave", true);

        }
        StartCoroutine("loadInfo");

    }

    public void enter()
    {

        anim.SetBool("Leave", false);
        anim2.SetBool("Enter", false);

        collection.destroyPhotos();
        

    }
   

    public void closeInfo()
    {
        anim2.SetBool("Enter", false);

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
    public void OnMouseDown()
    {
        
    }
}
