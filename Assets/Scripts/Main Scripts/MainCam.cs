using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCam : MonoBehaviour
{

    [SerializeField]
    private float lookSpeedH = 5f;

    [SerializeField]
    private float lookSpeedV = 5f;

    [SerializeField]
    private float zoomSpeed = 5f;

    [SerializeField]
    private float dragSpeed = 6f;

    private float yaw = 0f;
    private float pitch = 0f;

    private Vector3 default_pos;
    private Vector3 default_rot;

    private void Start()
    {
        // Initialize the correct initial rotation
        this.yaw = this.transform.eulerAngles.y;
        this.pitch = this.transform.eulerAngles.x;
        default_pos = this.transform.position;
        default_rot = this.transform.eulerAngles;
    }

    /*void OnGUI()
    {
        GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button);
        myButtonStyle.fontStyle = FontStyle.Bold;
        myButtonStyle.fontSize = 25;
        GUI.backgroundColor = Color.black;
        myButtonStyle.normal.textColor = Color.yellow;

        if (GUI.Button(new Rect(120, 10, 100, 50), "Camera \n Reset", myButtonStyle))
        {
            this.transform.position = default_pos;
            this.transform.eulerAngles = default_rot;
        }

    }*/

    private void Update()
    {
        //Move Fwd-Bwd-Right-Left with keyboard
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(this.dragSpeed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(-this.dragSpeed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(new Vector3(0, 0, -this.dragSpeed * Time.deltaTime));
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(new Vector3(0, 0, this.dragSpeed * Time.deltaTime));

        }
        // Only work with the Left Alt pressed
        if (Input.GetKey(KeyCode.LeftAlt))
       {
            //Look around with Left Mouse
            if (Input.GetMouseButton(0))
            {
                this.yaw += this.lookSpeedH * Input.GetAxis("Mouse X");
                this.pitch -= this.lookSpeedV * Input.GetAxis("Mouse Y");

                this.transform.eulerAngles = new Vector3(this.pitch, this.yaw, 0f);
            }

            //drag camera around with Middle Mouse
            if (Input.GetMouseButton(2))
            {
                transform.Translate(-Input.GetAxisRaw("Mouse X") * Time.deltaTime * dragSpeed, -Input.GetAxisRaw("Mouse Y") * Time.deltaTime * dragSpeed, 0);
            }

            /*if (Input.GetMouseButton(1))
            {
                //Zoom in and out with Right Mouse
                this.transform.Translate(0, 0, Input.GetAxisRaw("Mouse X") * this.zoomSpeed * .07f, Space.Self);
            }*/

            //Zoom in and out with Mouse Wheel
            this.transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel") * this.zoomSpeed, Space.Self);
        }
    }
}

