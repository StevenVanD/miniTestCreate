using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class Buttons : MonoBehaviour {
    private string key1 = "AIzaSyC6JEaOJbC9ut-k713kL2btnWYpM-qqPro";
    private string key2 = "AIzaSyCFi0PGjjJ0CJqWOEry3ZoqZ1due2EdIW0";
    private string getURL = "https://maps.googleapis.com/maps/api/place/textsearch/xml?query=Affligem&key=AIzaSyC6JEaOJbC9ut-k713kL2btnWYpM-qqPro";
    private string fotoURL = "https://maps.googleapis.com/maps/api/place/photo?maxwidth=400&photoreference=CnRtAAAATLZNl354RwP_9UKbQ_5Psy40texXePv4oAlgP4qNEkdIrkyse7rPXYGd9D_Uj1rVsQdWT4oRz4QrYAJNpFX7rzqqMlZw2h2E2y5IKMUZ7ouD_SlcHxYq1yL4KbKUv3qtWgTK0A6QbGh87GB3sscrHRIQiG2RrmU_jF4tENr9wGS_YxoUSSDrYjWmrNfeEHSGSc3FyhNLlBU&key=AIzaSyC6JEaOJbC9ut-k713kL2btnWYpM-qqPro";
    private string test;
    private string refe = "";
    private bool realPlace;
    private Canvas can;
    private Animator anim;
    private Animator anim2;
    private GameObject total;
    private GameObject bluebox;
    public PhotoCollection collection;
    private Sprite sprite;
    private Texture2D texture;
    private XmlDocument xml;
    public TextAsset infile;


    private static bool HasConnection()
    {
        try
        {
            using (WebClient client = new WebClient())
            using (Stream stream = new WebClient().OpenRead("http://www.google.com"))
            {
                return true;
            }
        }
        catch
        {
            return false;
        }
    }

    private IEnumerator loadInfo()
    {
        yield return new WaitForSeconds(0.5f);
        refe = "";
        getURL = "https://maps.googleapis.com/maps/api/place/textsearch/xml?query=" + bluebox.GetComponentInChildren<InputField>().text.Replace(" ", "+") + "&key=" + key1;
        xml = new XmlDocument();
        WWW getInfo = new WWW(getURL);
        if (HasConnection() != true)
        {
            //Offline loading
            string titel = "";
            string type = "";
            string adres = "";
            string locatie = "";
            if (infile != null)
            {
                test = infile.text;
                xml.Load(new StringReader(test));
                string xmlPathPattern = "//PlaceSearchResponse/result";
                XmlNodeList myNodeList = xml.SelectNodes(xmlPathPattern);
                foreach (XmlNode node in myNodeList)
                {
                    XmlNode name = node.FirstChild;
                    type = node.SelectSingleNode("type").InnerText;
                    adres = node.SelectSingleNode("formatted_address").InnerText;
                    locatie = "\n    Longitude: " + node.SelectSingleNode("geometry").SelectSingleNode("location").SelectSingleNode("lng").InnerText + "\n    Latitude: " + node.SelectSingleNode("geometry").SelectSingleNode("location").SelectSingleNode("lat").InnerText;
                    titel = name.InnerText;
                }
            }

            foreach (Photo o in collection.transform.GetComponentsInChildren<Photo>())
            {
                o.titel = titel;
                o.infoText = "Type: " + type + "\n\nAdres: " + adres + "\n\nLocatie: " + locatie;
            }

            //foto
            texture = (Texture2D) AssetDatabase.LoadAssetAtPath("Assets/texture.asset", typeof (Texture2D));
            sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one / 2);
        }
        else
        {
            while (!getInfo.isDone)
            {
                yield return new WaitForSeconds(0.1f);
            }

            yield return getInfo;
            test = getInfo.text;
            xml.Load(new StringReader(test));
            string xmlPathPattern0 = "//PlaceSearchResponse";
            XmlNodeList myNodeList0 = xml.SelectNodes(xmlPathPattern0);
            realPlace = false;
            foreach (XmlNode node in myNodeList0)
            {
                XmlNode status = node.FirstChild;
                if (status.InnerText == "OK")
                {
                    realPlace = true;
                }
            }

            xmlPathPattern0 = "//PlaceSearchResponse/result";
            myNodeList0 = xml.SelectNodes(xmlPathPattern0);

            foreach (XmlNode node in myNodeList0)
            {
                try
                {
                    XmlNode photo = node.SelectSingleNode("photo");
                    if (photo.HasChildNodes == true)
                    {
                        realPlace = true;
                    }
                }
                catch
                {
                    realPlace = false;
                }
            }
            
            if (realPlace == true)
            {
                xml.Save(Path.Combine(Application.dataPath, "xml.xml"));

                string xmlPathPattern = "//PlaceSearchResponse/result";
                XmlNodeList myNodeList = xml.SelectNodes(xmlPathPattern);
                string titel = "";
                string type = "";
                string adres = "";
                string locatie = "";

                foreach (XmlNode node in myNodeList)
                {
                    XmlNode name = node.SelectSingleNode("name");
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

                //foto
                fotoURL = "https://maps.googleapis.com/maps/api/place/photo?maxwidth=400&photoreference=" + refe + "&key=" + key1;
                WWW getFoto = new WWW(fotoURL);

                while (!getFoto.isDone)
                {
                    yield return new WaitForSeconds(0.1f);
                }

                yield return getFoto;

                texture = getFoto.texture;
                AssetDatabase.CreateAsset(texture, "Assets/texture.asset");
                sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one / 2);
            }
            else
            {
                //Offline loading
                string titel = "";
                string type = "";
                string adres = "";
                string locatie = "";

                if (infile != null)
                {
                    test = infile.text;
                    xml.Load(new StringReader(test));
                    string xmlPathPattern = "//PlaceSearchResponse/result";
                    XmlNodeList myNodeList = xml.SelectNodes(xmlPathPattern);
                    foreach (XmlNode node in myNodeList)
                    {
                        XmlNode name = node.FirstChild;
                        type = node.SelectSingleNode("type").InnerText;
                        adres = node.SelectSingleNode("formatted_address").InnerText;
                        locatie = "\n    Longitude: " + node.SelectSingleNode("geometry").SelectSingleNode("location").SelectSingleNode("lng").InnerText + "\n    Latitude: " + node.SelectSingleNode("geometry").SelectSingleNode("location").SelectSingleNode("lat").InnerText;
                        titel = name.InnerText;
                    }
                }

                foreach (Photo o in collection.transform.GetComponentsInChildren<Photo>())
                {
                    o.titel = titel;
                    o.infoText = "Type: " + type + "\n\nAdres: " + adres + "\n\nLocatie: " + locatie;
                }

                //foto
                texture = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/texture.asset", typeof(Texture2D));
                sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one / 2);
            }
        }

        foreach (Photo o in collection.transform.GetComponentsInChildren<Photo>())
        {
            o.GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }

    void Start () {
        can = GameObject.FindObjectOfType<Canvas>();
        total = can.transform.Find("TotalPanel").gameObject;

        anim = can.GetComponentInChildren<Animator>();
        anim2 = total.gameObject.transform.Find("ResultPanel").gameObject.GetComponentInChildren<Animator>();

        bluebox = total.transform.Find("BlueBox").gameObject;
    }

    public void changeLeave()
    {
        if (total.GetComponentInChildren<InputField>().text != "" && total.GetComponentInChildren<InputField>().text != "0" && total.GetComponentInChildren<InputField>().text != "-" && total.GetComponentInChildren<InputField>().text != "+" 
            && bluebox.GetComponentInChildren<InputField>().text != "")
        {
            anim.SetBool("Leave", true);
            StartCoroutine("loadInfo");
        }
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
        collection.drag = false;
        collection.rotateLeft();
    }

    public void moveRight()
    {
        collection.drag = false;
        collection.rotateRight();

    }
}
