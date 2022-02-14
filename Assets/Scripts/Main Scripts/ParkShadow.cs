using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkShadow : MonoBehaviour

{
    public Sun sun;
    // See Order of Execution for Event Functions for information on FixedUpdate() and Update() related to physics queries
    void FixedUpdate()
    {
        RaycastHit hit;
        
        Vector3 dir1 = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 dir1_1 = new Vector3(transform.position.x, transform.position.y, transform.position.z+0.5f);
        Vector3 dir2 = new Vector3(transform.position.x , transform.position.y, transform.position.z+1f);
        Vector3 dir2_2 = new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z+0.5f);
        Vector3 dir3 = new Vector3(transform.position.x+0.5f, transform.position.y, transform.position.z + 1f);
        Vector3 dir3_3 = new Vector3(transform.position.x+0.5f, transform.position.y, transform.position.z);
        Vector3 dir4 = new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z + 0.5f);
        Vector3 dir4_4 = new Vector3(transform.position.x +1f, transform.position.y, transform.position.z +1f);
        Vector3 dir5 = new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z);




        if (Physics.Raycast(dir1, GameObject.Find("Sun").transform.position, out hit, 25f))
        {
            Debug.DrawRay(dir1, GameObject.Find("Sun").transform.position * hit.distance, Color.white);
        }
        if (Physics.Raycast(dir1_1, GameObject.Find("Sun").transform.position, out hit, 25f))
        {
            Debug.DrawRay(dir1_1, GameObject.Find("Sun").transform.position * hit.distance, Color.white);
        }


        if (Physics.Raycast(dir2, GameObject.Find("Sun").transform.position, out hit, 25f))
        {

            Debug.DrawRay(dir2, GameObject.Find("Sun").transform.position * hit.distance, Color.white);
        }
        if (Physics.Raycast(dir2_2, GameObject.Find("Sun").transform.position, out hit, 25f))
        {

            Debug.DrawRay(dir2_2, GameObject.Find("Sun").transform.position * hit.distance, Color.white);
        }



        if (Physics.Raycast(dir3, GameObject.Find("Sun").transform.position, out hit, 25f))
        {


            Debug.DrawRay(dir3, GameObject.Find("Sun").transform.position * hit.distance, Color.white);
        }
        if (Physics.Raycast(dir3_3, GameObject.Find("Sun").transform.position, out hit, 25f))
        {


            Debug.DrawRay(dir3_3, GameObject.Find("Sun").transform.position * hit.distance, Color.white);
        }
       


        if (Physics.Raycast(dir4, GameObject.Find("Sun").transform.position, out hit, 25f))
        {

            Debug.DrawRay(dir4, GameObject.Find("Sun").transform.position * hit.distance, Color.white);
        }

        if (Physics.Raycast(dir4_4, GameObject.Find("Sun").transform.position, out hit, 25f))
        {

            Debug.DrawRay(dir4_4, GameObject.Find("Sun").transform.position * hit.distance, Color.white);
        }

        if (Physics.Raycast(dir5, GameObject.Find("Sun").transform.position, out hit, 25f))
        {

            Debug.DrawRay(dir5, GameObject.Find("Sun").transform.position * hit.distance, Color.white);
        }




        else

            Debug.DrawRay(transform.position, GameObject.Find("Sun").transform.position * 1000, Color.clear);
        {

        }
        
    }
}
                  
               
            
        
    

