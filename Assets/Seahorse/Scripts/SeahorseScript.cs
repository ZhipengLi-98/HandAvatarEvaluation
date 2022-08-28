using UnityEngine;
using System.Collections;

public class SeahorseScript : MonoBehaviour {
	Animator seahorseAnimator;

	// Use this for initialization
	void Start () {
		seahorseAnimator = GetComponent<Animator> ();
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Q)) {
			seahorseAnimator.SetTrigger("Q");
		}else if (Input.GetKeyDown(KeyCode.W)) {
			seahorseAnimator.SetTrigger("W");
		}else if (Input.GetKeyDown(KeyCode.E)) {
			seahorseAnimator.SetTrigger("E");
		}else if (Input.GetKeyDown(KeyCode.R)) {
			seahorseAnimator.SetTrigger("R");
		}else if (Input.GetKeyDown(KeyCode.T)) {
			seahorseAnimator.SetTrigger("T");
		}else if (Input.GetKeyDown(KeyCode.Y)) {
			seahorseAnimator.SetTrigger("Y");
		}else if (Input.GetKeyDown(KeyCode.A)) {
			seahorseAnimator.SetTrigger("A");
		}else if (Input.GetKeyDown(KeyCode.S)) {
			seahorseAnimator.SetTrigger("S");
		}else if (Input.GetKeyDown(KeyCode.D)) {
			seahorseAnimator.SetTrigger("D");
		}else if (Input.GetKeyDown(KeyCode.F)) {
			seahorseAnimator.SetTrigger("F");
		}else if (Input.GetKeyDown(KeyCode.G)) {
			seahorseAnimator.SetTrigger("G");
		}else if (Input.GetKeyDown(KeyCode.H)) {
			seahorseAnimator.SetTrigger("H");
		}else if (Input.GetKeyDown(KeyCode.Z)) {
			seahorseAnimator.SetTrigger("Z");
		}else if (Input.GetKeyDown(KeyCode.X)) {
			seahorseAnimator.SetTrigger("X");
		}else if (Input.GetKeyDown(KeyCode.C)) {
			seahorseAnimator.SetTrigger("C");
		}



	}
}
