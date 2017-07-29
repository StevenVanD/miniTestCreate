using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

//Klasse voor het afhandel gedeeltelijk het ophalen van de informatie en gedeeltelijk de comando's van de userinterface
public class Buttons : MonoBehaviour {
    private string key1 = "AIzaSyC6JEaOJbC9ut-k713kL2btnWYpM-qqPro";
    private string key2 = "AIzaSyCFi0PGjjJ0CJqWOEry3ZoqZ1due2EdIW0";
    private string getURL = "https://maps.googleapis.com/maps/api/place/textsearch/xml?query=Affligem&key=AIzaSyC6JEaOJbC9ut-k713kL2btnWYpM-qqPro";
    private string fotoURL = "https://maps.googleapis.com/maps/api/place/photo?maxwidth=400&photoreference=CnRtAAAATLZNl354RwP_9UKbQ_5Psy40texXePv4oAlgP4qNEkdIrkyse7rPXYGd9D_Uj1rVsQdWT4oRz4QrYAJNpFX7rzqqMlZw2h2E2y5IKMUZ7ouD_SlcHxYq1yL4KbKUv3qtWgTK0A6QbGh87GB3sscrHRIQiG2RrmU_jF4tENr9wGS_YxoUSSDrYjWmrNfeEHSGSc3FyhNLlBU&key=AIzaSyC6JEaOJbC9ut-k713kL2btnWYpM-qqPro";
    private string refe = "";
    private bool realPlace;
    private Canvas canvas;
    private Animator animTotal;
    private Animator animInfo;
    private GameObject total;
    private GameObject bluebox;
    public PhotoCollection collection;
    private Sprite sprite;
    private Texture2D texture;
    private XmlDocument xml;
    public TextAsset infile;

    //Connectie checken
    private static bool hasConnection()
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

    //Info laden
    private IEnumerator loadInfo()
    {
        //Checken of er internetconnectie is
        yield return new WaitForSeconds(0.5f);
        refe = "";
        getURL = "https://maps.googleapis.com/maps/api/place/textsearch/xml?query=" 
                 + bluebox.GetComponentInChildren<InputField>().text.Replace(" ", "+") + "&key=" + key1;
        xml = new XmlDocument();
        WWW getInfo = new WWW(getURL);
        if (hasConnection() != true)
        {
            loadOffline();
        }
        else
        {
            //Checken of de zoekopdracht effectief een resultaat met een foto geeft
            while (!getInfo.isDone)
            {
                yield return new WaitForSeconds(0.1f);
            }

            yield return getInfo;
            xml.Load(new StringReader(getInfo.text));
            string testPathPattern = "//PlaceSearchResponse";
            XmlNodeList testNodeList = xml.SelectNodes(testPathPattern);
            realPlace = false;
            foreach (XmlNode node in testNodeList)
            {
                XmlNode status = node.FirstChild;
                if (status.InnerText == "OK")
                {
                    realPlace = true;
                }
            }

            //Info uit de xmluitlezen + xml bestand opslaan
            testPathPattern = "//PlaceSearchResponse/result";
            testNodeList = xml.SelectNodes(testPathPattern);

            foreach (XmlNode node in testNodeList)
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
                string naam = "";
                string type = "";
                string adres = "";
                string locatie = "";

                foreach (XmlNode node in myNodeList)
                {
                    XmlNode name = node.SelectSingleNode("name");
                    XmlNode photo = node.SelectSingleNode("photo");
                    XmlNode photoRef = photo.SelectSingleNode("photo_reference");
                    naam = name.InnerText;
                    type = node.SelectSingleNode("type").InnerText;
                    adres = node.SelectSingleNode("formatted_address").InnerText;
                    locatie = "\n    Longitude: " + node.SelectSingleNode("geometry").SelectSingleNode("location").SelectSingleNode("lng").InnerText 
                            + "\n    Latitude: " + node.SelectSingleNode("geometry").SelectSingleNode("location").SelectSingleNode("lat").InnerText;
                    if (refe == "")
                    {
                        refe = photoRef.InnerText;
                    }
                }

                //Opgehaalde info in alle photo objecten laden
                foreach (Photo o in collection.transform.GetComponentsInChildren<Photo>())
                {
                    o.naam = naam;
                    o.infoText = "Type: " + type + "\n\nAdres: " + adres + "\n\nLocatie: " + locatie;
                }

                //Foto halen van de referentie website
                fotoURL = "https://maps.googleapis.com/maps/api/place/photo?maxwidth=400&photoreference=" + refe + "&key=" + key1;
                WWW getFoto = new WWW(fotoURL);

                while (!getFoto.isDone)
                {
                    yield return new WaitForSeconds(0.1f);
                }

                yield return getFoto;
                
                //Opgehaalde foto in alle photo objecten laden + foto opslaan
                texture = getFoto.texture;
                AssetDatabase.CreateAsset(texture, "Assets/texture.asset");
                sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one / 2);
            }
            else
            {
                loadOffline();
            }
        }

        //Opgehaalde foto in alle photo objecten laden

        foreach (Photo o in collection.transform.GetComponentsInChildren<Photo>())
        {
            o.GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }

    //Offline data laden
    private void loadOffline()
    {
        string naam = "";
        string type = "";
        string adres = "";
        string locatie = "";
        //Info halen uit het xml bestand indien die er is.
        if (infile != null)
        {
            xml.Load(new StringReader(infile.text));
            string xmlPathPattern = "//PlaceSearchResponse/result";
            XmlNodeList myNodeList = xml.SelectNodes(xmlPathPattern);
            foreach (XmlNode node in myNodeList)
            {
                XmlNode name = node.FirstChild;
                naam = name.InnerText;
                type = node.SelectSingleNode("type").InnerText;
                adres = node.SelectSingleNode("formatted_address").InnerText;
                locatie = "\n    Longitude: " + node.SelectSingleNode("geometry").SelectSingleNode("location").SelectSingleNode("lng").InnerText 
                        + "\n    Latitude: " + node.SelectSingleNode("geometry").SelectSingleNode("location").SelectSingleNode("lat").InnerText;
            }
        }

        //Data van het xml bestand in de photo objecten steken
        foreach (Photo o in collection.transform.GetComponentsInChildren<Photo>())
        {
            o.naam = naam;
            o.infoText = "Type: " + type + "\n\nAdres: " + adres + "\n\nLocatie: " + locatie;
        }

        //Foto halen van de assets
        texture = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/texture.asset", typeof(Texture2D));
        sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one / 2);
    }
    

    void Start () {
        canvas = GameObject.FindObjectOfType<Canvas>();
        total = canvas.transform.Find("TotalPanel").gameObject;

        animTotal = canvas.GetComponentInChildren<Animator>();
        animInfo = total.gameObject.transform.Find("ResultPanel").gameObject.GetComponentInChildren<Animator>();

        bluebox = total.transform.Find("BlueBox").gameObject;
    }

    //Aanmaken van de photo objecten als men op de OK knop heeft gedrukt
    public void changeLeave()
    {
        if (total.GetComponentInChildren<InputField>().text != "" && total.GetComponentInChildren<InputField>().text != "0" 
            && total.GetComponentInChildren<InputField>().text != "-" && total.GetComponentInChildren<InputField>().text != "+" 
            && bluebox.GetComponentInChildren<InputField>().text != "")
        {
            animTotal.SetBool("Leave", true);
            StartCoroutine("loadInfo");
        }
    }

    //Photo objecten verwijderen als men op de terugkeer knop heeft gedrukt
    public void enter()
    {
        animTotal.SetBool("Leave", false);
        animInfo.SetBool("Enter", false);
        collection.destroyPhotos();
    }

    //Info vester sluiten
    public void closeInfo()
    {
        animInfo.SetBool("Enter", false);
    }

    //Carousel links laten draaien
    public void moveLeft()
    {
        collection.drag = false;
        collection.rotateLeft();
    }

    //Carousel rechts laten draaien
    public void moveRight()
    {
        collection.drag = false;
        collection.rotateRight();
    }
}
