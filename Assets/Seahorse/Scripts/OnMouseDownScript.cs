using UnityEngine;
using System.Collections;

public class OnMouseDownScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	void OnMouseDown(){
		GetComponent<Rigidbody> ().AddForce ((Vector3.forward+Vector3.up)*5f,ForceMode.Impulse);

	}

}
