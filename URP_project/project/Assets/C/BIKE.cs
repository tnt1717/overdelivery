using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using Unity.VisualScripting;
using UnityEngine;


public class BIKE : MonoBehaviour
{
    RaycastHit hit;
    // Start is called before the first frame update\
    float moveinput, steerinput,rayLength, currentVelicityoffset;
    public LayerMask derivabelSurface;

    public float Maxspeed, accleration, steerStrength, zTiltAngle = 45f,gravity,bikeXtiltIncrement=0.9f, handleRotVal=30f,habdleRotSpeed=.15f;
    [Range (1,10)]
    public float breakingFactor;
    public GameObject handle;
    

    [HideInInspector] public Vector3 velocity;
    public Rigidbody sphereRB, bikebody;
    public Animator PlayerAnimator;

    void Start()
    {
        sphereRB.transform.parent = null;
        bikebody.transform.parent = null;
        rayLength = sphereRB.GetComponent<SphereCollider>().radius + 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        moveinput = Input.GetAxis("Vertical");
        steerinput = Input.GetAxis("Horizontal");
        if (steerinput > 0.5f)
        {
            PlayerAnimator.SetBool("L_pan", true);
        }
        else PlayerAnimator.SetBool("L_pan", false);

        if (steerinput < -0.5f)
        {
            PlayerAnimator.SetBool("R_pan", true);

        }
        else PlayerAnimator.SetBool("R_pan", false);

        transform.position = sphereRB.transform.position;

        
        velocity = bikebody.transform.InverseTransformDirection(bikebody.linearVelocity);
        currentVelicityoffset = velocity.z / Maxspeed;
        bikebody.MoveRotation(transform.rotation);
        
        
    }
    private void FixedUpdate()
    {
        Movement();
        
        //bikeTile();
    }
    void Movement() {
        if (Grounded())
        {
            if (!Input.GetKey(KeyCode.Space))
            {
                Acceleration();
                rotation();
            }
            Brake();
        }
        else
        {
            Gravity();
        }

    }


    void Acceleration()
    {
        sphereRB.linearVelocity = Vector3.Lerp(sphereRB.linearVelocity, Maxspeed * moveinput * transform.forward, Time.fixedDeltaTime * accleration);
    }
    void rotation()
    {
        transform.Rotate(0, steerinput * steerStrength * Time.fixedDeltaTime, 0, Space.World);

        handle.transform.localRotation = Quaternion.Slerp(handle.transform.localRotation, Quaternion.Euler(handle.transform.localRotation.eulerAngles.x, handleRotVal * steerinput, handle.transform.localRotation.eulerAngles.z), habdleRotSpeed);
    }

    void Brake() {
        if (Input.GetKey(KeyCode.Space)) { 
            sphereRB.linearVelocity *= breakingFactor/ 10;
        }
    }

    bool Grounded() {

        if (Physics.Raycast(sphereRB.position, Vector3.down, out hit, rayLength, derivabelSurface)){ 
            return true;
        }
        else { return false; }
    }
    void Gravity() {
        sphereRB.AddForce(gravity * Vector3.down, ForceMode.Acceleration);
    }


    void bikeTile()
    {
        float xRot = (Quaternion.FromToRotation(bikebody.transform.up,hit.normal) * bikebody.transform.rotation).eulerAngles.x;
        float zRot = 0;

//if (currentVelicityoffset > 0) 
        zRot = -zTiltAngle * steerinput * currentVelicityoffset;

        Quaternion targetRot = Quaternion.Slerp(bikebody.transform.rotation, Quaternion.Euler(xRot, transform.eulerAngles.y, zRot), bikeXtiltIncrement);
        Quaternion newRotation = Quaternion.Euler(targetRot.eulerAngles.x, transform.eulerAngles.y, targetRot.eulerAngles.z);
        bikebody.MoveRotation(newRotation);
    }
    
}
