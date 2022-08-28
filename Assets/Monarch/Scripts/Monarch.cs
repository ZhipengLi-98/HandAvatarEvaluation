using UnityEngine;
using System.Collections;

public class Monarch : MonoBehaviour
{
    Animator monarch;
    public GameObject MainCamera;
    public float gravity = 1.0f;
    private Vector3 moveDirection = Vector3.zero;
    CharacterController characterController;

    void Start ()
    {
        monarch = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }
	void Update ()
    {
        characterController.Move(moveDirection * Time.deltaTime);
        moveDirection.y = gravity * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.G))
        {
            monarch.SetBool("glide", true);
            monarch.SetBool("fly", false);
        }
        if (Input.GetKeyUp(KeyCode.G))
        {
            monarch.SetBool("fly", true);
            monarch.SetBool("glide", false);
        }
        if ((monarch.GetCurrentAnimatorStateInfo(0).IsName("takeoff"))||(monarch.GetCurrentAnimatorStateInfo(0).IsName("landing")))
        {
            monarch.SetBool("takeoff", false);
            monarch.SetBool("landing", false);
        }
        if ((Input.GetKeyUp(KeyCode.W))||(Input.GetKeyUp(KeyCode.A))||(Input.GetKeyUp(KeyCode.D)))
        {
            monarch.SetBool("idle", true);
            monarch.SetBool("walk", false);
            monarch.SetBool("fly", true);
            monarch.SetBool("eat", false);
            monarch.SetBool("turnleft", false);
            monarch.SetBool("turnright", false);
            monarch.SetBool("flyleft", false);
            monarch.SetBool("flyright", false);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            monarch.SetBool("walk", true);
            monarch.SetBool("idle", false);
            monarch.SetBool("idle2", false);
            monarch.SetBool("fly", true);
            monarch.SetBool("flyinplace", false);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            monarch.SetBool("turnleft", true);
            monarch.SetBool("flyleft", true);
            monarch.SetBool("turnright", false);
            monarch.SetBool("idle", false);
            monarch.SetBool("idle2", false);
            monarch.SetBool("walk", false);
            monarch.SetBool("fly", false);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            monarch.SetBool("turnright", true);
            monarch.SetBool("flyright", true);
            monarch.SetBool("turnleft", false);
            monarch.SetBool("idle", false);
            monarch.SetBool("idle2", false);
            monarch.SetBool("walk", false);
            monarch.SetBool("fly", false);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            monarch.SetBool("takeoff", true);
            monarch.SetBool("landing", true);
            monarch.SetBool("idle", false);
            monarch.SetBool("fly", false);
            monarch.SetBool("eat", false);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            monarch.SetBool("eat", true);
            monarch.SetBool("idle", false);
            monarch.SetBool("idle2", false);
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            monarch.SetBool("eat", false);
            monarch.SetBool("idle", true);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            monarch.SetBool("flyfast", true);
            monarch.SetBool("fly", false);
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            monarch.SetBool("fly", true);
            monarch.SetBool("flyfast", false);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            monarch.SetBool("flyinplace", true);
            monarch.SetBool("fly", false);
        }
        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            MainCamera.GetComponent<CameraFollow>().enabled = false;
        }
        if (Input.GetKeyUp(KeyCode.RightControl))
        {
            MainCamera.GetComponent<CameraFollow>().enabled = true;
        }

	}
}
