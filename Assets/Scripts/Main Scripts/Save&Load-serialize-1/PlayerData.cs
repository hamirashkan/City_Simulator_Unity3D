using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerData
{
    public float[] position;
    public List<GameObject> AllObjects = new List<GameObject>();
    public List<Vector3> AllObjectsPos = new List<Vector3>();


    public PlayerData(Player player)
    {

        foreach (GameObject go in GameObject.FindObjectsOfType(typeof(GameObject)))
        {

            AllObjects.Add(go);

        }
        foreach (GameObject pos in AllObjects)
        {
            //Vector3 position = new Vector3( GameObject.FindGameObjectWithTag("pos").transform.position, );
            Vector3 position = new Vector3(pos.transform.position.x, pos.transform.position.y, pos.transform.position.z);
            AllObjectsPos.Add(position);

        }
        
    }

}
