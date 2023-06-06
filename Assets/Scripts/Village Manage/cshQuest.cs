using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshQuest : MonoBehaviour
{
    public GameObject structureManager;
    public int currentComplete;
    public int purposeComplete;

    private cshStructure structureInformation;
    public int[] reward = new int[9] { 500, 500, 500, 1000, 1000, 1000, 2000, 2000, 2000 };

    // Start is called before the first frame update
    void Start()
    {
        structureInformation = structureManager.GetComponent<cshStructure>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool questIsComplete()
    {
        if (cshBasicInformation.currentQuest == 1)
        {
            // 오렌지나무 하나 건설
            currentComplete = 12 - structureInformation.limitedNumberArray[2];
            purposeComplete = 1;
            if (structureInformation.limitedNumberArray[2] == -1) currentComplete = 0;
        }
        else if (cshBasicInformation.currentQuest == 2)
        {
            // 카페 하나 건설
            currentComplete = 4 - structureInformation.limitedNumberArray[11];
            purposeComplete = 1;
            if (structureInformation.limitedNumberArray[11] == -1) currentComplete = 0;
        }
        else if (cshBasicInformation.currentQuest == 3)
        {
            // 풍차 2개 건설
            currentComplete = 8 - structureInformation.limitedNumberArray[10];
            purposeComplete = 2;
            if (structureInformation.limitedNumberArray[10] == -1) currentComplete = 0;
        }
        else if (cshBasicInformation.currentQuest == 4)
        {
            // 당근 밭 3개 건설
            currentComplete = 12 - structureInformation.limitedNumberArray[8];
            purposeComplete = 3;
            if (structureInformation.limitedNumberArray[8] == -1) currentComplete = 0;
        }
        else if (cshBasicInformation.currentQuest == 5)
        {
            // 상점 5가지 건설
            currentComplete = 0;

            if (structureInformation.limitedNumberArray[10] < 8 && structureInformation.limitedNumberArray[10] != -1) currentComplete++;
            for (int i = 1; i < 10; i++)
            {
                if (structureInformation.limitedNumberArray[10 + i] < 4 && structureInformation.limitedNumberArray[10 + i] != -1) currentComplete++;
            }

            purposeComplete = 5;
        }
        else if (cshBasicInformation.currentQuest == 6)
        {
            // 꽃집 두개 건설
            currentComplete = 4 - structureInformation.limitedNumberArray[17];
            purposeComplete = 2;
            if (structureInformation.limitedNumberArray[17] == -1) currentComplete = 0;
        }
        else if (cshBasicInformation.currentQuest == 7)
        {
            // 상점 8가지 건설
            currentComplete = 0;

            if (structureInformation.limitedNumberArray[10] < 8 && structureInformation.limitedNumberArray[10] != -1) currentComplete++;
            for (int i = 1; i < 10; i++)
            {
                if (structureInformation.limitedNumberArray[10 + i] < 4 && structureInformation.limitedNumberArray[10 + i] != -1) currentComplete++;
            }

            purposeComplete = 8;
        }
        else if (cshBasicInformation.currentQuest == 8)
        {
            // 벚꽃 다섯그루 건설
            currentComplete = 12 - structureInformation.limitedNumberArray[5];
            purposeComplete = 5;
            if (structureInformation.limitedNumberArray[5] == -1) currentComplete = 0;
        }
        else if (cshBasicInformation.currentQuest == 9)
        {
            // 식물 8가지 4개씩 심기
            currentComplete = 0;

            for (int i = 0; i < 8; i++)
            {
                if (structureInformation.limitedNumberArray[2 + i] < 9 && structureInformation.limitedNumberArray[2 + i] != -1) currentComplete++;
            }

            purposeComplete = 8;
        }

        if (currentComplete >= purposeComplete)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
