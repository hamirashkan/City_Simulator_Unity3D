using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public class Test7 : MonoBehaviour

{

    public float Width;
    public float Length;
    //public GameObject Park;
    public GameObject SkyCamera;
    private int[,] matrix = new int[41, 61];

    void Update()
    {
        RaycastHit hit;
        int max = 0;
        for (int X = 0; X < Width * 10 + 1; X += 1)
        {

            for (int Z = 0; Z < Length * 10 + 1; Z += 1)
            {
                Vector3 dir = new Vector3((GameObject.Find("Target").transform.position.x - Width / 2) + (0.1f * X), transform.position.y, (GameObject.Find("Target").transform.position.z - Length / 2) + (0.1f * Z));
                // Debug.DrawRay(Org, GameObject.Find("sun").transform.position * 500, Color.clear);
                Debug.DrawRay(transform.position, dir * 100, Color.cyan);
                if (Physics.Raycast(transform.position, dir, out hit, 20f))
                {
                    Debug.DrawRay(transform.position, dir * hit.distance, Color.red);
                    matrix[X, Z] += 1;
                }


            }

        }
    }
}
