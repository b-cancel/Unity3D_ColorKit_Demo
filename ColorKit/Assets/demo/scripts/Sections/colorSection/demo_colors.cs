using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class demo_colors : MonoBehaviour
{
    public GameObject colorListPanel;
    public GameObject colorDataPrefab;

    //List that Keeps track of all of our color data
    public Dictionary<GameObject, colorData> colorDataList; //NEVER add color or remove color from here... use the function in the demo script...
    public GameObject addColorBtn;

    void Awake()
    {
        colorDataList = new Dictionary<GameObject, colorData>();

        addColorBtn.GetComponent<Button>().onClick.AddListener(onClick_addColor);
    }

    public Color[] getAllColors()
    {
        Color[] colorsToMix = new Color[colorDataList.Count];

        int index = 0;
        foreach (var colorData in colorDataList)
        {
            colorsToMix[index] = colorData.Value.getColor();
            index++;
        }

        return colorsToMix;
    }

    public float[] getAllQuants()
    {
        float[] quantsToMix = new float[colorDataList.Count];

        int index = 0;
        foreach (var colorData in colorDataList)
        {
            quantsToMix[index] = colorData.Value.getQuantity();
            index++;
        }

        return quantsToMix;
    }

    //function triggered by plus sign
    void onClick_addColor()
    {
        addColor(Color.black, 1.5f);
    }

    public void addColor(Color color, float quantity)
    {
        GameObject newColorDataGO = Instantiate(colorDataPrefab, colorListPanel.transform);
        colorDataList.Add(newColorDataGO, new colorData(newColorDataGO, color, quantity));
        Camera.main.GetComponent<demo>().updateColorMixtures();
    }

    public void removeColor(GameObject GO)
    {
        colorDataList.Remove(GO);
        DestroyImmediate(GO.GetComponent<colorRefs>().toolBar);
        DestroyImmediate(GO);
        Camera.main.GetComponent<demo>().updateColorMixtures();
    }
}