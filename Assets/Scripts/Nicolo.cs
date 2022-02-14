using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class Nicolo : MonoBehaviour
{


    public Sun sun;
    public int Width;
    public int Length;
    public float timer;
    public GameObject Park;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.C))
        {
            Object[] allObjects = Resources.FindObjectsOfTypeAll(typeof(GameObject));
            List<GameObject> buildings = new List<GameObject>();

            foreach (Object o in allObjects)
            {
                if (o.name.StartsWith("building"))
                {
                    buildings.Add((GameObject)o);
                }
            }

            GameObject park = GameObject.Find("park");
            GameObject sun = GameObject.Find("Sun");

            Vector3 maxBound = park.GetComponent<Renderer>().bounds.max;
            Vector3 minBound = park.GetComponent<Renderer>().bounds.min;

            var sequence = "10"; // sequence for CFH

            var shadowPoints = new Dictionary<Vector3, int>();
            var cfhPoints = new Dictionary<Vector3, string>();
            var cumulativeCFH = new Dictionary<Vector3, int>();


            Vector3 directionSun = sun.transform.forward;
            int alphaMax = 360;

            for (int alpha = 0; alpha < alphaMax; alpha++)
            {
                // Move sun
                sun.transform.localEulerAngles = new Vector3(sun.transform.localEulerAngles.x, alpha, sun.transform.localEulerAngles.z);

                Vector3 directionMovingSun = sun.transform.forward;

                for (int i = (int)minBound.x; i < (int)maxBound.x; ++i)
                {
                    for (int j = (int)minBound.z; j < (int)maxBound.z; ++j)
                    {
                        Vector3 currentOrigin = new Vector3(i, minBound.y, j);
                        directionMovingSun.Normalize();

                        if (Physics.Raycast(currentOrigin, -directionMovingSun, 10000)) // shadow
                        {
                            if (shadowPoints.ContainsKey(currentOrigin))
                            {
                                shadowPoints[currentOrigin] += 1;
                            }
                            else
                            {
                                shadowPoints[currentOrigin] = 1;
                            }

                            if (cfhPoints.ContainsKey(currentOrigin))
                            {
                                cfhPoints[currentOrigin] = System.String.Concat(cfhPoints[currentOrigin], "1");
                            }
                            else
                            {
                                cfhPoints[currentOrigin] = "1";
                            }
                        }
                        else // no shadow
                        {
                            if (cfhPoints.ContainsKey(currentOrigin))
                            {
                                cfhPoints[currentOrigin] = System.String.Concat(cfhPoints[currentOrigin], "0");
                            }
                            else
                            {
                                cfhPoints[currentOrigin] = "0";
                            }
                        }



                    }
                }
            }

            foreach (KeyValuePair<Vector3, string> entry in cfhPoints)
            {
                cumulativeCFH[entry.Key] = Regex.Matches(entry.Value, sequence).Count;
            }


            var rows = (int)maxBound.x - (int)minBound.x;
            var columns = (int)maxBound.z - (int)minBound.z;

            int[,] matrixShadowMap = new int[rows, columns];
            int[,] matrixCFH = new int[rows, columns]; // the time is determined by the steps of the sun movement

            foreach (KeyValuePair<Vector3, int> entry in shadowPoints)
            {
                matrixShadowMap[(int)entry.Key.x - (int)minBound.x, (int)entry.Key.z - (int)minBound.z] = entry.Value;
            }

            foreach (KeyValuePair<Vector3, int> entry in cumulativeCFH)
            {
                matrixCFH[(int)entry.Key.x - (int)minBound.x, (int)entry.Key.z - (int)minBound.z] = entry.Value;
            }



            Texture2D texture = new Texture2D(Mathf.RoundToInt(Width * 10), Mathf.RoundToInt(Length * 10));
            Park.GetComponent<MeshRenderer>().materials = new Material[0];
            Park.GetComponent<Renderer>().material.mainTexture = texture;
            //Park.GetComponent<Renderer>().material.mainTexture.filterMode = FilterMode.Point;

            if (texture.height * texture.width <= ((16 * 10) * (22 * 10)))
                for (int x = 0; x < texture.width; x++)
                {
                    for (int y = 0; y < texture.height; y++)
                    {

                        //Shadow variation
                        //Color color = (matrix2[x, y] != 0 ? Color.blue : Color.red);
                        //texture.SetPixel(-x, -y, color);

                        //heatmap for shadow variation
                        Color color = new Color(matrixCFH[x, y] / 25f, 0, 1 - matrixCFH[x, y] / 25f);
                        texture.SetPixel(-x, -y, color);


                    }
                }
            texture.Apply();

            //generateGraph(matrixShadowMap, "ShadowMap.html", rows, columns);
            //generateGraph(matrixCFH, "CFH.html", rows, columns);
        }
    }

    /*void generateGraph(int[,] matrix, string file_name, int rows, int columns)
    {


        using (TextWriter tw = new StreamWriter(file_name))
        {
            tw.Write("<head>   <script src = 'https://cdn.plot.ly/plotly-latest.min.js' ></script></head><body><div id = 'myDiv' ></div></body><script> var data = [{z: [");
            for (int j = 0; j < rows; j++)
            {

                tw.Write("[");


                for (int i = 0; i < columns; i++)
                {
                    tw.Write(matrix[j, i]);
                    if (i != columns - 1)
                    {
                        tw.Write(", ");
                    }
                }
                if (j == rows - 1)
                {
                    tw.Write("]");
                }
                else
                {
                    tw.Write("], ");
                }

            }
            tw.Write("], type: 'heatmap'}]; Plotly.newPlot('myDiv', data);</script>");
        }

        Debug.Log("The file has been generated");

    }

    bool compatible(string observed, string sequence)
    {
        for (int i = 0; i < observed.Length; ++i)
        {
            if (observed[i] != sequence[i])
            {
                return false;
            }
        }
        return true;
    }*/
}
