using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordAvatar : MonoBehaviour
{
    private bool flag  = false;
    private bool replay = false;
    private int cnt = 0;

    public List<GameObject> avatarRecord;
    public GameObject avatar;
    public GameObject avatarMesh;
    private string avatarMeshName = "";

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
        avatarMeshName = avatarMesh.transform.name;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            flag = !flag;
            if (flag)
            {
                foreach (GameObject temp in avatarRecord)
                {
                    Destroy(temp);
                }
                avatarRecord.Clear();
            }
        }
        if (avatarRecord.Count > 0 && avatarRecord[avatarRecord.Count - 1].activeSelf)
        {
            avatarRecord[avatarRecord.Count - 1].SetActive(false);
        }
        if (flag)
        {
            GameObject copyAvatar = Instantiate(avatar);
            copyAvatar.transform.position = avatar.transform.position;
            copyAvatar.transform.rotation = avatar.transform.rotation;
            copyAvatar.GetComponent<ElephantMapping>().enabled = false;
            copyAvatar.transform.Find(avatarMeshName).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;

            avatarRecord.Add(copyAvatar);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            replay = !replay;  
            cnt = 0;
            foreach (GameObject temp in avatarRecord)
            {
                temp.transform.Find(avatarMeshName).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;
                temp.SetActive(false);
            }
        }        
        if (replay)
        {
            avatarRecord[cnt].SetActive(false);
            cnt += 1;
            if (cnt > avatarRecord.Count - 1)
            {
                replay = false;
                cnt = 0;
            }
            else
            {
                avatarRecord[cnt].SetActive(true);
            }
        }
    }
}
