using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Histogram : MonoBehaviour

{

    private bool isClicked = false;
    public Sun sun;
    public int Width;
    public int Length;
    public GameObject Park;
    public Text MaxMatrix;
    public Text bar1,bar2, bar3, bar4, bar5
        , bar6, bar7, bar8, bar9, bar10, bar11;
    int largest;

    public GameObject mapImage; //UI reference

    public Slider Slider1;
    public Slider Slider2;
    public Slider Slider3;
    public Slider Slider4;
    public Slider Slider5;
    public Slider Slider6;
    public Slider Slider7;
    public Slider Slider8;
    public Slider Slider9;
    public Slider Slider10;
    public Slider Slider11;

    public Color X1 { get; private set; }
    public Color X2 { get; private set; }

    public bool updateOn = true;

    private int[,] shadow = new int[33, 45];

    private void Start()
    {

    }


    void setText()
    {
        MaxMatrix.text = (largest.ToString());
        bar1.text = (Slider1.value.ToString());
        bar2.text = (Slider2.value.ToString());
        bar3.text = (Slider3.value.ToString());
        bar4.text = (Slider4.value.ToString());
        bar5.text = (Slider5.value.ToString());
        bar6.text = (Slider6.value.ToString());
        bar7.text = (Slider7.value.ToString());
        bar8.text = (Slider8.value.ToString());
        bar9.text = (Slider9.value.ToString());
        bar10.text = (Slider10.value.ToString());
        bar11.text = (Slider11.value.ToString());
    }
    public void histogram1()
    {

        float cnt1 = 0;

        for (int m = 0; m < Width * 2 + 1; m += 1)
        {

            for (int n = 0; n < Length * 2 + 1; n += 1)
            {
                if (shadow[m, n] / (float)largest == 0)
                {
                    cnt1 += 1;
                    Slider1.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(shadow[m, n] / (float)largest, 0, 1 - shadow[m, n] / (float)largest);
                }
            }
        }
        Slider1.value = cnt1;

        //Slider1.maxValue = (float)largest;

    }
    public void histogram2()
    {

        float cnt2 = 0;

        for (int m = 0; m < Width * 2 + 1; m += 1)
        {

            for (int n = 0; n < Length * 2 + 1; n += 1)
            {
                if (shadow[m, n] / (float)largest > 0 && shadow[m, n] / (float)largest <= 0.1f)
                {
                    cnt2 += 1;
                    Slider2.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(shadow[m, n] / (float)largest, 0, 1 - shadow[m, n] / (float)largest);
                }
            }
        }
        Slider2.value = cnt2;

        //Slider1.maxValue = (float)largest;
    }
    public void histogram3()
    {

        float cnt3 = 0;

        for (int m = 0; m < Width * 2 + 1; m += 1)
        {

            for (int n = 0; n < Length * 2 + 1; n += 1)
            {
                if (shadow[m, n] / (float)largest > 0.1f && shadow[m, n] / (float)largest <= 0.2f)
                {
                    cnt3 += 1;
                    Slider3.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(shadow[m, n] / (float)largest, 0, 1 - shadow[m, n] / (float)largest);
                }
            }
        }
        Slider3.value = cnt3;

        //Slider1.maxValue = (float)largest;
    }
    public void histogram4()
    {

        float cnt4 = 0;

        for (int m = 0; m < Width * 2 + 1; m += 1)
        {

            for (int n = 0; n < Length * 2 + 1; n += 1)
            {
                if (shadow[m, n] / (float)largest > 0.2f && shadow[m, n] / (float)largest <= 0.3f)
                {
                    cnt4 += 1;
                    Slider4.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(shadow[m, n] / (float)largest, 0, 1 - shadow[m, n] / (float)largest);
                }
            }
        }
        Slider4.value = cnt4;

        //Slider1.maxValue = (float)largest;
    }
    public void histogram5()
    {

        float cnt5 = 0;

        for (int m = 0; m < Width * 2 + 1; m += 1)
        {

            for (int n = 0; n < Length * 2 + 1; n += 1)
            {
                if (shadow[m, n] / (float)largest > 0.3f && shadow[m, n] / (float)largest <= 0.4f)
                {
                    cnt5 += 1;
                    Slider5.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(shadow[m, n] / (float)largest, 0, 1 - shadow[m, n] / (float)largest);
                }
            }
        }
        Slider5.value = cnt5;

        //Slider1.maxValue = (float)largest;
    }
    public void histogram6()
    {

        float cnt6 = 0;

        for (int m = 0; m < Width * 2 + 1; m += 1)
        {

            for (int n = 0; n < Length * 2 + 1; n += 1)
            {
                if (shadow[m, n] / (float)largest > 0.4f && shadow[m, n] / (float)largest <= 0.5f)
                {
                    cnt6 += 1;
                    Slider6.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(shadow[m, n] / (float)largest, 0, 1 - shadow[m, n] / (float)largest);
                }
            }
        }
        Slider6.value = cnt6;

        //Slider1.maxValue = (float)largest;
    }
    public void histogram7()
    {

        float cnt7 = 0;

        for (int m = 0; m < Width * 2 + 1; m += 1)
        {

            for (int n = 0; n < Length * 2 + 1; n += 1)
            {
                if (shadow[m, n] / (float)largest > 0.5f && shadow[m, n] / (float)largest <= 0.6f)
                {
                    cnt7 += 1;
                    Slider7.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(shadow[m, n] / (float)largest, 0, 1 - shadow[m, n] / (float)largest);
                }
            }
        }
        Slider7.value = cnt7;

        //Slider1.maxValue = (float)largest;
    }
    public void histogram8()
    {
        float cnt8 = 0;

        for (int m = 0; m < Width * 2 + 1; m += 1)
        {

            for (int n = 0; n < Length * 2 + 1; n += 1)
            {

                if (shadow[m, n] / (float)largest > 0.6f && shadow[m, n] / (float)largest <= 0.7f)
                {
                    cnt8 += 1;
                    Slider8.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(shadow[m, n] / (float)largest, 0, 1 - shadow[m, n] / (float)largest);
                }
            }
        }
        Slider8.value = cnt8;
        //Slider2.maxValue = (float)largest;

    }
    public void histogram9()
    {
        float cnt9 = 0;
        for (int m = 0; m < Width * 2 + 1; m += 1)
        {

            for (int n = 0; n < Length * 2 + 1; n += 1)
            {

                if (shadow[m, n] / (float)largest > 0.7f && shadow[m, n] / (float)largest <= 0.8f)
                {
                    cnt9 += 1;
                    Slider9.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(shadow[m, n] / (float)largest, 0, 1 - shadow[m, n] / (float)largest);
                }
            }
        }
        Slider9.value = cnt9;
        //Slider3.maxValue = (float)largest;
    }
    public void histogram10()
    {
        float cnt10 = 0;
        for (int m = 0; m < Width * 2 + 1; m += 1)
        {

            for (int n = 0; n < Length * 2 + 1; n += 1)
            {

                if (shadow[m, n] / (float)largest > 0.8f && shadow[m, n] / (float)largest <= 0.9f)
                {
                    cnt10 += 1;
                    Slider10.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(shadow[m, n] / (float)largest, 0, 1 - shadow[m, n] / (float)largest);
                }
            }
        }
        Slider10.value = cnt10;
        //Slider4.maxValue = (float)largest;
    }
    public void histogram11()
    {
        float cnt11 = 0;
        for (int m = 0; m < Width * 2 + 1; m += 1)
        {

            for (int n = 0; n < Length * 2 + 1; n += 1)
            {

                if (shadow[m, n] / (float)largest > 0.9f && shadow[m, n] / (float)largest <= 1.0f)
                {
                    cnt11 += 1;
                    Slider11.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(shadow[m, n] / (float)largest, 0, 1 - shadow[m, n] / (float)largest);
                }
            }
        }
        Slider11.value = cnt11;
        //Slider5.maxValue = (float)largest;
    }


    public void Raycasting()
    {
        RaycastHit hit;
        for (int i = 0; i < Width * 2 + 1; i += 1)
        {

            for (int j = 0; j < Length * 2 + 1; j += 1)
            {

                Vector3 Origin = new Vector3((transform.position.x - Width / 2) + (0.5f * i), transform.position.y, (transform.position.z - Length / 2) + (0.5f * j));
                Debug.DrawRay(Origin, GameObject.Find("Sun").transform.position, X1);

                if (Physics.Raycast(Origin, GameObject.Find("Sun").transform.position, out hit, Mathf.Infinity))
                {
                    Debug.DrawRay(Origin, GameObject.Find("Sun").transform.position * hit.distance, X2);

                    shadow[i, j] += 1;

                }
            }
        }
    }
    public void ResetAll()
    {
        for (int i = 0; i < Width * 2 + 1; i += 1)
        {
            for (int j = 0; j < Length * 2 + 1; j += 1)
            {
                shadow[i, j] = 0;
            }
        }

        Slider1.value = Slider2.value  = Slider3.value = Slider4.value = Slider5.value = Slider6.value
            = Slider7.value = Slider8.value = Slider9.value = Slider10.value = Slider11.value=0;
        largest = 0;
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
                    Color color = new Color(1 - shadow[x, y] / (float)largest, shadow[x, y] / (float)largest,0 );
                    texture.SetPixel(-x - 1, -y - 1, color);

                }
            }
        mapImage.GetComponent<RawImage>().texture = texture;
        texture.Apply();
    }

    public void ResetHistogram()
    {
        updateOn = false;
        ResetAll();
    }
    void OnGUI()
    {
        GUI.backgroundColor = Color.black;
        GUIStyle mystyle2 = new GUIStyle(GUI.skin.button);
        mystyle2.fontSize = 20;
        mystyle2.alignment = TextAnchor.MiddleCenter;
        mystyle2.fontStyle = FontStyle.Bold;
        mystyle2.normal.textColor = Color.white;

        if (GUI.Button(new Rect(211, 60, 80, 40), "Reset", mystyle2))
        {
            ResetHistogram();
        }
        if (GUI.Button(new Rect(292, 60, 50, 40), "Ray", mystyle2))
        {
            if (isClicked)
            {
                isClicked = false;
                X1 = Color.clear;
                X2 = Color.magenta;

            }
            else
            {
                isClicked = true;
                X1 = Color.clear;
                X2 = Color.magenta;
            }
        }
    }


    private void Update()
    {
        if(Sun.hour == 18)
        {
            enabled = false;
        }
        setText();
        Raycasting();
        Texture2D();

        if (Sun.hour == 18 || Sun.hour == 6)
        {
            ResetAll();
        }


        updateOn = true;


        if (updateOn == true)
        {
            histogram1();
            histogram2();
            histogram3();
            histogram4();
            histogram5();
            histogram6();
            histogram7();
            histogram8();
            histogram9();
            histogram10();
            histogram11();

            largest = shadow.Cast<int>().Max();

            for (int i = 0; i < 33; i++)
            {
                for (int j = 0; j < 45; j++)
                {

                    largest = shadow[i, j];
                }
            }

        }
        
    }
}





