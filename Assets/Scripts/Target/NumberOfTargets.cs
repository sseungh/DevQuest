using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberOfTargets : MonoBehaviour
{
    public int Targets = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Incr()
    {
        Targets++;
    }

    public void Decr()
    {
        Targets--;
    }
}
