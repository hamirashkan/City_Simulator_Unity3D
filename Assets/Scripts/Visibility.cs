using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;


public class ReadXmlData : MonoBehaviour // the Class.
{
    public GameObject CubeObject; // object to get a apply the transforms.
    public GUISkin customSkin;

    private Rect windowRect = new Rect((Screen.width / 2) - 250, 20, 500, 200);

    private string x = ""; // string to work with the xml file.
    private string y = ""; // string to work with the xml file.
    private string z = ""; // string to work with the xml file.

    private float X = 0; // we will apply the values ​​of the strings here.
    private float Y = 0; // we will apply the values ​​of the strings here.
    private float Z = 0; // we will apply the values ​​of the strings here.

    void Update()
    {
        x = CubeObject.transform.rotation.eulerAngles.x.ToString(); // taking the values ​​of transformation.
        y = CubeObject.transform.rotation.eulerAngles.y.ToString(); // taking the values ​​of transformation.
        z = CubeObject.transform.rotation.eulerAngles.z.ToString(); // taking the values ​​of transformation.
    }

    void OnGUI()
    {
        if (customSkin)
            GUI.skin = customSkin;
        else
            Debug.Log("StartMenuGUI : GUI Skin object missing!");

        windowRect = GUILayout.Window(0, windowRect, DoMyWindow, "Transforms");

        GUILayout.BeginArea(new Rect(Screen.width / 2 - 100, 110, 150, 100));
        GUILayout.Label("Click and drag the cube!");
        GUILayout.EndArea();
    }

    void DoMyWindow(int windowID)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Angles  X:");
        x = GUILayout.TextField(x, 10);

        GUILayout.Label("  Y:");
        y = GUILayout.TextField(y, 10);

        GUILayout.Label("  Z:");
        z = GUILayout.TextField(z, 10);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            WriteToXml(); // calls the function when the button is pressed.
        }
        if (GUILayout.Button("Restore"))
        {
            LoadFromXml(); // calls the function when the button is pressed.
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

    }

    public void WriteToXml()
    {

        string filepath = Application.dataPath + @"/Data/gamexmldata.xml";
        XmlDocument xmlDoc = new XmlDocument();

        if (File.Exists(filepath))
        {
            xmlDoc.Load(filepath);

            XmlElement elmRoot = xmlDoc.DocumentElement;

            elmRoot.RemoveAll(); // remove all inside the transforms node.

            XmlElement elmNew = xmlDoc.CreateElement("rotation"); // create the rotation node.

            XmlElement rotation_X = xmlDoc.CreateElement("x"); // create the x node.
            rotation_X.InnerText = x; // apply to the node text the values of the variable.

            XmlElement rotation_Y = xmlDoc.CreateElement("y"); // create the y node.
            rotation_Y.InnerText = y; // apply to the node text the values of the variable.

            XmlElement rotation_Z = xmlDoc.CreateElement("z"); // create the z node.
            rotation_Z.InnerText = z; // apply to the node text the values of the variable.

            elmNew.AppendChild(rotation_X); // make the rotation node the parent.
            elmNew.AppendChild(rotation_Y); // make the rotation node the parent.
            elmNew.AppendChild(rotation_Z); // make the rotation node the parent.
            elmRoot.AppendChild(elmNew); // make the transform node the parent.

            xmlDoc.Save(filepath); // save file.
        }
    }

    public void LoadFromXml()
    {
        string filepath = Application.dataPath + @"/Data/gamexmldata.xml";
        XmlDocument xmlDoc = new XmlDocument();

        if (File.Exists(filepath))
        {
            xmlDoc.Load(filepath);

            XmlNodeList transformList = xmlDoc.GetElementsByTagName("rotation");

            foreach (XmlNode transformInfo in transformList)
            {
                XmlNodeList transformcontent = transformInfo.ChildNodes;

                foreach (XmlNode transformItens in transformcontent)
                {
                    if (transformItens.Name == "x")
                    {
                        X = float.Parse(transformItens.InnerText); // convert the strings to float and apply to the X variable.
                    }
                    if (transformItens.Name == "y")
                    {
                        Y = float.Parse(transformItens.InnerText); // convert the strings to float and apply to the Y variable.
                    }
                    if (transformItens.Name == "z")
                    {
                        Z = float.Parse(transformItens.InnerText); // convert the strings to float and apply to the Z variable.
                    }

                }
            }
        }
        CubeObject.transform.eulerAngles = new Vector3(X, Y, Z); // Apply the values to the cube object.

    }

}