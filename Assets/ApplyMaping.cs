using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ApplyMaping : MonoBehaviour
{
    private string mapping_file = "./Assets/spider_mapping_baseline.txt";

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
            print(joints[0]);
            print(joints[0].Length);
            print(joints[1]);
            print(joints[1].Length);
            GameObject ajoint = GameObject.Find(joints[0]);
            GameObject hjoint = GameObject.Find(joints[1]);
            print(ajoint.transform.name);
            print(hjoint.transform.name);
            mapping.Add(ajoint, hjoint);
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
                pair.Key.transform.localRotation = pair.Value.transform.rotation * Quaternion.EulerAngles(90, 0, 0);
            }
            else
            {
                pair.Key.transform.localRotation = pair.Value.transform.rotation;
            }
        }
    }
}
