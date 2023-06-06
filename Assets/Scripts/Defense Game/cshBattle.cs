using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class cshBattle : MonoBehaviour
{
    public GameObject[] chapter = new GameObject[3];
    private GameObject battleArea;
    public GameObject characterList;
    public GameObject enemyList;
    public GameObject duelListPanel;
    public GameObject[] characterSpawnButtons = new GameObject[10];
    public GameObject[] characterPrefabs = new GameObject[10];
    public GameObject eneryQuantityText;
    public GameObject combatantText;

    private int[] selectedCharacters = new int[5];
    private int[] costSpawn = new int[10] { 100, 100, 100, 150, 150, 150, 150, 200, 200, 200 };
    private int[] costUpgrade = new int[5] { 1000, 2000, 5000, 10000, 30000 };
    public int[] levels = new int[3] { 0, 0, 0 };
    public int[] energyLevels = new int[6] { 20, 35, 50, 65, 80, 100 };
    public int[] combatantLevels = new int[6] { 10, 12, 15, 20, 25, 30 };
    public int[] offenseLevels = new int[6] { 0, 5, 10, 20, 50, 100 };

    public int energyQuantity = 0;
    public int energyProductionAmount = 20;
    public int currentCombatant = 0;
    public int maxCombatant = 15;
    private float currentEnergyProductionTime = 0f;
    private float energyProductionCoolTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        battleArea = chapter[cshBasicInformation.currentChapter - 1];
        battleArea.SetActive(true);
        energyQuantity = 500;
        selectedCharacters = cshBasicInformation.selectedCharacters;
        for (int i = 0; i < selectedCharacters.Length; i++)
        {
            int temp = selectedCharacters[i] - 1;
            GameObject button = Instantiate(characterSpawnButtons[temp]);
            button.transform.SetParent(duelListPanel.transform);
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = costSpawn[temp].ToString();
            button.GetComponent<Button>().onClick.AddListener(() => spawnCharacter(temp));
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        // 에너지 생산
        currentEnergyProductionTime += Time.deltaTime;
        if(currentEnergyProductionTime >= energyProductionCoolTime)
        {
            energyQuantity += energyProductionAmount;
            currentEnergyProductionTime = 0f;
        }
        eneryQuantityText.GetComponent<TextMeshProUGUI>().text = energyQuantity.ToString();
        combatantText.GetComponent<TextMeshProUGUI>().text = currentCombatant + " / " + maxCombatant;

        for (int i = 0; i < 3; i++)
        {
            if(levels[i] != 5 && energyQuantity >= costUpgrade[levels[i]])
            {
                duelListPanel.transform.GetChild(i).gameObject.GetComponent<Button>().interactable = true;
            }
            else
            {
                duelListPanel.transform.GetChild(i).gameObject.GetComponent<Button>().interactable = false;
            }
        }

        for (int i = 3; i < duelListPanel.transform.childCount; i++)
        {
            if(currentCombatant < maxCombatant && costSpawn[selectedCharacters[i-3]-1] <= energyQuantity)
            {
                duelListPanel.transform.GetChild(i).gameObject.GetComponent<Button>().interactable = true;
            }
            else
            {
                duelListPanel.transform.GetChild(i).gameObject.GetComponent<Button>().interactable = false;
            }
        }
    }

    public void spawnCharacter(int n)
    {
        GameObject character = Instantiate(characterPrefabs[n]);
        character.transform.parent = characterList.transform;
        character.transform.GetComponent<cshCharacterAttack>().startSetting(cshBasicInformation.characterLevels[n]);
        character.transform.GetComponent<cshCharacterAttack>().currentOffensePower = character.transform.GetComponent<cshCharacterAttack>().originalOffensePower + offenseLevels[levels[2]];
        currentCombatant++;
        energyQuantity -= costSpawn[n];
    }

    public void upgrade(int n)
    {
        energyQuantity -= costUpgrade[levels[n]];
        levels[n]++;
        if(levels[n] == 5)
        {
            duelListPanel.transform.GetChild(n).transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Lv. MAX";
            duelListPanel.transform.GetChild(n).transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "";
        }
        else
        {
            duelListPanel.transform.GetChild(n).transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Lv. " + levels[n] + " / 5";
            duelListPanel.transform.GetChild(n).transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = costUpgrade[levels[n]].ToString();
        }
        
        // n
        // 0 : 에너지 생산량  1 : 소환 가능한 아군 수  2 : 공격력
        if(n == 0)
        {
            energyProductionAmount = energyLevels[levels[n]];
        }
        else if(n == 1)
        {
            maxCombatant = combatantLevels[levels[n]];
        }
        else
        {
            // 공격력을 업그레이드 했다면 이미 소환된 아군들에게도 적용
            for(int i = 0; i < characterList.transform.childCount; i++)
            {
                characterList.transform.GetChild(i).GetComponent<cshCharacterAttack>().currentOffensePower = characterList.transform.GetChild(i).GetComponent<cshCharacterAttack>().originalOffensePower + offenseLevels[levels[n]];
            }
        }

        // 업그레이드 한 정도에 따라 변하는 기지의 모습
        if (levels[0] + levels[1] + levels[2] == 5)
        {
            battleArea.transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(false);
            battleArea.transform.GetChild(1).transform.GetChild(1).gameObject.SetActive(true);
        }
        else if(levels[0] + levels[1] + levels[2] == 10){
            battleArea.transform.GetChild(1).transform.GetChild(1).gameObject.SetActive(false);
            battleArea.transform.GetChild(1).transform.GetChild(2).gameObject.SetActive(true);
        }
    }

    public void endDuel(int n)
    {
        cshBasicInformation.startUI = n;
        SceneManager.LoadScene("Village Scene");
    }
}
