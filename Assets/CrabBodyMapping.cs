using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CrabBodyMapping : MonoBehaviour
{
    public GameObject avatar;
    public GameObject user;
    public GameObject anotherUser;

    public string mapping_file = "./Assets/crab_mapping_baseline.txt";
    private Dictionary<GameObject, GameObject> mapping = new Dictionary<GameObject, GameObject>();
    private Dictionary<string, Quaternion> initialRotations = new Dictionary<string, Quaternion>();
    private Dictionary<string, Quaternion> initialUserRotations = new Dictionary<string, Quaternion>();
    
    private string poseFile = "crab_user_pose.txt";

    public string userName = "";
    private bool flag = false;
    private StreamWriter writer;

    public RecordAvatar recorder;

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
            GameObject hjoint = null;
            foreach (Transform g in avatar.transform.GetComponentsInChildren<Transform>())
            {
                if (g.name == joints[0])
                {
                    ajoint = g.gameObject;
                    break;
                }
            }
            foreach (Transform g in user.transform.GetComponentsInChildren<Transform>())
            {
                if (g.name == joints[1])
                {
                    hjoint = g.gameObject;
                    break;
                }
            }
            if (ajoint != null && hjoint != null)
            {
                mapping.Add(ajoint, hjoint);
                initialRotations.Add(joints[0], ajoint.transform.localRotation);
            }
        }
    }
    
    void ReadUserJoints() 
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
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        userName += "_crab_body.txt";
        writer = new StreamWriter(userName);
        StreamReader reader = new StreamReader(poseFile);
        string[] content = reader.ReadToEnd().Split("\n");
        foreach (string s in content)
        {
            string[] information = s.Split(" ");
            foreach (Transform g in anotherUser.GetComponentsInChildren<Transform>())
            {
                if (g.name == information[0])
                {
                    g.position = new Vector3(float.Parse(information[1]), float.Parse(information[2]), float.Parse(information[3]));
                    g.localRotation = new Quaternion(float.Parse(information[4]), float.Parse(information[5]), float.Parse(information[6]), float.Parse(information[7]));
                    initialUserRotations.Add(information[0], g.transform.localRotation);
                    break;
                }
            }
        }

        ReadUserJoints();
        anotherUser.transform.position = new Vector3(0, -1, 4);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            flag = true;
            readMapping();
        }
        if (flag)
        {
            foreach (KeyValuePair<GameObject, GameObject> pair in mapping)
            {
                if (pair.Key.transform.name.Contains("Body"))
                {
                    Quaternion temp = pair.Value.transform.rotation;
                    pair.Key.transform.rotation = Quaternion.Euler(-temp.eulerAngles.z + 45, temp.eulerAngles.y + 90, 0);
                }
                else if (pair.Key.transform.name.Contains("Arm_0.L"))
                {
                    Quaternion temp = pair.Value.transform.localRotation * Quaternion.Inverse(initialUserRotations[pair.Value.transform.name]);
                    Quaternion initial = initialRotations[pair.Key.transform.name];
                    // pair.Key.transform.rotation = Quaternion.Euler(temp.eulerAngles.x, temp.eulerAngles.y, temp.eulerAngles.z + 225);
                    pair.Key.transform.localRotation = Quaternion.Euler(-temp.eulerAngles.x - 45, 0, -temp.eulerAngles.z + 45) * initialRotations[pair.Key.transform.name];
                }
                else if (pair.Key.transform.name.Contains("Arm_0.R"))
                {
                    Quaternion temp = pair.Value.transform.localRotation * Quaternion.Inverse(initialUserRotations[pair.Value.transform.name]);
                    Quaternion initial = initialRotations[pair.Key.transform.name];
                    // pair.Key.transform.rotation = Quaternion.Euler(temp.eulerAngles.x, temp.eulerAngles.y, temp.eulerAngles.z + 225);
                    pair.Key.transform.localRotation = Quaternion.Euler(-temp.eulerAngles.x - 45, 0, temp.eulerAngles.z - 45) * initialRotations[pair.Key.transform.name];
                }
                 else if (pair.Key.transform.name.Contains("Arm_3.L"))
                {
                    Quaternion temp = pair.Value.transform.localRotation * Quaternion.Inverse(initialUserRotations[pair.Value.transform.name]);
                    Quaternion initial = initialRotations[pair.Key.transform.name];
                    // pair.Key.transform.rotation = Quaternion.Euler(temp.eulerAngles.x, temp.eulerAngles.y, temp.eulerAngles.z + 225);
                    pair.Key.transform.localRotation = Quaternion.Euler(0, 0, temp.eulerAngles.z - 90) * initialRotations[pair.Key.transform.name];
                }
                else if (pair.Key.transform.name.Contains("Arm_3.R"))
                {
                    Quaternion temp = pair.Value.transform.localRotation * Quaternion.Inverse(initialUserRotations[pair.Value.transform.name]);
                    Quaternion initial = initialRotations[pair.Key.transform.name];
                    // pair.Key.transform.rotation = Quaternion.Euler(temp.eulerAngles.x, temp.eulerAngles.y, temp.eulerAngles.z + 225);
                    pair.Key.transform.localRotation = Quaternion.Euler(0, 0, -temp.eulerAngles.z + 90) * initialRotations[pair.Key.transform.name];
                }
                else if (pair.Key.transform.name.Contains("Leg2_0.L"))
                {
                    Quaternion temp = pair.Value.transform.localRotation * Quaternion.Inverse(initialUserRotations[pair.Value.transform.name]);
                    Quaternion initial = initialRotations[pair.Key.transform.name];
                    // pair.Key.transform.rotation = Quaternion.Euler(temp.eulerAngles.x, temp.eulerAngles.y, temp.eulerAngles.z + 225);
                    pair.Key.transform.localRotation = Quaternion.Euler(0, temp.eulerAngles.x + 30, temp.eulerAngles.z) * initialRotations[pair.Key.transform.name];
                }
                else if (pair.Key.transform.name.Contains("Leg_2_3.L"))
                {
                    Quaternion temp = pair.Value.transform.localRotation * Quaternion.Inverse(initialUserRotations[pair.Value.transform.name]);
                    Quaternion initial = initialRotations[pair.Key.transform.name];
                    // pair.Key.transform.rotation = Quaternion.Euler(temp.eulerAngles.x, temp.eulerAngles.y, temp.eulerAngles.z + 225);
                    pair.Key.transform.localRotation = Quaternion.Euler(-temp.eulerAngles.z, 0, 0) * initialRotations[pair.Key.transform.name];
                }
                else if (pair.Key.transform.name.Contains("Leg2_0.R"))
                {
                    Quaternion temp = pair.Value.transform.localRotation * Quaternion.Inverse(initialUserRotations[pair.Value.transform.name]);
                    Quaternion initial = initialRotations[pair.Key.transform.name];
                    // pair.Key.transform.rotation = Quaternion.Euler(temp.eulerAngles.x, temp.eulerAngles.y, temp.eulerAngles.z + 225);
                    pair.Key.transform.localRotation = Quaternion.Euler(0, -temp.eulerAngles.x - 30, temp.eulerAngles.z) * initialRotations[pair.Key.transform.name];
                }
                else if (pair.Key.transform.name.Contains("Leg_2_3.R"))
                {
                    Quaternion temp = pair.Value.transform.localRotation * Quaternion.Inverse(initialUserRotations[pair.Value.transform.name]);
                    Quaternion initial = initialRotations[pair.Key.transform.name];
                    // pair.Key.transform.rotation = Quaternion.Euler(temp.eulerAngles.x, temp.eulerAngles.y, temp.eulerAngles.z + 225);
                    pair.Key.transform.localRotation = Quaternion.Euler(temp.eulerAngles.z, 0, 0) * initialRotations[pair.Key.transform.name];
                }
                else
                {
                    // pair.Key.transform.localRotation = pair.Value.transform.localRotation * Quaternion.Inverse(initialUserRotations[pair.Value.transform.name]) * initialRotations[pair.Key.transform.name];
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            int tempCnt = 0;
            foreach (GameObject obj in recorder.avatarRecord)
            {
                writer.WriteLine(tempCnt);
                tempCnt += 1;
                foreach (Transform g in obj.transform.GetComponentsInChildren<Transform>())
                {
                    writer.WriteLine(g.name + " " + ConvertTransformToString(g));
                }
            }
        }
    }
}
