using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayAnimation : MonoBehaviour
{
    public string mapping_file = "./Assets/elephant_mapping.txt";
    public List<string> controlledJoints = new List<string>();
    public List<List<Transform>> allMotions = new List<List<Transform>>();

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
            List<Transform> temp = new List<Transform>();
            foreach (Transform g in this.transform.GetComponentsInChildren<Transform>())
            {
                // if (controlledJoints.Contains(g.name))
                {
                    temp.Add(g);
                }
            }
            allMotions.Add(temp);
        }
    }
}
