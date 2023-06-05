using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshHurricane : MonoBehaviour
{
    private bool isStart = false;
    private float currentTime = 0f;
    private float showTime = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            currentTime += Time.deltaTime;
            if(currentTime >= showTime)
            {
                endSpin();
            }
        }
    }

    public void startSpin()
    {
        // ĳ���͸� �����ϰ� ����� �㸮���θ� ���̰� ��
        // �������� ��� ���� �����ϰ� ���� ����
        isStart = true;
        transform.parent.GetChild(0).GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 0);
        transform.parent.GetComponent<cshCharacterAttack>().isMultiAttack = true;
        transform.parent.GetComponent<cshCharacterAttack>().normalAttackCurrentCoolTime = 0.2f;
        transform.parent.GetComponent<cshCharacterAttack>().currentDefensePower = 40;
    }

    private void endSpin()
    {
        transform.parent.GetChild(0).GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        transform.parent.GetComponent<cshCharacterAttack>().isMultiAttack = false;
        transform.parent.GetComponent<cshCharacterAttack>().normalAttackCurrentCoolTime = transform.parent.GetComponent<cshCharacterAttack>().normalAttackOriginalCoolTime;
        transform.parent.GetComponent<cshCharacterAttack>().currentDefensePower = transform.parent.GetComponent<cshCharacterAttack>().originalDefensePower;
        Destroy(transform.gameObject);
    }
}
