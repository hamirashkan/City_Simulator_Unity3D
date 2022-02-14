using UnityEngine;
using UnityEngine.UI;

public class CFH : MonoBehaviour

{


    public Sun sun;
    public int Width;
    public int Length;
    public GameObject Park;
    public Text MaxMatrix;
    private int largest;
    private bool isClicked = false;

    public GameObject mapImage; //UI reference
    public Color X1 { get; private set; }
    public Color X2 { get; private set; }

    // add matrix definition;
    private int[,,] shadow = new int[33, 45, 19];
    private int[,] heatmap = new int[33 , 45];
    private int[,] CFH_matrix = new int[35, 47];

    private void Matrix()
    {
        RaycastHit hit;

        for (int i = 0; i < Width * 2 + 1; i += 1)
        {

            for (int j = 0; j < Length * 2 + 1; j += 1)
            {

                Vector3 Origin = new Vector3((transform.position.x - Width / 2) + (0.5f * i), transform.position.y, (transform.position.z - Length / 2) + (0.5f * j));
                Vector3 Dir = GameObject.Find("Sun").transform.position;
                Debug.DrawRay(Origin, GameObject.Find("Sun").transform.position, Color.clear);

                if (Physics.Raycast(Origin, Dir, out hit, Mathf.Infinity))
                {
                    Debug.DrawRay(Origin, Dir * hit.distance, X1);

                    shadow[i, j, Sun.Hour] = 1;
                }
                else if (!Physics.Raycast(Origin, Dir, out hit, 25f))
                {
                    Debug.DrawRay(Origin, Dir * 500, X2);
                    shadow[i, j, Sun.Hour] = 0;
                }
                //heatmap[i, j] += shadow[i, j, Sun.Hour];

                if (shadow[i, j, Sun.Hour] != shadow[i, j, Sun.Hour - 1])
                {

                    CFH_matrix[i, j] += 1;
                }
            }
        }

        for (int m = 0; m < Width * 2 + 1; m += 1)
        {
            for (int n = 0; n < Length * 2 + 1; n += 1)
            {
                if (CFH_matrix[m, n] > largest)
                {
                    largest = CFH_matrix[m,n];
                    print(largest);
                }
            }
        }



    }

    void OnGUI()
    {
        GUI.backgroundColor = Color.black;
        GUIStyle mystyle2 = new GUIStyle(GUI.skin.button);
        mystyle2.fontSize = 20;
        mystyle2.alignment = TextAnchor.MiddleCenter;
        mystyle2.fontStyle = FontStyle.Bold;
        mystyle2.normal.textColor = Color.white;

        if (GUI.Button(new Rect(211, 100, 80, 40), "Reset", mystyle2))
        {
            ResetAll();
        }
        if (GUI.Button(new Rect(292, 100, 50, 40), "Ray", mystyle2))
        {
            if (isClicked)
            {
                isClicked = false;
                X1 = Color.clear;
                X2 = Color.clear;

            }
            else
            {
                isClicked = true;
                X1 = Color.magenta;
                X2 = Color.clear;
            }
        }
    }

    public void Texture()
    {
        Texture2D texture = new Texture2D(Mathf.RoundToInt(Width * 2 + 1), Mathf.RoundToInt(Length * 2 + 1));
        Park.GetComponent<MeshRenderer>().materials = new Material[0];
        Park.GetComponent<Renderer>().material.mainTexture = texture;
        Park.GetComponent<Renderer>().material.mainTexture.filterMode = FilterMode.Point;

        if (texture.height * texture.width <= ((Width * 2 + 1) * (Length * 2 + 1)))
            for (int x = 0; x < texture.width; x++)
            {
                for (int y = 0; y < texture.height; y++)
                {
                    //SHadow variation
                    //Color color = (matrix[x, y] != 0 ? Color.gray : Color.green);
                    // texture.SetPixel(-x, -y, color);

                    //heatmap for shadow variation
                    Color color = new Color(CFH_matrix[x, y] / (float)largest, 0, 1 - CFH_matrix[x, y] / (float)largest);
                    texture.SetPixel(-x - 1, -y - 1, color);

                }
            }
        mapImage.GetComponent<RawImage>().texture = texture;
        texture.Apply();
    }
    public void ResetAll()
    {
        largest = 0;
        for (int i = 0; i < Width * 2 + 1; i += 1)
        {
            for (int j = 0; j < Length * 2 + 1; j += 1)
            {
                CFH_matrix[i, j] = 0;
            }
        }
    }


    void Update()
    {

        
        MaxMatrix.text = (largest.ToString());
        setText();
        Matrix();
        Texture();
        if (Sun.hour == 18 || Sun.hour == 6)
        {
            ResetAll();
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            enabled = false;
        }


    }
    void setText()
    {
        MaxMatrix.text = (largest.ToString("0"));
    }

}





