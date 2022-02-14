using UnityEngine;
public class ObjectManager : MonoBehaviour
{
    Vector3 Dist;
    private float PosX;
    private float PosY;
    bool shiftOn = true;
    private Vector3 default_pos;
    private Vector3 default_rot;

    private void Awake()
    {
        default_pos = GameObject.Find("Main Camera").transform.position;
        default_rot = GameObject.Find("Main Camera").transform.eulerAngles;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("Shift Pressed");
            shiftOn = false;
            GameObject.Find("Main Camera").transform.position = default_pos;
            GameObject.Find("Main Camera").transform.eulerAngles = default_rot;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Debug.Log("Shift Released");
            shiftOn = true;
        }
    }

    private void OnMouseDown()
    {
        if (!shiftOn)
        {
            Dist = Camera.main.WorldToScreenPoint(this.transform.position);
            PosX = Input.mousePosition.x - Dist.x;
            PosY = Input.mousePosition.y - Dist.y;
        }
    }

    private void OnMouseDrag()
    {
        if (!shiftOn)
        {
            Vector3 CurrentPos = new Vector3(Input.mousePosition.x - PosX, Input.mousePosition.y - PosY, Dist.z);
            Vector3 WorldPos = Camera.main.ScreenToWorldPoint(CurrentPos);
            this.transform.position = WorldPos;
        }
    }
}