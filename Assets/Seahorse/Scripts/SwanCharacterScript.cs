using UnityEngine;
using System.Collections;

public class SwanCharacterScript : MonoBehaviour {
	public Animator swanAnimator;
	public float swanSpeed=1f;
	Rigidbody swanRigid;
	public bool isFlying=false;
	
	
	public float rotateSpeed=.2f;
	
	
	void Start () {
		swanAnimator = GetComponent<Animator> ();
		swanAnimator.speed = swanSpeed;
		swanRigid = GetComponent<Rigidbody> ();
		if (isFlying) {
			swanAnimator.SetTrigger ("Soar");
			swanAnimator.applyRootMotion = false;
			isFlying = true;
		}
	}	
	
	public void SpeedSet(float animSpeed){
		swanAnimator.speed = animSpeed;
	}
	
	public void Landing(){
		if (isFlying) {
			swanAnimator.SetTrigger ("Landing");
			swanAnimator.applyRootMotion = true;
			isFlying = false;
		}
	}
	
	public void Soar(){
		if (!isFlying) {
			swanAnimator.SetTrigger ("Soar");
			swanAnimator.applyRootMotion = false;
			isFlying = true;
		}
	}
	public void Hit(){
		swanAnimator.SetTrigger ("Hit");
	}


	public void Attack(){
		swanAnimator.SetTrigger ("Attack");
	}

	public void Down(){
		swanAnimator.SetTrigger ("Down");
	}

	public void Rebirth(){
		swanAnimator.SetTrigger ("Rebirth");
	}

	public void Grooming(){
		swanAnimator.SetTrigger ("Grooming");
	}
	
	public void Move(float v,float h){
		swanAnimator.SetFloat ("Forward",v);
		swanAnimator.SetFloat ("Turn",h);
		if(isFlying) {
			if (v > 0.1f) {
				swanRigid.AddForce ((transform.forward * 5f +transform.up*10f)* v);
			}else if(v<0.1f) {
				swanRigid.AddForce ((transform.forward * 2f +transform.up * 11f) * (-v));
			}else{
				swanRigid.AddForce (transform.up * 9f);
			}
			
			swanRigid.AddTorque(transform.up*h*rotateSpeed);
			
		}
	}
}
