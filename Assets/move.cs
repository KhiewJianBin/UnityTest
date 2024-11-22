using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject a;

    public float Test { get; set; }

    public float Test2 { get; set; }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        dotransform2();
    }
    void dotransform()
    {
        float hi = Mathf.Sin(Time.time);
        float hi2 = Mathf.Cos(Time.time);

        a.transform.position = new Vector3(hi, hi2*1, hi);
    }
    void dotransform2()
    {
        Test2 = Mathf.Sin(Time.time);
        Test2 = Mathf.Cos(Time.time);
        a.transform.position = new Vector3(Test2, 1, Test2);
    }
}
