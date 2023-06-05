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

        // ĳ���Ϳ� ���� ��ȯ�� �Ʊ� ������Ʈ(�̴� �Ź�)�� ���� �ð��� ������ �����
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
        // ĳ���� ���� ���� ���� ��� ������Ʈ ����
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
            // ���� ���� ���� ���� ���� 1 �̻��̸� �Ϲ� ���� �� ��ų ���
            normalAttackCurrentTime += Time.deltaTime;
            if (normalAttackCurrentCoolTime <= normalAttackCurrentTime)
            {
                // ���� ������ ª���� �ٰŸ� ���� ��ٸ� �߻�ü �߻�
                if(attackArea < 1.5f)
                {
                    if (isMultiAttack)
                    {
                        // ���� ������ ������ ���¸� ���� ���� ����� ����
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
            // ��ų ��Ÿ���� ������ ��ų ���
            if(specialAttackCoolTime <= specialAttackCurrentTime)
            {
                specialAttackCurrentTime = 0;
                skill();
            }
        }
        else 
        {
            // ���� ���� �ȿ� ���� ���ٸ� �̵�
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

    // ���Ÿ� ����
    public void shoot(GameObject projectile)
    {
        GameObject normalProjectilePrefab = Instantiate(projectile);
        normalProjectilePrefab.GetComponent<cshProjectile>().setAttackDamage(currentOffensePower);
        normalProjectilePrefab.transform.position = transform.GetChild(1).position;
    }

    // �ٰŸ� ����
    public void punch(GameObject attackEffect, int n)
    {
        GameObject attackEffectPrefab = Instantiate(attackEffect);
        if (n == 0)
        {
            // �ٰŸ� ���� ����Ʈ
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
            // ���� ü�� ������ ���� �Ʊ� ġ��
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
                // ��ȣ���� �̹� ������ ���� ���� �� ���� �Ʊ����� ��ȣ�� �ο�
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
