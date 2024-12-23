using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phone_sys_controller : MonoBehaviour
{
    public GameObject phone;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            phone.SetActive(!phone.active);
            AudioManager.Instance.PlaySound("bo");

        }

    }
}
