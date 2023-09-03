using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderLine : MonoBehaviour
{
    public LineRenderer line;

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        line.SetPosition(0,gameObject.transform.parent.position);
        line.SetPosition(1,gameObject.transform.position);
    }
}
