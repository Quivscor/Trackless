using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIDataManager : MonoBehaviour
{
    [SerializeField]
    private GameObject canvas;

    [SerializeField]
    private Image heatMeter;
    [SerializeField]
    private TextMeshProUGUI coalText;
    [SerializeField]
    private TextMeshProUGUI steelText;
    [SerializeField]
    private TextMeshProUGUI passengersText;
    [SerializeField]
    private TextMeshProUGUI maxPassengersText;

    public Image blackscreen;

    [SerializeField]
    private GameObject endGameText;

    private Cauldron cauldron;
    public Inventory inventory;
    public TracklessGenerator.MapGenerator mapGenerator;

    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        FindObjectOfType<TracklessGenerator.MapGenerator>().action += FindReferences;
        StartCoroutine(FadeFromBlack(2f));

    }

    // Update is called once per frame
    void Update()
    {
        UpdateHeatMeter();
        UpdateInventory();
    }

    private void FindReferences()
    {
        cauldron = FindObjectOfType<Cauldron>();
        inventory = FindObjectOfType<Inventory>();
        mapGenerator = FindObjectOfType<TracklessGenerator.MapGenerator>();

        maxPassengersText.text = " / " + mapGenerator.numberOfPassengers;
    }

    private void UpdateHeatMeter()
    {
        heatMeter.fillAmount = cauldron.CurrentCauldronLevel / cauldron.CauldronMaxLevel;
    }

    private void UpdateInventory()
    {
        coalText.text = inventory.Coal.ToString();
        steelText.text = inventory.Steel.ToString();
        passengersText.text = inventory.Passengers.ToString();
        if (inventory.Passengers == mapGenerator.numberOfPassengers)
            TurnOnEndGameText(true, "Great! Find a way out of this place");
        //maxPassengersText.text = " / " + mapGenerator.numberOfPassengers;
    }

    public void TurnOnEndGameText(bool isOn, string text)
    {
        endGameText.SetActive(isOn);
        endGameText.GetComponent<TextMeshProUGUI>().text = text;
    }

    public IEnumerator FadeFromBlack(float time)
    {
        float fullTime = time;
        while (time > 0)
        {
            yield return new WaitForEndOfFrame();
            blackscreen.color = new Color(blackscreen.color.r, blackscreen.color.g, blackscreen.color.b, time / fullTime);
            time -= Time.deltaTime;
        }
        //blackscreen.color = new Color(blackscreen.color.r, blackscreen.color.g, blackscreen.color.b, 1f);
    }

    public IEnumerator FadeToBlack(float time)
    {
        float fullTime = 0;
        while (fullTime < time)
        {
            yield return new WaitForEndOfFrame();
            blackscreen.color = new Color(blackscreen.color.r, blackscreen.color.g, blackscreen.color.b, fullTime / time);
            fullTime += Time.deltaTime;
        }

        EndGameCanvasController.Instance.DisplayCanvas(true);
        //blackscreen.color = new Color(blackscreen.color.r, blackscreen.color.g, blackscreen.color.b, 0f);
    }
}
