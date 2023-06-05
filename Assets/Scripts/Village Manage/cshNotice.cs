using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// 적절하지 않은 버튼을 터치할 경우 간단한 알림을 보여줌
public class cshNotice : MonoBehaviour
{
    private bool isStart = false;
    private float currentTime = 0f;
    private float duration = 0.8f;
    private float speed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            currentTime += Time.deltaTime;
            if (duration < currentTime)
            {
                Destroy(transform.gameObject);
            }
            transform.Translate(new Vector3(0.0f, speed * Time.deltaTime, 0.0f));
        }
    }

    public void startSetting(string notice)
    {
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = notice;
        isStart = true;
    }
}
