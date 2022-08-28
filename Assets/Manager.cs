using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Manager : MonoBehaviour
{
    public GameObject leftHand;
    public GameObject rightHand;

    public List<GameObject> leftHandRecord;
    public List<GameObject> rightHandRecord;

    private bool flag  = false;
    private bool replay = false;
    private int cnt = 0;
    
    public Animator animator;     

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

    // Start is called before the first frame update
    void Start()
    {
        writer = new StreamWriter(fileName);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            animator.enabled = true;
            animator.speed = 1f;
            animator.Play("Walk", 0, 0f);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            flag = !flag;
            if (flag)
            {
                foreach (GameObject temp in leftHandRecord)
                {
                    Destroy(temp);
                }
                leftHandRecord.Clear();
                foreach (GameObject temp in rightHandRecord)
                {
                    Destroy(temp);
                }
                rightHandRecord.Clear();
            }
        }
        if (leftHandRecord.Count > 0 && leftHandRecord[leftHandRecord.Count - 1].activeSelf)
        {
            leftHandRecord[leftHandRecord.Count - 1].SetActive(false);
        }
        if (rightHandRecord.Count > 0 && rightHandRecord[rightHandRecord.Count - 1].activeSelf)
        {
            rightHandRecord[rightHandRecord.Count - 1].SetActive(false);
        }
        if (flag)
        {
            // if (leftSkeleton.IsDataHighConfidence && rightSkeleton.IsDataHighConfidence)
            {
                GameObject copyLeftHand = Instantiate(leftHand);
                copyLeftHand.transform.position = leftHand.transform.position;
                copyLeftHand.transform.rotation = leftHand.transform.rotation;
                copyLeftHand.GetComponent<OVRHand>().enabled = false;
                copyLeftHand.GetComponent<SkinnedMeshRenderer>().enabled = false;

                leftHandRecord.Add(copyLeftHand);

                GameObject copyRightHand = Instantiate(rightHand);
                copyRightHand.transform.position = rightHand.transform.position;
                copyRightHand.transform.rotation = rightHand.transform.rotation;
                copyRightHand.GetComponent<OVRHand>().enabled = false;
                copyRightHand.GetComponent<SkinnedMeshRenderer>().enabled = false;

                rightHandRecord.Add(copyRightHand);
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            replay = !replay;  
            cnt = 0;
            foreach (GameObject temp in leftHandRecord)
            {
                temp.GetComponent<SkinnedMeshRenderer>().enabled = true;
                temp.SetActive(false);
            }
            foreach (GameObject temp in rightHandRecord)
            {
                temp.GetComponent<SkinnedMeshRenderer>().enabled = true;
                temp.SetActive(false);
            }
        }
        if (replay)
        {
            leftHandRecord[cnt].SetActive(false);
            rightHandRecord[cnt].SetActive(false);
            cnt += 1;
            if (cnt > leftHandRecord.Count - 1)
            {
                replay = false;
                cnt = 0;
            }
            else
            {
                leftHandRecord[cnt].SetActive(true);
                rightHandRecord[cnt].SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            for (int i = 0; i < leftHandRecord.Count; i++)
            {
                OVRSkeleton leftSkeleton = leftHandRecord[i].GetComponent<OVRSkeleton>();
                for (int j = 1; j < leftSkeleton.Bones.Count; j++)
                {      
                    writer.WriteLine(ConvertTransformToString(leftSkeleton.Bones[j].Transform));
                }
                OVRSkeleton rightSkeleton = rightHandRecord[i].GetComponent<OVRSkeleton>();
                for (int j = 1; j < rightSkeleton.Bones.Count; j++)
                {      
                    writer.WriteLine(ConvertTransformToString(rightSkeleton.Bones[j].Transform));
                }
            }
        }
    }
}
