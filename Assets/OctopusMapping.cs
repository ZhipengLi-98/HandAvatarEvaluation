using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class OctopusMapping : MonoBehaviour
{
    private string mapping_file = "./Assets/octopus_mapping.txt";
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
            temp += " " + trans.localRotation[i];
        }
        return temp;
    }

    string ConvertQuaternionToString(Quaternion trans)
    {
        string temp = "";
        for (int i = 0; i < 4; i++)
        {
            temp += trans[i];
            if (i < 3)
            {
                temp += " ";
            }
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
                print(joints[0]);
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
            pair.Key.transform.rotation = pair.Value.transform.rotation;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            print(player.allMotions.Count);
            print(recorder.poses.Count);
            writer.WriteLine("Avatar");
            int tempCnt = 0;
            foreach (Dictionary<string, Quaternion> t in player.allMotions)
            {
                writer.WriteLine(tempCnt);
                tempCnt += 1;
                foreach (KeyValuePair<string, Quaternion> pair in t)
                {
                    if (controlledJoints.Contains(pair.Key))
                    {
                        writer.WriteLine(pair.Key + " " + ConvertQuaternionToString(pair.Value));
                    }
                }
            }
            writer.WriteLine("Users");
            tempCnt = 0;
            foreach (Dictionary<string, Quaternion> i in recorder.poses)
            {
                writer.WriteLine(tempCnt);
                tempCnt += 1;
                foreach (KeyValuePair<string, Quaternion> pair in i)
                {
                    if (controlledJoints.Contains(pair.Key))
                    {
                        writer.WriteLine(pair.Key + " " + ConvertQuaternionToString(pair.Value));
                    }
                }
            }
        }
    }

    void OnApplicationQuit()
    {
        writer.Close();
    }
}
