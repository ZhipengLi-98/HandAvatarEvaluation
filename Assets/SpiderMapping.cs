using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class SpiderMapping : MonoBehaviour
{
    private string mapping_file = "./Assets/spider_mapping.txt";
    private Dictionary<GameObject, GameObject> mapping = new Dictionary<GameObject, GameObject>();
    private List<string> controlledJoints = new List<string>();
    private List<string> controlledHandJoints = new List<string>();
    private Dictionary<string, Quaternion> initialRotations = new Dictionary<string, Quaternion>();
    private Dictionary<string, Quaternion> initialHandRotations = new Dictionary<string, Quaternion>();
    private Dictionary<string, Quaternion> clusterPoseRotations = new Dictionary<string, Quaternion>();
    private List<float> poseDeviations = new List<float>();

    public PlayAnimation player;
    public RecordAvatar recorder;

    public TextMeshProUGUI text;

    public string fileName = "test.txt";
    private StreamWriter writer;
    
    private string poseFile = "spider_hand_pose.txt";
    public GameObject initialLeftHand;
    public GameObject initialRightHand;

    public GameObject leftHand;
    public GameObject rightHand;

    private bool flag = false;
    private bool poseFlag = false;

    private float timer = 0f;
    private float recordTimer = 0f;

    private string clusterFile = "./cluster_poses/spider_poses.txt";
    private List<Dictionary<string, List<float>>> clusterPoses = new List<Dictionary<string, List<float>>>();
    private int clusterPoseCnt = 0;

    public GameObject avatar;
    public GameObject anotherAvatar;

    private float bestDeviation = 1e4f;

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

    void ReadHandJoints() 
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
            controlledHandJoints.Add(joints[1]);
        }
    }

    void ReadClusterPoses()
    {
        StreamReader reader = new StreamReader(clusterFile);
        string content = reader.ReadToEnd();
        reader.Close();
        string[] poses = content.Split("#");
        foreach (string pose in poses)
        {
            string[] pairs = pose.Split("\n");
            Dictionary<string, List<float>> tt = new Dictionary<string, List<float>>();
            foreach (string pair in pairs)
            {
                string[] data = pair.Split(" ");
                if (data.Length == 8)
                {
                    List<float> t = new List<float>();
                    for (int i = 1; i < 8; i++)
                    {
                        t.Add(float.Parse(data[i]));
                    }
                    tt.Add(data[0], t);
                }
            }
            clusterPoses.Add(tt);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        text.text = "Perform the transparent gesture";
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
        initialLeftHand.transform.position = new Vector3(0f, -0.1f, 0.2f);
        // initialLeftHand.transform.rotation = Quaternion.Euler(0, 180, 0);
        initialRightHand.transform.position = new Vector3(0f, -0.1f, 0.2f);
        // initialRightHand.transform.rotation = Quaternion.Euler(0, 180, 0);

        ReadHandJoints();
        ReadClusterPoses();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            flag = true;
            readMapping();
        }
        if (!flag)
        {
            OVRSkeleton rightSkeleton = rightHand.GetComponent<OVRSkeleton>();
            if (rightSkeleton.Bones.Count > 10)
            {
                bool tempFlag = true;
                for (int i = 0; i < rightSkeleton.Bones.Count; i++)
                {
                    if (controlledHandJoints.Contains(rightSkeleton.Bones[i].Transform.name))
                    {
                        Quaternion a = initialHandRotations[rightSkeleton.Bones[i].Transform.name];
                        Quaternion b = rightSkeleton.Bones[i].Transform.localRotation;
                        float angle = 0f;
                        Vector3 axis = Vector3.zero;
                        (a * Quaternion.Inverse(b)).ToAngleAxis(out angle, out axis);
                        if (angle > 30)
                        {
                            tempFlag = false;
                            break;
                        }
                    }
                }
                if (tempFlag)
                {
                    timer += Time.deltaTime;
                    if (timer > 1f)
                    {
                        flag = true;
                        timer = 0f;
                        readMapping();
                    }
                }
                else
                {
                    timer = 0f;
                }
            }
        }
        if (flag)
        {
            foreach (KeyValuePair<GameObject, GameObject> pair in mapping)
            { 
                if (pair.Key.transform.name == "Bone")
                {
                    // pair.Key.transform.rotation = pair.Value.transform.rotation * Quaternion.Euler(90, 180, 0);
                    GameObject leftForearm = GameObject.Find("Left_ForearmStub");
                    GameObject rightForearm = GameObject.Find("Right_ForearmStub");
                    pair.Key.transform.rotation = Quaternion.Euler((leftForearm.transform.rotation.eulerAngles.x + leftForearm.transform.rotation.eulerAngles.x) / 2,
                        (leftForearm.transform.rotation.eulerAngles.y + rightForearm.transform.rotation.eulerAngles.y) / 2,
                        (leftForearm.transform.rotation.eulerAngles.z + leftForearm.transform.rotation.eulerAngles.z) / 2) * Quaternion.Euler(-90, 90, 0);
                    // pair.Key.transform.rotation = leftForearm.transform.rotation * rightForearm.transform.rotation;
                }
                else
                {
                    Quaternion temp = pair.Value.transform.localRotation * Quaternion.Inverse(initialHandRotations[pair.Value.transform.name]);
                    Quaternion initial = initialRotations[pair.Key.transform.name];
                    // pair.Key.transform.localRotation = Quaternion.Euler(temp.eulerAngles.x, temp.eulerAngles.z, temp.eulerAngles.y) * initialAvatar;
                    if (pair.Key.transform.name.Contains("Leg"))
                    {
                        if (pair.Key.transform.name.Contains("R."))
                        {
                            pair.Key.transform.localRotation = Quaternion.Euler(temp.eulerAngles.x, temp.eulerAngles.z, temp.eulerAngles.y) * initial;

                        }
                        else if (pair.Key.transform.name.Contains("L."))
                        {
                            pair.Key.transform.localRotation = Quaternion.Euler(temp.eulerAngles.x, -temp.eulerAngles.z, -temp.eulerAngles.y) * initial;
                        }
                    }
                    else
                    {
                        if (pair.Key.transform.name.Contains("R."))
                        {
                            pair.Key.transform.localRotation = Quaternion.Euler(temp.eulerAngles.x, temp.eulerAngles.y, temp.eulerAngles.z) * initial;
                        }
                        else if (pair.Key.transform.name.Contains("L."))
                        {
                            pair.Key.transform.localRotation = Quaternion.Euler(temp.eulerAngles.x, temp.eulerAngles.y, -temp.eulerAngles.z) * initial;
                        }
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.A) && clusterPoseCnt < 3)
        {
            text.text = clusterPoseCnt.ToString();
            clusterPoseRotations.Clear();
            poseDeviations.Clear();
            timer = 0f;
            recordTimer = Time.time;
            poseFlag = false;
            foreach (KeyValuePair<string, List<float>> pair in clusterPoses[clusterPoseCnt])
            {
                foreach (Transform g in anotherAvatar.transform.GetComponentsInChildren<Transform>())
                {
                    if (g.name == pair.Key)
                    {
                        g.localPosition = new Vector3(pair.Value[0], pair.Value[1], pair.Value[2]);
                        g.localRotation = new Quaternion(pair.Value[3], pair.Value[4], pair.Value[5], pair.Value[6]);
                        clusterPoseRotations.Add(g.name, g.localRotation);
                        break;
                    }
                }
            }
            clusterPoseCnt += 1;
        }
        if (!poseFlag && clusterPoseCnt > 0 && clusterPoseCnt < 4)
        {
            // after 1s, record the deviaiton of each joint (average)
            // record the timer
            bool tempFlag = true;
            float tDevia = 0f;
            foreach (Transform child in avatar.GetComponentsInChildren<Transform>())
            {
                if (controlledJoints.Contains(child.name))
                {
                    Quaternion a = clusterPoseRotations[child.name];
                    Quaternion b = child.localRotation;
                    float angle = 0f;
                    Vector3 axis = Vector3.zero;
                    (a * Quaternion.Inverse(b)).ToAngleAxis(out angle, out axis);
                    if (angle > 180)
                    {
                        angle -= 360;
                    }
                    angle = Mathf.Abs(angle);
                    tDevia += angle;
                }
            }
            if (tDevia / controlledJoints.Count < bestDeviation)
            {
                bestDeviation = tDevia / controlledJoints.Count;
            }
            if (tDevia / controlledJoints.Count > 15)
            {
                // text.text = child.name + " " + angle.ToString();
                tempFlag = false;
            }
            text.text = (tDevia / controlledJoints.Count).ToString();
            if (tempFlag)
            {
                tDevia /= controlledJoints.Count;
                poseDeviations.Add(tDevia);
                timer += Time.deltaTime;
                if (timer > 1f)
                {
                    float ttDevia = 0f;
                    foreach (float t in poseDeviations)
                    {
                        ttDevia += t;
                    }
                    ttDevia /= poseDeviations.Count;
                    float duration = Time.time - recordTimer;
                    writer.WriteLine(clusterPoseCnt + " " + ttDevia + " " + duration.ToString());
                    poseFlag = true;
                    timer = 0f;
                    poseDeviations.Clear();
                    text.text = "Complete";
                }
            }
            else
            {
                poseDeviations.Clear();
                timer = 0f;
            }
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            float duration = Time.time - recordTimer;
            writer.WriteLine(clusterPoseCnt + " " + bestDeviation + " " + duration.ToString());
            poseFlag = true;
            timer = 0f;
            poseDeviations.Clear();
            text.text = "Complete";
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            print(player.allMotions.Count);
            print(recorder.poses.Count);
            writer.WriteLine("Avatar");
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
