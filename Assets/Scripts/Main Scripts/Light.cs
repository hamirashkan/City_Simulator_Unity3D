using UnityEngine;
using UnityEngine.UI;

//using System.Collections.Generic;

public class Light : MonoBehaviour
{
    private float angle;
    public Text SunAngle;
    public Text Percentage;
    private float LightIntensity;
    //public Slider Sliderbar;
    internal Color color;
    float Percent;
    float Lux;
    internal float intensity;

    void setText_sunangle()
    {
        SunAngle.text = ("Sun angle: " + angle.ToString("0.0")+ "°");
        Percentage.text = (Percent.ToString("0.0") + "%");
        if ((Input.GetKey(KeyCode.Tab)))
        {
           Percentage.text = (Lux.ToString("0" + " lux"));
        }
    }
    
    public void Light_intensity()
    {
        Percent = (LightIntensity / 90.000f) * 100;
        Lux = (111000 * LightIntensity) / 90;
        //Sliderbar.value = LightIntensity;

        RaycastHit hit;
        Vector3 Origin = this.transform.position;
        Vector3 dir = (GameObject.Find("Sun").transform.position - Origin).normalized;

        Debug.DrawRay(Origin, dir * 5000, Color.clear, Mathf.Infinity);
        angle = Mathf.Abs(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);



        if (angle > 90)
        {
            angle = 180 - angle;
        }


        LightIntensity = angle;
        if (Physics.Raycast(Origin, dir, out hit, 10000f) || Sun.Sun_Pos == false)
        {
            Debug.DrawRay(Origin, dir * hit.distance, Color.clear);
            LightIntensity = 0.0001f;
        }


        this.transform.localScale = new Vector3(this.transform.localScale.x, LightIntensity / 50, this.transform.localScale.z);
        Percentage.color = new Color(LightIntensity / 90f, 0, 1 - LightIntensity / 90f); ;
        //Percentage.fontSize = (int)(LightIntensity) / 1000;
        //GameObject.Find("Cylinder").GetComponent<MeshRenderer>().material.color = new Color(LightIntensity / 90f , 0f , 1 - LightIntensity / 90f);
    }


    void Update()
    {
        setText_sunangle();
        Light_intensity();
    }
}


/*        public void SkyObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            if (Input.GetMouseButtonDown(1))
            {
                currentPlaceableObject = Instantiate(currentPlaceableObject);
                currentPlaceableObject.name = "SkyBarClone";
                currentPlaceableObject.transform.position = hitInfo.point;
                //currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
                currentPlaceableObject.transform.localScale = new Vector3(currentPlaceableObject.transform.localScale.x, Exposure * 0.2f, currentPlaceableObject.transform.localScale.z);
                //currentPlaceableObject.GetComponentsInChildren<Renderer>().material.color = new Color(1-Exposure / 100f, Exposure / 100f,0 );
                Percentage.color = new Color(1 - Exposure / 100f, Exposure / 100f, 0);

                foreach (Renderer r in currentPlaceableObject.GetComponentsInChildren<Renderer>())
                {
                    r.material.color = new Color(1 - Exposure / 100f, Exposure / 100f, 0);

                }

            }
        }

    }*/
