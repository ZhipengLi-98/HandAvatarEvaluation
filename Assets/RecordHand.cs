using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class RecordHand : MonoBehaviour
{
    public GameObject leftHand;
    public GameObject rightHand;

    public string fileName = "elephant_hand_pose.txt";
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

    // Start is called before the first frame update
    void Start()
    {
        writer = new StreamWriter(fileName);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            OVRSkeleton leftSkeleton = leftHand.GetComponent<OVRSkeleton>();
            for (int i = 0; i < leftSkeleton.Bones.Count; i++)
            {
                writer.WriteLine(ConvertTransformToString(leftSkeleton.Bones[i].Transform));
            }
            OVRSkeleton rightSkeleton = rightHand.GetComponent<OVRSkeleton>();
            for (int i = 0; i < rightSkeleton.Bones.Count; i++)
            {
                writer.WriteLine(ConvertTransformToString(rightSkeleton.Bones[i].Transform));
            }
        }    
    }

    void OnApplicationQuit()
    {
        writer.Close();
    }
}
