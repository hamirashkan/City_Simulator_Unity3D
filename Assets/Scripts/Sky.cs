using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Sky : MonoBehaviour

{
    
    public int Width;
    public int Length;
    public float timer;
    public GameObject SkySpace;



    // add matrix definition;
    private int[,] matrix = new int[161, 221]; //0,1;integer, 0.00001
    //private int[,] matrix2 = new int[161, 221];

    void Update()
    {

        RaycastHit hit;
        for (int i = 0; i < Width  + 1; i += 1)
        {

            for (int j = 0; j < Length; j += 1)
            {
                Vector3 Origin = new Vector3((transform.position.x - Width / 2) + (2f * i), transform.position.y, (transform.position.z - Length / 2) + (2f * j));
                Debug.DrawRay(Origin, GameObject.Find("Capsule").transform.position * 50, Color.blue);

                if (Physics.Raycast(Origin, GameObject.Find("Capsule").transform.position, out hit, 25f))
                {
                    Debug.DrawRay(Origin, GameObject.Find("Capsule").transform.position * hit.distance, Color.red);
                    matrix[i, j] += 1;
                }
                //Debug.Log("Matrix = ....  " + matrix[i, j] + "i->"+i+"j->"+j); 
            }



        }



        Texture2D texture = new Texture2D(Mathf.RoundToInt(Width * 10), Mathf.RoundToInt(Length * 10));
        SkySpace.GetComponent<MeshRenderer>().materials = new Material[0];
        SkySpace.GetComponent<Renderer>().material.mainTexture = texture;
        //Park.GetComponent<Renderer>().material.mainTexture.filterMode = FilterMode.Point;

        if (texture.height * texture.width <= ((Width * 10) * (Length * 10)))
            for (int x = 0; x < texture.width; x++)
            {
                for (int y = 0; y < texture.height; y++)
                {

                    //SHadow variation
                    Color color = (matrix[x, y] != 0 ? Color.green : Color.black);
                    texture.SetPixel(-x, -y, color);

                    //heatmap for shadow variation
                    //Color color = new Color(1 - matrix[x, y] / 60f, 0, matrix[x, y] / 25f);
                    //texture.SetPixel(-x, -y, color);


                }
            }
        texture.Apply();



    }
}





