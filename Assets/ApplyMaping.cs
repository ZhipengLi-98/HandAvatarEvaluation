using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ApplyMaping : MonoBehaviour
{
    public GameObject leftHandIK;
    private string mapping_file = "./Assets/spider_mapping_baseline.txt";
    private Dictionary<string, Quaternion> initialRotations = new Dictionary<string, Quaternion>();

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
            GameObject ajoint = GameObject.Find(joints[0]);
            GameObject hjoint = GameObject.Find(joints[1]);
            initialRotations.Add(joints[0], ajoint.transform.rotation);
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
                pair.Key.transform.rotation = pair.Value.transform.rotation * initialRotations[pair.Key.transform.name];
            }
        }
    }
}
