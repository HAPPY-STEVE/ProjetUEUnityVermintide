using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunManager : MonoBehaviour
{
    public float tempsRun = 0f;
    private bool runStart = false; 
    // Start is called before the first frame update
    void Start()
    {
        runStart = true;     
    }

    // Update is called once per frame
    void Update()
    {
        while (runStart)
        {
            tempsRun += Time.deltaTime;
        }
    }
}
