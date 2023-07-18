using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class CodeMemory : MonoBehaviour
{
    public List<List<char>> rawLines;
    public List<GameObject> visualLines;
    public int currentLineFocus;
    public int currentDepthFocus;
    public GameObject linePrefab;
    public GameObject cursor;


    // Start is called before the first frame update
    void Start()
    {
        rawLines = new() {new()};    
        //visualLines = new();
        currentLineFocus = 0;
        currentDepthFocus = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCursor();

        KeyCode key = Keyboard.CurrentKeyDown();

        switch (key)
        {
            case KeyCode.LeftArrow:
                currentDepthFocus = Mathf.Clamp(--currentDepthFocus, 0, rawLines[currentLineFocus].Count);
                break;

            case KeyCode.RightArrow:
                currentDepthFocus = Mathf.Clamp(++currentDepthFocus, 0, rawLines[currentLineFocus].Count);
                break;

            case KeyCode.UpArrow:
                currentLineFocus = Mathf.Clamp(--currentLineFocus, 0, int.MaxValue);
                currentDepthFocus = Mathf.Clamp(currentDepthFocus, 0, rawLines[currentLineFocus].Count);
                break;

            case KeyCode.DownArrow:
                currentLineFocus = Mathf.Clamp(++currentLineFocus, 0, int.MaxValue);
                if (currentLineFocus >= rawLines.Count)
                {
                    rawLines.Add(new());
                    visualLines.Add(Instantiate(linePrefab, visualLines.Last().transform.position + new Vector3(0,-35), Quaternion.Euler(0, 0, 0), gameObject.transform));
                    gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(383,visualLines.Count * 35 + 40);
                    for (int i = 0; i < visualLines.Count; i++)
                    {
                        visualLines[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "<mspace=0em>" + (i + 1);
                    }
                    currentDepthFocus = 0;
                }
                else
                {
                    currentDepthFocus = Mathf.Clamp(currentDepthFocus, 0, rawLines[currentLineFocus].Count);
                }
                break;

            case KeyCode.Backspace:
                if (currentDepthFocus != 0)
                {
                    rawLines[currentLineFocus].RemoveAt(--currentDepthFocus);
                }
                else if(currentDepthFocus == 0 && currentLineFocus != 0)
                {
                    Destroy(visualLines[--currentLineFocus].gameObject);
                    visualLines.RemoveAt(currentLineFocus);
                    gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(383, visualLines.Count * 35 + 40);
                    for (int i = currentLineFocus; i < visualLines.Count; i++)
                    {
                        visualLines[i].gameObject.transform.Translate(new(0,35));
                    }
                    rawLines.RemoveAt(currentLineFocus+1);
                    currentDepthFocus = rawLines[currentLineFocus].Count;
                    for(int i = 0; i < visualLines.Count; i++)
                    {
                        visualLines[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "<mspace=0em>" + (i + 1);
                    }

                }
                break;

            case KeyCode.Return:

                break;

            default:
                if (key.ToString().Length == 1)
                {
                    rawLines[currentLineFocus].Insert(currentDepthFocus++, key.ToString().ToCharArray()[0]);
                }
                if (key.ToString().Contains("Alpha"))
                {
                    rawLines[currentLineFocus].Insert(currentDepthFocus++, key.ToString().ToCharArray()[5]);
                }
                if (key.ToString().Contains("Space"))
                {
                    rawLines[currentLineFocus].Insert(currentDepthFocus++, ' ');
                }

                visualLines[currentLineFocus].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "<mspace=0em>" + new string(rawLines[currentLineFocus].ToArray());

                break;

        }
    }
    private void UpdateCursor()
    {
        int x = 125 + 13 * currentDepthFocus;
        int y = -40 + -35 * currentLineFocus;

        cursor.transform.localPosition = new(x, y);
        cursor.transform.SetAsLastSibling();

    }
}
