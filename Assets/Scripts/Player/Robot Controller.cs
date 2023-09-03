using System;
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

    public static bool Paused = true;
    public bool beginningOfLevel = true;

    public Task currentTask = new();

    public static GameObject waitingForObject;
    public GameObject hook;
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
            CodeParsing.Tasks.Dequeue();
        }
        if (currentTask.command == Commands.None)
            return;
        Vector2 normalizedDirection = new();
        switch (currentTask.command)
        {
            case Commands.Walk:
                normalizedDirection = DirectionToVector2(new() { currentTask.direction });
                robotBody.AddForce(normalizedDirection * (2<<2), ForceMode.Acceleration);
                break;
            case Commands.Slide:
                //do set image/hitbox here
                break;
            case Commands.Run:
                normalizedDirection = DirectionToVector2(new() { currentTask.direction });
                robotBody.AddForce(normalizedDirection * (2 << 3), ForceMode.Acceleration);
                break;
            case Commands.Jump:
                robotBody.AddForce(Vector2.up * 400, ForceMode.Acceleration);
                currentTask.command = Commands.None;
                break;
            case Commands.Stop:
                robotBody.AddForce(new Vector2(Mathf.Clamp(-robotBody.velocity.x,-0.25f,0.25f), 0), ForceMode.Acceleration);
                break;
            default:
                break;
        }

        try
        {
            Task nextTask = CodeParsing.Tasks.Peek();
            switch (nextTask.command)
            {
                case Commands.Wait:
                    nextTask.value -= Time.fixedDeltaTime;
                    if (nextTask.value <= 0)
                    {
                        CodeParsing.Tasks.Dequeue();
                        currentTask.command = Commands.None;
                    }
                    break;
                case Commands.Until:
                    Debug.Log(waitingForObject);
                    if (waitingForObject != null && waitingForObject.name.Contains(Enum.GetName(typeof(WaitForType), nextTask.waitType)))
                    {
                        CodeParsing.Tasks.Dequeue();
                        currentTask.command = Commands.None;
                    }
                    break;
                case Commands.Hook:
                    
                    StartCoroutine(ExtendHook(CodeParsing.Tasks.Dequeue()));
                    break;
            }
        }
        catch (InvalidOperationException) 
        {}
    }
    private IEnumerator ExtendHook(Task task)
    {
        LineRenderer line = hook.GetComponent<LineRenderer>();

        while (true)
        {
            if (Paused)
            {
                yield return null;
            }
            Vector2 direction = DirectionToVector2(new() { task.direction, task.direction2 });
            
            if(Mathf.Sqrt(line.GetPosition(0).magnitude * line.GetPosition(1).magnitude) > 5)
            {
                line.SetPosition(1, robot.transform.position);
                break;
            }

            hook.transform.Translate(direction);

            yield return new WaitForFixedUpdate();
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
                default:
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
        buttons[0].GetComponent<Button>().onClick.AddListener(delegate { waitingForObject = null; });
        buttons[0].GetComponent<Button>().onClick.AddListener(() => LevelBuilder.RestartLevel());
        buttons[0].GetComponent<Button>().onClick.AddListener(() => CodeParsing.GenerateCommands());
        buttons[1].GetComponent<Button>().onClick.AddListener(() => PauseCode());
        buttons[2].GetComponent<Button>().onClick.AddListener(delegate { Advance(true); });
        buttons[3].GetComponent<Button>().onClick.AddListener(() => LevelBuilder.RestartLevel());
        buttons[3].GetComponent<Button>().onClick.AddListener(delegate { currentTask = new(); CodeParsing.Tasks.Clear(); });

    }
}
