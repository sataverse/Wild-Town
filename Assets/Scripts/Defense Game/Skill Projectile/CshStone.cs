using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CshStone : MonoBehaviour
{
    public GameObject stone;
    private int count = 0;
    private bool isStart = false;
    public int damage = 0;
    public float attackCoolTime = 0.1f;
    public float attackCurrentTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            // 돌을 빠른속도로 8회 발사
            if (count >= 8)
            {
                Destroy(transform.gameObject);
            }
            attackCurrentTime += Time.deltaTime;
            if (attackCurrentTime >= attackCoolTime)
            {
                shoot(stone);
                attackCurrentTime = 0;
                count++;
            }
        }
    }

    public void shoot(GameObject projectile)
    {
        GameObject normalProjectilePrefab = Instantiate(projectile);
        normalProjectilePrefab.GetComponent<cshProjectile>().setAttackDamage(damage);
        normalProjectilePrefab.transform.position = transform.parent.GetChild(1).position;
    }

    public void setDamage(int damage)
    {
        this.damage = damage;
        isStart = true;
    }
}
