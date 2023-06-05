using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class cshCharacterAttack : MonoBehaviour
{
    public GameObject battleManager;
    public int characterCode;
    public GameObject normalAttack;
    public GameObject specialAttack;

    public float spawnTime = 0f;
    public float despawnTime = 3f;
    public int maxHealthPoint = 10000;
    public int currentHealthPoint;
    public int originalOffensePower = 100;
    public int currentOffensePower;
    public int originalDefensePower = 10;
    public int currentDefensePower;
    public float moveSpeed = 1f;
    public float normalAttackOriginalCoolTime = 0.8f;
    public float normalAttackCurrentCoolTime;
    public float normalAttackCurrentTime = 0f;
    public float specialAttackCoolTime = 10f;
    public float specialAttackCurrentTime = 0f;
    public float attackArea = 5f;
    public bool isMultiAttack = false;
    public bool isSpawnCreature = false;

    private bool isStart = false;
    private bool isAttack = false;
    private float[] levelOP = { 0, 0.1f, 0.2f, 0.4f, 0.7f, 1.0f };
    private int[] levelDP = { 0, 2, 5, 10, 15, 20 };
    private float[] levelAS = { 0, 0.01f, 0.02f, 0.04f, 0.07f, 0.1f };
    private float[] levelHP = { 0, 0.1f, 0.2f, 0.4f, 0.7f, 1.0f };
    private Vector2 enemyPosition;
    private Transform []enemy;

    // Start is called before the first frame update
    void Start()
    {
        battleManager = GameObject.Find("Battle Manager");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStart) return;

        // 캐릭터에 의해 소환된 아군 오브젝트(미니 거미)는 일정 시간이 지나면 사라짐
        if (isSpawnCreature)
        {
            spawnTime += Time.deltaTime;
        }

        if(currentHealthPoint <= 0)
        {
            if(!isSpawnCreature) battleManager.GetComponent<cshBattle>().currentCombatant--;
            Destroy(transform.gameObject);
        }
        if (despawnTime <= spawnTime && isSpawnCreature)
        {
            Destroy(transform.gameObject);
        }

        int count = 0;
        // 캐릭터 공격 범위 내의 모든 오브젝트 저장
        Collider2D[] colls = Physics2D.OverlapAreaAll(new Vector2(transform.position.x, transform.position.y - 1), new Vector2(transform.position.x - attackArea, transform.position.y - 1 + attackArea));
        isAttack = false;
        for(int i = 0; i < colls.Length; i++)
        {
            if(colls[i].tag == "Enemy" || colls[i].tag == "EnemyShelter")
            {
                count++;
            }
        }

        if (count >= 1) isAttack = true;
        enemy = new Transform[count];

        count = 0;
        for (int i = 0; i < colls.Length; i++)
        {
            if (colls[i].tag == "Enemy" || colls[i].tag == "EnemyShelter")
            {
                if (count == 0) enemyPosition = new Vector2(colls[i].transform.position.x, colls[i].transform.position.y);
                enemy[count++] = colls[i].transform;
            }
        }
        specialAttackCurrentTime += Time.deltaTime;

        if (isAttack)
        {
            // 공격 범위 내의 적의 수가 1 이상이면 일반 공격 및 스킬 사용
            normalAttackCurrentTime += Time.deltaTime;
            if (normalAttackCurrentCoolTime <= normalAttackCurrentTime)
            {
                // 공격 범위가 짧으면 근거리 공격 길다면 발사체 발사
                if(attackArea < 1.5f)
                {
                    if (isMultiAttack)
                    {
                        // 다중 공격이 가능한 상태면 범위 내의 모든적 공격
                        for(int i = 0; i < enemy.Length; i++)
                        {
                            punch(normalAttack, i);
                        }
                    }
                    else
                    {
                        punch(normalAttack, 0);
                    }
                }
                else
                {
                    shoot(normalAttack);
                }
                normalAttackCurrentTime = 0;
            }
            // 스킬 쿨타임이 지나면 스킬 사용
            if(specialAttackCoolTime <= specialAttackCurrentTime)
            {
                specialAttackCurrentTime = 0;
                skill();
            }
        }
        else 
        {
            // 공격 범위 안에 적이 없다면 이동
            transform.Translate(new Vector3(moveSpeed * Time.deltaTime, 0.0f, 0.0f));
            normalAttackCurrentTime = 0;
        }
    }

    public void startSetting(int level)
    {
        originalOffensePower += (int)(originalOffensePower * levelOP[level]);
        originalDefensePower += levelDP[level];
        normalAttackOriginalCoolTime -= levelAS[level];
        maxHealthPoint += (int)(maxHealthPoint * levelHP[level]);
        
        currentHealthPoint = maxHealthPoint;
        if (currentOffensePower == 0) currentOffensePower = originalOffensePower;
        currentDefensePower = originalDefensePower;
        normalAttackCurrentCoolTime = normalAttackOriginalCoolTime;
        isStart = true;
    }

    // 원거리 공격
    public void shoot(GameObject projectile)
    {
        GameObject normalProjectilePrefab = Instantiate(projectile);
        normalProjectilePrefab.GetComponent<cshProjectile>().setAttackDamage(currentOffensePower);
        normalProjectilePrefab.transform.position = transform.GetChild(1).position;
    }

    // 근거리 공격
    public void punch(GameObject attackEffect, int n)
    {
        GameObject attackEffectPrefab = Instantiate(attackEffect);
        if (n == 0)
        {
            // 근거리 공격 이펙트
            System.Random r = new System.Random();
            float randX = r.Next(-50, 50) / 100f;
            float randY = r.Next(-50, 50) / 100f;
            attackEffectPrefab.transform.position = new Vector2(enemy[n].position.x + randX, enemy[n].position.y + randY);
        }
        if(enemy[n].tag == "Enemy")
        {
            enemy[n].parent.GetComponent<cshEnemyAttack>().currentHealthPoint -= currentOffensePower;
        }
        else if(enemy[n].tag == "EnemyShelter")
        {
            enemy[n].parent.GetComponent<cshShelter>().currentDurability -= currentOffensePower;
        }
        
    }

    public void skill()
    {
        if(characterCode == 1)
        {
            GameObject skillProjectilePrefab = Instantiate(specialAttack);
            skillProjectilePrefab.transform.position = transform.GetChild(1).position;
            skillProjectilePrefab.GetComponent<cshInk>().setFallPosition(transform.position.x, enemyPosition.x, (int)(currentOffensePower * 2.5f));
        }
        else if(characterCode == 2)
        {
            // 가장 체력 비율이 적은 아군 치유
            GameObject charList = battleManager.GetComponent<cshBattle>().characterList;
            float min = charList.transform.GetChild(0).GetComponent<cshCharacterAttack>().currentHealthPoint / charList.transform.GetChild(0).GetComponent<cshCharacterAttack>().maxHealthPoint;
            int idx = 0;
            for (int i=1;i < charList.transform.childCount; i++)
            {
                float temp = charList.transform.GetChild(i).GetComponent<cshCharacterAttack>().currentHealthPoint / charList.transform.GetChild(i).GetComponent<cshCharacterAttack>().maxHealthPoint;
                if(temp < min)
                {
                    min = temp;
                    idx = i;
                }
            }
            GameObject skillProjectilePrefab = Instantiate(specialAttack);
            skillProjectilePrefab.transform.parent = charList.transform.GetChild(idx);
            skillProjectilePrefab.transform.position = charList.transform.GetChild(idx).position;
            skillProjectilePrefab.GetComponent<cshHeal>().heal();
        }
        else if (characterCode == 3)
        {
            GameObject skillProjectilePrefab = Instantiate(specialAttack);
            skillProjectilePrefab.transform.position = new Vector2(transform.GetChild(1).position.x, transform.GetChild(1).position.y + 0.3f);
            skillProjectilePrefab.transform.parent = transform;
            skillProjectilePrefab.GetComponent<cshHurricane>().startSpin();
        }
        else if (characterCode == 4)
        {
            GameObject skillProjectilePrefab = Instantiate(specialAttack);
            skillProjectilePrefab.transform.position = transform.GetChild(1).position;
            skillProjectilePrefab.GetComponent<cshPlasma>().setDamage(currentOffensePower * 2);
        }
        else if(characterCode == 5)
        {
            GameObject skillProjectilePrefab = Instantiate(specialAttack);
            skillProjectilePrefab.transform.parent = transform;
            skillProjectilePrefab.transform.position = transform.GetChild(1).position;
            skillProjectilePrefab.GetComponent<CshStone>().setDamage(currentOffensePower * 2);
        }
        else if(characterCode == 6)
        {
            GameObject skillProjectilePrefab = Instantiate(specialAttack);
            skillProjectilePrefab.transform.position = new Vector2(enemyPosition.x, 0.65f);
            skillProjectilePrefab.GetComponent<cshWaterSpout>().setDamage(currentOffensePower * 3);
        }
        else if(characterCode == 7)
        {
            GameObject charList = battleManager.GetComponent<cshBattle>().characterList;
            int idx = -1;

            for (int i = 0; i < charList.transform.childCount; i++)
            {
                // 보호막을 이미 가지고 있지 않은 맨 앞의 아군에게 보호막 부여
                if(charList.transform.GetChild(i).childCount > 3)
                {
                    bool flag = false;
                    for(int j=2;j< charList.transform.GetChild(i).childCount; j++)
                    {
                        if(charList.transform.GetChild(i).transform.GetChild(j).name.Split("(")[0] == "Shield")
                        {
                            flag = true;
                        }
                    }
                    if (!flag)
                    {
                        if (idx == -1)
                        {
                            idx = i;
                        }
                        else
                        {
                            idx = (charList.transform.GetChild(i).transform.position.x > charList.transform.GetChild(idx).transform.position.x) ? idx : i;
                        }
                    } 
                }
                else
                {
                    if(idx == -1)
                    {
                        idx = i;
                    }
                    else
                    {
                        idx = (charList.transform.GetChild(i).transform.position.x > charList.transform.GetChild(idx).transform.position.x) ? idx : i;
                    }
                }
            }

            if (idx == -1)
            {
                specialAttackCurrentTime = specialAttackCoolTime;
            }
            else
            {
                GameObject skillProjectilePrefab = Instantiate(specialAttack);
                skillProjectilePrefab.transform.parent = charList.transform.GetChild(idx);
                skillProjectilePrefab.transform.position = charList.transform.GetChild(idx).position;
            }
        }
        else if(characterCode == 8)
        {
            GameObject skillProjectilePrefab = Instantiate(specialAttack);
            skillProjectilePrefab.transform.parent = transform;
            skillProjectilePrefab.transform.position = transform.GetChild(1).position;
            skillProjectilePrefab.GetComponent<cshMiniSpider>().setStart();
        }
        else if(characterCode == 9)
        {
            GameObject skillProjectilePrefab = Instantiate(specialAttack);
            skillProjectilePrefab.transform.position = enemyPosition;
            skillProjectilePrefab.GetComponent<cshSword>().setDamage(currentOffensePower * 8);
        }
        else if (characterCode == 10)
        {
            GameObject skillProjectilePrefab = Instantiate(specialAttack);
            skillProjectilePrefab.transform.position = new Vector2(transform.GetChild(1).position.x, 0.3f);
            skillProjectilePrefab.GetComponent<cshBlaze>().setDamage(currentOffensePower * 6);
        }
    }
}
