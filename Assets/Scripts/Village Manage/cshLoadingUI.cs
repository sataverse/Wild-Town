using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cshLoadingUI : MonoBehaviour
{
    private float loadingTime = 3.0f;
    private float currentTime = 0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime > loadingTime)
        {
            Destroy(transform.gameObject);
        }
        transform.GetChild(2).GetComponent<Slider>().value = currentTime / loadingTime;
    }

}
