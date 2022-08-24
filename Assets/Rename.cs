using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rename : MonoBehaviour
{
    public string LeftorRight = "";

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform g in transform.GetComponentsInChildren<Transform>())
        {
            if (!g.name.Contains(LeftorRight))
            {
                if (g.name.Contains("Hand") && g.name.Contains("_"))
                {
                    string temp = g.name.Split("_")[1];
                    g.name = LeftorRight + "_" + temp;
                }
                else
                {
                    g.name = LeftorRight + "_" + g.name;
                }
            }
        }    
    }
}
