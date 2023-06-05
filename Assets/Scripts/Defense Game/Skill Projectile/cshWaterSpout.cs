using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshWaterSpout : MonoBehaviour
{
    private bool isStart = false;
    public int damage = 200;
    private float currentTime = 0f;
    private float showTime = 1.2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            if (currentTime == 0f)
            {
                // 범위 내의 모든 적에게 피해를 입히며 기절 상태이상을 부여
                Collider2D[] colls = Physics2D.OverlapBoxAll(transform.position, new Vector2(3.2f, 5.0f), 0);
                for (int i = 0; i < colls.Length; i++)
                {
                    if (colls[i].tag == "Enemy")
                    {
                        int defense = colls[i].transform.parent.GetComponent<cshEnemyAttack>().currentDefensePower;
                        int currentDamage = damage - (damage * defense / 100);
                        if (currentDamage <= 0) currentDamage = 1;
                        colls[i].transform.parent.GetComponent<cshEnemyAttack>().getHarmfulEffect(2);
                        colls[i].transform.parent.GetComponent<cshEnemyAttack>().currentHealthPoint -= currentDamage;
                    }
                    else if (colls[i].tag == "EnemyShelter")
                    {
                        colls[i].transform.parent.GetComponent<cshShelter>().currentDurability -= damage;
                    }
                }
            }
            else if (currentTime >= showTime)
            {
                Destroy(transform.gameObject);
            }
            currentTime += Time.deltaTime;
        }
    }

    public void setDamage(int damage)
    {
        this.damage = damage;
        isStart = true;
    }
}
