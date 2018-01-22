using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using colorKit;

public class demo : MonoBehaviour {

    public GameObject colorListPanel;

    void Awake()
    {
        gameObject.GetComponent<demo_mixtures>().manualAwake();
        gameObject.GetComponent<demo_lerping>().manualAwake(); //this awake must run first to set vars that startDemo uses
        startDemo();
    }

    void startDemo()
    {
        //---remove all possible gameobjects created in run or execute mode

        List<GameObject> childGOs = new List<GameObject>();
        foreach (Transform child in colorListPanel.transform)
            childGOs.Add(child.gameObject);

        for (int i = 0; i < childGOs.Count; i++)
            DestroyImmediate(childGOs[i]);
        childGOs.Clear();

        //---starter colors list

        gameObject.GetComponent<demo_colors>().addColor(colorTypeConversion.array_to_color(colorFormatConversion.color255_to_colorFloat(new float[] { 255, 0, 0 })), 1.25f);
        gameObject.GetComponent<demo_colors>().addColor(colorTypeConversion.array_to_color(colorFormatConversion.color255_to_colorFloat(new float[] { 255, 255, 0 })), 1.25f);

        //---starter mixtures list

        gameObject.GetComponent<demo_mixtures>().addMixture(desiredMixtureType.additive, false, colorSpace.RGB, mixingMethod.colorAveraging);
        gameObject.GetComponent<demo_mixtures>().addMixture(desiredMixtureType.subtractive, false, colorSpace.RYB, mixingMethod.colorComponentAveraging);

        //---start lerping because your know that your values are what they should be
        //TODO... start lerping...
    }

    //-------------------------Other Functions-------------------------

    public void closeAllOtherDrawers(GameObject dontCloseThisOne)
    {
        foreach (var child in Camera.main.GetComponent<demo_colors>().colorDataList)
        {
            if (child.Key != dontCloseThisOne)
                if (child.Key.GetComponent<colorRefs>().toolBarOpen)
                    child.Key.GetComponent<colorRefs>().onClick_ColorDrawer();
        }
    }

    public void updateColorMixtures() //Updates when a color is added, removed, or changed in any way
    {
        Dictionary<GameObject, colorData> colorDataList_Temp = Camera.main.GetComponent<demo_colors>().colorDataList;

        if (colorDataList_Temp.Count != 0)
        {
            Color[] colorsToMix = Camera.main.GetComponent<demo_colors>().getAllColors();
            float[] quantsToMix = Camera.main.GetComponent<demo_colors>().getAllQuants();

            Dictionary<GameObject, mixtureData> mixtureDataList_Temp = Camera.main.GetComponent<demo_mixtures>().mixtureDataList;

            if(mixtureDataList_Temp != null)
                foreach (var item in mixtureDataList_Temp)
                    item.Value.updateMixture(colorsToMix, quantsToMix);
        }

        gameObject.GetComponent<demo_lerping>().updateLerpingOptions();
    }
}