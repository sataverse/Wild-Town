using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshStructureProduce : MonoBehaviour
{
    private GameObject uiManager;
    private GameObject productManager;
    private cshProducts productInfo;
    private bool isStart = false;
    public StructureInstance structureInstance;

    // Start is called before the first frame update
    void Start()
    {
        uiManager = GameObject.Find("UI Manager");
        productManager = GameObject.Find("Product Manager");
        productInfo = productManager.GetComponent<cshProducts>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            // 상점은 일정 시간마다 도토리 생산
            if (structureInstance.structure.GetType().Name == "Store")
            {
                structureInstance.currentAcornTime += Time.deltaTime;
                if (structureInstance.currentAcornTime >= 20f)
                {
                    cshBasicInformation.acornNum += structureInstance.structure.acorn;
                    structureInstance.currentAcornTime = 0f;
                }
            }
            structureInstance.currentProductTime += Time.deltaTime;

            // 생산 완료된 물건을 보여줌
            if (structureInstance.currentProductTime >= structureInstance.structure.productTime)
            {
                GameObject product = Instantiate(productInfo.productArray[structureInstance.structure.product].smallPrefab);
                product.transform.SetParent(transform.parent.parent);
                product.transform.position = new Vector2(transform.parent.parent.position.x, transform.parent.parent.position.y + 0.4f);
                productInfo.productArray[structureInstance.structure.product].quantity++;
                uiManager.GetComponent<cshUIControl>().updateStorage();
                structureInstance.currentProductTime = 0f;
            }
        }
    }

    public void startProduce(StructureInstance structure)
    {
        structureInstance = structure;
        isStart = true;
    }
}
