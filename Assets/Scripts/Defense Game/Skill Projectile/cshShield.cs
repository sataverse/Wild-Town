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
        // ���� ���Ÿ� ������ 2ȸ ����ϰų� �ð��� ������ �����
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
