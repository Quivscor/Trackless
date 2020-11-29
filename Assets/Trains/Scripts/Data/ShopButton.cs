using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    public Image image;
    public TMPro.TextMeshProUGUI description;

    public void BuyItem()
    {
        this.GetComponentInParent<ShopCanvasManager>().Buy(this);
    }
}
