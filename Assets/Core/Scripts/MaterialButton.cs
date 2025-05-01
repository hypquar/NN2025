using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class MaterialButton : MonoBehaviour
{
    public int materialIndex;
    public WallMaterialManager wallManager;
    public TMP_Text buttonText; 
    public Image coinIcon;     

    [Header("÷вета текста")]
    public Color priceColor = Color.white; 
    public Color selectColor = Color.white;   
    public Color selectedColor = Color.green; 

    private void Start()
    {
        wallManager.OnMaterialSelected += UpdateButtonState;
        UpdateButtonState();
    }

    private void OnDestroy()
    {
        if (wallManager != null)
            wallManager.OnMaterialSelected -= UpdateButtonState;
    }

    public void OnButtonClick()
    {
        if (wallManager.IsMaterialUnlocked(materialIndex))
        {
            wallManager.ApplyMaterial(materialIndex);
        }
        else
        {
            if (wallManager.BuyMaterial(materialIndex))
            {
                UpdateButtonState();
            }
        }
    }

    public void UpdateButtonState()
    {
        if (wallManager.IsMaterialUnlocked(materialIndex))
        {
            coinIcon.gameObject.SetActive(false);

            if (wallManager.GetCurrentMaterialIndex() == materialIndex)
            {
                buttonText.text = "выбрано";
                buttonText.color = selectedColor;
            }
            else
            {
                buttonText.text = "выбрать";
                buttonText.color = selectColor;
            }
        }
        else
        {
            buttonText.text = wallManager.GetMaterialPrice(materialIndex).ToString();
            buttonText.color = priceColor;
            coinIcon.gameObject.SetActive(true);
        }
    }
}