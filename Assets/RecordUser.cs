using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class RecordUser : MonoBehaviour
{
    public GameObject user;

    public string fileName = "elephant_user_pose.txt";
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
            foreach (Transform g in user.GetComponentsInChildren<Transform>())
            {
                writer.WriteLine(ConvertTransformToString(g));
            }
        }
        
    }

    void OnApplicationQuit()
    {
        writer.Close();
    }
}
