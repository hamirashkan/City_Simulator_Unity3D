using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System;

public class HeatMap : MonoBehaviour
{

    //private float SliderStartTimeValue = 0f; // current start value
    //private float SliderStopTimeValue = 2000f; // current stop value
    private bool isClicked = false;
    private float nextUpdate = 1f;
    public Sun sun;
    public int Width;
    public int Length;
    public float timer;
    public GameObject Park;
    public Text MaxMatrix;
    int largest;
    public bool updateOn = true;
    private bool toggleTxt = true;
    public Color X1 { get; private set; }
    public Color X2 { get; private set; }



    // add matrix definition;
    private int[,,] matrix = new int[33, 45, 19];
    private int[,] shadow = new int[33, 45];

    void setText()
    {
        MaxMatrix.text = (largest.ToString());

    }

    public void ResetAll()
    {
        updateOn = !updateOn;
        largest = 0;
        for (int i = 0; i < Width * 2 + 1; i += 1)
        {
            for (int j = 0; j < Length * 2 + 1; j += 1)
            {
                shadow[i, j] = 0;
            }
        }
    }

    void OnGUI()
    {
        GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button);
        myButtonStyle.fontSize = 15;
        if (GUI.Button(new Rect(180, 100, 70, 40), "Reset", myButtonStyle))
        {
            ResetAll();
        }
        if (GUI.Button(new Rect(250, 100, 40, 40), "Ray", myButtonStyle))
        {
            if (isClicked)
            {
                isClicked = false;
                X1 = Color.blue;
                X2 = Color.red;

            }
            else
            {
                isClicked = true;
                X1 = Color.blue;
                X2 = Color.red;
            }
        }

    }
    public void heatmap_matrix()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            enabled = false;
        }


        /*timer -= Time.deltaTime;
        if (timer <= 0)
        {
            enabled = false;
        }*/

        RaycastHit hit;

        for (int i = 0; i < Width * 2 + 1; i += 1)
        {

            for (int j = 0; j < Length * 2 + 1; j += 1)
            {

                Vector3 Origin = new Vector3((transform.position.x - Width / 2) + (0.5f * i), transform.position.y, (transform.position.z - Length / 2) + (0.5f * j));
                Debug.DrawRay(Origin, GameObject.Find("Sun").transform.position * 5, X1);

                if (Physics.Raycast(Origin, GameObject.Find("Sun").transform.position, out hit, 25f))
                {
                    Debug.DrawRay(Origin, GameObject.Find("Sun").transform.position * hit.distance, X2);
                    for (int t = 0; t < 19; t++)
                    {
                        if (Sun.Delta_time > 0)
                        {
                            matrix[i, j, t] += 1;
                        }
                        else if (Sun.Delta_time < 0)
                        {
                            matrix[i, j, t] -= 1;
                        }
                        else if (Sun.Delta_time == 0)
                        {
                            matrix[i, j, t] = 1;
                        }

                        shadow[i, j] = matrix[i, j, t];
                    }


                }
            }
        }
        largest = shadow.Cast<int>().Max();

        for (int i = 0; i < 33; i++)
        {
            for (int j = 0; j < 45; j++)
            {
                for (int time = 0; time < 19; time++)
                {
                    largest = shadow[i, j];

                }
            }

        }
    }
    public void Texture2D()
    {


        Texture2D texture = new Texture2D(Mathf.RoundToInt(Width * 2 + 1), Mathf.RoundToInt(Length * 2 + 1));
        Park.GetComponent<MeshRenderer>().materials = new Material[0];
        Park.GetComponent<Renderer>().material.mainTexture = texture;
        Park.GetComponent<Renderer>().material.mainTexture.filterMode = FilterMode.Trilinear;

        if (texture.height * texture.width <= ((Width * 2 + 1) * (Length * 2 + 1)))
            for (int x = 0; x < texture.width; x++)
            {
                for (int y = 0; y < texture.height; y++)
                {
                    //SHadow variation
                    //Color color = (matrix[x, y] != 0 ? Color.gray : Color.green);
                    // texture.SetPixel(-x, -y, color);

                    //heatmap for shadow variation
                    Color color = new Color(0, shadow[x, y] / (float)largest, 1 - shadow[x, y] / (float)largest);
                    texture.SetPixel(-x - 1, -y - 1, color);
                }
            }
        texture.Apply();

        setText();
        
    }

    void UpdateEverySecond()
    {

        if (updateOn == true)
        {
            Texture2D();
            heatmap_matrix();
        }

    }


    void Update()
    {
        if (Time.time > nextUpdate)
        {

                //Debug.Log(Time.time + ">=" + nextUpdate);
                // Change the next update (current second+1)
               nextUpdate = Mathf.FloorToInt(Time.time) * 1;
                // Call your fonction
                UpdateEverySecond();
        }

    }
}





