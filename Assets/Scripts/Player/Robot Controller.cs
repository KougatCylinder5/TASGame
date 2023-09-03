using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class RobotController : MonoBehaviour
{
    public GameObject robot;
    public Rigidbody robotBody;

    public List<GameObject> buttons;

    public static bool Paused = true, beginningOfLevel = true;

    public Task currentTask = new();
    // Start is called before the first frame update
    void Awake()
    {
        Time.fixedDeltaTime = 1f/60f;
        StartCoroutine(waitForLoad());
        StartCoroutine(SettleObjects());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Paused) return;
        Advance(false);
    }
    private void Advance(bool buttonPress)
    {
        if (buttonPress)
        {
            Paused = true;
        }
        ExecuteStep();
        Physics.Simulate(Time.fixedDeltaTime);
    }
    private IEnumerator SettleObjects()
    {
        for(int i = 0; i < 120; i++)
        {
            Physics.Simulate(Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
        
    }
    

    public void PauseCode()
    {
        Paused = !Paused;

        if (Paused)
        {
            buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "Unpause";
        }
        else
        {
            buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "Pause";
        }
    }

    private void ExecuteStep()
    {
        if(currentTask.command == Commands.None && CodeParsing.Tasks.TryPeek(out Task task))
        {
            currentTask = task;
        }
        if (currentTask.command == Commands.None)
            return;
        Vector2 normalizedDirection = new();
        switch (currentTask.command)
        {
            case Commands.Walk:
                normalizedDirection = DirectionToVector2(new() { currentTask.direction });
                robotBody.AddForce(normalizedDirection * (2<<2));
                break;
            case Commands.Slide:
                //do set image/hitbox here
                goto case Commands.Run;
            case Commands.Run:
                normalizedDirection = DirectionToVector2(new() { currentTask.direction });
                robotBody.AddForce(normalizedDirection);
                break;
            case Commands.Jump:
                robotBody.AddForce(Vector2.up * 200);
                currentTask.command = Commands.None;
                CodeParsing.Tasks.Dequeue();
                break;
            default:
                break;
        }
    }

    private Vector2 DirectionToVector2(List<Direction> directions)
    {
        Vector2 vector = Vector2.zero;
        foreach (Direction direction in directions)
        {
            switch (direction)
            {
                case Direction.Up:
                    vector.y++;
                    break;
                case Direction.Down:
                    vector.y--;
                    break;
                case Direction.Right:
                    vector.x++;
                    break;
                case Direction.Left:
                    vector.x--;
                    break;
            }
            vector.Normalize();
        }
        return vector;
    }
    
    public IEnumerator waitForLoad()
    {
        while (!LevelBuilder.LevelLoaded) { 
            yield return null;
        }
        robot = GameObject.Find("_1x1 Robot(Clone)");
        robotBody = robot.GetComponent<Rigidbody>();
        buttons.Add(GameObject.Find("Start"));
        buttons.Add(GameObject.Find("Pause"));
        buttons.Add(GameObject.Find("Advance"));
        buttons.Add(GameObject.Find("Restart"));
        buttons[0].GetComponent<Button>().onClick.AddListener(delegate { Paused = false; });
        buttons[0].GetComponent<Button>().onClick.AddListener(() => CodeParsing.GenerateCommands());
        buttons[1].GetComponent<Button>().onClick.AddListener(() => PauseCode());
        buttons[2].GetComponent<Button>().onClick.AddListener(delegate { Advance(true); });
        buttons[3].GetComponent<Button>().onClick.AddListener(() => LevelBuilder.RestartLevel());
        buttons[3].GetComponent<Button>().onClick.AddListener(
            delegate
            {
                currentTask = new();
                CodeParsing.Tasks.Clear();
            });

    }
}
