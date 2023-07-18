using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Blink : MonoBehaviour
{
    private bool visible = true;
    private UnityEngine.UI.Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<UnityEngine.UI.Image>();
        StartCoroutine(nameof(BlnkerTimer));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator BlnkerTimer()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            image.enabled = !image.enabled;
            visible = !visible;
        }

    }
}
