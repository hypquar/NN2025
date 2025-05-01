using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class WallMaterialManager : MonoBehaviour
{
    [System.Serializable]
    public class MaterialData
    {
        public Material material;
        public int price;
        public string name;
    }

    [Header("Materials")]
    public MaterialData[] materialsData;
    public event Action OnMaterialSelected; 

    private Renderer[] wallRenderers;
    private int currentMaterialIndex = 0;

    private const string UnlockedKeyPrefix = "MaterialUnlocked_";
    private const string SelectedMaterialKey = "SelectedMaterial";

    void Start()
    {
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        wallRenderers = new Renderer[walls.Length];

        for (int i = 0; i < walls.Length; i++)
        {
            wallRenderers[i] = walls[i].GetComponentInChildren<Renderer>();
        }

        currentMaterialIndex = PlayerPrefs.GetInt(SelectedMaterialKey, 0);
        ApplyMaterial(currentMaterialIndex);
        OnMaterialSelected?.Invoke(); 
    }

public int GetCurrentMaterialIndex()
{
    return currentMaterialIndex;
}

    public void ApplyMaterial(int materialIndex)
    {
        if (!IsMaterialUnlocked(materialIndex)) return;

        foreach (var renderer in wallRenderers)
        {
            if (renderer != null)
            {
                renderer.material = materialsData[materialIndex].material;
            }
        }

        currentMaterialIndex = materialIndex;
        PlayerPrefs.SetInt(SelectedMaterialKey, currentMaterialIndex);
        PlayerPrefs.Save();

        OnMaterialSelected?.Invoke(); 
    }





    public bool BuyMaterial(int materialIndex)
    {
        if (IsMaterialUnlocked(materialIndex))
        {
            ApplyMaterial(materialIndex);
            return true;
        }

        if (Wallet.Instance.SpendCurrency(materialsData[materialIndex].price))
        {
            UnlockMaterial(materialIndex);
            ApplyMaterial(materialIndex);
            return true;
        }

        return false;
    }

    public bool IsMaterialUnlocked(int materialIndex)
    {
        return PlayerPrefs.GetInt(UnlockedKeyPrefix + materialIndex, 0) == 1;
    }

    public int GetMaterialPrice(int materialIndex)
    {
        return materialsData[materialIndex].price;
    }

    public string GetMaterialName(int materialIndex)
    {
        return materialsData[materialIndex].name;
    }

    private void UnlockMaterial(int materialIndex)
    {
        PlayerPrefs.SetInt(UnlockedKeyPrefix + materialIndex, 1);
        PlayerPrefs.Save();
    }
}