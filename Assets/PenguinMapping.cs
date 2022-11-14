using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PenguinMapping : MonoBehaviour
{
    private string mapping_file = "./Assets/penguin_mapping.txt";
    private Dictionary<GameObject, GameObject> mapping = new Dictionary<GameObject, GameObject>();
    private Dictionary<string, Quaternion> initialRotations = new Dictionary<string, Quaternion>();
    private Dictionary<string, Quaternion> initialHandRotations = new Dictionary<string, Quaternion>();

    private string poseFile = "penguin_hand_pose.txt";
    public GameObject initialLeftHand;
    public GameObject initialRightHand;

    public GameObject leftHand;
    public GameObject rightHand;
    
    public string userName = "";
    public bool flag = false;
    private StreamWriter writer;

    public GameObject avatar;
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
            foreach (Transform g in avatar.transform.GetComponentsInChildren<Transform>())
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
        userName += "_penguin_hand.txt";
        writer = new StreamWriter(userName);
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
                        initialHandRotations.Add(information[0], g.transform.localRotation);
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
                        initialHandRotations.Add(information[0], g.transform.localRotation);
                        break;
                    }
                }
            }
        }
        initialLeftHand.transform.position = new Vector3(0f, 0.2f, 0.02f);
        // initialLeftHand.SetActive(false);
        // initialLeftHand.transform.rotation = Quaternion.Euler(0, 180, 0);
        initialRightHand.transform.position = new Vector3(0f, 0.2f, 0.02f);
        // initialRightHand.transform.rotation = Quaternion.Euler(0, 180, 0);
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
                if (pair.Key.transform.name.Contains("AlienPelvis"))
                {
                    Quaternion temp = pair.Value.transform.rotation;
                    pair.Key.transform.rotation = Quaternion.Euler(temp.eulerAngles.x, temp.eulerAngles.y, temp.eulerAngles.z) * Quaternion.Euler(180, 180, 30);
                }
                else if (pair.Key.transform.name.Contains("AlienLLeg2"))
                {
                    Quaternion temp = pair.Value.transform.localRotation * Quaternion.Inverse(initialHandRotations[pair.Value.transform.name]);
                    Quaternion initial = initialRotations[pair.Key.transform.name];
                    pair.Key.transform.localRotation = Quaternion.Euler(temp.eulerAngles.y, 0, -temp.eulerAngles.z) * initial;
                }
                else if (pair.Key.transform.name.Contains("AlienLLegPalm"))
                {
                    Quaternion temp = pair.Value.transform.localRotation * Quaternion.Inverse(initialHandRotations[pair.Value.transform.name]);
                    Quaternion initial = initialRotations[pair.Key.transform.name];
                    pair.Key.transform.localRotation = Quaternion.Euler(0, 0, -temp.eulerAngles.z) * initial;
                }
                else if (pair.Key.transform.name.Contains("AlienRLegPalm"))
                {
                    Quaternion temp = pair.Value.transform.localRotation * Quaternion.Inverse(initialHandRotations[pair.Value.transform.name]);
                    Quaternion initial = initialRotations[pair.Key.transform.name];
                    pair.Key.transform.localRotation = Quaternion.Euler(0, 0, -temp.eulerAngles.z) * initial;
                }
                else if (pair.Key.transform.name.Contains("AlienRLeg2"))
                {
                    Quaternion temp = pair.Value.transform.localRotation * Quaternion.Inverse(initialHandRotations[pair.Value.transform.name]);
                    Quaternion initial = initialRotations[pair.Key.transform.name];
                    pair.Key.transform.localRotation = Quaternion.Euler(-temp.eulerAngles.y, 0, -temp.eulerAngles.z) * initial;
                }
                else if (pair.Key.transform.name.Contains("AlienSpine5"))
                {
                    Quaternion temp = pair.Value.transform.localRotation * Quaternion.Inverse(initialHandRotations[pair.Value.transform.name]);
                    Quaternion initial = initialRotations[pair.Key.transform.name];
                    pair.Key.transform.localRotation = Quaternion.Euler(0, 0, temp.eulerAngles.z) * initial;
                }
                else if (pair.Key.transform.name.Contains("AlienSpine35"))
                {
                    Quaternion temp = pair.Value.transform.localRotation * Quaternion.Inverse(initialHandRotations[pair.Value.transform.name]);
                    Quaternion initial = initialRotations[pair.Key.transform.name];
                    pair.Key.transform.localRotation = Quaternion.Euler(0, 0, temp.eulerAngles.z) * initial;
                }
                else if (pair.Key.transform.name.Contains("AlienHeadBone001"))
                {
                    Quaternion temp = pair.Value.transform.localRotation * Quaternion.Inverse(initialHandRotations[pair.Value.transform.name]);
                    Quaternion initial = initialRotations[pair.Key.transform.name];
                    pair.Key.transform.localRotation = Quaternion.Euler(0, 0, temp.eulerAngles.z) * initial;
                }
                else if (pair.Key.transform.name.Contains("AlienLArmCollarbone"))
                {
                    Quaternion temp = pair.Value.transform.localRotation * Quaternion.Inverse(initialHandRotations[pair.Value.transform.name]);
                    Quaternion initial = initialRotations[pair.Key.transform.name];
                    pair.Key.transform.localRotation = Quaternion.Euler(0, 2f * -temp.eulerAngles.z, 0) * initial;
                }
                else if (pair.Key.transform.name.Contains("AlienLArm2"))
                {
                    Quaternion temp = pair.Value.transform.localRotation * Quaternion.Inverse(initialHandRotations[pair.Value.transform.name]);
                    Quaternion initial = initialRotations[pair.Key.transform.name];
                    pair.Key.transform.localRotation = Quaternion.Euler(0, 2f * -temp.eulerAngles.z, 0) * initial;
                }
                else if (pair.Key.transform.name.Contains("AlienLArmPalm"))
                {
                    Quaternion temp = pair.Value.transform.localRotation * Quaternion.Inverse(initialHandRotations[pair.Value.transform.name]);
                    Quaternion initial = initialRotations[pair.Key.transform.name];
                    pair.Key.transform.localRotation = Quaternion.Euler(0, 1f * -temp.eulerAngles.z, 0) * initial;
                }
                else if (pair.Key.transform.name.Contains("AlienRArmCollarbone"))
                {
                    Quaternion temp = pair.Value.transform.localRotation * Quaternion.Inverse(initialHandRotations[pair.Value.transform.name]);
                    Quaternion initial = initialRotations[pair.Key.transform.name];
                    pair.Key.transform.localRotation = Quaternion.Euler(0, temp.eulerAngles.z, 0) * initial;
                }
                else if (pair.Key.transform.name.Contains("AlienRArm2"))
                {
                    Quaternion temp = pair.Value.transform.localRotation * Quaternion.Inverse(initialHandRotations[pair.Value.transform.name]);
                    Quaternion initial = initialRotations[pair.Key.transform.name];
                    pair.Key.transform.localRotation = Quaternion.Euler(0, temp.eulerAngles.z, 0) * initial;
                }
                else if (pair.Key.transform.name.Contains("AlienRArmPalm"))
                {
                    Quaternion temp = pair.Value.transform.localRotation * Quaternion.Inverse(initialHandRotations[pair.Value.transform.name]);
                    Quaternion initial = initialRotations[pair.Key.transform.name];
                    pair.Key.transform.localRotation = Quaternion.Euler(0, temp.eulerAngles.z, 0) * initial;
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
