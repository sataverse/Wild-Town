using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshProjectile : MonoBehaviour
{
    public bool startAttack = false;
    public float speed = 1f;
    public int damage = 100;
    public Vector2 size;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (startAttack)
        {
            Collider2D coll = null;
            Collider2D []colls = Physics2D.OverlapBoxAll(transform.position, size, 0);
            for(int i = 0; i < colls.Length; i++)
            {
                if(colls[i].tag == "Enemy" && transform.tag == "CharacterProjectile")
                {
                    coll = colls[i];
                    break;
                }
                if (colls[i].tag == "EnemyShelter" && transform.tag == "CharacterProjectile")
                {
                    coll = colls[i];
                    break;
                }
                if (colls[i].tag == "Character" && transform.tag == "EnemyProjectile")
                {
                    coll = colls[i];
                    break;
                }
                if (colls[i].tag == "MyShelter" && transform.tag == "EnemyProjectile")
                {
                    coll = colls[i];
                    break;
                }
                if (colls[i].tag == "Shield" && transform.tag == "EnemyProjectile")
                {
                    coll = colls[i];
                    break;
                }
            }
            if(coll != null)
            {
                if (coll.tag == "Enemy" && transform.tag == "CharacterProjectile")
                {
                    int defense = coll.transform.parent.GetComponent<cshEnemyAttack>().currentDefensePower;
                    int currentDamage = damage - (damage * defense / 100);
                    if (currentDamage <= 0) currentDamage = 1;
                    coll.transform.parent.GetComponent<cshEnemyAttack>().currentHealthPoint -= currentDamage;
                    Destroy(transform.gameObject);
                }
                if (coll.tag == "EnemyShelter" && transform.tag == "CharacterProjectile")
                {
                    coll.transform.parent.GetComponent<cshShelter>().currentDurability -= damage;
                    Destroy(transform.gameObject);
                }
                if (coll.tag == "Character" && transform.tag == "EnemyProjectile")
                {
                    int defense = coll.transform.parent.GetComponent<cshCharacterAttack>().currentDefensePower;
                    int currentDamage = damage - (damage * defense / 100);
                    if (currentDamage <= 0) currentDamage = 1;
                    coll.transform.parent.GetComponent<cshCharacterAttack>().currentHealthPoint -= currentDamage;
                    Destroy(transform.gameObject);
                }
                if (coll.tag == "MyShelter" && transform.tag == "EnemyProjectile")
                {
                    coll.transform.parent.GetComponent<cshShelter>().currentDurability -= damage;
                    Destroy(transform.gameObject);
                }
                if (coll.tag == "Shield" && transform.tag == "EnemyProjectile")
                {
                    coll.transform.GetComponent<cshShield>().increaseCount();
                    Destroy(transform.gameObject);
                }
            }
            transform.Translate(new Vector3(speed * Time.deltaTime, 0.0f, 0.0f));
        }
    }

    public void setAttackDamage(int damage)
    {
        this.damage = damage;
        startAttack = true;
    }
}
