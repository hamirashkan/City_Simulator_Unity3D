using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class ShadowVariation : MonoBehaviour

{
    private bool isClicked = false;
    public Sun sun;
    public int Width;
    public int Length;
    public GameObject Park;
    public Color X1 { get; private set; }
    public Color X2 { get; private set; }
    public GameObject mapImage; //UI reference


    // add matrix definition;
    private int[,] shadow = new int[161, 221];

    void OnGUI()
    {
        GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button);
        myButtonStyle.fontSize = 15;

        GUI.backgroundColor = Color.black;
        GUIStyle mystyle2 = new GUIStyle(GUI.skin.button);
        mystyle2.fontSize = 20;
        mystyle2.alignment = TextAnchor.MiddleCenter;
        mystyle2.fontStyle = FontStyle.Bold;
        mystyle2.normal.textColor = Color.white;
        if (GUI.Button(new Rect(211, 140, 50, 40), "Ray", mystyle2))
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
                X1 = Color.clear;
                X2 = Color.gray;
            }
        }
    }




    public void Shadow_Variation()
    {

        RaycastHit hit;
        for (int i = 0; i < Width * 5 + 1; i += 1)
        {
            for (int j = 0; j < Length * 5 + 1; j += 1)
            {
                Vector3 Origin = new Vector3((transform.position.x - Width / 2) + (0.2f * i), transform.position.y, (transform.position.z - Length / 2) + (0.2f * j));
                Debug.DrawRay(Origin, GameObject.Find("Sun").transform.position * 5, X1);

                if (Physics.Raycast(Origin, GameObject.Find("Sun").transform.position, out hit, 25f))
                {
                    Debug.DrawRay(Origin, GameObject.Find("Sun").transform.position * hit.distance, X2);
                    shadow[i, j] = 1;
                }
                else
                {
                    shadow[i, j] = 0;
                }

            }
        }


        Texture2D texture = new Texture2D(Mathf.RoundToInt(Width * 5 + 1), Mathf.RoundToInt(Length * 5 + 1));
        Park.GetComponent<MeshRenderer>().materials = new Material[0];
        Park.GetComponent<Renderer>().material.mainTexture = texture;
        Park.GetComponent<Renderer>().material.mainTexture.filterMode = FilterMode.Trilinear;

        if (texture.height * texture.width <= ((Width * 5+1) * (Length * 5+1)))
            for (int x = 0; x < texture.width; x++)
            {
                for (int y = 0; y < texture.height; y++)
                {

                    //SHadow variation
                    Color color = (shadow[x, y] != 0 ? Color.black : Color.green);
                    texture.SetPixel(-x-1, -y-1, color);

                    //heatmap for shadow variation
                    //Color color = new Color(matrix[x, y] / 25f, 0, 1 - matrix[x, y] / 25f);
                    //texture.SetPixel(-x, -y, color);
                }
            }
        mapImage.GetComponent<RawImage>().texture = texture;
        texture.Apply();
    }

    void Update()
    {
        Shadow_Variation();
    }

}





