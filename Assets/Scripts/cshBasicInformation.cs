using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class SaveFileInformation
{
    public bool autoSave;
    public int currentChapter;
    public int currentQuest;
    public int acornNum;
    public int[] selectedCharacters = new int[5];
    public int[] characterLevels = new int[10];
    public int[] productQuantity = new int[18];
    public int[] limitedNumberArray = new int[21];
    public string[] structureName = new string[21];
    public string[] acornSaveTime = new string[21];
    public string[] productSaveTime = new string[21];
}

public class cshBasicInformation : MonoBehaviour
{
    public static int startUI = 0;
    public static bool autoSave = true;
    public static int currentChapter = 1;
    public static int currentQuest = 1;
    public static int acornNum = 2000;
    public static int[] selectedCharacters = new int[5] { 1, 2, 3, 4, 5 };
    public static int[] characterLevels = new int[10] { 0, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
    public static int[] productQuantity;
    public static int[] limitedNumberArray;
    public static string[] structureName;
    public static string[] acornSaveTime;
    public static string[] productSaveTime ;

    public float currentTime = 0f;
    public float saveTime = 180f;

    // Start is called before the first frame update
    void Start()
    {
        makeDirectory();
        loadData();
    }

    // Update is called once per frame
    void Update()
    {
        if (autoSave)
        {
            currentTime += Time.deltaTime;
            if(currentTime > saveTime)
            {
                saveData();
                currentTime = 0f;
            }
        }
        else
        {
            currentTime = 0f;
        }
    }

    public void makeDirectory()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Json"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Json");
        }
    }

    public void loadData()
    {
        string fileName = "Save";
        string path = Application.persistentDataPath + "/Json/" + fileName + ".Json";

        FileStream fstream;
        try
        {
            fstream = new FileStream(path, FileMode.Open, FileAccess.Read);
        }
        catch (FileNotFoundException ex)
        {
            Debug.Log(ex);
            createData();
            fstream = new FileStream(path, FileMode.Open, FileAccess.Read);
        }

        byte[] data = new byte[fstream.Length];
        fstream.Read(data, 0, data.Length);
        fstream.Close();
        string json = Encoding.UTF8.GetString(data);

        SaveFileInformation info = JsonUtility.FromJson<SaveFileInformation>(json);

        autoSave = info.autoSave;
        currentChapter = info.currentChapter;
        currentQuest = info.currentQuest;
        acornNum = info.acornNum;
        selectedCharacters = info.selectedCharacters;
        characterLevels = info.characterLevels;
        productQuantity = info.productQuantity;
        limitedNumberArray = info.limitedNumberArray;
        structureName = info.structureName;
        acornSaveTime = info.acornSaveTime;
        productSaveTime = info.productSaveTime;

        GameObject.Find("UI Manager").GetComponent<cshUIControl>().startSetting();
        GameObject.Find("Character Manager").GetComponent<cshCharacter>().startSetting();
        GameObject.Find("Product Manager").GetComponent<cshProducts>().startSetting();
        GameObject.Find("Structure Manager").GetComponent<cshStructure>().startSetting();
    }

    public void saveData()
    {
        cshCharacter characterInfo = GameObject.Find("Character Manager").GetComponent<cshCharacter>();
        cshProducts productInfo = GameObject.Find("Product Manager").GetComponent<cshProducts>();
        cshStructure structureInfo = GameObject.Find("Structure Manager").GetComponent<cshStructure>();

        SaveFileInformation info = new SaveFileInformation();
        info.autoSave = autoSave;
        info.currentChapter = currentChapter;
        info.currentQuest = currentQuest;
        info.acornNum = acornNum;
        info.selectedCharacters = selectedCharacters;
        for(int i = 0; i < 10; i++)
        {
            info.characterLevels[i] = characterInfo.characterArray[i].level;
        }
        for(int i = 0; i < 18; i++)
        {
            info.productQuantity[i] = productInfo.productArray[i].quantity;
        }
        for (int i = 0; i < 20; i++)
        {
            info.limitedNumberArray[i] = structureInfo.limitedNumberArray[i];
        }
        for (int i = 0; i < 21; i++)
        {
            string str = "";
            for(int j = 0; j < 21; j++)
            {
                if (structureInfo.structureArray[i, j] == null) str += "null,";
                else str += structureInfo.structureArray[i, j].structure.name + ",";
            }
            info.structureName[i] = str;
        }
        for (int i = 0; i < 21; i++)
        {
            string str = "";
            for (int j = 0; j < 21; j++)
            {
                if(structureInfo.structureArray[i, j] != null)
                {
                    if (structureInfo.structureArray[i, j].structure.GetType().Name == "Store")
                    {
                        str += structureInfo.structureArray[i, j].currentAcornTime + ",";
                    }
                    else
                    {
                        str += "null,";
                    }
                }
                else
                {
                    str += "null,";
                }
            }
            info.acornSaveTime[i] = str;
        }
        for (int i = 0; i < 21; i++)
        {
            string str = "";
            for (int j = 0; j < 21; j++)
            {
                if (structureInfo.structureArray[i, j] != null)
                {
                    if (structureInfo.structureArray[i, j].structure.GetType().Name == "Store" || structureInfo.structureArray[i, j].structure.GetType().Name == "Plant")
                    {
                        str += structureInfo.structureArray[i, j].currentProductTime + ",";
                    }
                    else
                    {
                        str += "null,";
                    }
                }
                else
                {
                    str += "null,";
                }
            }
            info.productSaveTime[i] = str;
        }

        string json = JsonUtility.ToJson(info);
        string fileName = "Save";
        string path = Application.persistentDataPath + "/Json/" + fileName + ".Json";

        FileStream fstream = new FileStream(path, FileMode.Create, FileAccess.Write);
        byte[] data = Encoding.UTF8.GetBytes(json);
        fstream.Write(data, 0, data.Length);
        fstream.Close();

        currentTime = 0f;
    }

    public void createData()
    {
        SaveFileInformation info = new SaveFileInformation();
        info.autoSave = true;
        info.currentChapter = 0;
        info.currentQuest = 1;
        info.acornNum = 3000;
        info.selectedCharacters = new int[5] { 0, 0, 0, 0, 0 };
        info.characterLevels = new int[10] { 0, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
        info.productQuantity = new int[18] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        info.limitedNumberArray = new int[20] { 0, 1, 12, -1, -1, -1, -1, 12, -1, -1, 8, 4, 4, -1, -1, -1, -1, -1, -1, -1 };

        for (int i = 0; i < 21; i++)
        {
            string str = "";
            for (int j = 0; j < 21; j++)
            {
                if (i < 3 || i > 17 || j < 3 || j > 17)
                {
                    str += "null,";
                }
                else if (i >= 8 && i<=12 && j >= 8 && j <= 12)
                {
                    if (i == 10 && j == 9) str += "주민아파트,";
                    else str += "null,";
                }
                else
                {
                    str += "구름,";
                }
            }
            info.structureName[i] = str;
        }
        for(int i = 0; i < 21; i++)
        {
            string str = "";
            for(int j = 0; j < 21; j++)
            {
                str += "null,";
            }
            info.acornSaveTime[i] = str;
        }
        for (int i = 0; i < 21; i++)
        {
            string str = "";
            for (int j = 0; j < 21; j++)
            {
                str += "null,";
            }
            info.productSaveTime[i] = str;
        }

        string json = JsonUtility.ToJson(info);
        string fileName = "Save";
        string path = Application.persistentDataPath + "/Json/" + fileName + ".Json";

        FileStream fstream = new FileStream(path, FileMode.Create, FileAccess.Write);
        byte[] data = Encoding.UTF8.GetBytes(json);
        fstream.Write(data, 0, data.Length);
        fstream.Close();
    }
}
