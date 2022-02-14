using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class Roundingcast : MonoBehaviour
    
{
    public float Rayorigin;
    public float ScanSpeed;
    public Color X { get; private set; }



    void Update()
    {
        Landmark();
    }

    private void Landmark()
    {

        if (Input.GetKey(KeyCode.LeftControl))
        {
            Rayorigin += (Input.GetAxis("Mouse ScrollWheel"));

        }

        RaycastHit hit;


        Quaternion q = Quaternion.AngleAxis(ScanSpeed * Time.time, Vector3.up);
        Vector3 d = transform.forward * 50;

        float y = Rayorigin - 5;



        Vector3 height = new Vector3(0, y, 0);
        if (Rayorigin >= 0 && Rayorigin < 5)
        {
            X = Color.red;
        }
        else if (Rayorigin >= 5 && Rayorigin < 6)
        {
            X = Color.green;
        }
        else if (Rayorigin >= 6 && Rayorigin < 7)
        {
            X = Color.magenta;
        }
        else if (Rayorigin >= 7 && Rayorigin < 8)
        {
            X = Color.yellow;
        }
        else if (Rayorigin >= 8 && Rayorigin < 9)
        {
            X = Color.blue;
        }
        else if (Rayorigin >= 9)
        {
            X = Color.cyan;
        }


        Debug.DrawRay(transform.position + height, q * d, X);


        if (Physics.Raycast(transform.position + height, q * d, out hit))
        {

            hit.collider.gameObject.GetComponent<Renderer>().material.color = X;
            print("Landmark Visibility from point " + Rayorigin);
        }


    }

}
    