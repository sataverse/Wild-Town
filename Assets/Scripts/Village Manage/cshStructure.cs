using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �Ǽ��� ������ ������ ����
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

// ������ ������ ����
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
        facilityArray[0] = new Facility(0, "�ֹξ���Ʈ", 0, facilityPrefabArray[0]);
        facilityArray[1] = new Facility(1, "â��", 0, facilityPrefabArray[1]);

        plantArray[0] = new Plant(2, "������ ����", 50, plantPrefabArray[0], 0, 20f);
        plantArray[1] = new Plant(3, "��纣�� ����", 70, plantPrefabArray[1], 1, 30f);
        plantArray[2] = new Plant(4, "���ڼ�", 100, plantPrefabArray[2], 2, 45f);
        plantArray[3] = new Plant(5, "������", 150, plantPrefabArray[3], 3, 60f);
        plantArray[4] = new Plant(6, "�볪��", 200, plantPrefabArray[4], 4, 90f);
        plantArray[5] = new Plant(7, "ƫ����", 70, plantPrefabArray[5], 5, 30f);
        plantArray[6] = new Plant(8, "��ٹ�", 100, plantPrefabArray[6], 6, 45f);
        plantArray[7] = new Plant(9, "���ڹ�", 200, plantPrefabArray[7], 7, 90f);

        storeArray[0] = new Store(10, "ǳ��", 300, storePrefabArray[0], 2, 8, 45f);
        storeArray[1] = new Store(11, "ī��", 500, storePrefabArray[1], 5, 9, 60f);
        storeArray[2] = new Store(12, "����Ŀ��", 500, storePrefabArray[2], 5, 10, 60f);
        storeArray[3] = new Store(13, "�౹", 700, storePrefabArray[3], 10, 11, 90f);
        storeArray[4] = new Store(14, "��ä����", 700, storePrefabArray[4], 10, 12, 90f);
        storeArray[5] = new Store(15, "�������", 1000, storePrefabArray[5], 20, 13, 120f);
        storeArray[6] = new Store(16, "�峭������", 1000, storePrefabArray[6], 20, 14, 120f);
        storeArray[7] = new Store(17, "�ɰ���", 1000, storePrefabArray[7], 20, 15, 120f);
        storeArray[8] = new Store(18, "���ι�", 2000, storePrefabArray[8], 40, 16, 240f);
        storeArray[9] = new Store(19, "������", 2000, storePrefabArray[9], 40, 17, 240f);

        cloud = new Enviroment(-1, "����", cloudPrefab);
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
                    case "����": structureArray[i, j] = new StructureInstance(cloud); break;
                    case "�ֹξ���Ʈ": structureArray[i, j] = new StructureInstance(facilityArray[0]); break;
                    case "â��": structureArray[i, j] = new StructureInstance(facilityArray[1]); break;
                    case "������ ����": structureArray[i, j] = new StructureInstance(plantArray[0]); break;
                    case "��纣�� ����": structureArray[i, j] = new StructureInstance(plantArray[1]); break;
                    case "���ڼ�": structureArray[i, j] = new StructureInstance(plantArray[2]); break;
                    case "������": structureArray[i, j] = new StructureInstance(plantArray[3]); break;
                    case "�볪��": structureArray[i, j] = new StructureInstance(plantArray[4]); break;
                    case "ƫ����": structureArray[i, j] = new StructureInstance(plantArray[5]); break;
                    case "��ٹ�": structureArray[i, j] = new StructureInstance(plantArray[6]); break;
                    case "���ڹ�": structureArray[i, j] = new StructureInstance(plantArray[7]); break;
                    case "ǳ��": structureArray[i, j] = new StructureInstance(storeArray[0]); break;
                    case "ī��": structureArray[i, j] = new StructureInstance(storeArray[1]); break;
                    case "����Ŀ��": structureArray[i, j] = new StructureInstance(storeArray[2]); break;
                    case "�౹": structureArray[i, j] = new StructureInstance(storeArray[3]); break;
                    case "��ä����": structureArray[i, j] = new StructureInstance(storeArray[4]); break;
                    case "�������": structureArray[i, j] = new StructureInstance(storeArray[5]); break;
                    case "�峭������": structureArray[i, j] = new StructureInstance(storeArray[6]); break;
                    case "�ɰ���": structureArray[i, j] = new StructureInstance(storeArray[7]); break;
                    case "���ι�": structureArray[i, j] = new StructureInstance(storeArray[8]); break;
                    case "������": structureArray[i, j] = new StructureInstance(storeArray[9]); break;
                }
            }
        }

        // ���� ���� �ð��� �ҷ��� �����ϰ� �ǹ��� ����
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
        // ������ �����ϰ� ����� �������� �����ϰų� �������� ���� ����
        Transform parent = grounds.transform.GetChild(x).transform.GetChild(y).transform.GetChild(1);
        GameObject structureGameObject = Instantiate(structure.structure.prefab);
        structureGameObject.name = structureGameObject.name.Split("(")[0]; // ������Ʈ�� �����ϸ� �̸��� "(clone)"�� �پ ����
        structureGameObject.GetComponent<SpriteRenderer>().sortingOrder = (x + y) * 2;
        structureGameObject.transform.SetParent(parent, false);
        if (isSave)
        {
            // ����� �������� ���
            if (structure.structure.GetType().Name == "Store" || structure.structure.GetType().Name == "Plant")
            {
                // �������� �����Ǹ� ���� ����
                structureGameObject.GetComponent<cshStructureProduce>().startProduce(structure);
            }
        }
        
        return structureGameObject;
    }

    public GameObject moveStructureOld(GameObject structure, int x, int y, bool isTemporary)
    {
        // �巡�׷� ī�޶� �̵��� ��� �������� �̵� (���� �ִ� ��ġ�� ������ ���� -> ���ο� ��ġ�� ����)
        // ���ο� �� ��ġ�� ������ ����
        Transform parent = grounds.transform.GetChild(x).transform.GetChild(y).transform.GetChild(1);
        GameObject newSelectedStructure = Instantiate(structure);
        newSelectedStructure.name = newSelectedStructure.name.Split("(")[0];
        // ���� �̵����� �������� ������ �������� ��ĥ ��� ���� �̵����� �������� �� ������ ���̰� ��
        newSelectedStructure.GetComponent<SpriteRenderer>().sortingOrder = (x + y) * 2 + (isTemporary ? 1 : 0); 
        newSelectedStructure.transform.SetParent(parent, false);

        // ������ ��ġ�� �ִ� ������ ����
        Destroy(structure); 
        return newSelectedStructure;
    }

    public void moveStructure(GameObject structure, int x, int y, bool isTemporary)
    {
        // �巡�׷� ī�޶� �̵��� ��� �������� �̵� (GameObject�� �θ�(��) ����)
        Transform parent = grounds.transform.GetChild(x).transform.GetChild(y).transform.GetChild(1);
        // ���� �̵����� �������� ������ �������� ��ĥ ��� ���� �̵����� �������� �� ������ ���̰� ��
        structure.GetComponent<SpriteRenderer>().sortingOrder = (x + y) * 2 + (isTemporary ? 1 : 0);
        structure.transform.SetParent(parent, false);
    }

    public void confirmStructurePosition(GameObject structure, int beforeX, int beforeY, int afterX, int afterY, StructureInstance structureInfo)
    {
        // ���ο� �������� �Ǽ��ϰų� ������ �̵� ��ġ�� Ȯ��������
        structure.GetComponent<SpriteRenderer>().sortingOrder = (afterX + afterY) * 2;
        if(structureInfo == null)
        {
            // ��ġ �̵�
            if(afterX != beforeX || afterY != beforeY)
            {
                structureArray[afterX, afterY] = structureArray[beforeX, beforeY];
                structureArray[beforeX, beforeY] = null;
            }
        }
        else
        {
            // ���ο� ������ �Ǽ�
            structureArray[afterX, afterY] = structureInfo;
            cshBasicInformation.acornNum -= structureInfo.structure.price;
            limitedNumberArray[structureInfo.structure.code]--; // �Ǽ� ������ �ǹ� �� ����
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
        limitedNumberArray[structureArray[x, y].structure.code]++; // �Ǽ� ������ �ǹ� �� ����
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
