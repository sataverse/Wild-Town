using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshPlasma : MonoBehaviour
{
    public int damage;
    public float speed = -5f;
    private bool startAttack = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (startAttack)
        {
            // 플라즈마는 적진 끝까지 이동하며 모든 적에게 피해를 입힘
            transform.Translate(new Vector3(speed * Time.deltaTime, 0.0f, 0.0f));
            if(transform.position.x <= -10)
            {
                Destroy(transform.gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            // 충돌체가 들어오는 순간 적일 경우 데미지를 주고 방어력 감소 디버프를 부여
            int defense = collision.transform.parent.GetComponent<cshEnemyAttack>().currentDefensePower;
            int currentDamage = damage - (damage * defense / 100);
            if (currentDamage <= 0) currentDamage = 1;
            collision.transform.parent.GetComponent<cshEnemyAttack>().getHarmfulEffect(1);
            collision.transform.parent.GetComponent<cshEnemyAttack>().currentHealthPoint -= currentDamage;
        }
        else if (collision.tag == "EnemyShelter")
        {
            collision.transform.parent.GetComponent<cshShelter>().currentDurability -= damage;
        }
    }

    public void setDamage(int damage)
    {
        this.damage = damage;
        startAttack = true;
    }
}
