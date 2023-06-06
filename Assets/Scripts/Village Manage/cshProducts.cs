using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Product
{
    public string name;
    public int quantity;
    public int price;
    public Sprite image;
    public GameObject smallPrefab;
    public GameObject button;

    public Product(string name, int quantity, int price, Sprite image, GameObject smallPrefab, GameObject button)
    {
        this.name = name;
        this.quantity = quantity;
        this.price = price;
        this.image = image;
        this.smallPrefab = smallPrefab;
        this.button = button;
    }
}

public class cshProducts : MonoBehaviour
{
    private string[] productName = new string[18] { "오렌지", "블루베리", "코코넛", "벚꽃", "죽순", "튤립", "당근", "수박", "밀가루", "커피", "식빵", "비타민", "야채바구니", "수제버거", "곰인형", "꽃다발", "와인", "다이아몬드" };
    private int[] productQuantity = new int[18] { 100, 100, 100, 100, 100, 100, 100, 100, 20, 20, 20, 20, 20, 20, 20, 20, 20 ,20 };
    private int[] productPrice = new int[18] { 2, 5, 10, 20, 40, 5, 10, 20, 5, 10, 10, 20, 20, 50, 50, 50, 100, 100 };
    public Sprite[] productImages = new Sprite[18];
    public GameObject[] productSmallPrefabs = new GameObject[18];
    public GameObject[] productButtons = new GameObject[18];
    public Product[] productArray = new Product[18];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startSetting()
    {
        productQuantity = cshBasicInformation.productQuantity;
        for (int i = 0; i < 18; i++)
        {
            productArray[i] = new Product(productName[i], productQuantity[i], productPrice[i], productImages[i], productSmallPrefabs[i], productButtons[i]);
        }
    }
}
