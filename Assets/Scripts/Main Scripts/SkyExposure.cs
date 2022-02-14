using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;

public class SkyExposure : MonoBehaviour
{
    public int Width;
    public int Length;
    private int[,] matrix = new int[25  , 45];
    int count;
    int countRay;
    float Exposure;
    public static float Exposure_text;
    public Text Skytext;
    private bool isClicked = false;
    public Color X1 { get; private set; }
    public Color X2 { get; private set; }

    void Start()
    {

        setText();
        X1 = Color.cyan;
        X2 = Color.red;

    }

    private void RayCast()
    {

        countRay = 0;
        RaycastHit hit;
        for (int i = 0; i < Width * 2 - 1; i += 1)
        {

            for (int j = 0; j < Length * 2 - 1; j += 1)
            {
                Vector3 Direction = new Vector3((- Width / 2) + (0.5f * i), 9 , (- Length / 2) + (0.5f * j));
                Debug.DrawRay(transform.position, Direction * 5, X1);
                countRay += 1;


                if (Physics.Raycast(transform.position, Direction, out hit, 25f))
                {
                    Debug.DrawRay(transform.position, hit.point, X2);
                    count += 1;
                    
                }
            }
        }
    }

    void OnGUI()
    {
        GUI.backgroundColor = Color.magenta;
        GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button);
        myButtonStyle.fontSize = 25;
        GUIStyle myButtonStyle1 = new GUIStyle(GUI.skin.button);
        myButtonStyle1.fontSize = 15;
        Rect myRect1 = new Rect(10, 350, 270, 40);
        GUI.Box(myRect1, "Sky exposure: " + SkyExposure.Exposure_text.ToString("0.0") + "%", myButtonStyle);
    }

    //     if (GUI.Button(new Rect(280, 350, 40, 40), "Ray", myButtonStyle1))
    //     {

    //         if (isClicked)
    //         {
    //             isClicked = false;
    //             X1 = Color.clear;
    //             X2 = Color.white;

    //         }
    //         else
    //         {
    //             isClicked = true;
    //             X1 = Color.cyan;
    //             X2 = Color.red;
    //         }
    //     }
    // }


    void Update  ()
    {

        count = 0;
        Exposure = 0;
        RayCast();
        Exposure = ((1125 - count) / 1125f) * 100;
        setText();
        Exposure_text = Exposure;
    }
    void setText()
    {
            Skytext.text = ("SkyExposure : " + Exposure.ToString("0.00") + "%");    
    }
 }
