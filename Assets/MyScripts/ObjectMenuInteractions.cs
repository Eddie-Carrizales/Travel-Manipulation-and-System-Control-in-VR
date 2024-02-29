using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMenuInteractions : MonoBehaviour
{
    private GameObject object1;
    private GameObject object2;
    private GameObject object3;
    private GameObject object4;
    private GameObject object5;

    private bool is_selected;
    
    // Start is called before the first frame update
    void Start()
    {
        object1 = GameObject.Find("Unique_Object_1");
        object2 = GameObject.Find("Unique_Object_2");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Write the public menu functions here
    //show
    //hide
    //copy
    //cut
    //exit
}
