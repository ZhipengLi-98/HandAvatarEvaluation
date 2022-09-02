using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayAnimation : MonoBehaviour
{
    public string mapping_file = "./Assets/elephant_mapping.txt";
    public List<string> controlledJoints = new List<string>();
    public List<Dictionary<string, Quaternion>> allMotions = new List<Dictionary<string, Quaternion>>();

    public Animator animator;     
    public bool isAnimation = false;

    private string fileName;
    private StreamWriter writer;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        fileName = this.transform.name + ".txt";
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
            controlledJoints.Add(joints[0]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isAnimation && animator.GetCurrentAnimatorClipInfo(0).Length == 0)
        {
            isAnimation = false;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            animator.enabled = true;
            animator.speed = 1f;
            animator.Play("Walk", 0, 0f);
            isAnimation = true;
            allMotions.Clear();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            animator.enabled = true;
            animator.speed = 1f;
            animator.Play("Walk", 0, 0f);
        }
        if (isAnimation)
        {
            Dictionary<string, Quaternion> temp = new Dictionary<string, Quaternion>();
            foreach (Transform g in this.transform.GetComponentsInChildren<Transform>())
            {
                temp.Add(g.name, g.localRotation);
            }
            allMotions.Add(temp);
        }
    }
}
