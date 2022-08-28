using UnityEngine;
using System.Collections;

public class SwanUserControllerScript : MonoBehaviour {
	public SwanCharacterScript swanCharacter;
	
	void Start () {
		swanCharacter = GetComponent<SwanCharacterScript> ();	
	}
	
	void Update(){
		if (Input.GetKeyDown(KeyCode.L)) {
			swanCharacter.Landing();
		}
		if (Input.GetKeyDown(KeyCode.N)) {
			swanCharacter.Down();
		}
		if (Input.GetKeyDown(KeyCode.U)) {
			swanCharacter.Rebirth();
		}
		if (Input.GetKeyDown(KeyCode.H)) {
			swanCharacter.Hit();
		}
		if (Input.GetKeyDown(KeyCode.G)) {
			swanCharacter.Grooming();
		}


		if (Input.GetButtonDown ("Fire1")) {
			swanCharacter.Attack();
		}
		if (Input.GetButtonDown ("Jump")) {
			swanCharacter.Soar ();
		}

	}
	
	void FixedUpdate(){
		float v = Input.GetAxis ("Vertical");
		float h = Input.GetAxis ("Horizontal");
		swanCharacter.Move (v,h);		
	}
}
