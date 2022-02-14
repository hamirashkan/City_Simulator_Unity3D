using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AddingNewObject : MonoBehaviour
{


    public void AddBuilding()
    {
        if (Physics.Raycast(ray, out hit))
        {

            if (Input.GetKey(KeyCode.N))
            {
                Thread.Sleep(500);

                GameObject obj = Instantiate(Biulding, new Vector3(hit.point.x, hit.point.y, hit.point.z), Quaternion.identity) as GameObject;
                Landmark.Building_numbers += 1;
                obj.tag = "Selectable";
                obj.name = "Prefab";
                obj.transform.parent = GameObject.Find("PrefabBuilding").transform;
            }

        }
    }

    public void AddSkyExposure()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {

            if (Input.GetKey(KeyCode.E))
            {
                //SkyExposure.active = true;

                Thread.Sleep(500);
                GameObject obj = Instantiate(SkyExposure, new Vector3(hit.point.x, hit.point.y, hit.point.z), Quaternion.identity) as GameObject;
            }
        }
    }

    public void AddLightIntensity()
    {
        if (Physics.Raycast(ray, out hit))
        {

            if (Input.GetKey(KeyCode.I))
            {

                Destroy(Instantiate(Light_Intensity_Object));
                Thread.Sleep(500);
                GameObject obj = Instantiate(Light_Intensity_Object, new Vector3(hit.point.x, hit.point.y, hit.point.z), Quaternion.identity) as GameObject;
            }

        }
    }

    Ray ray;
    RaycastHit hit;
    public GameObject SkyExposure;
    public GameObject Biulding;
    public GameObject Light_Intensity_Object;

    [Obsolete]
    void Update()
    {
        AddBuilding();
        AddSkyExposure();
        AddLightIntensity();

    }

}
