using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class RecordAnimation : MonoBehaviour
{
    private class TransformData
    {
        public Vector3 LocalPosition = Vector3.zero;
        public Vector3 LocalEulerRotation = Vector3.zero;
        public Vector3 LocalScale = Vector3.one;
        public Quaternion localRotation;

        // Unity requires a default constructor for serialization
        public TransformData() { }

        public TransformData(Transform transform)
        {
            LocalPosition = transform.localPosition;
            LocalEulerRotation = transform.localEulerAngles;
            LocalScale = transform.localScale;
            localRotation = transform.localRotation;
        }

        public void ApplyTo(Transform transform)
        {
            transform.localPosition = LocalPosition;
            transform.localEulerAngles = LocalEulerRotation ;
            transform.localScale = LocalScale;
        }
    }

    public Animator animator;
    private Dictionary<string, List<Dictionary<string, TransformData>>> allMotions;
    private bool isAnimation = false;
    private List<Dictionary<string, TransformData>> testMotions;
    private int TEST_MOTION_NUMBER = 5;

    public GameObject avatar;
    public string fileName = "seahorse_poses.txt";

    string ConvertTransformToString(TransformData trans)
    {
        string temp = "";
        for (int i = 0; i < 3; i++)
        {
            temp += " " + trans.LocalPosition[i];
        }
        for (int i = 0; i < 4; i++)
        {
            temp += " " + trans.localRotation[i];
        }
        return temp;
    }

    // Start is called before the first frame update
    void Start()
    {
        allMotions = new Dictionary<string, List<Dictionary<string, TransformData>>>();
        testMotions = new List<Dictionary<string, TransformData>>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            animator.enabled = true;
            animator.Play("Walk", 0, 0f);
            isAnimation = true;
            allMotions.Clear();
        }
        if (isAnimation)
        {
            if (animator.GetCurrentAnimatorClipInfo(0).Length > 0)
            {
                string state = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
                if (!allMotions.ContainsKey(state))
                {
                    List<Dictionary<string, TransformData>> t = new List<Dictionary<string, TransformData>>();
                    allMotions.Add(state, t);
                }
                Dictionary<string, TransformData> temp = new Dictionary<string, TransformData>();
                foreach(Transform child in avatar.GetComponentsInChildren<Transform>())
                {
                    if (avatar.transform.name == "trithemis_Main_ctrl")
                    {
                        if (! (child.name.Contains("bone")))
                        {
                            continue;
                        }
                    }
                    TransformData tempChild = new TransformData(child);
                    temp.Add(child.name, tempChild);
                }
                allMotions[state].Add(temp);
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            testMotions.Clear();
            isAnimation = false;
            List<string> motions = new List<string>(allMotions.Keys);
            print(motions.Count);
            int aniCount = motions.Count;
            for (int i = 0; i < TEST_MOTION_NUMBER / aniCount; i++)
            {
                foreach (KeyValuePair<string, List<Dictionary<string, TransformData>>> pair in allMotions)
                {
                    int temp = Random.Range(0, pair.Value.Count);
                    testMotions.Add(pair.Value[temp]);
                }
            }
            int curCount = testMotions.Count;
            List<string> recordMotion = new List<string>();
            for (int i = curCount; i < TEST_MOTION_NUMBER; i++)
            {
                string randomMotion = motions[Random.Range(0, motions.Count)];
                while(recordMotion.Contains(randomMotion))
                {
                    randomMotion = motions[Random.Range(0, motions.Count)];
                }
                recordMotion.Add(randomMotion);
                int temp = Random.Range(0, allMotions[randomMotion].Count);
                testMotions.Add(allMotions[randomMotion][temp]);
            }
            print(testMotions.Count);
            animator.enabled = false;
            StreamWriter writer = new StreamWriter(fileName);
            foreach (Dictionary<string, TransformData> motion in testMotions)
            {
                foreach (KeyValuePair<string, TransformData> pair in motion)
                {
                    writer.WriteLine(pair.Key + ConvertTransformToString(pair.Value));
                }
                writer.WriteLine("#");
            }
        }
    }
}
