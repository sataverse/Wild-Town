using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshPunch : MonoBehaviour
{
    private float currentTime = 0f;
    private float showTime = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (showTime <= currentTime)
        {
            Destroy(transform.gameObject);
        }
    }
}
