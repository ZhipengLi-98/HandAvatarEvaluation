using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecordAvatar : MonoBehaviour
{
    public bool flag  = false;
    private bool replay = false;
    private int cnt = 0;

    public List<GameObject> avatarRecord = new List<GameObject>();
    public List<GameObject> avatarTempRecord = new List<GameObject>();
    public GameObject avatar;
    public GameObject avatarMesh;
    private string avatarMeshName = "";

    public TextMeshProUGUI text;

    public List<Dictionary<string, Quaternion>> poses = new List<Dictionary<string, Quaternion>>();

    // Start is called before the first frame update
    void Start()
    {
        avatarMeshName = avatarMesh.transform.name;
        text.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (flag && Input.GetKeyDown(KeyCode.W))
        {
            text.text = "";
            flag = false;
        }
        if (!flag && Input.GetKeyDown(KeyCode.Q))
        {
            text.text = "Recording";
            flag = true;
            if (flag)
            {
                foreach (GameObject temp in avatarTempRecord)
                {
                    temp.SetActive(true);
                    Destroy(temp);
                }
                avatarTempRecord.Clear();
                poses.Clear();
            }
        }
        if (avatarTempRecord.Count > 0 && avatarTempRecord[avatarTempRecord.Count - 1].activeSelf)
        {
            avatarTempRecord[avatarTempRecord.Count - 1].SetActive(false);
        }
        if (flag)
        {
            GameObject copyAvatar = Instantiate(avatar);
            copyAvatar.transform.position = avatar.transform.position;
            copyAvatar.transform.rotation = avatar.transform.rotation;
            copyAvatar.transform.Find(avatarMeshName).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;

            avatarTempRecord.Add(copyAvatar);

            // Dictionary<string, Quaternion> temp = new Dictionary<string, Quaternion>();
            // foreach (Transform g in avatar.transform.GetComponentsInChildren<Transform>())
            // {
            //     temp.Add(g.name, g.localRotation);
            // }
            // poses.Add(temp);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            text.text = "Saved";
            foreach (GameObject temp in avatarRecord)
            {
                temp.SetActive(true);
                Destroy(temp);
            }
            avatarRecord.Clear();
            foreach (GameObject temp in avatarTempRecord)
            {
                avatarRecord.Add(Instantiate(temp));
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            text.text = "Replay";
            replay = !replay;  
            cnt = 0;
            foreach (GameObject temp in avatarRecord)
            {
                temp.transform.Find(avatarMeshName).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;
                temp.SetActive(false);
            }
            if (replay)
            {
                avatar.SetActive(false);
            }
        }        
        if (replay && cnt < avatarRecord.Count)
        {
            avatarRecord[cnt].SetActive(false);
            cnt += 1;
            if (cnt > avatarRecord.Count - 1)
            {
                text.text = "";
                replay = false;
                avatar.SetActive(true);
                cnt = 0;
            }
            else
            {
                avatarRecord[cnt].SetActive(true);
            }
        }
    }
}
