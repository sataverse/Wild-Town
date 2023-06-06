using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshHealthPointGage : MonoBehaviour
{
    Transform character;

    // Start is called before the first frame update
    void Start()
    {
        character = transform.parent.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        float multiple = 1f;
        float current, max;
        if(character.GetChild(0).tag == "Enemy")
        {
            if (character.GetComponent<cshEnemyAttack>().isBoss) multiple = 2f;
            current = (float)(character.GetComponent<cshEnemyAttack>().currentHealthPoint);
            max = (float)(character.GetComponent<cshEnemyAttack>().maxHealthPoint);
        }
        else if(character.GetChild(0).tag == "Character")
        {
            current = (float)(character.GetComponent<cshCharacterAttack>().currentHealthPoint);
            max = (float)(character.GetComponent<cshCharacterAttack>().maxHealthPoint);
        }
        else
        {
            multiple = 3f;
            current = (float)(character.GetComponent<cshShelter>().currentDurability);
            max = (float)(character.GetComponent<cshShelter>().maxDurability);
        }

        // 사각형의 크기를 변경하고 이동하여 체력바를 나타냄
        // mutiple : 타워나 보스같이 큰 오브젝트는 체력바도 크게 나타내려면 큰 이동이 필요
        float healthPointGage = current / max;
        transform.localScale = new Vector2(healthPointGage, 1f);
        transform.localPosition = new Vector2(-multiple * ((1 - healthPointGage) / 2f), 0f);

        // HP 비율에 따라 체력바의 색이 다름
        if(healthPointGage > 0.6f)
        {
            transform.GetComponent<SpriteRenderer>().color = new Color32(0, 255, 0, 200);
        }
        else if(healthPointGage > 0.2f && healthPointGage <= 0.6f)
        {
            transform.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 0, 200);
        }
        else
        {
            transform.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 200);
        }
    }
}
