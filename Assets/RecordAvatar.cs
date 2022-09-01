using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordAvatar : MonoBehaviour
{
    public bool flag  = false;
    private bool replay = false;
    private int cnt = 0;

    public PlayAnimation player;

    public List<GameObject> avatarRecord = new List<GameObject>();
    public GameObject avatar;
    // public GameObject avatarMesh;
    private string avatarMeshName = "";

    public List<List<Transform>> poses = new List<List<Transform>>();

    // Start is called before the first frame update
    void Start()
    {
        // avatarMeshName = avatarMesh.transform.name;
    }

    // Update is called once per frame
    void Update()
    {
        if (flag && player.animator.GetCurrentAnimatorClipInfo(0).Length == 0)
        {
            flag = false;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            flag = true;
            if (flag)
            {
                foreach (GameObject temp in avatarRecord)
                {
                    Destroy(temp);
                }
                avatarRecord.Clear();
                poses.Clear();
            }
        }
        if (avatarRecord.Count > 0 && avatarRecord[avatarRecord.Count - 1].activeSelf)
        {
            avatarRecord[avatarRecord.Count - 1].SetActive(false);
        }
        if (flag)
        {
            // GameObject copyAvatar = Instantiate(avatar);
            // copyAvatar.transform.position = avatar.transform.position;
            // copyAvatar.transform.rotation = avatar.transform.rotation;
            // copyAvatar.GetComponent<SwanBodyMapping>().enabled = false;
            // copyAvatar.transform.Find(avatarMeshName).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;

            // avatarRecord.Add(copyAvatar);
            List<Transform> temp = new List<Transform>();
            foreach (Transform g in avatar.transform.GetComponentsInChildren<Transform>())
            {
                if (player.controlledJoints.Contains(g.name))
                {
                    temp.Add(g);
                }
            }
            poses.Add(temp);
        }
        // if (Input.GetKeyDown(KeyCode.W))
        // {
        //     replay = !replay;  
        //     cnt = 0;
        //     foreach (GameObject temp in avatarRecord)
        //     {
        //         temp.transform.Find(avatarMeshName).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;
        //         temp.SetActive(false);
        //     }
        //     if (replay)
        //     {
        //         avatar.SetActive(false);
        //     }
        // }        
        // if (replay && cnt < avatarRecord.Count)
        // {
        //     avatarRecord[cnt].SetActive(false);
        //     cnt += 1;
        //     if (cnt > avatarRecord.Count - 1)
        //     {
        //         replay = false;
        //         avatar.SetActive(true);
        //         cnt = 0;
        //     }
        //     else
        //     {
        //         avatarRecord[cnt].SetActive(true);
        //     }
        // }
    }
}
