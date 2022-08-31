using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ApplyMaping : MonoBehaviour
{
    public GameObject avatar;
    public GameObject user;
    public GameObject anotherUser;

    public string mapping_file = "./Assets/spider_mapping_baseline.txt";
    private Dictionary<string, Quaternion> initialRotations = new Dictionary<string, Quaternion>();
    private Dictionary<string, Quaternion> initialUserRotations = new Dictionary<string, Quaternion>();

    private string poseFile = "elephant_user_pose.txt";

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
                initialRotations.Add(joints[0], ajoint.transform.localRotation);
                mapping.Add(ajoint, hjoint);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
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
                    g.rotation = new Quaternion(float.Parse(information[4]), float.Parse(information[5]), float.Parse(information[6]), float.Parse(information[7]));
                    initialUserRotations.Add(information[0], g.transform.localRotation);
                    break;
                }
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.T))
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
                pair.Key.transform.localRotation = pair.Value.transform.localRotation * Quaternion.Inverse(initialUserRotations[pair.Value.transform.name]) * initialRotations[pair.Key.transform.name];
            }
        }
    }
}
