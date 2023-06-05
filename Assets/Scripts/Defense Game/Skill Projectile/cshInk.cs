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
        // ��ǥ�� ���� �Ÿ��� ���ݺ��� ���� �̵� �Ÿ��� �۴ٸ� ���� ���� �̵� ũ�ٸ� ���� �Ʒ��� �̵�
        if(middlePoint > transform.position.x && startShoot && !isPop)
        {
            transform.Translate(new Vector3(speed * Time.deltaTime, speed * Time.deltaTime, 0.0f));
        }
        else if (middlePoint <= transform.position.x && startShoot && !isPop)
        {
            transform.Translate(new Vector3(speed * Time.deltaTime, -speed * Time.deltaTime, 0.0f));
        }

        // �Թ��� �������ٰ� �ٴ� ��ġ�� �����ߴٸ� �Ͷ߸���
        if(transform.position.y <= -0.9f && !isPop)
        {
            isPop = true;
        }
        
        if(isPop)
        {
            if(currentTime == 0f)
            {
                transform.GetChild(0).gameObject.SetActive(false); // �Թ� ������ �� ������Ʈ
                transform.GetChild(1).gameObject.SetActive(true); // ���� �Թ� ������Ʈ
                // �Թ��� ���� ��ġ���� �������� ���鿡�� �������� ������ �Ǹ� ���¸� �ο� (�⺻ ������ ���ط��� 1�� ����)
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
        // ����� ��ǥ���� �߰� X��ǥ
        middlePoint = (octopusX + enemyX) / 2;
        this.damage = damage;
        startShoot = true;
    }
}
