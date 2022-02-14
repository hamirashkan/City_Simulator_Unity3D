using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class Landmark : MonoBehaviour
{

    public float RaysNumber = 360f;
    public static int Building_numbers = 54;
    public static int Building_numbers3 = 0 ;
    private float Rayorigin;
    float ComputeLandmark;
    float count;
    public Text LandMark;
    public GameObject Buildings;
    private GameObject[] respawns;
    private Vector3 Origin;


    public Color X2 { get; private set; }
    public Color X1 { get; private set; }
    private bool isClicked = false;
    RaycastHit hit;

    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material highlightMaterial;


    private void Start()
    {
        X1 = Color.clear;
        X2 = Color.white;

    }
    void settingText()
    {

            Building_numbers3 = Building_numbers - GroundPlacementController.Building_numbers2;
            LandMark.text = ("Landmark Visibility: " + ComputeLandmark.ToString("0") + " /" + Building_numbers3 + " at Height: " + Rayorigin.ToString("0.0"));
            //Raycounter.text = ("Ray count : " + countRay.ToString());

    }


    void OnGUI()
    {
        GUI.backgroundColor = Color.magenta;
        GUIStyle myButtonStyle1 = new GUIStyle(GUI.skin.button);
        myButtonStyle1.fontSize = 25;
        GUIStyle mystyle = new GUIStyle(GUI.skin.button);

        mystyle.fontSize = 25;
        mystyle.alignment = TextAnchor.MiddleLeft;
        mystyle.fontStyle = FontStyle.Bold;
        mystyle.normal.textColor = Color.white;

        GUIStyle mystyle2 = new GUIStyle(GUI.skin.button);
        mystyle2.normal.textColor = Color.red;
        mystyle2.fontSize = 25;
        mystyle2.alignment = TextAnchor.MiddleLeft;
        mystyle2.fontStyle = FontStyle.Bold;

        Rect myRect1 = new Rect(10, 380, 270, 40);
        Rect myRect2 = new Rect(10, 420, 270, 40);
        Rect myRect3 = new Rect(10, 460, 270, 40);




        GUI.Box(myRect1, "Landmark Visibility:", mystyle); 
        GUI.Box(myRect2, "height: " + Rayorigin.ToString("0.0"), mystyle);
        GUI.Box(myRect3, "Visibility: "+ ComputeLandmark.ToString("0") + " /" + Building_numbers3 , mystyle);

        GUIStyle myButtonStyle2 = new GUIStyle(GUI.skin.button);
        myButtonStyle2.fontSize = 20;

        if (GUI.Button(new Rect(10, 340, 140, 40), "Ray on/off", mystyle2))
        {

            if (isClicked)
            {
                isClicked = false;
                X1 = Color.clear;
                X2 = Color.white;

            }
            else
            {
                isClicked = true;
                X1 = Color.clear;
                X2 = Color.clear;
            }
        }
    }


    private void Update()
    {

        Landmark_computation();
        ComputeLandmark = count;
        settingText();
        count = 0;

    }
    public void Landmark_computation()
    {

        respawns = GameObject.FindGameObjectsWithTag("Selectable");

        for (int i = 0; i < respawns.Length; i++)
        {

            if (respawns[i].GetComponent<Renderer>().material.color == Color.magenta)
            {
                count++;
            }

        }

        foreach (GameObject Selectable in respawns)
        {
            Selectable.GetComponent<Renderer>().material.color = Color.white;
        }


        if (Input.GetKey(KeyCode.Escape))
        {
            enabled = !enabled;
        }



        if (Input.GetKey(KeyCode.LeftControl))
        {
            Rayorigin += (Input.GetAxis("Mouse ScrollWheel") * 3f);

        }
        //float angle = 30f;

        //RaycastHit hit;
        for (float i = 0; i <= RaysNumber; i += 1)
        {
            //angle += 2 * Mathf.PI / RaysNumber;
            if (gameObject.name == "Landmark")
            {
                Origin = new Vector3(transform.position.x, transform.position.y + Rayorigin - 8.1f, transform.position.z);
            }
            else
            {
                Origin = new Vector3(transform.position.x, transform.position.y + Rayorigin, transform.position.z);
            }
            Vector3 dir = new Vector3(Mathf.Cos(Mathf.Deg2Rad * 2 * i), 0, Mathf.Sin(Mathf.Deg2Rad * 2 * i));

            Debug.DrawRay(Origin, dir * 45f, X1);

            if (Physics.Raycast(Origin, dir, out hit, 45f))
            {

                Ray ray = new Ray(Origin, dir);
                Debug.DrawLine(Origin, hit.point, X2);

                if (hit.collider.gameObject.GetComponent<Collider>().Raycast(ray, out hit, Mathf.Infinity))
                {
                    hit.collider.gameObject.GetComponent<Renderer>().material.color = Color.magenta;

                }

            }

        }
    }
}






