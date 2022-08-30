using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SwanMapping : MonoBehaviour
{
    private string mapping_file = "./Assets/swan_mapping.txt";
    private Dictionary<GameObject, GameObject> mapping = new Dictionary<GameObject, GameObject>();
    private List<string> controlledJoints = new List<string>();
    private Dictionary<string, Quaternion> initialRotations = new Dictionary<string, Quaternion>();
    private Dictionary<string, Quaternion> initialHandRotations = new Dictionary<string, Quaternion>();

    public PlayAnimation player;
    public RecordAvatar recorder;

    public string fileName = "test.txt";
    private StreamWriter writer;

    private string poseFile = "swan_hand_pose.txt";
    public GameObject initialLeftHand;
    public GameObject initialRightHand;

    public GameObject leftHand;
    public GameObject rightHand;

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
                mapping.Add(ajoint, hjoint);
                controlledJoints.Add(ajoint.transform.name);
                initialRotations.Add(ajoint.transform.name, ajoint.transform.localRotation);
            }
        }
    }

    string UppercaseFirst(string s)
    {
        if (string.IsNullOrEmpty(s))
        {
            return string.Empty;
        }
        return char.ToUpper(s[0]) + s.Substring(1);
    }

    // Start is called before the first frame update
    void Start()
    {
        writer = new StreamWriter(fileName);
        StreamReader reader = new StreamReader(poseFile);
        string[] content = reader.ReadToEnd().Split("\n");
        foreach (string s in content)
        {
            string[] information = s.Split(" ");
            if (s.Split("_")[0] == "Left")
            {
                foreach (Transform g in initialLeftHand.transform.GetComponentsInChildren<Transform>())
                {
                    if (!g.name.Contains("_") || g.name.Split("_")[0] != "b")
                    {
                        continue;
                    }
                    string temp = UppercaseFirst(g.name.Split("_")[2]);
                    if (information[0].Contains(temp))
                    {
                        g.position = new Vector3(float.Parse(information[1]), float.Parse(information[2]), float.Parse(information[3]));
                        g.rotation = new Quaternion(float.Parse(information[4]), float.Parse(information[5]), float.Parse(information[6]), float.Parse(information[7]));
                        temp = "Left_" + temp;
                        if (g.name.Split("_").Length == 4 && g.name.Split("_")[3] == "null")
                        {
                            temp = temp + "Tip";
                        }
                        initialHandRotations.Add(temp, g.transform.localRotation);
                        break;
                    }
                }
            }
            else
            {
                foreach (Transform g in initialRightHand.transform.GetComponentsInChildren<Transform>())
                {
                    if (!g.name.Contains("_") || g.name.Split("_")[0] != "b")
                    {
                        continue;
                    }
                    string temp = UppercaseFirst(g.name.Split("_")[2]);
                    if (information[0].Contains(temp))
                    {
                        g.position = new Vector3(float.Parse(information[1]), float.Parse(information[2]), float.Parse(information[3]));
                        g.rotation = new Quaternion(float.Parse(information[4]), float.Parse(information[5]), float.Parse(information[6]), float.Parse(information[7]));
                        temp = "Right_" + temp;
                        if (g.name.Split("_").Length == 4 && g.name.Split("_")[3] == "null")
                        {
                            temp = temp + "Tip";
                        }
                        initialHandRotations.Add(temp, g.transform.localRotation);
                        break;
                    }
                }
            }
        }
        initialLeftHand.transform.position = new Vector3(0f, -0.1f, 0.2f);
        // initialLeftHand.transform.rotation = Quaternion.Euler(0, 180, 0);
        initialRightHand.transform.position = new Vector3(0f, -0.1f, 0.2f);
        // initialRightHand.transform.rotation = Quaternion.Euler(0, 180, 0);
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
            if (pair.Value.transform.name.Contains("Right_Forearm"))
            {
                pair.Key.transform.rotation = pair.Value.transform.rotation * Quaternion.Euler(0, 90, 0);
            }
            else if (pair.Key.transform.name.Contains("Neck"))
            {
                Quaternion temp = pair.Value.transform.localRotation * Quaternion.Inverse(initialHandRotations[pair.Value.transform.name]);
                Quaternion initial = initialRotations[pair.Key.transform.name];
                pair.Key.transform.localRotation = Quaternion.Euler(temp.eulerAngles.z, temp.eulerAngles.x, -temp.eulerAngles.y) * initial;
                // pair.Key.transform.localRotation = pair.Value.transform.localRotation * Quaternion.Inverse(initialHandRotations[pair.Value.transform.name]) * initialRotations[pair.Key.transform.name];
            }
            else if (pair.Key.transform.name.Contains("Tarsus.1"))
            {
                Quaternion temp = pair.Value.transform.localRotation * Quaternion.Inverse(initialHandRotations[pair.Value.transform.name]);
                Quaternion initial = initialRotations[pair.Key.transform.name];
                pair.Key.transform.localRotation = Quaternion.Euler(-temp.eulerAngles.z, temp.eulerAngles.y, temp.eulerAngles.x) * initial;
                // pair.Key.transform.localRotation = pair.Value.transform.localRotation * Quaternion.Inverse(initialHandRotations[pair.Value.transform.name]) * initialRotations[pair.Key.transform.name];
            }
            else if (pair.Key.transform.name.Contains("Tarsus.2"))
            {
                Quaternion temp = pair.Value.transform.localRotation * Quaternion.Inverse(initialHandRotations[pair.Value.transform.name]);
                Quaternion initial = initialRotations[pair.Key.transform.name];
                pair.Key.transform.localRotation = Quaternion.Euler(temp.eulerAngles.z, temp.eulerAngles.y, temp.eulerAngles.x) * initial;
                // pair.Key.transform.localRotation = pair.Value.transform.localRotation * Quaternion.Inverse(initialHandRotations[pair.Value.transform.name]) * initialRotations[pair.Key.transform.name];
            }
            else
            {
                Quaternion temp = pair.Value.transform.localRotation * Quaternion.Inverse(initialHandRotations[pair.Value.transform.name]);
                Quaternion initial = initialRotations[pair.Key.transform.name];
                pair.Key.transform.localRotation = Quaternion.Euler(temp.eulerAngles.z, temp.eulerAngles.x, temp.eulerAngles.y) * initial;
                // pair.Key.transform.localRotation = pair.Value.transform.localRotation * Quaternion.Inverse(initialHandRotations[pair.Value.transform.name]) * initialRotations[pair.Key.transform.name];
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

    void OnApplicationQuit()
    {
        writer.Close();
    }
}
