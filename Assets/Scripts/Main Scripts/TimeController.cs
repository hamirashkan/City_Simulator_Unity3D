using UnityEngine;
using System.Collections;

public class TimeController : MonoBehaviour
{
    public GameObject player;
    public ArrayList playerPositions;
    public ArrayList playerRotations;
    public bool isReversing = false;

    void Start()
    {
        playerPositions = new ArrayList();
        playerRotations = new ArrayList();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            isReversing = true;
        }
        else
        {
            isReversing = false;
        }

        if (Input.GetKey(KeyCode.F)) { 
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            Pause();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Resume();
        }

        void Pause()
        {
            Time.timeScale = 0;
        }

        void Resume()
        {
            Time.timeScale = 1;
        }
    }

    void FixedUpdate()
    {
        if (!isReversing)
        {
            playerPositions.Add(player.transform.position);
            playerRotations.Add(player.transform.localEulerAngles);
        }
        else
        {
            player.transform.position = (Vector3)playerPositions[playerPositions.Count - 1];
            playerPositions.RemoveAt(playerPositions.Count - 1);

            player.transform.localEulerAngles = (Vector3)playerRotations[playerRotations.Count - 1];
            playerRotations.RemoveAt(playerRotations.Count - 1);
        }
    }
}