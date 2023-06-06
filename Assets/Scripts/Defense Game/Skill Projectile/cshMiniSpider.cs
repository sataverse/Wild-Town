using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshMiniSpider : MonoBehaviour
{
    public GameObject miniSpider;
    private int count = 0;
    private bool isStart = false;
    public float spawnCoolTime = 0.3f;
    public float spawnCurrentTime = 0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            if (count >= 3)
            {
                Destroy(transform.gameObject);
            }
            spawnCurrentTime += Time.deltaTime;
            if (spawnCurrentTime >= spawnCoolTime)
            {
                spawn(miniSpider, count);
                spawnCurrentTime = 0;
                count++;
            }
        }
    }

    public void spawn(GameObject spider, int n)
    {
        //크기가 다른 거미 3마리 소환
        GameObject miniSpiderPrefab = Instantiate(spider);
        
        if(n == 0)
        {
            miniSpiderPrefab.transform.position = new Vector2(transform.parent.GetChild(1).position.x, transform.parent.GetChild(1).position.y - 0.2f);
        }
        else if(n == 1)
        {
            miniSpider.transform.localScale = new Vector3(1.25f, 1.25f, 1f);
            miniSpiderPrefab.transform.position = new Vector2(transform.parent.GetChild(1).position.x, transform.parent.GetChild(1).position.y - 0.3f);
        }
        else
        {
            miniSpider.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
            miniSpiderPrefab.transform.position = new Vector2(transform.parent.GetChild(1).position.x, transform.parent.GetChild(1).position.y - 0.4f);
        }
        miniSpiderPrefab.transform.GetComponent<cshCharacterAttack>().startSetting(0);
    }

    public void setStart()
    {
        isStart = true;
    }
}
