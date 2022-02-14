using UnityEngine;
using UnityEngine.UI;

public class Mahdis2 : MonoBehaviour
{
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

    //public Text t1, t2, t3, t4, t5, t6, t7, t8, t9, t10;

    public float Width;
    public float Length;
    public GameObject Park;
    public GameObject Sun;
    private int[,] matrix = new int[161, 221];
    int max = 0;
    [SerializeField] private Material defaultMaterial;

    // public float delayTime;
    // public float timeCount;




    public void histogram1()
    {

        float cnt1 = 0;

        for (int m = 0; m < Width * 10 + 1; m += 1)
        {

            for (int n = 0; n < Length * 10 + 1; n += 1)
            {

                if ((float)max > 0.0f && matrix[m, n] / (float)max <= 0.1f)
                {
                    cnt1 += 1;
                    Slider1.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(matrix[m, n] / (float)max, 0, 1 - matrix[m, n] / (float)max);
                }
            }
        }
        Slider1.value = cnt1;
        //t1.text = "#" + cnt1;
    }


    public void histogram2()
    {

        float cnt2 = 0;

        for (int m = 0; m < Width * 10 + 1; m += 1)
        {

            for (int n = 0; n < Length * 10 + 1; n += 1)
            {

                if (matrix[m, n] / (float)max > 0.1f && matrix[m, n] / (float)max <= 0.2f)
                {
                    cnt2 += 1;
                    Slider2.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(matrix[m, n] / (float)max, 0, 1 - matrix[m, n] / (float)max);
                }
            }
        }
        Slider2.value = cnt2;
       // t2.text = "#" + cnt2;
    }

    public void histogram3()
    {

        float cnt3 = 0;

        for (int m = 0; m < Width * 10 + 1; m += 1)
        {

            for (int n = 0; n < Length * 10 + 1; n += 1)
            {

                if (matrix[m, n] / (float)max > 0.2f && matrix[m, n] / (float)max <= 0.3f)
                {
                    cnt3 += 1;
                    Slider3.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(matrix[m, n] / (float)max, 0, 1 - matrix[m, n] / (float)max);
                }
            }
        }
        Slider3.value = cnt3;
       // t3.text = "#" + cnt3;
    }


    public void histogram4()
    {

        float cnt4 = 0;

        for (int m = 0; m < Width * 10 + 1; m += 1)
        {

            for (int n = 0; n < Length * 10 + 1; n += 1)
            {

                if (matrix[m, n] / (float)max > 0.3f && matrix[m, n] / (float)max <= 0.4f)
                {
                    cnt4 += 1;
                    Slider4.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(matrix[m, n] / (float)max, 0, 1 - matrix[m, n] / (float)max);
                }
            }
        }
        Slider4.value = cnt4;
       // t4.text = "#" + cnt4;
    }



    public void histogram5()
    {

        float cnt5 = 0;

        for (int m = 0; m < Width * 10 + 1; m += 1)
        {

            for (int n = 0; n < Length * 10 + 1; n += 1)
            {

                if (matrix[m, n] / (float)max > 0.4f && matrix[m, n] / (float)max <= 0.5f)
                {
                    cnt5 += 1;
                    Slider5.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(matrix[m, n] / (float)max, 0, 1 - matrix[m, n] / (float)max);
                }
            }
        }
        Slider5.value = cnt5;
        //t5.text = "#" + cnt5;
    }


    public void histogram6()
    {

        float cnt6 = 0;

        for (int m = 0; m < Width * 10 + 1; m += 1)
        {

            for (int n = 0; n < Length * 10 + 1; n += 1)
            {

                if (matrix[m, n] / (float)max > 0.5f && matrix[m, n] / (float)max <= 0.6f)
                {
                    cnt6 += 1;
                    Slider6.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(matrix[m, n] / (float)max, 0, 1 - matrix[m, n] / (float)max);
                }
            }
        }
        Slider6.value = cnt6;
        //t6.text = "#" + cnt6;
    }



    public void histogram7()
    {

        float cnt7 = 0;

        for (int m = 0; m < Width * 10 + 1; m += 1)
        {

            for (int n = 0; n < Length * 10 + 1; n += 1)
            {

                if (matrix[m, n] / (float)max > 0.6f && matrix[m, n] / (float)max <= 0.7f)
                {
                    cnt7 += 1;
                    Slider7.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(matrix[m, n] / (float)max, 0, 1 - matrix[m, n] / (float)max);
                }
            }
        }
        Slider7.value = cnt7;
       // t7.text = "#" + cnt7;
    }





    public void histogram8()
    {

        float cnt8 = 0;

        for (int m = 0; m < Width * 10 + 1; m += 1)
        {

            for (int n = 0; n < Length * 10 + 1; n += 1)
            {

                if (matrix[m, n] / (float)max > 0.7f && matrix[m, n] / (float)max <= 0.8f)
                {
                    cnt8 += 1;
                    Slider8.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(matrix[m, n] / (float)max, 0, 1 - matrix[m, n] / (float)max);
                }
            }
        }
        Slider8.value = cnt8;
        //t8.text = "#" + cnt8;
    }




    public void histogram9()
    {

        float cnt9 = 0;

        for (int m = 0; m < Width * 10 + 1; m += 1)
        {

            for (int n = 0; n < Length * 10 + 1; n += 1)
            {

                if (matrix[m, n] / (float)max > 0.8f && matrix[m, n] / (float)max <= 0.9f)
                {
                    cnt9 += 1;
                    Slider9.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(matrix[m, n] / (float)max, 0, 1 - matrix[m, n] / (float)max);
                }
            }
        }
        Slider9.value = cnt9;
       // t9.text = "#" + cnt9;
    }



    public void histogram10()
    {

        float cnt10 = 0;

        for (int m = 0; m < Width * 10 + 1; m += 1)
        {

            for (int n = 0; n < Length * 10 + 1; n += 1)
            {

                if (matrix[m, n] / (float)max > 0.9f && matrix[m, n] / (float)max <= 1.0f)
                {
                    cnt10 += 1;
                    Slider10.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(matrix[m, n] / (float)max, 0, 1 - matrix[m, n] / (float)max);
                }
            }
        }
        Slider10.value = cnt10;
        //t10.text = "#" + cnt10;
    }






    void Update()
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


        RaycastHit hit;

        for (int X = 0; X < Width * 10 + 1; X += 1)
        {

            for (int Z = 0; Z < Length * 10 + 1; Z += 1)
            {
                Vector3 Org = new Vector3((transform.position.x - Width / 2) + (0.1f * X), transform.position.y, (transform.position.z - Length / 2) + (0.1f * Z));
                Debug.DrawRay(Org, GameObject.Find("Sun").transform.position * 500, Color.clear);

                if (Physics.Raycast(Org, GameObject.Find("Sun").transform.position, out hit, 20f))
                {
                    Debug.DrawRay(Org, GameObject.Find("Sun").transform.position * 500 * hit.distance, Color.clear);
                    matrix[X, Z] += 1;
                }


            }

        }



        for (int i = 0; i < Width * 10 + 1; i += 1)
        {
            for (int j = 0; j < Length * 10 + 1; j += 1)
            {
                if (matrix[i, j] > max)
                {
                    max = matrix[i, j];
                    print(max);
                }
            }
        }

        //     print (max);
        //     if (timeCount < delayTime) 
        //     {
        //         timeCount += Time.deltaTime;
        //         return;
        //     }
        //     else
        //    {
        //       enabled = false;
        //     }


        Texture2D texture = new Texture2D(Mathf.RoundToInt(Width * 10 + 1), Mathf.RoundToInt(Length * 10 + 1));
        Park.GetComponent<MeshRenderer>().materials = new Material[0];
        Park.GetComponent<Renderer>().material.mainTexture = texture;
        Park.GetComponent<Renderer>().material.mainTexture.filterMode = FilterMode.Point;
        if (texture.height * texture.width <= ((Width * 10 + 1) * (Length * 10 + 1)))
            for (int w = 0; w < texture.width; w++)
            {
                for (int h = 0; h < texture.height; h++)
                {
                    //Color color = (matrix[w, h] != 0 ? Color.black : Color.green);
                    //texture.SetPixel(-w, -h, color);
                    Color color = new Color(matrix[w, h] / (float)max, 0, 1 - matrix[w, h] / (float)max);
                    texture.SetPixel(-w, -h, color);

                }
            }
        texture.Apply();
        ParkRestColor();

    }

    public void ParkRestColor()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            enabled = false;
            Park.GetComponent<Renderer>().material = defaultMaterial;
        }
    }


}