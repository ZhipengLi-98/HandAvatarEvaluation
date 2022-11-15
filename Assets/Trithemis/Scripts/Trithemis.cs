using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trithemis : MonoBehaviour
{
    private Animator trithemis;
    private bool Idle = true;
    private bool Perched = false;
    // Start is called before the first frame update
    void Start()
    {
        trithemis = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Idle = !Idle;
            Perched = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Perched = !Perched;
            Idle = false;
        }
        if (trithemis.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            trithemis.SetBool("preen", false);
            trithemis.SetBool("takeoff", false);
            trithemis.SetBool("landing", false);
            trithemis.SetBool("walk", false);
        }
        if (trithemis.GetCurrentAnimatorStateInfo(0).IsName("perched"))
        {
            trithemis.SetBool("preen", false);
            trithemis.SetBool("takeoff2", false);
            trithemis.SetBool("landing2", false);
        }
        if (trithemis.GetCurrentAnimatorStateInfo(0).IsName("fly"))
        {
            trithemis.SetBool("preen", false);
            trithemis.SetBool("takeoff", false);
        }
        if (trithemis.GetCurrentAnimatorStateInfo(0).IsName("walk"))
        {
            trithemis.SetBool("turnleft", false);
            trithemis.SetBool("turnright", false);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            trithemis.SetBool("walk", true);
            trithemis.SetBool("idle", false);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            trithemis.SetBool("walk", false);
            trithemis.SetBool("idle", true);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            trithemis.SetBool("backward", true);
            trithemis.SetBool("idle", false);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            trithemis.SetBool("backward", false);
            trithemis.SetBool("idle", true);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            trithemis.SetBool("turnleft", true);
            trithemis.SetBool("flyleft", true);
            trithemis.SetBool("idle", false);
            trithemis.SetBool("fly", false);
            trithemis.SetBool("walkleft", true);
            trithemis.SetBool("walk", false);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            trithemis.SetBool("turnleft", false);
            trithemis.SetBool("flyleft", false);
            trithemis.SetBool("idle", true);
            trithemis.SetBool("fly", true);
            trithemis.SetBool("walkleft", false);
            trithemis.SetBool("walk", true);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            trithemis.SetBool("turnright", true);
            trithemis.SetBool("flyright", true);
            trithemis.SetBool("idle", false);
            trithemis.SetBool("fly", false);
            trithemis.SetBool("walkright", true);
            trithemis.SetBool("walk", false);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            trithemis.SetBool("turnright", false);
            trithemis.SetBool("flyright", false);
            trithemis.SetBool("idle", true);
            trithemis.SetBool("fly", true);
            trithemis.SetBool("walkright", false);
            trithemis.SetBool("walk", true);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            trithemis.SetBool("preen", true);
            trithemis.SetBool("idle", false);
        }
        if (Input.GetKeyDown(KeyCode.Space)&&(Idle==true))
        {
            trithemis.SetBool("idle", false);
            trithemis.SetBool("perched", false);
            trithemis.SetBool("takeoff", true);
            trithemis.SetBool("takeoff2", false);
            trithemis.SetBool("landing", true);
            trithemis.SetBool("landing2", false);
            trithemis.SetBool("fly", false);
            trithemis.SetBool("flyleft", false);
            trithemis.SetBool("flyright", false);
        }
        if (Input.GetKeyDown(KeyCode.Space) && (Perched == true))
        {
            trithemis.SetBool("idle", false);
            trithemis.SetBool("perched", false);
            trithemis.SetBool("takeoff", false);
            trithemis.SetBool("takeoff2", true);
            trithemis.SetBool("landing2", true);
            trithemis.SetBool("landing", false);
            trithemis.SetBool("fly", false);
            trithemis.SetBool("flyleft", false);
            trithemis.SetBool("flyright", false);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            trithemis.SetBool("fly", false);
            trithemis.SetBool("flyleft", false);
            trithemis.SetBool("flyright", false);
            trithemis.SetBool("die", true);
        }
    }
}
