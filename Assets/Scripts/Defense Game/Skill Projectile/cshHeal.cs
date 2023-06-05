using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshHeal : MonoBehaviour
{
    private float currentTime = 0f;
    private float showTime = 0.5f;
    public int healPoint = 200;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(showTime <= currentTime)
        {
            Destroy(transform.gameObject);
        }
    }

    public void heal()
    {
        int max = transform.parent.GetComponent<cshCharacterAttack>().maxHealthPoint;
        transform.parent.GetComponent<cshCharacterAttack>().currentHealthPoint += healPoint;
        if(transform.parent.GetComponent<cshCharacterAttack>().currentHealthPoint > max)
        {
            transform.parent.GetComponent<cshCharacterAttack>().currentHealthPoint = max;
        }
    }
}
