using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AwardType
{
    Product,
    Module,
    Credits,
}

public class Award : MonoBehaviour
{
    public int awardCount;
    public AwardType awardType;
    public Modules module;
    public Product product;
    public Credits credits;

    public void SetUpAwardProduct(Product _product, int prodCount)
    {
        if(_product != null)
        {
            awardType = AwardType.Product;
            product = _product;
            awardCount = prodCount;
        }
    }

    public void SetUpAwardModule(Modules _modules, int moduleCount)
    {
        if (_modules != null)
        {
            awardType = AwardType.Module;
            module = _modules;
            awardCount = moduleCount;
        }
    }

    public void SetUpAwardCredits(int credCount)
    {
        awardType = AwardType.Credits;
        awardCount = credCount;
    }
}
