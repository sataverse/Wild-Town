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

    // 캐릭터 UI
    private Character currentCharacter;

    // 창고 UI
    private int sellIndex;

    // 상점 UI
    private StructureInstance currentProduceStructure;
    private bool isShowStructureStatus = false;

    // 건설 UI
    private GameObject selectedStructure = null;
    private StructureInstance selectedStructureInformation = null;
    private bool isNew = false;
    private int startStructurePositionX = -1;
    private int startStructurePositionY = -1;
    private int previousStructurePositionX = -1;
    private int previousStructurePositionY = -1;

    // 전투 UI
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

    // 창고 UI
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
        // 창고 항목 터치시 판매 가능
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
        // 판매 수량 슬라이더 변경할 경우
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

    // 주민 UI
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
        // 주민 항목 터치하면 주민 세부 사항 확인
        currentCharacter = characterInformation.characterArray[n];
        characterWindow.SetActive(true);
        isTooltipOn = false;
        characterNameText.GetComponent<TextMeshProUGUI>().text = currentCharacter.name;
        characterSkillNameText.GetComponent<TextMeshProUGUI>().text = currentCharacter.skillName;
        characterSkillDirectionsText.GetComponent<TextMeshProUGUI>().text = currentCharacter.skillDirections;
        characterSkillCoolTimeText.GetComponent<TextMeshProUGUI>().text = currentCharacter.skillCoolTime.ToString() + "초";
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
        // 스킬 설명을 보여주는 툴팁
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
        // 레벨에 따라 스탯, 레벨업 비용 등을 다르게 나타냄
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
                    // 도토리 또는 재료가 부족할 경우
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

    // 건설 UI
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
        // 건설 가능한 구조물들을 보여줌
        // n : 0일 경우 시설, 1일 경우 식물, 2일 경우 상점 리스트를 보여줌
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
        // 새로 구조물을 건설할 경우 구조물 Prefab을 생성
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
        // 건설된 구조물을 선택할 경우 구조물이 투명해지고 이동 및 철거 버튼이 보여짐
        if (selectedStructure != null)
        {
            // 원래 선택한 구조물 불투명
            selectedStructure.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        }
        selectedStructure = parent.GetChild(1).transform.GetChild(0).gameObject;
        selectedStructure.GetComponent<SpriteRenderer>().color = new Color32(100, 100, 100, 100);
        structureModifyButtons.SetActive(true);
    }

    public void deselectStructureToModify()
    {
        // 선택된 구조물을 선택 취소할 경우 구조물이 불투명해짐
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
        // 카메라의 위치가 변경되었는지 확인하고 
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

        // 변경되었다면 구조물을 이동
        if (posX != previousStructurePositionX || posY != previousStructurePositionY)
        {
            structureInformation.moveStructure(selectedStructure, posX, posY, true);
            previousStructurePositionX = posX;
            previousStructurePositionY = posY;
        }

        // 이미 바라보는 위치에 구조물이 있다면 건설할 수 없음
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
            // 새로운 구조물 건설을 취소
            Destroy(selectedStructure);
        }
        else
        {
            // 건설된 구조물 이동 취소
            structureInformation.moveStructure(selectedStructure, startStructurePositionX, startStructurePositionY, false);
        }
        deselectStructureToModify();
        changeUI(buildMoveUI, buildUI);
        status = "build";
        isNew = false;
    }

    public void confirmStructurePosition()
    {
        // 새로운 구조물 건설 확정, 건설된 구조물 이동 확정
        structureInformation.confirmStructurePosition(selectedStructure, startStructurePositionX, startStructurePositionY, previousStructurePositionX, previousStructurePositionY, selectedStructureInformation);
        deselectStructureToModify();
        changeUI(buildMoveUI, buildUI);
        showStructureToBuild(0);
        status = "build";
        isNew = false;
    }

    public void openRemoveStructureAlertDialog()
    {
        // 선택된 구조물에 대한 철거 버튼을 누를 경우
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

    // 구조물 UI
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
        // 남은 생산 시간 보여주기
        if (currentProduceStructure.structure.GetType().Name == "Store")
        {
            Transform sliders = produceSliders.transform.GetChild(0);
            sliders.gameObject.SetActive(true);
            sliders.GetChild(0).GetComponent<Slider>().value = currentProduceStructure.currentAcornTime / 20f;
            sliders.GetChild(0).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (20 - (int)(currentProduceStructure.currentAcornTime)).ToString() + "초";
            sliders.GetChild(1).GetComponent<Slider>().value = currentProduceStructure.currentProductTime / currentProduceStructure.structure.productTime;
            sliders.GetChild(1).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = ((int)(currentProduceStructure.structure.productTime) - (int)(currentProduceStructure.currentProductTime)).ToString() + "초";
            sliders.GetChild(3).GetComponent<TextMeshProUGUI>().text = "X " + currentProduceStructure.structure.acorn;
            sliders.GetChild(4).GetComponent<Image>().sprite = productsInformation.productArray[currentProduceStructure.structure.product].image;
        }
        else if (currentProduceStructure.structure.GetType().Name == "Plant")
        {
            Transform sliders = produceSliders.transform.GetChild(1);
            sliders.gameObject.SetActive(true);
            sliders.GetChild(0).GetComponent<Slider>().value = currentProduceStructure.currentProductTime / currentProduceStructure.structure.productTime;
            sliders.GetChild(0).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = ((int)(currentProduceStructure.structure.productTime) - (int)(currentProduceStructure.currentProductTime)).ToString() + "초";
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

    // 퀘스트 UI
    public void openQuest()
    {
        status = "ui";
        changeUI(normalUI, questUI);
        isQuestClear = false;
        updateQuest();
    }

    public void closeQuest()
    {
        // 퀘스트 4 클리어하면 새로운 구조물 잠금 해제
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

    // 구조물 및 캐릭터 해금 관리
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

    // 탐험 UI
    public void openAdventure()
    {
        if(cshBasicInformation.currentChapter == 0)
        {
            floatNotice("잠겨있어요!\n(주민 5명 보유)");
            return;
        }
        // 탐험 가능한 지역을 보여줌
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
            floatNotice("준비중입니다.");
        }
        else
        {
            floatNotice("입장 조건\n(챕터2 클리어)");
        }
    }

    private void floatNotice(string notice)
    {
        // 현재 선택된 버튼에 대한 알림을 띄움
        Transform currentEventUI = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform;
        GameObject noticeTextObject = Instantiate(noticeText);
        noticeTextObject.transform.SetParent(currentEventUI);
        noticeTextObject.transform.localPosition = new Vector2(0f, 100f);
        noticeTextObject.GetComponent<cshNotice>().startSetting(notice);
    }

    // 캐릭터 선택 UI
    public void openCharacterSelect()
    {
        int n = cshBasicInformation.currentChapter;

        // 챕터에 따라 다른 몬스터 리스트를 보여줌
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
                // 캐릭터를 해금하지 못함
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
        // 선택된 캐릭터와 선택되지 않은 캐릭터를 구분하여 배치
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
            // 아직 해금되지 않은 캐릭터는 선택할 수 없음
            if(characterIsSelected[i] != -1)
            {
                int temp = i;
                GameObject button = Instantiate(characterInformation.characterArray[i].button);
                if (characterIsSelected[i] == 1)
                {
                    // 선택된 캐릭터는 선택 패널에 추가하고 클릭하면 선택 해제
                    Transform par = selectedCharacterListObject.GetChild(idx++);
                    button.transform.position = par.position;
                    button.GetComponent<Button>().onClick.AddListener(() => excludeCharacterInTeam(temp));
                    button.transform.SetParent(par);
                }
                else
                {
                    // 선택되지 않은 캐릭터는 비선택 패널에 추가하고 클릭하면 선택됨
                    button.GetComponent<Button>().onClick.AddListener(() => includeCharacterInTeam(temp));
                    button.transform.SetParent(deselectedCharacterListObject);
                }
            } 
        }
    }

    public void duelStart()
    {
        // 디펜스 게임 시작
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

    // 디펜스 게임 종류 후 UI
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
        // 승리했을 경우 보상을 받고 구름을 제거해 건설 가능한 공간이 늘어남
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

    // 설정 UI
    public void openSetting()
    {
        status = "ui";
        changeUI(normalUI, settingUI);
        autoSave = cshBasicInformation.autoSave;
        setAutoSave(autoSave);
    }

    public void setAutoSave(bool autoSave)
    {
        // 자동 저장 여부 토글 버튼
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

    // 저장 UI
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

    // 초기 설정하기
    public void startSetting()
    {
        // 게임을 지금 시작했는지 탐험(디펜스 게임)을 마친 상태인지 구분하여 다르게 동작
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

    // 현재 카메라가 바라보는 땅의 좌표 리턴
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
