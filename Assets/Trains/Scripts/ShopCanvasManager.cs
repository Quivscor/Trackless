using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShopCanvasManager : MonoBehaviour
{
    private class ButtonToWagon
    {
        public WagonShop wagonShop;
        public Button button;

        public ButtonToWagon(WagonShop wagonShop, Button button)
        {
            this.wagonShop = wagonShop;
            this.button = button;
        }
    }

    public List<WagonShop> wagonsInOffer = new List<WagonShop>();
    public List<Button> buyButtons = new List<Button>();
    public TMPro.TextMeshProUGUI steelDisplay;

    private List<ButtonToWagon> offer = new List<ButtonToWagon>();
    private int steel = 0;

    public void PrepareOffer()
    {
        this.GetComponent<Canvas>().enabled = true;
        GetSteel();

        foreach (Button button in buyButtons)
        {
            WagonShop wagonToOffer;

            wagonToOffer = wagonsInOffer[Random.Range(0, wagonsInOffer.Count)];

            offer.Add(new ButtonToWagon(wagonToOffer, button));

            if (steel < wagonToOffer.steelPrice)
                button.interactable = false;
        }

        UpdateImagesAndDescriptions();
    }

    private void GetSteel()
    {
        if (PlayerPrefs.HasKey("steel"))
        {
            steel = PlayerPrefs.GetInt("steel");
        }
        else
        {
            steel = 0;
        }

        steelDisplay.text = steel + "";
    }

    private void UpdateImagesAndDescriptions()
    {
        foreach (ButtonToWagon btw in offer)
        {
            btw.button.GetComponent<ShopButton>().image.sprite = btw.wagonShop.image;
            btw.button.GetComponent<ShopButton>().description.text = btw.wagonShop.description;
        }
    }

    public void Buy(ShopButton button)
    {
        foreach (ButtonToWagon btw in offer)
        {
            if (btw.button.gameObject != button.gameObject)
                continue;

            if (steel >= btw.wagonShop.steelPrice)
            {
                btw.button.GetComponent<Button>().interactable = false;

                steel -= btw.wagonShop.steelPrice;

                TrainDataContainer.Instance.AddWagon(btw.wagonShop.wagonType);

                steelDisplay.text = steel + "";
            }
        }
    }

    public void LoadNewLevel()
    {
        PlayerPrefs.SetInt("steel", steel);
        PlayerPrefs.Save();

        SceneManager.LoadScene("Map Generating");
    }

}
