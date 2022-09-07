using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TucanoBodyMapping : MonoBehaviour
{
    public GameObject avatar;
    public GameObject user;
    public GameObject anotherUser;

    public string mapping_file = "./Assets/tucano_mapping_baseline.txt";
    private Dictionary<GameObject, GameObject> mapping = new Dictionary<GameObject, GameObject>();
    private Dictionary<string, Quaternion> initialRotations = new Dictionary<string, Quaternion>();
    private Dictionary<string, Quaternion> initialUserRotations = new Dictionary<string, Quaternion>();
    
    private string poseFile = "tucano_user_pose.txt";

    public string userName = "";
    private bool flag = false;

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
        userName += "_tucano_body.txt";
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
                if (pair.Key.transform.name.Contains("Pelvis"))
                {
                    Quaternion temp = pair.Value.transform.rotation;
                    pair.Key.transform.rotation = Quaternion.Euler(temp.eulerAngles.x, temp.eulerAngles.y, temp.eulerAngles.z + 225);
                }
                else if (pair.Key.transform.name.Contains("DragonLArm1343"))
                {
                    Quaternion temp = pair.Value.transform.localRotation * Quaternion.Inverse(initialUserRotations[pair.Value.transform.name]);
                    Quaternion initial = initialRotations[pair.Key.transform.name];
                    pair.Key.transform.rotation = Quaternion.Euler(temp.eulerAngles.x, temp.eulerAngles.y, temp.eulerAngles.z + 225);
                    // pair.Key.transform.localRotation = Quaternion.Euler(temp.eulerAngles.x, temp.eulerAngles.z, temp.eulerAngles.y) * initialRotations[pair.Key.transform.name];
                }
                else
                {
                    pair.Key.transform.localRotation = pair.Value.transform.localRotation * Quaternion.Inverse(initialUserRotations[pair.Value.transform.name]) * initialRotations[pair.Key.transform.name];
                }
            }
        }
    }
}
