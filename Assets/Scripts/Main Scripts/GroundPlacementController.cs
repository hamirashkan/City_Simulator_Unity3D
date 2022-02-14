using UnityEngine;
using System.Threading;

public class GroundPlacementController : MonoBehaviour
{

    private bool pressed;
    private bool hovered;
    [SerializeField]
    private GameObject[] placeableObjectPrefabs;
    private GameObject currentPlaceableObject;
    public GameObject Park;
    public GameObject Camera_sky;
    public GameObject LightObject;
    Vector3 temp;
    public Texture2D GUI_Texture;
    public Texture2D GUI_Texture2;

    private float mouseWheelRotation;
    private int currentPrefabIndex = -1;
    public static int Building_numbers2 = 0;
    public static int Building_numbers4 = 0;

    private GameObject[] respawns;
    private GameObject[] ObjCollider;
    private bool isClicked = false;
    public static bool Landmark_click = false;

    [SerializeField] private Material defaultMaterial;


    private void Update()
    {
        DeletingItem();
        Scaling();
        Landmark();


        if (currentPlaceableObject != null)
        {
            MoveCurrentObjectToMouse();
            RotateFromMouseWheel();
            ReleaseIfClicked();
        }
    }
    public void disactiveObjects()
    {
        
        GameObject[] killEmAll;
        killEmAll = GameObject.FindGameObjectsWithTag("CapsuleClone");
        for (int m = 0; m < killEmAll.Length; m++)
        {
            Destroy(killEmAll[m]);
        }

        GameObject[] killEmAll2;
        killEmAll2 = GameObject.FindGameObjectsWithTag("LightClone");
        for (int n = 0; n < killEmAll2.Length; n++)
        {
            Destroy(killEmAll2[n]);
        }


        GameObject.Find("MiniMap").transform.localScale = new Vector3(0, 0, 0);
        //GameObject.Find("Sky_image").transform.localScale = new Vector3(0, 0, 0);
        //GameObject.Find("Landmark_image").transform.localScale = new Vector3(0, 0, 0);
        GameObject.Find("Histogram").transform.localScale = new Vector3(0, 0, 0);
        GameObject.Find("Gradient Bar").transform.localScale = new Vector3(0, 0, 0);
        GameObject.Find("Texture_Image").transform.localScale = new Vector3(0, 0, 0);
        Park.transform.gameObject.GetComponent<Histogram>().enabled = false;
        Park.transform.gameObject.GetComponent<CFH>().enabled = false;
        Park.transform.gameObject.GetComponent<ShadowVariation>().enabled = false;
        Camera_sky.transform.gameObject.GetComponent<SkyExposure>().enabled = false;
        LightObject.transform.gameObject.GetComponent<Light>().enabled = false;
        ParkRestColor();
        respawns = GameObject.FindGameObjectsWithTag("Selectable");
        foreach (GameObject Selectable in respawns)
        {
            Selectable.GetComponent<Renderer>().material.color = Color.white;
        }
    }


    private bool PressedKeyOfCurrentPrefab()
    {
        return currentPlaceableObject != null && currentPrefabIndex == 0;
    }

    private void MoveCurrentObjectToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            currentPlaceableObject.transform.position = hitInfo.point;
            currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        }
    }

    private void RotateFromMouseWheel()
    {
        Debug.Log(Input.mouseScrollDelta);
        mouseWheelRotation += Input.mouseScrollDelta.y;
        currentPlaceableObject.transform.Rotate(Vector3.up, mouseWheelRotation * 5f);
    }

    private void ReleaseIfClicked()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentPlaceableObject = null;
            ObjCollider = GameObject.FindGameObjectsWithTag("Selectable");
            foreach (GameObject CapsuleClone in ObjCollider)
            {
                CapsuleClone.GetComponent<Collider>().enabled = true;
            }

        }
        if (Input.GetMouseButtonDown(1))
        {
            GameObject.Find("MiniMap").transform.localScale = new Vector3(0, 0, 0);
            Destroy(currentPlaceableObject);
        }
    }

    private void DeletingItem()
    {

        if (Input.GetMouseButton(1))
        {
            Debug.Log("Select an object");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
                Debug.Log("Object Selected is :" + hitInfo.transform.gameObject.name);

            if (Input.GetKeyUp(KeyCode.C))
            {
                //hitInfo.collider.gameObject.GetComponent<Renderer>().enabled = !(hitInfo.collider.gameObject.GetComponent<MeshRenderer>().enabled);
                if(hitInfo.collider.gameObject == GameObject.Find("Terrain"))
                {
                    Debug.Log("The terrain cannot be deleted!");
                }
                else
                {
                  Destroy(hitInfo.collider.gameObject);
                  Building_numbers2 += 1;
                }
                

            }
                
        }
    }

    private void Scaling()
    {

        if (Input.GetMouseButton(1))
        {
            Debug.Log("Select an object");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
                Debug.Log("Object Selected is :" + hitInfo.transform.gameObject.name);

            temp = new Vector3(0, 0, 0);
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {

                temp.y += 0.2f;
                hitInfo.transform.localScale += temp;
            }
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                temp.z += 0.2f;
                hitInfo.transform.localScale += temp;
            }
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                temp.x += 0.2f;
                hitInfo.transform.localScale += temp;
            }

            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                hitInfo.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }

            mouseWheelRotation += Input.mouseScrollDelta.y;
            hitInfo.transform.transform.Rotate(Vector3.up, mouseWheelRotation * 0.1f);
        }
    }

    private void Landmark()
    {
        if (Input.GetMouseButton(1))
        {
            Debug.Log("Select an object");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
                Debug.Log("Object Selected is :" + hitInfo.transform.gameObject.name);

            if (Input.GetKeyUp(KeyCode.L))
            {
                //GameObject.Find("Landmark_image").transform.localScale = new Vector3(1, 1, 1);
                hitInfo.transform.gameObject.GetComponent<Landmark>().enabled = true;


            }
        }
    }

    public void CFH()
    {
        Park.transform.gameObject.GetComponent<CFH>().enabled = !Park.transform.gameObject.GetComponent<CFH>().enabled;
    }

    public void Shadow()
    {

        Park.transform.gameObject.GetComponent<ShadowVariation>().enabled = !Park.transform.gameObject.GetComponent<ShadowVariation>().enabled;
    }

    public void ParkRestColor()
    {
        Park.GetComponent<Renderer>().material = defaultMaterial;
    }


    public void Sun()
    {
        Park.transform.gameObject.GetComponent<Sun>().enabled = true;
    
    }

    public void Histogram()
    {
        Park.transform.gameObject.GetComponent<Histogram>().enabled = !Park.transform.gameObject.GetComponent<Histogram>().enabled;
    }


    void OnGUI()
    {

        GUIStyle mystyle2 = new GUIStyle(GUI.skin.button);
        mystyle2.onActive.background = GUI_Texture;
        mystyle2.fontSize = 25;
        mystyle2.alignment = TextAnchor.MiddleLeft;
        mystyle2.fontStyle = FontStyle.Bold;
        mystyle2.normal.textColor = Color.red;

        GUI.backgroundColor = Color.black;

        GUIStyle mystyle = new GUIStyle(GUI.skin.button);
        mystyle.onActive.background = GUI_Texture;
        mystyle.fontSize = 25;
        mystyle.alignment = TextAnchor.MiddleLeft;
        mystyle.fontStyle = FontStyle.Bold;
        mystyle.normal.textColor = Color.yellow;
        


        if (GUI.Button(new Rect(10, 60, 200, 40), "Heatmap", mystyle))
        {
            if (isClicked)
            {
                isClicked = false;
                GameObject.Find("Histogram").transform.localScale = new Vector3(0, 0, 0);
                GameObject.Find("Gradient Bar").transform.localScale = new Vector3(0, 0, 0);
                GameObject.Find("Texture_Image").transform.localScale = new Vector3(0, 0, 0);
                Park.transform.gameObject.GetComponent<Histogram>().enabled = false;
                ParkRestColor();
            }
            else
            {
                isClicked = true;
                Histogram();
                GameObject.Find("Histogram").transform.localScale = new Vector3(2, 1, 1);
                GameObject.Find("Gradient Bar").transform.localScale = new Vector3(2, 2, 1);
                GameObject.Find("Texture_Image").transform.localScale = new Vector3(1, 1, 1);
            }

        }

        if (GUI.Button(new Rect(10, 100, 200, 40), "CFH", mystyle))
        {

            if (isClicked)
            {
                isClicked = false;
                GameObject.Find("Gradient Bar").transform.localScale = new Vector3(0, 0, 0);
                GameObject.Find("Texture_Image").transform.localScale = new Vector3(0, 0, 0);
                ParkRestColor();
                Park.transform.gameObject.GetComponent<CFH>().enabled = false;
            }
            else
            {
                isClicked = true;
                CFH();
                GameObject.Find("Gradient Bar").transform.localScale = new Vector3(2, 2, 1);
                GameObject.Find("Texture_Image").transform.localScale = new Vector3(1,1,1);
            }
            
        }
        if (GUI.Button(new Rect(10, 140, 200, 40), "Shadow", mystyle))
        {

            if (isClicked)
            {
                isClicked = false;
                ParkRestColor();
                Park.transform.gameObject.GetComponent<ShadowVariation>().enabled = false;
                GameObject.Find("Texture_Image").transform.localScale = new Vector3(0, 0, 0);
            }
            else
            {
                isClicked = true;
                Shadow();
                GameObject.Find("Texture_Image").transform.localScale = new Vector3(1, 1, 1);
            }

        }
        if (GUI.Button(new Rect(10, 180, 200, 40), "Sky Exposure", mystyle))
        {

            Camera_sky.transform.gameObject.GetComponent<SkyExposure>().enabled = true;
            Thread.Sleep(100);
            
            GameObject.Find("MiniMap").transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            //GameObject.Find("Sky_image").transform.localScale = new Vector3(1, 1, 1);

            if (PressedKeyOfCurrentPrefab())
            {
                Destroy(currentPlaceableObject);
                currentPrefabIndex = -1;
            }
            else
            {
                if (currentPlaceableObject != null)
                {
                    Destroy(currentPlaceableObject);
                }

                currentPlaceableObject = Instantiate(placeableObjectPrefabs[0]);
                currentPlaceableObject.tag = "CapsuleClone";
                currentPlaceableObject.name = "CapsuleClone";
                currentPlaceableObject.transform.parent = GameObject.Find("Predefiend").transform;
            }
        }

        if (GUI.Button(new Rect(10, 10, 200, 40), "Escape", mystyle2))
        {

            disactiveObjects();
            
        }

        if (GUI.Button(new Rect(10, 220, 200, 40), "Light intensity", mystyle))
        {
            LightObject.transform.gameObject.GetComponent<Light>().enabled = true;
            Thread.Sleep(100);


            if (PressedKeyOfCurrentPrefab())
            {
                Destroy(currentPlaceableObject);
                currentPrefabIndex = -1;
            }
            else
            {
                if (currentPlaceableObject != null)
                {
                    Destroy(currentPlaceableObject);
                }

                currentPlaceableObject = Instantiate(placeableObjectPrefabs[1]);
                currentPlaceableObject.tag = "LightClone";
                currentPlaceableObject.name = "LightClone";
                currentPlaceableObject.transform.parent = GameObject.Find("Predefiend").transform;
            }
        }

        if (GUI.Button(new Rect(10, 260, 200, 40), "New Building", mystyle))
        {
            Building_numbers2 -= 1;
            ObjCollider = GameObject.FindGameObjectsWithTag("Selectable");
            foreach (GameObject Clone in ObjCollider)
            {
                Clone.GetComponent<Collider>().enabled = false;
            }

            if (PressedKeyOfCurrentPrefab())
            {
                Destroy(currentPlaceableObject);
                currentPrefabIndex = -1;
            }
            else
            {
                if (currentPlaceableObject != null)
                {
                    Destroy(currentPlaceableObject);
                    Building_numbers2 += 1;
                }

                currentPlaceableObject = Instantiate(placeableObjectPrefabs[2]);
                currentPlaceableObject.tag = "Selectable";
                currentPlaceableObject.name = "Selectable";
                currentPlaceableObject.transform.parent = GameObject.Find("Predefiend").transform;
            }
        }

        /*if (GUI.Button(new Rect(10, 300, 200, 40), "Landmark", mystyle))
        {

            if (isClicked)
            {
                isClicked = false;
                Landmark_click = false;
                

            }
            else
            {
                isClicked = true;
                Landmark_click = true;

            }

        }*/

    }


}