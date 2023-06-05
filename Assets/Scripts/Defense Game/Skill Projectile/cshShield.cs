using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshShield : MonoBehaviour
{
    private float currentTime = 0f;
    private float showTime = 2f;
    private float count = 0;
    private float maxCount = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 방어막은 원거리 공격을 2회 방어하거나 시간이 지나면 사라짐
        if (count >= maxCount)
        {
            Destroy(transform.gameObject);
        }
        currentTime += Time.deltaTime;
        if (currentTime >= showTime)
        {
            Destroy(transform.gameObject);
        }
    }

    public void increaseCount()
    {
        count++;
    }
}
