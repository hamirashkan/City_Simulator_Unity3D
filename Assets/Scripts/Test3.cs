using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test3 : MonoBehaviour
{
    public int RaysToShoot = 50;
    void FixedUpdate()

    {

        float angle = 30;
        for (int i = 0; i < RaysToShoot; i++)
        {
            float z = Mathf.Sin(angle);
            float x = Mathf.Cos(angle);
            angle += 2 * Mathf.PI / RaysToShoot;

            Vector3 dir = new Vector3(transform.position.x + x, 0 , transform.position.z + z);
            RaycastHit hit;
            Debug.DrawLine(transform.position, dir, Color.red);
            if (Physics.Raycast(transform.position, dir, out hit))
            {
                //here is how to do your cool stuff ;)
            }
        }


    }
}
