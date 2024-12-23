using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject UI_riding;
    public GameObject UI_unriding;

    // Start is called before the first frame update
    void Start()
    {
        UI_riding.gameObject.SetActive(false);
        UI_unriding.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (PlayerController.isRiding)
        {
            UI_riding.SetActive(true);
            UI_unriding.SetActive(false);
        }
        else if(PlayerController.isRiding==false)
        {
            UI_riding.SetActive(false);
            UI_unriding.SetActive(true);
        }
    }
}
