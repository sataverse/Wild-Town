using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshInk : MonoBehaviour
{
    public bool startShoot = false;
    public float speed = 1f;
    public int damage = 200;
    private float middlePoint;
    private bool isPop = false;
    private float currentTime = 0f;
    private float showTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 목표물 까지 거리의 절반보다 현재 이동 거리가 작다면 왼쪽 위로 이동 크다면 왼쪽 아래로 이동
        if(middlePoint > transform.position.x && startShoot && !isPop)
        {
            transform.Translate(new Vector3(speed * Time.deltaTime, speed * Time.deltaTime, 0.0f));
        }
        else if (middlePoint <= transform.position.x && startShoot && !isPop)
        {
            transform.Translate(new Vector3(speed * Time.deltaTime, -speed * Time.deltaTime, 0.0f));
        }

        // 먹물이 떨어지다가 바닥 위치에 도달했다면 터뜨리기
        if(transform.position.y <= -0.9f && !isPop)
        {
            isPop = true;
        }
        
        if(isPop)
        {
            if(currentTime == 0f)
            {
                transform.GetChild(0).gameObject.SetActive(false); // 먹물 터지기 전 오브젝트
                transform.GetChild(1).gameObject.SetActive(true); // 터진 먹물 오브젝트
                // 먹물이 터진 위치에서 범위안의 적들에게 데미지를 입히며 실명 상태를 부여 (기본 공격의 피해량을 1로 변경)
                Collider2D[] colls = Physics2D.OverlapBoxAll(transform.position, new Vector2(3.0f, 3.0f), 0);
                for (int i = 0; i < colls.Length; i++)
                {
                    if (colls[i].tag == "Enemy")
                    {
                        int defense = colls[i].transform.parent.GetComponent<cshEnemyAttack>().currentDefensePower;
                        int currentDamage = damage - (damage * defense / 100);
                        if (currentDamage <= 0) currentDamage = 1;
                        colls[i].transform.parent.GetComponent<cshEnemyAttack>().getHarmfulEffect(0);
                        colls[i].transform.parent.GetComponent<cshEnemyAttack>().currentHealthPoint -= currentDamage;
                    }
                    else if (colls[i].tag == "EnemyShelter")
                    {
                        colls[i].transform.parent.GetComponent<cshShelter>().currentDurability -= damage;
                    }
                }
            }
            else
            {
                if(showTime <= currentTime) Destroy(transform.gameObject);
            }
            currentTime += Time.deltaTime;
        }
    }

    public void setFallPosition(float octopusX, float enemyX, int damage)
    {
        // 문어와 목표물의 중간 X좌표
        middlePoint = (octopusX + enemyX) / 2;
        this.damage = damage;
        startShoot = true;
    }
}
