using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class demo_mixtures : MonoBehaviour {

    public GameObject mixtureListPanel;
    public GameObject mixtureDataPrefab;

    public Dictionary<GameObject, mixtureData> mixtureDataList; //NEVER add mixture or remove mixture from here... use the function in the demo script...
    public GameObject addMixtureBtn;

    public void manualAwake()
    {
        mixtureDataList = new Dictionary<GameObject, mixtureData>();

        addMixtureBtn.GetComponent<Button>().onClick.AddListener(onClick_addMixture);
    }

    //function triggered by plus sign
    void onClick_addMixture()
    {
        addMixture(desiredMixtureType.subtractive, false, colorSpace.CMYK, mixingMethod.colorComponentAveraging);
    }

    public void addMixture(desiredMixtureType aimingFor, bool ignoreQuants, colorSpace csUsed, mixingMethod mixAlgo)
    {
        GameObject newMixtureDataGO = Instantiate(mixtureDataPrefab, mixtureListPanel.transform);
        mixtureDataList.Add(newMixtureDataGO, new mixtureData(newMixtureDataGO, aimingFor, ignoreQuants, csUsed, mixAlgo));
        mixtureDataList[newMixtureDataGO].updateMixture(Camera.main.GetComponent<demo_colors>().getAllColors(), Camera.main.GetComponent<demo_colors>().getAllQuants());
    }

    public void removeMixture(GameObject GO)
    {
        mixtureDataList.Remove(GO);
        DestroyImmediate(GO);
    }
}