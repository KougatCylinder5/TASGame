using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelBuilder : MonoBehaviour
{
    public Queue<GameObject> Objects;

    public string chosenLevel;
    public TextAsset levelInfo;
    // Start is called before the first frame update
    void Start()
    {

        SceneManager.LoadScene("LevelOverlay", LoadSceneMode.Additive);

        PlayerPrefs.SetString("ChosenLevel", "Level 1");
        
        chosenLevel = PlayerPrefs.GetString("ChosenLevel");
        Debug.Log(chosenLevel);
        levelInfo = Resources.Load("Data/Levels/"+chosenLevel) as TextAsset;
        WorldInfo info = JsonUtility.FromJson<WorldInfo>(levelInfo.text);
        Debug.Log(info);
        foreach(WorldObject obj in info.objects)
        {
            Instantiate(Resources.Load("Level Generator/" + obj.size + " " + obj.type) as GameObject, new Vector2(obj.x, obj.y), Quaternion.Euler(0, 0, 0));
        }
    }
    void Update()
    {
        
    }
}
public class WorldInfo
{
    public string name;
    public string description;
    public List<WorldObject> objects;

    public override string ToString()
    {
        string output = name + " " + description + " ";

        foreach(WorldObject Object in objects)
        {
            output += Object.x;
            output += Object.y;
            output += Object.size;
            output += Object.type;
        }


        return output;
    }
}
[System.Serializable]
public class WorldObject
{
    public float x;
    public float y;
    public ObjectType type;
    public ObjectSize size;
} 
public enum ObjectSize
{
    _1x1,
    _1x2,
    _1x3,
    _1x4,
    _2x1,
    _2x2,
    _2x3,
    _2x4,
    _3x1,
    _3x2,
    _3x3,
    _3x4,
    _4x1,
    _4x2,
    _4x3,
    _4x4,
}

public enum ObjectType
{
    Chain,
    Tile,
    Boost,
    Checkpoint,
    End
}
