using UnityEngine;

public class SaveGames
{

    //serialized
    public string PlayerName = "Player";
    public int XP = 0;
    public Vector3 PlayerPosition = Vector3.zero;
    public Vector3 Playerlocalscale = Vector3.zero;

    private static string _gameDataFileName = "data.json";

    private static SaveGames _instance;
    public static SaveGames Instance
    {
        get
        {
            if (_instance == null)
                Load();
            return _instance;
        }

    }

    public static void Save()
    {
        FileManager.Save(_gameDataFileName, _instance);
    }

    public static void Load()
    {
        _instance = FileManager.Load<SaveGames>(_gameDataFileName);
    }

}