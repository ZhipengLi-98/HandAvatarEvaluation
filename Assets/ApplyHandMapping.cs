using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ApplyHandMapping : MonoBehaviour
{
    private string mapping_file = "./Assets/spider_mapping.txt";
    private Dictionary<GameObject, GameObject> mapping = new Dictionary<GameObject, GameObject>();

    void readMapping()
    {
        StreamReader reader = new StreamReader(mapping_file);
        string content = reader.ReadToEnd();
        reader.Close();
        string[] pairs = content.Split("\n");
        foreach (string pair in pairs)
        {
            string[] joints = pair.Split(": ");
            if (joints.Length != 2)
            {
                continue;
            }
            print(joints[1]);
            GameObject ajoint = GameObject.Find(joints[0]);
            GameObject hjoint = GameObject.Find(joints[1]);
            if (hjoint != null)
            {
                print(ajoint);
                print(hjoint);
                mapping.Add(ajoint, hjoint);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.A))
        {
            readMapping();
        }
        foreach (KeyValuePair<GameObject, GameObject> pair in mapping)
        {
            if (pair.Key.transform.name == "Bone")
            {
                pair.Key.transform.rotation = pair.Value.transform.rotation;
            }
            else
            {
                pair.Key.transform.rotation = pair.Value.transform.rotation;
            }
        }
    }
}
