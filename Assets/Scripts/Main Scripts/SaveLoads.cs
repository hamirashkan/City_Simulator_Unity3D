using UnityEngine;
using System.Collections.Generic;



public class SaveLoads : MonoBehaviour
{
    //public Vector3 DefaultScale;
    public List<Vector3> DefaultPosition = new List<Vector3>();
    public List<GameObject> AllObjects = new List<GameObject>();
    public List<Vector3> AllObjectsPos = new List<Vector3>();

    void Start()
    {

    }
    // Start is called before the first frame update
    void Awake()
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

        DefaultPosition = AllObjectsPos;
        //DefaultScale = transform.localScale;
    }

    public void Restart()
    {
        //AllObjectsPos.ForEach();
         AllObjectsPos = DefaultPosition;

    }

    public void Load()
    {
        SaveGames.Load();
        transform.localPosition = SaveGames.Instance.PlayerPosition;
        //transform.localScale = SaveGames.Instance.Playerlocalscale;
        
    }

    public void Save()
    {

        SaveGames.Instance.PlayerPosition = this.transform.localPosition;
        SaveGames.Instance.Playerlocalscale = this.transform.localScale;
        SaveGames.Save();
    }
}
