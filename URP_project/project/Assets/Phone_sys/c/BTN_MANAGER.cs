using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTN_MANAGER : MonoBehaviour
{
    public GameObject oneui;
    public GameObject achui;
    public GameObject setui;
    public GameObject twoui;
    public GameObject threeui;
    // Start is called before the first frame update
    void Start()
    {
        oneui.SetActive(true);
        achui.SetActive(false);
        setui.SetActive(false);
        twoui.SetActive(false);
        threeui.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void one()
    {
        AudioManager.Instance.PlaySound("tap2");
        oneui.SetActive(true);
        achui.SetActive(false);
        setui.SetActive(false);
        twoui.SetActive(false);
        threeui.SetActive(false);

    }
    public void two()
    {
        AudioManager.Instance.PlaySound("tap2");
        twoui.SetActive(true);
        oneui.SetActive(false);
        achui.SetActive(false);
        setui.SetActive(false);
        threeui.SetActive(false);

    }
    public void ach()
    {
        AudioManager.Instance.PlaySound("tap2");
        achui.SetActive(true);
        oneui.SetActive(false);
        setui.SetActive(false);
        twoui.SetActive(false); 
        threeui.SetActive(false);
    }
    public void set()
    {
        AudioManager.Instance.PlaySound("tap2");
        setui.SetActive(true);
        achui.SetActive(false);
        oneui.SetActive(false);
        twoui.SetActive(false);
        threeui.SetActive(false);
    }
    public void three() 
    {
        AudioManager.Instance.PlaySound("tap2");
        setui.SetActive(false);
        achui.SetActive(false);
        oneui.SetActive(false);
        twoui.SetActive(false);
        threeui.SetActive(true);


    }
}
