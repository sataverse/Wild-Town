using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshShelter : MonoBehaviour
{
    public GameObject battleManager;
    public GameObject enemyList;
    public GameObject[] enemyPrefabs = new GameObject[3];
    public bool isMyShelter;

    public float maxDurability = 500000;
    public float currentDurability;
    public float spawnEnemyCoolTime = 3.0f;
    public float spawnEnemyCurrentTime = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        currentDurability = maxDurability;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMyShelter)
        {
            if (currentDurability <= 0)
            {
                // 아군 기지의 내구도가 0이되면 패배 처리
                battleManager.GetComponent<cshBattle>().endDuel(2);
                Destroy(transform.gameObject);
            }
        }
        else
        {
            // 적군 기지의 내구도에 따라 소환 속도가 다르며 0이되면 보스를 소환하고 기지는 파괴됨
            if(currentDurability/maxDurability <= 0.0f)
            {
                spawnEnemy(2);
                Destroy(transform.gameObject);
            }
            else if(currentDurability / maxDurability < 0.2f)
            {
                spawnEnemyCoolTime = 0.8f;
            }
            else if(currentDurability / maxDurability < 0.5f)
            {
                spawnEnemyCoolTime = 1.9f;
            }

            spawnEnemyCurrentTime += Time.deltaTime;
            if(spawnEnemyCoolTime <= spawnEnemyCurrentTime)
            {
                System.Random r = new System.Random();
                int rand = r.Next(0, 2);
                spawnEnemy(rand);
                spawnEnemyCurrentTime = 0f;
            }
        }
    }

    public void spawnEnemy(int n)
    {
        GameObject enemy = Instantiate(enemyPrefabs[n]);
        enemy.transform.parent = enemyList.transform;
    }
}
