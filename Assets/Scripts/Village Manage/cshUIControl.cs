using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class cshUIControl : MonoBehaviour
{
    public GameObject basicInfoManager;
    public GameObject productManager;
    public GameObject characterManager;
    public GameObject structureManager;
    public GameObject questManager;

    private cshBasicInformation basicInformation;
    private cshProducts productsInformation;
    private cshCharacter characterInformation;
    private cshStructure structureInformation;
    private cshQuest questInformation;

    public GameObject canvas;
    public GameObject acornText;
    public GameObject normalUI;
    public GameObject buildUI;
    public GameObject buildMoveUI;
    public GameObject storageUI;
    public GameObject residentUI;
    public GameObject structureUI;
    public GameObject questUI;
    public GameObject adventureUI;
    public GameObject selectUI;
    public GameObject afterBattleUI;
    public GameObject settingUI;
    public GameObject titleUI;
    public GameObject loadingUI;

    public GameObject noticeText;

    public GameObject characterListObject;
    public GameObject characterWindow;
    public GameObject characterStars;
    public GameObject characterNameText;
    public GameObject characterOPText;
    public GameObject characterDPText;
    public GameObject characterASText;
    public GameObject characterHPText;
    public GameObject characterSkillTooltip;
    public GameObject characterSkillNameText;
    public GameObject characterSkillDirectionsText;
    public GameObject characterSkillCoolTimeText;
    public Image characterImageObject;
    public GameObject characterSkillImageObject;
    public GameObject levelUpButton;
    public GameObject levelUpObjects;

    public GameObject productListObject;
    public GameObject sellWindow;
    public GameObject sellButton;
    public GameObject acornQuantityText;
    public GameObject productSellQuantityText;
    public GameObject productSellQuantitySlider;
    public Image productSellImageObject;

    public GameObject buildStructureListObject;
    public GameObject structureModifyButtons;
    public GameObject structureRemoveAlertDialog;
    public GameObject structureMoveConfirmButton;

    public GameObject produceSliders;
    public GameObject structureProductButtonsPos;
    public GameObject exchangePanel;
    public GameObject exchangeButton;
    public GameObject exchangeIngredientObject; 
    public GameObject exchangeProductObject;
    public GameObject productStorageQuantity;
    public GameObject exchangeQuantitySlider;

    public Transform questListObject;
    public GameObject unlockCharacterDialog;
    public GameObject unlockStructureDialog;

    public Transform selectedCharacterListObject;
    public Transform deselectedCharacterListObject;
    public Transform areaNameObject;
    public Transform enemyListObject;

    public GameObject battleVictoryAcornPointText;

    public Transform autoSaveToggleButton;
    public GameObject saveCompleteDialog;

    public string status;
    public bool isTooltipOn = false;
    private bool isStart = false;
    private bool autoSave = true;
    private bool isQuestClear = false;

    // ĳ���� UI
    private Character currentCharacter;

    // â�� UI
    private int sellIndex;

    // ���� UI
    private StructureInstance currentProduceStructure;
    private bool isShowStructureStatus = false;

    // �Ǽ� UI
    private GameObject selectedStructure = null;
    private StructureInstance selectedStructureInformation = null;
    private bool isNew = false;
    private int startStructurePositionX = -1;
    private int startStructurePositionY = -1;
    private int previousStructurePositionX = -1;
    private int previousStructurePositionY = -1;

    // ���� UI
    private int[] characterIsSelected = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    private int selectedCharacterLength = 0;

    // Start is called before the first frame update
    void Start()
    {
        status = "ui";
        basicInformation = basicInfoManager.GetComponent<cshBasicInformation>();
        productsInformation = productManager.GetComponent<cshProducts>();
        characterInformation = characterManager.GetComponent<cshCharacter>();
        structureInformation = structureManager.GetComponent<cshStructure>();
        questInformation = questManager.GetComponent<cshQuest>();

        if (cshBasicInformation.startUI == 0)
        {
            titleUI.SetActive(true);
        }
        else
        {
            showLoading();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            acornText.GetComponent<TextMeshProUGUI>().text = cshBasicInformation.acornNum.ToString();
            if (isShowStructureStatus)
            {
                showStructureProduce();
            }
        }
    }

    private void showLoading()
    {
        GameObject loading = Instantiate(loadingUI);
        loading.transform.SetParent(canvas.transform);
        loading.transform.position = canvas.transform.position;
    }

    private void changeUI(GameObject before, GameObject after)
    {
        before.SetActive(false);
        after.SetActive(true);
    }

    public void updateStorage()
    {
        for (int i = 0; i < productListObject.transform.childCount; i++)
        {
            Destroy(productListObject.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < productsInformation.productArray.Length; i++)
        {
            if (productsInformation.productArray[i].quantity > 0)
            {
                int temp = i;
                GameObject productButton = Instantiate(productsInformation.productArray[i].button);
                productButton.transform.SetParent(productListObject.transform, false);
                productButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = productsInformation.productArray[i].quantity.ToString();
                productButton.GetComponent<Button>().onClick.AddListener(() => openSellWindow(temp));
            }
        }
    }

    private void updateCharacter()
    {
        for (int i = 0; i < characterListObject.transform.childCount; i++)
        {
            characterListObject.transform.GetChild(i).GetChild(1).gameObject.
                SetActive((characterInformation.characterArray[i].level == -1) ? true : false);
        }
    }

    public void gameStartButtonTouch()
    {
        showLoading();
        Invoke("setStatusVillage", 3f);
        changeUI(titleUI, normalUI);
    }

    // â�� UI
    public void openStorage()
    {
        status = "ui";
        updateStorage();
        changeUI(normalUI, storageUI);
    }

    public void closeStorage()
    {
        Invoke("setStatusVillage", 0.01f);
        changeUI(storageUI, normalUI);
    }

    public void openSellWindow(int n)
    {
        // â�� �׸� ��ġ�� �Ǹ� ����
        Slider sliderComponent = productSellQuantitySlider.GetComponent<Slider>();
        sellIndex = n;

        productSellImageObject.GetComponent<Image>().sprite = productsInformation.productArray[sellIndex].image;
        sliderComponent.maxValue = productsInformation.productArray[sellIndex].quantity;
        sliderComponent.minValue = 0;
        sliderComponent.value = 1;
        productSellQuantityText.GetComponent<TextMeshProUGUI>().text = "X 1";
        acornQuantityText.GetComponent<TextMeshProUGUI>().text = "X " + productsInformation.productArray[sellIndex].price;
        sellWindow.SetActive(true);
    }

    public void closeSellWindow()
    {
        sellWindow.SetActive(false);
    }

    public void productSellSliderValueChanged()
    {
        // �Ǹ� ���� �����̴� ������ ���
        productSellQuantityText.GetComponent<TextMeshProUGUI>().text = "X " + productSellQuantitySlider.GetComponent<Slider>().value;
        acornQuantityText.GetComponent<TextMeshProUGUI>().text = "X " + productsInformation.productArray[sellIndex].price * productSellQuantitySlider.GetComponent<Slider>().value;
        if (productSellQuantitySlider.GetComponent<Slider>().value == 0)
        {
            sellButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            sellButton.GetComponent<Button>().interactable = true;
        }
    }

    public void plusSellProduct()
    {
        if(productSellQuantitySlider.GetComponent<Slider>().value < productSellQuantitySlider.GetComponent<Slider>().maxValue)
        {
            productSellQuantitySlider.GetComponent<Slider>().value++;
        }
    }

    public void minusSellProduct()
    {
        if (productSellQuantitySlider.GetComponent<Slider>().value > 0)
        {
            productSellQuantitySlider.GetComponent<Slider>().value--;
        }
    }

    public void sellProduct()
    {
        productsInformation.productArray[sellIndex].quantity -= (int)productSellQuantitySlider.GetComponent<Slider>().value;
        cshBasicInformation.acornNum += productsInformation.productArray[sellIndex].price * (int)productSellQuantitySlider.GetComponent<Slider>().value;
        updateStorage();
        closeSellWindow();
    }

    // �ֹ� UI
    public void openResident()
    {
        status = "ui";
        updateCharacter();
        changeUI(normalUI, residentUI);
    }

    public void closeResident()
    {
        Invoke("setStatusVillage", 0.01f);
        changeUI(residentUI, normalUI);
    }

    public void openCharacterWindow(int n)
    {
        // �ֹ� �׸� ��ġ�ϸ� �ֹ� ���� ���� Ȯ��
        currentCharacter = characterInformation.characterArray[n];
        characterWindow.SetActive(true);
        isTooltipOn = false;
        characterNameText.GetComponent<TextMeshProUGUI>().text = currentCharacter.name;
        characterSkillNameText.GetComponent<TextMeshProUGUI>().text = currentCharacter.skillName;
        characterSkillDirectionsText.GetComponent<TextMeshProUGUI>().text = currentCharacter.skillDirections;
        characterSkillCoolTimeText.GetComponent<TextMeshProUGUI>().text = currentCharacter.skillCoolTime.ToString() + "��";
        characterImageObject.GetComponent<Image>().sprite = currentCharacter.image;
        characterSkillImageObject.GetComponent<Image>().sprite = currentCharacter.skillImage;
        refreshCharacterWindow();
    }

    public void refreshCharacterWindow()
    {
        fillStar(currentCharacter.level);
        setStatusByLevel();
    }

    public void showTooltip()
    {
        // ��ų ������ �����ִ� ����
        characterSkillTooltip.SetActive(!isTooltipOn);
        isTooltipOn = !isTooltipOn;
    }

    private void fillStar(int n)
    {
        for(int i = 0; i < 5; i++)
        {
            if(n > i)
            {
                characterStars.transform.GetChild(i).GetComponent<Image>().color = new Color32(255, 255, 0, 255);
            }
            else
            {
                characterStars.transform.GetChild(i).GetComponent<Image>().color = new Color32(200, 200, 200, 255);
            }
        }
    }

    private void setStatusByLevel()
    {
        // ������ ���� ����, ������ ��� ���� �ٸ��� ��Ÿ��
        int level = currentCharacter.level;
        int OP = currentCharacter.offensePower;
        int DP = currentCharacter.defensePower;
        float AS = currentCharacter.attackSpeed;
        int HP = currentCharacter.healthPoint;
        int acorn = 0, ingredient = 0;

        if(level == 0)
        {
            acorn = 500;
        }
        else if(level == 1)
        {
            OP += (int)(OP * 0.1f);
            DP += 2;
            AS -= 0.01f;
            HP += (int)(HP * 0.1f);
            acorn = 1000;
        }
        else if(level == 2)
        {
            OP += (int)(OP * 0.2f);
            DP += 5;
            AS -= 0.02f;
            HP += (int)(HP * 0.2f);
            acorn = 2000;
            ingredient = 5;
        }
        else if (level == 3)
        {
            OP += (int)(OP * 0.4f);
            DP += 10;
            AS -= 0.04f;
            HP += (int)(HP * 0.4f);
            acorn = 5000;
            ingredient = 10;
        }
        else if (level == 4)
        {
            OP += (int)(OP * 0.7f);
            DP += 15;
            AS -= 0.07f;
            HP += (int)(HP * 0.7f);
            acorn = 10000;
            ingredient = 50;
        }
        else
        {
            OP += (int)(OP * 1.0f);
            DP += 20;
            AS -= 0.1f;
            HP += (int)(HP * 1.0f);
        }

        characterOPText.GetComponent<TextMeshProUGUI>().text = OP.ToString();
        characterDPText.GetComponent<TextMeshProUGUI>().text = DP.ToString();
        characterASText.GetComponent<TextMeshProUGUI>().text = AS.ToString();
        characterHPText.GetComponent<TextMeshProUGUI>().text = HP.ToString();
        if(level == 5)
        {
            levelUpObjects.transform.GetChild(0).gameObject.SetActive(false);
            levelUpObjects.transform.GetChild(1).gameObject.SetActive(false);
            levelUpObjects.transform.GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            levelUpObjects.transform.GetChild(0).gameObject.SetActive(true);
            levelUpObjects.transform.GetChild(1).gameObject.SetActive(true);
            levelUpObjects.transform.GetChild(2).gameObject.SetActive(false);
            if(ingredient == 0)
            {
                levelUpObjects.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
                levelUpObjects.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);
                levelUpObjects.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = acorn.ToString();
                if (cshBasicInformation.acornNum >= acorn)
                {
                    levelUpButton.GetComponent<Button>().interactable = true;
                    levelUpObjects.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(0, 0, 0, 255);
                }
                else
                {
                    levelUpButton.GetComponent<Button>().interactable = false;
                    levelUpObjects.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(255, 0, 0, 255);
                }
            }
            else
            {
                levelUpObjects.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
                levelUpObjects.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(true);
                levelUpObjects.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = acorn.ToString();
                levelUpObjects.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>().sprite = productsInformation.productArray[currentCharacter.levelUpIngredient].image;
                levelUpObjects.transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ingredient.ToString();
                levelUpObjects.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(0, 0, 0, 255);
                levelUpObjects.transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(0, 0, 0, 255);
                if (cshBasicInformation.acornNum >= acorn && productsInformation.productArray[currentCharacter.levelUpIngredient].quantity >= ingredient)
                {
                    levelUpButton.GetComponent<Button>().interactable = true;
                }
                else
                {
                    // ���丮 �Ǵ� ��ᰡ ������ ���
                    levelUpButton.GetComponent<Button>().interactable = false;
                    if(cshBasicInformation.acornNum < acorn)
                    {
                        levelUpObjects.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(255, 0, 0, 255);
                    }
                    if(productsInformation.productArray[currentCharacter.levelUpIngredient].quantity < ingredient)
                    {
                        levelUpObjects.transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(255, 0, 0, 255);
                    }
                }
            }
        }
    }

    public void characterLevelUp()
    {
        cshBasicInformation.acornNum -= int.Parse(levelUpObjects.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);
        if (currentCharacter.level >= 2)
        {
            productsInformation.productArray[currentCharacter.levelUpIngredient].quantity -= int.Parse(levelUpObjects.transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);
        }
        currentCharacter.level++;
        refreshCharacterWindow();
    }

    public void closeCharacterWindow()
    {
        isTooltipOn = false;
        characterWindow.SetActive(false);
    }

    // �Ǽ� UI
    private void initStructureToBuild()
    {
        for (int i = 0; i < buildStructureListObject.transform.childCount; i++)
        {
            Destroy(buildStructureListObject.transform.GetChild(i).gameObject);
        }
    }

    private void createStructureBuildButton(GameObject button, Structure structure)
    {
        button.transform.SetParent(buildStructureListObject.transform, false);
        button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = structure.price.ToString();
        if (cshBasicInformation.acornNum >= structure.price)
        {
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(0, 0, 0, 255);
            button.GetComponent<Button>().interactable = true;
        }
        else
        {
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(255, 0, 0, 255);
            button.GetComponent<Button>().interactable = false;
        }
    }

    public void openBuild()
    {
        status = "build";
        changeUI(normalUI, buildUI);
        showStructureToBuild(0);
    }

    public void closeBuild()
    {
        status = "village";
        deselectStructureToModify();
        structureModifyButtons.SetActive(false);
        changeUI(buildUI, normalUI);
    }

    public void showStructureToBuild(int n)
    {
        // �Ǽ� ������ ���������� ������
        // n : 0�� ��� �ü�, 1�� ��� �Ĺ�, 2�� ��� ���� ����Ʈ�� ������
        Structure[] structureArray;
        initStructureToBuild();

        if (n == 0)
        {
            structureArray = structureInformation.facilityArray;
            for (int i = 0; i < structureArray.Length; i++)
            {
                if (structureInformation.limitedNumberArray[i] > 0)
                {
                    int temp = i;
                    GameObject button = Instantiate(structureInformation.buildFacilityButton[i]);
                    createStructureBuildButton(button, structureArray[i]);
                    button.GetComponent<Button>().onClick.AddListener(() => startBuildStructure(temp)); 
                }
            }
        }
        else if (n == 1)
        {
            structureArray = structureInformation.plantArray;
            for (int i = 0; i < structureArray.Length; i++)
            {
                if (structureInformation.limitedNumberArray[i + 2] > 0)
                {
                    int temp = i + 2;
                    GameObject button = Instantiate(structureInformation.buildPlantButton[i]);
                    createStructureBuildButton(button, structureArray[i]);
                    button.GetComponent<Button>().onClick.AddListener(() => startBuildStructure(temp));
                }
            }
        }
        else 
        {
            structureArray = structureInformation.storeArray;
            for (int i = 0; i < structureArray.Length; i++)
            {
                if (structureInformation.limitedNumberArray[i + 10] > 0)
                {
                    int temp = i + 10;
                    GameObject button = Instantiate(structureInformation.buildStoreButton[i]);
                    createStructureBuildButton(button, structureArray[i]);
                    button.GetComponent<Button>().onClick.AddListener(() => startBuildStructure(temp));
                }
            }
        }
    }

    public void startBuildStructure(int n)
    {
        // ���� �������� �Ǽ��� ��� ������ Prefab�� ����
        int[] pos = getCurrentCameraPosition();
        int posX = pos[0], posY = pos[1];
        isNew = true;

        deselectStructureToModify();
        if(n < 2)
        {
            selectedStructureInformation = new StructureInstance(structureInformation.facilityArray[n]);
            selectedStructure = structureInformation.createStructure(selectedStructureInformation, posX, posY, false);
        }
        else if(n >= 2 && n < 10)
        {
            selectedStructureInformation = new StructureInstance(structureInformation.plantArray[n - 2]);
            selectedStructure = structureInformation.createStructure(selectedStructureInformation, posX, posY, false);
        }
        else 
        {
            selectedStructureInformation = new StructureInstance(structureInformation.storeArray[n - 10]);
            selectedStructure = structureInformation.createStructure(selectedStructureInformation, posX, posY, false);
        }

        selectedStructure.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        startMoveStructure();
    }

    public void selectStructureToModify(Transform parent)
    {
        // �Ǽ��� �������� ������ ��� �������� ���������� �̵� �� ö�� ��ư�� ������
        if (selectedStructure != null)
        {
            // ���� ������ ������ ������
            selectedStructure.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        }
        selectedStructure = parent.GetChild(1).transform.GetChild(0).gameObject;
        selectedStructure.GetComponent<SpriteRenderer>().color = new Color32(100, 100, 100, 100);
        structureModifyButtons.SetActive(true);
    }

    public void deselectStructureToModify()
    {
        // ���õ� �������� ���� ����� ��� �������� ����������
        if (selectedStructure != null)
        {
            selectedStructure.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
            selectedStructure = null;
        }
        selectedStructureInformation = null;
        structureModifyButtons.SetActive(false);
    }

    public void startMoveStructure()
    {
        status = "build-move";
        changeUI(buildUI, buildMoveUI);
        structureModifyButtons.SetActive(false);

        int posX = selectedStructure.transform.parent.transform.parent.gameObject.GetComponent<cshGroundPosition>().posX;
        int posY = selectedStructure.transform.parent.transform.parent.gameObject.GetComponent<cshGroundPosition>().posY;
        startStructurePositionX = posX;
        startStructurePositionY = posY;
        previousStructurePositionX = posX;
        previousStructurePositionY = posY;
        selectedStructure.GetComponent<SpriteRenderer>().sortingOrder += 1;

        checkStructurePosition();
    }

    public void checkStructurePosition()
    {
        // ī�޶��� ��ġ�� ����Ǿ����� Ȯ���ϰ� 
        int posX = -1, posY = -1;
        Vector2 pos = Camera.main.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

        if (hit.collider != null)
        {
            if (hit.collider.name.Contains("Ground"))
            {
                GameObject par = hit.transform.parent.gameObject;
                posX = par.GetComponent<cshGroundPosition>().posX;
                posY = par.GetComponent<cshGroundPosition>().posY;
            }
        }

        // ����Ǿ��ٸ� �������� �̵�
        if (posX != previousStructurePositionX || posY != previousStructurePositionY)
        {
            structureInformation.moveStructure(selectedStructure, posX, posY, true);
            previousStructurePositionX = posX;
            previousStructurePositionY = posY;
        }

        // �̹� �ٶ󺸴� ��ġ�� �������� �ִٸ� �Ǽ��� �� ����
        if(hit.transform.parent.transform.GetChild(1).childCount > 1)
        {
            selectedStructure.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 150);
            structureMoveConfirmButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            selectedStructure.GetComponent<SpriteRenderer>().color = new Color32(100, 100, 100, 100);
            structureMoveConfirmButton.GetComponent<Button>().interactable = true;
        }
    }

    public void cancelMoveStructure()
    {
        if (isNew)
        {
            // ���ο� ������ �Ǽ��� ���
            Destroy(selectedStructure);
        }
        else
        {
            // �Ǽ��� ������ �̵� ���
            structureInformation.moveStructure(selectedStructure, startStructurePositionX, startStructurePositionY, false);
        }
        deselectStructureToModify();
        changeUI(buildMoveUI, buildUI);
        status = "build";
        isNew = false;
    }

    public void confirmStructurePosition()
    {
        // ���ο� ������ �Ǽ� Ȯ��, �Ǽ��� ������ �̵� Ȯ��
        structureInformation.confirmStructurePosition(selectedStructure, startStructurePositionX, startStructurePositionY, previousStructurePositionX, previousStructurePositionY, selectedStructureInformation);
        deselectStructureToModify();
        changeUI(buildMoveUI, buildUI);
        showStructureToBuild(0);
        status = "build";
        isNew = false;
    }

    public void openRemoveStructureAlertDialog()
    {
        // ���õ� �������� ���� ö�� ��ư�� ���� ���
        status = "ui";
        buildUI.SetActive(false);
        structureRemoveAlertDialog.SetActive(true);
        structureRemoveAlertDialog.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = structureInformation.getStructureName(selectedStructure);
    }

    public void closeRemoveStructureAlertDialog()
    {
        structureRemoveAlertDialog.SetActive(false);
        buildUI.SetActive(true);
        showStructureToBuild(0);
        Invoke("setStatusBuild", 0.01f);
        deselectStructureToModify();
    }

    public void removeStructure()
    {
        structureInformation.removeStructure(selectedStructure);
        selectedStructure = null;
        closeRemoveStructureAlertDialog();
    }

    // ������ UI
    public void openStructure(int x, int y)
    {
        changeUI(normalUI, structureUI);
        status = "ui";
        currentProduceStructure = structureInformation.structureArray[x, y];
        isShowStructureStatus = true;

        structureUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = currentProduceStructure.structure.name;
    }

    private void showStructureProduce()
    {
        // ���� ���� �ð� �����ֱ�
        if (currentProduceStructure.structure.GetType().Name == "Store")
        {
            Transform sliders = produceSliders.transform.GetChild(0);
            sliders.gameObject.SetActive(true);
            sliders.GetChild(0).GetComponent<Slider>().value = currentProduceStructure.currentAcornTime / 20f;
            sliders.GetChild(0).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (20 - (int)(currentProduceStructure.currentAcornTime)).ToString() + "��";
            sliders.GetChild(1).GetComponent<Slider>().value = currentProduceStructure.currentProductTime / currentProduceStructure.structure.productTime;
            sliders.GetChild(1).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = ((int)(currentProduceStructure.structure.productTime) - (int)(currentProduceStructure.currentProductTime)).ToString() + "��";
            sliders.GetChild(3).GetComponent<TextMeshProUGUI>().text = "X " + currentProduceStructure.structure.acorn;
            sliders.GetChild(4).GetComponent<Image>().sprite = productsInformation.productArray[currentProduceStructure.structure.product].image;
        }
        else if (currentProduceStructure.structure.GetType().Name == "Plant")
        {
            Transform sliders = produceSliders.transform.GetChild(1);
            sliders.gameObject.SetActive(true);
            sliders.GetChild(0).GetComponent<Slider>().value = currentProduceStructure.currentProductTime / currentProduceStructure.structure.productTime;
            sliders.GetChild(0).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = ((int)(currentProduceStructure.structure.productTime) - (int)(currentProduceStructure.currentProductTime)).ToString() + "��";
            sliders.GetChild(1).GetComponent<Image>().sprite = productsInformation.productArray[currentProduceStructure.structure.product].image;
        }
    }

    public void closeStructure()
    {
        produceSliders.transform.GetChild(0).gameObject.SetActive(false);
        produceSliders.transform.GetChild(1).gameObject.SetActive(false);
        isShowStructureStatus = false;
        Invoke("setStatusVillage", 0.01f);
        changeUI(structureUI, normalUI);
    }

    // ����Ʈ UI
    public void openQuest()
    {
        status = "ui";
        changeUI(normalUI, questUI);
        isQuestClear = false;
        updateQuest();
    }

    public void closeQuest()
    {
        // ����Ʈ 4 Ŭ�����ϸ� ���ο� ������ ��� ����
        if(cshBasicInformation.currentQuest == 4 && isQuestClear)
        {
            questUI.SetActive(false);
            Invoke("openUnlockStructureDialog", 0.5f);
        }
        else
        {
            changeUI(questUI, normalUI);
            Invoke("setStatusVillage", 0.01f);
        }
    }

    private void updateQuest()
    {
        for (int i = 0; i < questListObject.childCount; i++)
        {
            if (i + 1 < cshBasicInformation.currentQuest)
            {
                questListObject.GetChild(i).gameObject.SetActive(true);
                questListObject.GetChild(i).GetComponent<Image>().color = new Color32(180, 180, 180, 255);
                questListObject.GetChild(i).transform.GetChild(0).transform.localPosition = new Vector2(-220f, 0f);
                questListObject.GetChild(i).transform.GetChild(1).gameObject.SetActive(false);
                questListObject.GetChild(i).transform.GetChild(4).gameObject.SetActive(false);
                questListObject.GetChild(i).transform.GetChild(5).gameObject.SetActive(true);
            }
            else if (i + 1 == cshBasicInformation.currentQuest)
            {
                questListObject.GetChild(i).gameObject.SetActive(true);
                questListObject.GetChild(i).GetComponent<Image>().color = new Color32(204, 222, 180, 255);
                questListObject.GetChild(i).transform.GetChild(4).gameObject.SetActive(questInformation.questIsComplete());
                questListObject.GetChild(i).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "(" + questInformation.currentComplete + "/" + questInformation.purposeComplete + ")";
            }
            else
            {
                questListObject.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    // ������ �� ĳ���� �ر� ����
    public void openUnlockStructureDialog()
    {
        int chapter = 0;
        unlockStructureDialog.SetActive(true);
        if (cshBasicInformation.currentChapter != 0)
        {
            chapter = cshBasicInformation.currentChapter - 1;
        }

        if(chapter == 0)
        {
            structureInformation.limitedNumberArray[3] = 12;
            structureInformation.limitedNumberArray[4] = 12;
            structureInformation.limitedNumberArray[8] = 12;
            structureInformation.limitedNumberArray[13] = 4;
            structureInformation.limitedNumberArray[14] = 4;
        }
        else if(chapter == 1)
        {
            structureInformation.limitedNumberArray[5] = 12;
            structureInformation.limitedNumberArray[9] = 12;
            structureInformation.limitedNumberArray[15] = 4;
            structureInformation.limitedNumberArray[16] = 4;
            structureInformation.limitedNumberArray[17] = 4;
        }
        else
        {
            structureInformation.limitedNumberArray[6] = 12;
            structureInformation.limitedNumberArray[18] = 4;
            structureInformation.limitedNumberArray[19] = 4;
        }

        for (int i = 0; i < 3; i++)
        {
            unlockStructureDialog.transform.GetChild(3).transform.GetChild(chapter).gameObject.SetActive(false);
        }

        unlockStructureDialog.transform.GetChild(3).transform.GetChild(chapter).gameObject.SetActive(true);
    }

    public void closeUnlockStructureDialog()
    {
        unlockStructureDialog.SetActive(false);
        if (cshBasicInformation.currentChapter == 0)
        {
            normalUI.SetActive(true);
            Invoke("setStatusVillage", 0.01f);
        }
        else
        {
            openAdventure();
        }
    }

    public void openUnlockCharacterDialog(int n)
    {
        unlockCharacterDialog.SetActive(true);
        isQuestClear = true;
        cshBasicInformation.currentQuest++;
        cshBasicInformation.acornNum += questInformation.reward[n];
        unlockCharacterDialog.transform.GetChild(2).GetComponent<Image>().sprite = characterInformation.characterArray[n+1].imageRect;
        characterInformation.characterArray[n+1].level = 0;
        characterInformation.spawnCharacter(n + 1);
        if (n == 3) cshBasicInformation.currentChapter = 1;
    }

    public void closeUnlockCharacterDialog()
    {
        updateQuest();
        unlockCharacterDialog.SetActive(false);
    }

    // Ž�� UI
    public void openAdventure()
    {
        if(cshBasicInformation.currentChapter == 0)
        {
            floatNotice("����־��!\n(�ֹ� 5�� ����)");
            return;
        }
        // Ž�� ������ ������ ������
        Camera.main.transform.position = new Vector3(0f, 1.5f, -10f);
        Camera.main.GetComponent<Camera>().orthographicSize = 9;
        status = "ui";
        changeUI(normalUI, adventureUI);

        if(cshBasicInformation.currentChapter >= 2)
        {
            adventureUI.transform.GetChild(0).gameObject.SetActive(false);
            adventureUI.transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
            adventureUI.transform.GetChild(1).transform.GetChild(1).gameObject.SetActive(false);
        }
        if (cshBasicInformation.currentChapter >= 3)
        {
            adventureUI.transform.GetChild(1).gameObject.SetActive(false);
            adventureUI.transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(true);
            adventureUI.transform.GetChild(2).transform.GetChild(1).gameObject.SetActive(false);
        }
        if (cshBasicInformation.currentChapter >= 4)
        {
            adventureUI.transform.GetChild(2).gameObject.SetActive(false);
        }
    }

    public void closeAdventure()
    {
        Camera.main.transform.position = new Vector3(0f, 0f, -10f);
        Camera.main.GetComponent<Camera>().orthographicSize = 4;
        changeUI(adventureUI, normalUI);
        Invoke("setStatusVillage", 0.01f);
    }

    public void openClosedChapter(bool isNotReady)
    {
        if (isNotReady)
        {
            floatNotice("�غ����Դϴ�.");
        }
        else
        {
            floatNotice("���� ����\n(é��2 Ŭ����)");
        }
    }

    private void floatNotice(string notice)
    {
        // ���� ���õ� ��ư�� ���� �˸��� ���
        Transform currentEventUI = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform;
        GameObject noticeTextObject = Instantiate(noticeText);
        noticeTextObject.transform.SetParent(currentEventUI);
        noticeTextObject.transform.localPosition = new Vector2(0f, 100f);
        noticeTextObject.GetComponent<cshNotice>().startSetting(notice);
    }

    // ĳ���� ���� UI
    public void openCharacterSelect()
    {
        int n = cshBasicInformation.currentChapter;

        // é�Ϳ� ���� �ٸ� ���� ����Ʈ�� ������
        for (int i = 0; i < 3; i++)
        {
            areaNameObject.GetChild(i).gameObject.SetActive(false);
            enemyListObject.GetChild(i).gameObject.SetActive(false);
        }
        areaNameObject.GetChild(n-1).gameObject.SetActive(true);
        enemyListObject.GetChild(n-1).gameObject.SetActive(true);

        selectedCharacterLength = 0;
        for(int i = 0; i < 10; i++)
        {
            if(characterInformation.characterArray[i].level == -1)
            {
                // ĳ���͸� �ر����� ����
                characterIsSelected[i] = -1;
            }
            else
            {
                if(characterIsSelected[i] == 1) selectedCharacterLength++;
            }
        }
        changeUI(adventureUI, selectUI);
        updateCharacterSelect();
    }

    public void closeCharacterSelect()
    {
        changeUI(selectUI, adventureUI);
        updateCharacterSelect();
    }

    public void includeCharacterInTeam(int n)
    {
        if(selectedCharacterLength < 5)
        {
            characterIsSelected[n] = 1;
            selectedCharacterLength++;
            updateCharacterSelect();
        }
    }

    public void excludeCharacterInTeam(int n)
    {
        characterIsSelected[n] = 0;
        selectedCharacterLength--;
        updateCharacterSelect();
    }

    public void updateCharacterSelect()
    {
        // ���õ� ĳ���Ϳ� ���õ��� ���� ĳ���͸� �����Ͽ� ��ġ
        int idx = 0;
        for(int i = 0; i < 5; i++)
        {
            if(selectedCharacterListObject.transform.GetChild(i).childCount > 0)
            {
                Destroy(selectedCharacterListObject.GetChild(i).transform.GetChild(0).gameObject);
            }
        }
        for(int i = 0; i < deselectedCharacterListObject.childCount;i++)
        {
            Destroy(deselectedCharacterListObject.GetChild(i).gameObject);
        }

        for(int i = 0; i < 10; i++)
        {
            // ���� �رݵ��� ���� ĳ���ʹ� ������ �� ����
            if(characterIsSelected[i] != -1)
            {
                int temp = i;
                GameObject button = Instantiate(characterInformation.characterArray[i].button);
                if (characterIsSelected[i] == 1)
                {
                    // ���õ� ĳ���ʹ� ���� �гο� �߰��ϰ� Ŭ���ϸ� ���� ����
                    Transform par = selectedCharacterListObject.GetChild(idx++);
                    button.transform.position = par.position;
                    button.GetComponent<Button>().onClick.AddListener(() => excludeCharacterInTeam(temp));
                    button.transform.SetParent(par);
                }
                else
                {
                    // ���õ��� ���� ĳ���ʹ� ���� �гο� �߰��ϰ� Ŭ���ϸ� ���õ�
                    button.GetComponent<Button>().onClick.AddListener(() => includeCharacterInTeam(temp));
                    button.transform.SetParent(deselectedCharacterListObject);
                }
            } 
        }
    }

    public void duelStart()
    {
        // ���潺 ���� ����
        basicInformation.saveData();

        if(selectedCharacterLength == 5)
        {
            int idx = 0;
            for(int i = 0; i < 10; i++)
            {
                if (characterIsSelected[i] == 1)
                {
                    cshBasicInformation.selectedCharacters[idx++] = i + 1;
                }
                cshBasicInformation.characterLevels[i] = characterInformation.characterArray[i].level;
            }
            showLoading();
            Invoke("changeSceneVillageToDuel", 3.0f);
        }
    }

    private void changeSceneVillageToDuel()
    {
        SceneManager.LoadScene("Duel Scene");
    } 

    // ���潺 ���� ���� �� UI
    private void victoryDuel()
    {
        int acornPoint = 0;
        if(cshBasicInformation.currentChapter == 1)
        {
            acornPoint = 5000;
        }
        else if(cshBasicInformation.currentChapter == 2)
        {
            acornPoint = 10000;
        }
        else
        {
            acornPoint = 15000;
        }
        battleVictoryAcornPointText.GetComponent<TextMeshProUGUI>().text = acornPoint.ToString();
    }

    public void closeVictoryDialog()
    {
        // �¸����� ��� ������ �ް� ������ ������ �Ǽ� ������ ������ �þ
        int acornPoint = 0;
        if (cshBasicInformation.currentChapter == 1)
        {
            acornPoint = 5000;
        }
        else if (cshBasicInformation.currentChapter == 2)
        {
            acornPoint = 10000;
        }
        else
        {
            acornPoint = 15000;
        }
        cshBasicInformation.acornNum += acornPoint;
        removeCloud(-1 + cshBasicInformation.currentChapter++);
        afterBattleUI.transform.GetChild(0).gameObject.SetActive(false);
        if(cshBasicInformation.currentChapter < 4)
        {
            Invoke("openUnlockStructureDialog", 0.5f);
        }
        else
        {
            openAdventure();
        }
    }

    public void closeFailDialog()
    {
        afterBattleUI.transform.GetChild(1).gameObject.SetActive(false);
        openAdventure();
    }

    public void removeCloud(int n)
    {
        structureInformation.removeCloud(n);
    }

    // ���� UI
    public void openSetting()
    {
        status = "ui";
        changeUI(normalUI, settingUI);
        autoSave = cshBasicInformation.autoSave;
        setAutoSave(autoSave);
    }

    public void setAutoSave(bool autoSave)
    {
        // �ڵ� ���� ���� ��� ��ư
        this.autoSave = autoSave;
        autoSaveToggleButton.GetChild(0).gameObject.SetActive(autoSave);
        autoSaveToggleButton.GetChild(1).gameObject.SetActive(!autoSave);
    }

    public void saveSetting()
    {
        cshBasicInformation.autoSave = autoSave;
        closeSetting();
    }

    public void closeSetting()
    {
        Invoke("setStatusVillage", 0.01f);
        changeUI(settingUI, normalUI);
    }

    // ���� UI
    public void saveData()
    {
        basicInformation.saveData();
        status = "ui";
        normalUI.SetActive(false);
        saveCompleteDialog.SetActive(true);
    }

    public void closeSaveComplete()
    {
        Invoke("setStatusVillage", 0.01f);
        saveCompleteDialog.SetActive(false);
        normalUI.SetActive(true);
    }

    // �ʱ� �����ϱ�
    public void startSetting()
    {
        // ������ ���� �����ߴ��� Ž��(���潺 ����)�� ��ģ �������� �����Ͽ� �ٸ��� ����
        if (cshBasicInformation.startUI == 0)
        {
            titleUI.transform.GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            Camera.main.transform.position = new Vector3(0f, 1.5f, -10f);
            Camera.main.GetComponent<Camera>().orthographicSize = 9;
            status = "ui";
            afterBattleUI.SetActive(true);
            if (cshBasicInformation.startUI == 1)
            {
                afterBattleUI.transform.GetChild(0).gameObject.SetActive(true);
                afterBattleUI.transform.GetChild(0).transform.GetChild(1).transform.GetChild(cshBasicInformation.currentChapter - 1).gameObject.SetActive(true);
                victoryDuel();
            }
            else
            {
                afterBattleUI.transform.GetChild(1).gameObject.SetActive(true);
                afterBattleUI.transform.GetChild(1).transform.GetChild(1).transform.GetChild(cshBasicInformation.currentChapter - 1).gameObject.SetActive(true);
            }
        }
        
        isStart = true;
    }

    private void setStatusVillage()
    {
        status = "village";
    }

    private void setStatusBuild()
    {
        status = "build";
    }

    // ���� ī�޶� �ٶ󺸴� ���� ��ǥ ����
    private int[] getCurrentCameraPosition()
    {
        int posX = -1, posY = -1;
        Vector2 pos = Camera.main.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

        if (hit.collider != null)
        {
            if (hit.collider.name.Contains("Ground"))
            {
                GameObject par = hit.transform.parent.gameObject;
                posX = par.GetComponent<cshGroundPosition>().posX;
                posY = par.GetComponent<cshGroundPosition>().posY;
            }
        }
        return new int[2] { posX, posY };
    }
}
