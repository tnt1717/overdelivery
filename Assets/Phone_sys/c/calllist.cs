using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class calllist : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void clk()
    {

        Debug.Log(this.gameObject.name);

        list.dataList[0].Remove(0);
        Debug.Log(list.dataList[0]);
        Debug.Log(list.dataList[1]);
    }
}
