using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotController : MonoBehaviour
{
    public GameObject robot;

    public List<GameObject> buttons;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(waitForLoad());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    public IEnumerator waitForLoad()
    {
        while (!LevelBuilder.LevelLoaded) { 
            yield return null;
        }
        robot = GameObject.Find("_1x1 Robot(Clone)");
        buttons.Add(GameObject.Find("Start"));
        buttons.Add(GameObject.Find("Pause"));
        buttons.Add(GameObject.Find("Advance"));
        buttons.Add(GameObject.Find("Restart"));
        buttons[0].GetComponent<Button>().onClick.AddListener(() => CodeParsing.GenerateCommands());
        buttons[3].GetComponent<Button>().onClick.AddListener(() => LevelBuilder.RestartLevel());

    }
}
