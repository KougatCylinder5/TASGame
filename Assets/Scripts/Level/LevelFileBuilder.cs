using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFileBuilder : MonoBehaviour
{
    public new string name;
    public string description;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene("LevelOverlay", LoadSceneMode.Additive);

        WorldInfo info = new();

        info.name = base.name;
        info.description = description;

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("WorldTiles"))
        {
            string size = obj.name.Substring(0, obj.name.IndexOf(" "));
            string type = obj.name.Substring(obj.name.IndexOf(" ") + 1);
            Enum.TryParse(type, true, out ObjectType Type);
            Enum.TryParse(size, true, out ObjectSize Size);
            info.objects.Add(new WorldObject(obj.transform.position.x, obj.transform.position.y, Type, Size));
        }


        string output = JsonUtility.ToJson(info);
        Debug.Log(output);
    }
    void Update()
    {

    }
}