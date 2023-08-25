using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CodeMemory : MonoBehaviour
{
    public List<List<char>> rawLines;
    public List<GameObject> visualLines;
    [SerializeField]
    private int currentLineFocus;
    [SerializeField]
    private int currentDepthFocus;
    [SerializeField]
    private GameObject linePrefab;
    [SerializeField]
    private GameObject cursor;
    [SerializeField]
    private GameObject scrollWheel;
    [SerializeField]
    private Vector3 offset;

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
        

        KeyCode key = Keyboard.GetCurrentKeyDown();

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
                        visualLines[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
                    }
                    currentDepthFocus = 0;
                    scrollWheel.GetComponent<Scrollbar>().value = 0;
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
                        visualLines[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
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
                if (key.ToString().Contains("Period"))
                {
                    rawLines[currentLineFocus].Insert(currentDepthFocus++, '.');
                }

                visualLines[currentLineFocus].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = new string(rawLines[currentLineFocus].ToArray());

                break;

        }
        UpdateCursor();
    }
    private void UpdateCursor()
    {
        TextMeshProUGUI character = visualLines[currentLineFocus].transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        try
        {
            Vector3 height = Vector3.zero;
            if (character.text[currentDepthFocus - 1].CompareTo(' ') == 0)
            {
                height = character.textInfo.characterInfo[currentDepthFocus - 2].bottomRight - character.textInfo.characterInfo[currentDepthFocus - 2].topRight + new Vector3(9, 0);
            }
            else 
            {
                height = character.textInfo.characterInfo[currentDepthFocus - 1].bottomRight - character.textInfo.characterInfo[currentDepthFocus - 1].topRight;
            }
            cursor.transform.localPosition = character.textInfo.characterInfo[currentDepthFocus - 1].bottomRight + height / 2 + offset + new Vector3(0, -35 * currentLineFocus - 1);
        }
        catch
        {
            cursor.transform.localPosition = new Vector3(97, -40 + -35 * currentLineFocus);
        }
        cursor.transform.SetAsLastSibling();
    }
}
