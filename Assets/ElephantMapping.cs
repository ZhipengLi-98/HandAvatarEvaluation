using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ElephantMapping : MonoBehaviour
{
    private string mapping_file = "./Assets/elephant_mapping.txt";
    private Dictionary<GameObject, GameObject> mapping = new Dictionary<GameObject, GameObject>();
    private List<string> controlledJoints = new List<string>();

    public PlayAnimation player;
    public RecordAvatar recorder;

    public string fileName = "test.txt";
    private StreamWriter writer;

    string ConvertTransformToString(Transform trans)
    {
        string temp = trans.name;
        for (int i = 0; i < 3; i++)
        {
            temp += " " + trans.position[i];
        }
        for (int i = 0; i < 4; i++)
        {
            temp += " " + trans.rotation[i];
        }
        return temp;
    }

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
            GameObject ajoint = null;
            foreach (Transform g in this.transform.GetComponentsInChildren<Transform>())
            {
                if (g.name == joints[0])
                {
                    ajoint = g.gameObject;
                }
            }
            GameObject hjoint = GameObject.Find(joints[1]);
            if (ajoint != null && hjoint != null)
            {
                print(ajoint);
                print(hjoint);
                mapping.Add(ajoint, hjoint);
                controlledJoints.Add(ajoint.transform.name);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        writer = new StreamWriter(fileName);
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
            // pair.Key.transform.rotation = Quaternion.Euler(-pair.Value.transform.rotation.eulerAngles.z, pair.Value.transform.rotation.eulerAngles.x, pair.Value.transform.rotation.eulerAngles.y);
            if (pair.Key.transform.name.Contains("_R") && pair.Key.transform.name.Contains("Ear"))
            {
                pair.Key.transform.rotation = pair.Value.transform.rotation * Quaternion.Euler(-90, 270, 0);
            }
            else if (pair.Key.transform.name.Contains("_L") && pair.Key.transform.name.Contains("Ear"))
            {
                pair.Key.transform.rotation = pair.Value.transform.rotation * Quaternion.Euler(90, 270, 0);
            }
            else if (pair.Key.transform.name.Contains("Root_M"))
            {
                pair.Key.transform.rotation = pair.Value.transform.rotation * Quaternion.Euler(180, 180, 180);
            }
            else if (pair.Key.transform.name.Contains("_M"))
            {
                pair.Key.transform.rotation = pair.Value.transform.rotation * Quaternion.Euler(180, 180, 90);
            }
            else if (pair.Key.transform.name.Contains("nose"))
            {
                pair.Key.transform.rotation = pair.Value.transform.rotation * Quaternion.Euler(0, 180, 0);
            }
            else if (pair.Key.transform.name.Contains("_L"))
            {
                pair.Key.transform.rotation = pair.Value.transform.rotation * Quaternion.Euler(0, 180, 0);
            }
            else if (pair.Key.transform.name.Contains("_R"))
            {
                pair.Key.transform.rotation = pair.Value.transform.rotation * Quaternion.Euler(0, 0, 0);
            }
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            int tempCnt = 0;
            foreach (List<Transform> t in player.allMotions)
            {
                writer.WriteLine(tempCnt);
                tempCnt += 1;
                foreach (Transform tt in t)
                {
                    writer.WriteLine(ConvertTransformToString(tt));
                }
            }
            writer.WriteLine("Users");
            tempCnt = 0;
            foreach (List<Transform> i in recorder.poses)
            {
                writer.WriteLine(tempCnt);
                tempCnt += 1;
                foreach (Transform j in i)
                {
                    writer.WriteLine(ConvertTransformToString(j));
                }
            }
        }
    }
}
