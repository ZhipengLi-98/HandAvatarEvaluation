using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetKinect : MonoBehaviour
{
    private KinectManager manager;
	private GameObject[] joints = null;
	public GameObject jointPrefab;
    public int playerIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        manager = KinectManager.Instance;
		if(manager && manager.IsInitialized())
		{
			int jointsCount = manager.GetJointCount();

			if(jointPrefab)
			{
				// array holding the skeleton joints
				joints = new GameObject[jointsCount];
				
				for(int i = 0; i < joints.Length; i++)
				{
					joints[i] = Instantiate(jointPrefab) as GameObject;
					// joints[i].transform.parent = transform;
					joints[i].name = ((KinectInterop.JointType)i).ToString();
					joints[i].SetActive(false);
				}
			}
		}
        
    }

    // Update is called once per frame
    void Update()
    {
		KinectManager manager = KinectManager.Instance;
        if(manager && manager.IsInitialized())
        {
			if(manager.IsUserDetected(playerIndex))
			{
				long userId = manager.GetUserIdByIndex(playerIndex);
				int jointsCount = manager.GetJointCount();

				for(int i = 0; i < jointsCount; i++)
				{
					int joint = i;

					if(manager.IsJointTracked(userId, joint))
					{
						Vector3 posJoint = manager.GetJointPosition(userId, joint);
						
						if(joints != null)
						{
							// overlay the joint
							if(posJoint != Vector3.zero)
							{
								joints[i].SetActive(true);
								joints[i].transform.position = posJoint;

								Quaternion rotJoint = manager.GetJointOrientation(userId, joint, true);
								// rotJoint = initialRotation * rotJoint;
								joints[i].transform.rotation = rotJoint;
							}
							else
							{
								joints[i].SetActive(false);
							}
						}
                    }
                    else
                    {
						if(joints != null)
						{
							joints[i].SetActive(false);
						}
                    }
                }
            }
        }
    }
}
