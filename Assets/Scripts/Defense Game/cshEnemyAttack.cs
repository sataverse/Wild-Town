using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshEnemyAttack : MonoBehaviour
{
    public GameObject battleManager;
    public GameObject normalProjectile;
    public bool isBoss = false;
    public int maxHealthPoint = 10000;
    public int currentHealthPoint;
    public int originalOffensePower = 100;
    public int currentOffensePower;
    public int originalDefensePower = 5;
    public int currentDefensePower;
    public float moveSpeed = 1f;
    public float normalAttackCoolTime = 0.8f;
    public float normalAttackCurrentTime = 0f;
    public float attackArea = 3f;
    public bool isAttack = false;
    public float[] currentharmfulEffectTime = new float[3] { 0f, 0f, 0f };
    public float[] harmfulduration = new float[3] { 4f, 7f, 2f };
    public float[] currentharmfulImmunityTime = new float[3] { 0f, 0f, 0f };
    public float[] harmfulEffectImmunityTime = new float[3] { 10f, 13f, 9f };

    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        battleManager = GameObject.Find("Battle Manager");
        currentHealthPoint = maxHealthPoint;
        currentOffensePower = originalOffensePower;
        currentDefensePower = originalDefensePower;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealthPoint <= 0)
        {
            if (isBoss)
            {
                battleManager.GetComponent<cshBattle>().endDuel(1);
            }
            else
            {
                battleManager.GetComponent<cshBattle>().energyQuantity += 200;
            }
            Destroy(transform.gameObject);
        }

        Collider2D[] colls = Physics2D.OverlapAreaAll(new Vector2(transform.position.x, transform.position.y - 1), new Vector2(transform.position.x + attackArea, transform.position.y - 1 + attackArea));
        isAttack = false;
        for (int i = 0; i < colls.Length; i++)
        {
            if (colls[i].tag == "Character")
            {
                isAttack = true;
                target = colls[i].transform;
                break;
            }
            else if (colls[i].tag == "MyShelter")
            {
                isAttack = true;
                target = colls[i].transform;
                break;
            }
        }

        // 기절 상태(상태이상 3번)가 아니면 공격 또는 이동
        if(currentharmfulEffectTime[2] > harmfulduration[2] || currentharmfulEffectTime[2] == 0f)
        {
            if (isAttack)
            {
                normalAttackCurrentTime += Time.deltaTime;
                if (normalAttackCoolTime <= normalAttackCurrentTime)
                {
                    if (attackArea < 1.5f)
                    {
                        punch(normalProjectile);
                    }
                    else
                    {
                        shoot(normalProjectile);
                    }
                    normalAttackCurrentTime = 0;
                }
            }
            else
            {
                transform.Translate(new Vector3(moveSpeed * Time.deltaTime, 0.0f, 0.0f));
                normalAttackCurrentTime = 0;
            }
        }
        
        for(int i = 0; i < 3; i++)
        {
            if(currentharmfulEffectTime[i] > 0f)
            {
                currentharmfulEffectTime[i] += Time.deltaTime;
                // 상태이상 시간이 지나면 일정 시간 동안 특정 상태이상에 면역 상태가 됨
                if(currentharmfulEffectTime[i] > harmfulduration[i])
                {
                    cureHarmfulEffect(i);
                    currentharmfulImmunityTime[i] += Time.deltaTime;
                    currentharmfulEffectTime[i] = 0f;
                }
                
            }
            if(currentharmfulImmunityTime[i] > 0f)
            {
                currentharmfulImmunityTime[i] += Time.deltaTime;
                if (currentharmfulImmunityTime[i] > harmfulEffectImmunityTime[i])
                {
                    currentharmfulImmunityTime[i] = 0f;
                }
            }
        }
    }
    public void shoot(GameObject projectile)
    {
        GameObject normalProjectilePrefab = Instantiate(projectile);
        normalProjectilePrefab.GetComponent<cshProjectile>().setAttackDamage(currentOffensePower);
        normalProjectilePrefab.transform.position = transform.GetChild(1).position;
    }

    public void punch(GameObject attackEffect)
    {
        GameObject attackEffectPrefab = Instantiate(attackEffect);

        System.Random r = new System.Random();
        float randX = r.Next(-50, 50) / 100f;
        float randY = r.Next(-50, 50) / 100f;
        attackEffectPrefab.transform.position = new Vector2(target.position.x + randX, target.position.y + randY);
        if(target.tag == "Character")
        {
            target.parent.GetComponent<cshCharacterAttack>().currentHealthPoint -= currentOffensePower;
        }
        else if(target.tag == "MyShelter")
        {
            target.parent.GetComponent<cshShelter>().currentDurability -= currentOffensePower;
        }
    }

    public void getHarmfulEffect(int n)
    {
        // 상태 이상 시작
        if(n == 0 && currentharmfulImmunityTime[n] == 0f) 
        {
            currentOffensePower = 1;
            currentharmfulEffectTime[n] += Time.deltaTime;
            transform.GetChild(3).gameObject.SetActive(true);
        }
        else if (n == 1 && currentharmfulImmunityTime[n] == 0f)
        {
            currentDefensePower = 0;
            currentharmfulEffectTime[n] += Time.deltaTime;
            transform.GetChild(4).gameObject.SetActive(true);
        }
        else if (n == 2 && currentharmfulImmunityTime[n] == 0f)
        {
            currentharmfulEffectTime[n] += Time.deltaTime;
            transform.GetChild(5).gameObject.SetActive(true);
        }
    }

    public void cureHarmfulEffect(int n)
    {
        if (n == 0)
        {
            currentOffensePower = originalOffensePower;
            transform.GetChild(3).gameObject.SetActive(false);
        }
        else if (n == 1)
        {
            currentDefensePower = originalDefensePower;
            transform.GetChild(4).gameObject.SetActive(false);
        }
        else if (n == 2)
        {
            transform.GetChild(5).gameObject.SetActive(false);
        }
    }
}
