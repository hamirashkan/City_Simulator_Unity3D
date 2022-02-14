using UnityEngine;
using System.Collections;

public class Test5 : MonoBehaviour
{


    public Sun sun;

    private MeshRenderer mesh;
    private RaycastHit hit;

    private bool underSun = false;

    // Use this for initialization
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        underSun = false;
        Vector3 sunDir = sun.transform.forward;
        sunDir.Normalize();
        sunDir *= 100;

        foreach (Transform child in transform)
        {

            if (!Physics.Raycast(child.position, -1f * - sunDir, 30, LayerMask.GetMask("Wall")))
            {

                Debug.DrawLine(child.position, child.position - sunDir, Color.red);
                underSun = true;

            }
            else
            {
                Debug.DrawLine(child.position, child.position - sunDir, Color.green);
            }

        }



        if (underSun)
        {
            mesh.material.color = Color.red;
        }
        else
        {
            mesh.material.color = Color.green;
        }


    }
}