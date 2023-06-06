using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 건설된 구조물 정보를 저장
public class StructureInstance
{
    public float currentAcornTime = 0f;
    public float currentProductTime = 0f;
    public Structure structure;

    public StructureInstance(Structure structure)
    {
        this.structure = structure;
    }
}

// 구조물 정보를 저장
public class Structure
{
    public int code;
    public string name;
    public int price;
    public GameObject prefab;

    public int acorn;
    public int product;
    public float productTime;
}

public class Facility : Structure
{
    public Facility(int code, string name, int price, GameObject prefab)
    {
        this.code = code;
        this.name = name;
        this.price = price;
        this.prefab = prefab;
    }
}

public class Store : Structure
{
    public Store(int code, string name, int price, GameObject prefab, int acorn, int product, float productTime)
    {
        this.code = code;
        this.name = name;
        this.price = price;
        this.prefab = prefab;

        this.acorn = acorn;
        this.product = product;
        this.productTime = productTime;
    }
}

public class Plant : Structure
{
    public Plant(int code, string name, int price, GameObject prefab, int product, float productTime)
    {
        this.code = code;
        this.name = name;
        this.price = price;
        this.prefab = prefab;

        this.product = product;
        this.productTime = productTime;
    }
}

public class Enviroment : Structure
{
    public Enviroment(int code, string name, GameObject prefab)
    {
        this.code = code;
        this.name = name;
        this.prefab = prefab;
    }
}

public class cshStructure : MonoBehaviour
{
    public int[] limitedNumberArray = new int[20] { 1, 1, 12, 12, 12, 12, 12, 12, 12, 12, 8, 4, 4, 4, 4, 4, 4, 4, 4, 4 };
    public GameObject[] buildFacilityButton = new GameObject[3];
    public GameObject[] buildStoreButton = new GameObject[10];
    public GameObject[] buildPlantButton = new GameObject[8];

    public GameObject[] facilityPrefabArray = new GameObject[5];
    public GameObject[] storePrefabArray = new GameObject[10];
    public GameObject[] plantPrefabArray = new GameObject[8];
    public GameObject cloudPrefab;

    public Facility[] facilityArray = new Facility[2];
    public Store[] storeArray = new Store[10];
    public Plant[] plantArray = new Plant[8];
    public Enviroment cloud;

    public GameObject grounds;
    public StructureInstance [,] structureArray = new StructureInstance[21, 21];
    public int[,] cloudPos = new int[8, 2] { { 3, 8 }, { 8, 13 }, { 13, 8 }, { 8, 3 }, { 3, 3 }, { 3, 13 }, { 13, 13 }, { 13, 3 } } ;
    public bool isLoaded = false;

    // Start is called before the first frame update
    void Start()
    {
        setStructureInfo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void setStructureInfo()
    {
        facilityArray[0] = new Facility(0, "주민아파트", 0, facilityPrefabArray[0]);
        facilityArray[1] = new Facility(1, "창고", 0, facilityPrefabArray[1]);

        plantArray[0] = new Plant(2, "오렌지 나무", 50, plantPrefabArray[0], 0, 20f);
        plantArray[1] = new Plant(3, "블루베리 나무", 70, plantPrefabArray[1], 1, 30f);
        plantArray[2] = new Plant(4, "야자수", 100, plantPrefabArray[2], 2, 45f);
        plantArray[3] = new Plant(5, "벚나무", 150, plantPrefabArray[3], 3, 60f);
        plantArray[4] = new Plant(6, "대나무", 200, plantPrefabArray[4], 4, 90f);
        plantArray[5] = new Plant(7, "튤립밭", 70, plantPrefabArray[5], 5, 30f);
        plantArray[6] = new Plant(8, "당근밭", 100, plantPrefabArray[6], 6, 45f);
        plantArray[7] = new Plant(9, "수박밭", 200, plantPrefabArray[7], 7, 90f);

        storeArray[0] = new Store(10, "풍차", 300, storePrefabArray[0], 2, 8, 45f);
        storeArray[1] = new Store(11, "카페", 500, storePrefabArray[1], 5, 9, 60f);
        storeArray[2] = new Store(12, "베이커리", 500, storePrefabArray[2], 5, 10, 60f);
        storeArray[3] = new Store(13, "약국", 700, storePrefabArray[3], 10, 11, 90f);
        storeArray[4] = new Store(14, "야채가게", 700, storePrefabArray[4], 10, 12, 90f);
        storeArray[5] = new Store(15, "레스토랑", 1000, storePrefabArray[5], 20, 13, 120f);
        storeArray[6] = new Store(16, "장난감가게", 1000, storePrefabArray[6], 20, 14, 120f);
        storeArray[7] = new Store(17, "꽃가게", 1000, storePrefabArray[7], 20, 15, 120f);
        storeArray[8] = new Store(18, "와인바", 2000, storePrefabArray[8], 40, 16, 240f);
        storeArray[9] = new Store(19, "보석상", 2000, storePrefabArray[9], 40, 17, 240f);

        cloud = new Enviroment(-1, "구름", cloudPrefab);
    }

    private void initStructures()
    {
        for(int i = 0; i < 21; i++)
        {
            for(int j = 0; j < 21; j++)
            {
                string name = cshBasicInformation.structureName[i].Split(",")[j];
                switch (name)
                {
                    case "구름": structureArray[i, j] = new StructureInstance(cloud); break;
                    case "주민아파트": structureArray[i, j] = new StructureInstance(facilityArray[0]); break;
                    case "창고": structureArray[i, j] = new StructureInstance(facilityArray[1]); break;
                    case "오렌지 나무": structureArray[i, j] = new StructureInstance(plantArray[0]); break;
                    case "블루베리 나무": structureArray[i, j] = new StructureInstance(plantArray[1]); break;
                    case "야자수": structureArray[i, j] = new StructureInstance(plantArray[2]); break;
                    case "벚나무": structureArray[i, j] = new StructureInstance(plantArray[3]); break;
                    case "대나무": structureArray[i, j] = new StructureInstance(plantArray[4]); break;
                    case "튤립밭": structureArray[i, j] = new StructureInstance(plantArray[5]); break;
                    case "당근밭": structureArray[i, j] = new StructureInstance(plantArray[6]); break;
                    case "수박밭": structureArray[i, j] = new StructureInstance(plantArray[7]); break;
                    case "풍차": structureArray[i, j] = new StructureInstance(storeArray[0]); break;
                    case "카페": structureArray[i, j] = new StructureInstance(storeArray[1]); break;
                    case "베이커리": structureArray[i, j] = new StructureInstance(storeArray[2]); break;
                    case "약국": structureArray[i, j] = new StructureInstance(storeArray[3]); break;
                    case "야채가게": structureArray[i, j] = new StructureInstance(storeArray[4]); break;
                    case "레스토랑": structureArray[i, j] = new StructureInstance(storeArray[5]); break;
                    case "장난감가게": structureArray[i, j] = new StructureInstance(storeArray[6]); break;
                    case "꽃가게": structureArray[i, j] = new StructureInstance(storeArray[7]); break;
                    case "와인바": structureArray[i, j] = new StructureInstance(storeArray[8]); break;
                    case "보석상": structureArray[i, j] = new StructureInstance(storeArray[9]); break;
                }
            }
        }

        // 남은 생산 시간을 불러와 저장하고 건물을 생성
        for (int i = 0; i < 21; i++)
        {
            for (int j = 0; j < 21; j++)
            {
                string timeStringAcorn = cshBasicInformation.acornSaveTime[i].Split(",")[j];
                string timeStringProduct = cshBasicInformation.productSaveTime[i].Split(",")[j];
                if (timeStringAcorn != "null")
                {
                    float time = float.Parse(timeStringAcorn);
                    if (structureArray[i, j].structure.GetType().Name == "Store")
                    {
                        structureArray[i, j].currentAcornTime = time;
                    }
                }
                if (timeStringProduct != "null")
                {
                    float time = float.Parse(timeStringProduct);
                    if (structureArray[i, j].structure.GetType().Name == "Store" || structureArray[i, j].structure.GetType().Name == "Plant")
                    {
                        structureArray[i, j].currentProductTime = time;
                    }
                }
                if (structureArray[i, j] != null)
                {
                    createStructure(structureArray[i, j], i, j, true);
                }
            }
        }

        isLoaded = true;
    }

    public GameObject createStructure(StructureInstance structure, int x, int y, bool isSave)
    {
        // 게임을 시작하고 저장된 구조물을 생성하거나 구조물을 새로 생성
        Transform parent = grounds.transform.GetChild(x).transform.GetChild(y).transform.GetChild(1);
        GameObject structureGameObject = Instantiate(structure.structure.prefab);
        structureGameObject.name = structureGameObject.name.Split("(")[0]; // 오브젝트를 생성하면 이름에 "(clone)"이 붙어서 제거
        structureGameObject.GetComponent<SpriteRenderer>().sortingOrder = (x + y) * 2;
        structureGameObject.transform.SetParent(parent, false);
        if (isSave)
        {
            // 저장된 구조물의 경우
            if (structure.structure.GetType().Name == "Store" || structure.structure.GetType().Name == "Plant")
            {
                // 구조물이 생성되면 생산 시작
                structureGameObject.GetComponent<cshStructureProduce>().startProduce(structure);
            }
        }
        
        return structureGameObject;
    }

    public GameObject moveStructureOld(GameObject structure, int x, int y, bool isTemporary)
    {
        // 드래그로 카메라가 이동한 경우 구조물도 이동 (전에 있던 위치에 구조물 삭제 -> 새로운 위치에 생성)
        // 새로운 땅 위치에 구조물 생성
        Transform parent = grounds.transform.GetChild(x).transform.GetChild(y).transform.GetChild(1);
        GameObject newSelectedStructure = Instantiate(structure);
        newSelectedStructure.name = newSelectedStructure.name.Split("(")[0];
        // 아직 이동중인 구조물이 원래의 구조물과 겹칠 경우 현재 이동중인 구조물이 더 앞으로 보이게 함
        newSelectedStructure.GetComponent<SpriteRenderer>().sortingOrder = (x + y) * 2 + (isTemporary ? 1 : 0); 
        newSelectedStructure.transform.SetParent(parent, false);

        // 이전의 위치에 있는 구조물 삭제
        Destroy(structure); 
        return newSelectedStructure;
    }

    public void moveStructure(GameObject structure, int x, int y, bool isTemporary)
    {
        // 드래그로 카메라가 이동한 경우 구조물도 이동 (GameObject의 부모(땅) 변경)
        Transform parent = grounds.transform.GetChild(x).transform.GetChild(y).transform.GetChild(1);
        // 아직 이동중인 구조물이 원래의 구조물과 겹칠 경우 현재 이동중인 구조물이 더 앞으로 보이게 함
        structure.GetComponent<SpriteRenderer>().sortingOrder = (x + y) * 2 + (isTemporary ? 1 : 0);
        structure.transform.SetParent(parent, false);
    }

    public void confirmStructurePosition(GameObject structure, int beforeX, int beforeY, int afterX, int afterY, StructureInstance structureInfo)
    {
        // 새로운 구조물을 건설하거나 구조물 이동 위치를 확정했을때
        structure.GetComponent<SpriteRenderer>().sortingOrder = (afterX + afterY) * 2;
        if(structureInfo == null)
        {
            // 위치 이동
            if(afterX != beforeX || afterY != beforeY)
            {
                structureArray[afterX, afterY] = structureArray[beforeX, beforeY];
                structureArray[beforeX, beforeY] = null;
            }
        }
        else
        {
            // 새로운 구조물 건설
            structureArray[afterX, afterY] = structureInfo;
            cshBasicInformation.acornNum -= structureInfo.structure.price;
            limitedNumberArray[structureInfo.structure.code]--; // 건설 가능한 건물 수 감소
            if (structureInfo.structure.GetType().Name == "Store" || structureInfo.structure.GetType().Name == "Plant")
            {
                structure.GetComponent<cshStructureProduce>().startProduce(structureInfo);
            }
        }
    }

    public string getStructureName(GameObject removedStructure)
    {
        GameObject ground = removedStructure.transform.parent.transform.parent.gameObject;
        int x = ground.GetComponent<cshGroundPosition>().posX;
        int y = ground.GetComponent<cshGroundPosition>().posY;

        return (structureArray[x, y].structure.name != null) ? structureArray[x, y].structure.name : "";
    }

    public void removeStructure(GameObject removedStructure)
    {
        GameObject ground = removedStructure.transform.parent.transform.parent.gameObject;
        int x = ground.GetComponent<cshGroundPosition>().posX;
        int y = ground.GetComponent<cshGroundPosition>().posY;

        Destroy(removedStructure);
        limitedNumberArray[structureArray[x, y].structure.code]++; // 건설 가능한 건물 수 증가
        structureArray[x, y] = null;
    }

    public void removeCloud(int n)
    {
        for(int i = cloudPos[n, 0]; i < cloudPos[n, 0] + 5; i++)
        {
            for(int j = cloudPos[n, 1]; j < cloudPos[n, 1] + 5; j++)
            {
                Destroy(grounds.transform.GetChild(i).transform.GetChild(j).transform.GetChild(1).transform.GetChild(0).gameObject);
                structureArray[i, j] = null;
            }
        }
    }

    public void startSetting()
    {
        limitedNumberArray = cshBasicInformation.limitedNumberArray;
        initStructures();
    }
}
