using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Heatmaptest : MonoBehaviour

{

    private float SliderStartTimeValue = 0f; // current start value
    private float SliderStopTimeValue = 2000f; // current stop value

    public Sun sun;
    public int Width;
    public int Length;
    public float timer;
    public GameObject Park;
    public Text MaxMatrix;
    int largest;




    // add matrix definition;
    private int[,,] matrix = new int[19, 161, 221];
    private int[,,] CFH_matrix = new int[19, 161, 221];

    void setText()
    {
        MaxMatrix.text = (largest.ToString());

    }

    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            enabled = false;
        }



        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            enabled = false;
        }

        RaycastHit hit;
        for (int t = 0; t < Sun.Hour; t++)
            {

                for (int i = 0; i < Width * 10 + 1; i += 1)
                {

                    for (int j = 0; j < Length * 10 + 1; j += 1)
                    {

                        Vector3 Origin = new Vector3((transform.position.x - Width / 2) + (0.1f * i), transform.position.y, (transform.position.z - Length / 2) + (0.1f * j));
                        Debug.DrawRay(Origin, GameObject.Find("Sun").transform.position * 5, Color.gray);

                        if (Physics.Raycast(Origin, GameObject.Find("Sun").transform.position, out hit, 25f))
                        {
                            Debug.DrawRay(Origin, GameObject.Find("Sun").transform.position * hit.distance, Color.red);

                            matrix[t, i, j] += 1;

                            if (t > 0 && matrix[t, i, j] != matrix[t - 1, i, j])
                            {
                                CFH_matrix[t, i, j] += 1;
                            }
                        }

                    }
                }
            }
    
        



        Texture2D texture = new Texture2D(Mathf.RoundToInt(Width * 10), Mathf.RoundToInt(Length * 10));
        Park.GetComponent<MeshRenderer>().materials = new Material[0];
        Park.GetComponent<Renderer>().material.mainTexture = texture;
        //Park.GetComponent<Renderer>().material.mainTexture.filterMode = FilterMode.Point;

        if (texture.height * texture.width <= ((Width * 10) * (Length * 10)))
        {
            for(int T = 0; T < Sun.Hour ; T++)
            {

                for (int x = 0; x < texture.width; x++)
                {
                    for (int y = 0; y < texture.height; y++)
                    {
                        //SHadow variation
                        //Color color = (matrix[x, y] != 0 ? Color.gray : Color.green);
                        // texture.SetPixel(-x, -y, color);

                        //heatmap for shadow variation
                        Color color = new Color(CFH_matrix[T, x, y] / (float)largest, 0, 1 - CFH_matrix[T, x, y] / (float)largest);
                        texture.SetPixel(-x, -y, color);
                    }

                }
            }
        }
        texture.Apply();

        setText();
        largest = matrix.Cast<int>().Max();
        for (int time = 0; time < Sun.Hour; time++)
        {
                for (int i = 0; i < 161; i++)
                {
                    for (int j = 0; j < 221; j++)
                    {

                            largest = CFH_matrix[time ,i, j];
                    }

                }
        }




    }
    void start()
    {

        largest = 0;
        setText();
    }
}





