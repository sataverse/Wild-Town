using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public string name;
    public int level;
    public int offensePower;
    public int defensePower;
    public float attackSpeed;
    public int healthPoint;
    public int levelUpIngredient;
    public string skillName;
    public string skillDirections;
    public int skillCoolTime;
    public Sprite image;
    public Sprite imageRect;
    public Sprite skillImage;
    public GameObject prefab;
    public GameObject button;

    public Character(string name, int level, int OP, int DP, float AS, int HP, int ingredient, string skillName, string skillDirections, int skillCoolTime, Sprite image, Sprite imageRect, Sprite skillImage, GameObject prefab, GameObject button)
    {
        this.name = name;
        this.level = level;
        this.offensePower = OP;
        this.defensePower = DP;
        this.attackSpeed = AS;
        this.healthPoint = HP;
        this.levelUpIngredient = ingredient;
        this.skillName = skillName;
        this.skillDirections = skillDirections;
        this.skillCoolTime = skillCoolTime;
        this.image = image;
        this.imageRect = imageRect;
        this.skillImage = skillImage;
        this.prefab = prefab;
        this.button = button;
    }
}
public class cshCharacter : MonoBehaviour
{
    public GameObject structureManager;
    private cshStructure structureInformation;

    public Transform spawnedCharacterList;
    public Transform ground;
    private bool isSpawned = false;
    private bool isStart = false;

    private string[] characterName = { "문어", "오렌지", "오리", "로봇", "토끼", "수달", "강아지", "거미", "고양이", "양초" };
    private int[] characterLevel = new int[10] { 5, 4, 3, 2, 1, 0, 0, 0, -1, -1};
    private int[] characterOffensePower = new int[10] { 100, 80, 70, 100, 120, 100, 90, 90, 180, 100 };
    private int[] characterDefensePower = new int[10] { 10, 8, 20, 12, 6, 18, 8, 8, 15, 8 };
    private float[] characterAttackSpeed = new float[10] { 0.9f, 1f, 0.6f, 0.85f, 0.7f, 0.75f, 0.95f, 0.88f, 0.92f, 0.98f };
    private int[] characterHealthPoint = new int[10] { 1500, 1000, 3500, 1200, 1000, 2800, 1500, 1000, 2500, 1000 };
    private int[] characterIngredient = new int[10] { 1, 11, 13, 14, 12, 7, 15, 17, 4, 16 };
    private string[] characterSkillName = new string[10] { "먹물 폭탄", "비타민 충전", "돌풍", "플라즈마", "집중 연사", "물장구", "설탕 보호막", "공격 명령", "절단", "거대해진 불꽃" };
    private string[] characterSkillDirections = new string[10] { "먹물 폭탄을 발사해 터진 영역의 적에게 피해를 주고 실명 상태로 만듭니다.", "가장 체력 비율이 적은 아군을 치유합니다.",
        "회전하는 동안  방어력이 증가하며 여러 적에게 지속 피해를 줍니다.", "플라즈마를 방출해서 모든 적에게 피해를 입히며 방어력을 감소시킵니다.", "새총을 매우 빠른 속도로 연사하여 피해를 입힙니다.",
        "물기둥을 만들어 범위 안의 적에게 피해를 입히며 기절시킵니다.", "가장 앞에 있는 아군에게 원거리 피해를 막는 보호막을 부여합니다.", "작은거미 3마리를 소환해 전방으로 돌진하고 공격하도록 명령합니다.",
        "앞에 있는 여러 적을 베어 큰 피해를 입힙니다.", "거대한 불꽃을 내보내서 모든 적에게 큰 피해를 입힙니다." };
    private int[] characterCoolTime = new int[10] { 10, 6, 15, 16, 11, 13, 12, 24, 18, 20 };
    public Sprite[] characterImages = new Sprite[10];
    public Sprite[] characterImagesRect = new Sprite[10];
    public Sprite[] CharacterSkillImages = new Sprite[10];
    public GameObject[] CharacterPrefabs = new GameObject[10];
    public GameObject[] CharacterButtons = new GameObject[10];
    public Character[] characterArray = new Character[10];
    
    // Start is called before the first frame update
    void Start()
    {
        structureInformation = structureManager.GetComponent<cshStructure>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isSpawned && structureInformation.isLoaded && isStart)
        {
            spawnAllCharacters();
        }
    }

    public void spawnAllCharacters()
    {
        for(int i = 0; i < 10; i++)
        {
            if (characterArray[i].level >= 0)
            {
                spawnCharacter(i);
            }
        }
        isSpawned = true;
    }

    public void spawnCharacter(int n)
    {
        // 마을에 비어있는 공간을 찾아 그중 무작위의 지역에 주민을 소환
        System.Random r = new System.Random();
        List<int[]> emptyArea = new List<int[]>();

        for (int i = 3; i < 18; i++)
        {
            for(int j = 3; j < 18; j++)
            {
                if (structureInformation.structureArray[i, j] == null) emptyArea.Add(new int[2] { i, j });
            }
        }

        if(emptyArea.Count > 0)
        {
            int index = r.Next(0, emptyArea.Count);
            int x = emptyArea[index][0];
            int y = emptyArea[index][1];

            GameObject prefab = Instantiate(characterArray[n].prefab);
            prefab.transform.position = new Vector2(ground.GetChild(x).transform.GetChild(y).position.x, ground.GetChild(x).transform.GetChild(y).position.y + 0.2f);
            prefab.transform.SetParent(spawnedCharacterList);
            prefab.GetComponent<cshCharacterWalkVillage>().startSetting(x, y);
        }
    }

    public void startSetting()
    {
        characterLevel = cshBasicInformation.characterLevels;
        for (int i = 0; i < 10; i++)
        {
            characterArray[i] = new Character(characterName[i], characterLevel[i], characterOffensePower[i], characterDefensePower[i], characterAttackSpeed[i], characterHealthPoint[i], characterIngredient[i],
                characterSkillName[i], characterSkillDirections[i], characterCoolTime[i], characterImages[i], characterImagesRect[i], CharacterSkillImages[i], CharacterPrefabs[i], CharacterButtons[i]);
        }
        isStart = true;
    }
}
