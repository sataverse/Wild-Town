using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 물건 생산이 완료되었을때 보여줌
public class cshProduceComplete : MonoBehaviour
{
    private float speed = 0.3f;
    private float currentTime = 0f;
    private float time = 2.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(time < currentTime)
        {
            Destroy(transform.gameObject);
        }
        transform.Translate(new Vector3(0.0f, speed * Time.deltaTime, 0.0f));
        speed -= 0.03f * Time.deltaTime;
    }
}
