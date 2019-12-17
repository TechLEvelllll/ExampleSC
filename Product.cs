using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Product : MonoBehaviour
{
    public int productID;
    public int productCost;
    public string productName;
    public int productWeigth;
    public int curCount;
    public int maxCountBit=100; // max value for slider
    public string descriptionProduct;
    public Color resColor;
    public Sprite prodSprite;
    public SpriteRenderer[] sr;

    public Product(int _productID, int _productCost, string _productName, int _productWeigth, int _curCount, int _maxCountBit, string _descriptionProduct)
    {
        productID = _productID;
        productCost = _productCost;
        productName = _productName;
        productWeigth = _productWeigth;
        curCount = _curCount;
        maxCountBit = _maxCountBit;
        descriptionProduct = _descriptionProduct;
    }
}
