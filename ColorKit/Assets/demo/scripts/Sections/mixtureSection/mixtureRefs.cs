using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using colorKit;

[ExecuteInEditMode]
public class mixtureRefs : MonoBehaviour {

    public bool justSet;

    public GameObject colorSample;

    public GameObject rgb;
    public GameObject ryb;
    public GameObject cmyk;

    public GameObject removeMixtureBtn;

    public GameObject aimingForTypeGO;
    public GameObject colorspaceGO;
    public GameObject mixingGO;
    public GameObject ignoreQuantsGO;

    void Awake()
    {
        addMixtureListeners();
    }

    public void addMixtureListeners()
    {
        removeMixtureBtn.GetComponent<Button>().onClick.AddListener(this.onClick_removeMixture);
        aimingForTypeGO.GetComponent<Dropdown>().onValueChanged.AddListener(this.onValueChange_AimingForType);
        colorspaceGO.GetComponent<Dropdown>().onValueChanged.AddListener(this.onValueChange_ColorSpace);
        mixingGO.GetComponent<Dropdown>().onValueChanged.AddListener(this.onValueChange_Mixing);
        ignoreQuantsGO.GetComponent<Dropdown>().onValueChanged.AddListener(this.onValueChange_IgnoreQuants);
    }

    public void onClick_removeMixture()
    {
        Camera.main.GetComponent<demo_mixtures>().removeMixture(gameObject);
    }

    public void onValueChange_AimingForType(int i)
    {
        if(justSet == false)
            switch (i)
            {
                case 0: //sub
                    Camera.main.GetComponent<demo_mixtures>().mixtureDataList[gameObject].setAimingFor(desiredMixtureType.subtractive, i);
                    break;
                case 1: //add
                    Camera.main.GetComponent<demo_mixtures>().mixtureDataList[gameObject].setAimingFor(desiredMixtureType.additive, i);
                    break;
            }
    }

    public void onValueChange_ColorSpace(int i)
    {
        if (justSet == false)
            switch (i)
            {
                case 0: //rgb
                    Camera.main.GetComponent<demo_mixtures>().mixtureDataList[gameObject].setCSUsed(colorSpace.RGB, i);
                    break;
                case 1: //ryb
                    Camera.main.GetComponent<demo_mixtures>().mixtureDataList[gameObject].setCSUsed(colorSpace.RYB, i);
                    break;
                case 2: //cmyk
                    Camera.main.GetComponent<demo_mixtures>().mixtureDataList[gameObject].setCSUsed(colorSpace.CMYK, i);
                    break;
            }
    }

    public void onValueChange_Mixing(int i)
    {
        if (justSet == false)
            switch (i)
            {
                case 0: //cca
                    Camera.main.GetComponent<demo_mixtures>().mixtureDataList[gameObject].setMixAlgo(mixingMethod.colorComponentAveraging, i);
                    break;
                case 1: //ca
                    Camera.main.GetComponent<demo_mixtures>().mixtureDataList[gameObject].setMixAlgo(mixingMethod.colorAveraging, i);
                    break;
                case 2: //csa
                    Camera.main.GetComponent<demo_mixtures>().mixtureDataList[gameObject].setMixAlgo(mixingMethod.spaceAveraging, i);
                    break;
            }
    }

    public void onValueChange_IgnoreQuants(int i)
    {
        if (justSet == false)
            switch (i)
            {
                case 0: //dont
                    Camera.main.GetComponent<demo_mixtures>().mixtureDataList[gameObject].setIgnoreQuants(false, i);
                    break;
                case 1: //do
                    Camera.main.GetComponent<demo_mixtures>().mixtureDataList[gameObject].setIgnoreQuants(true, i);
                    break;
            }
    }

    public void updateColor(Color newColor)
    {
        colorSample.GetComponent<Image>().color = newColor;

        float[] rgbFloat = colorTypeConversion.color_to_array(newColor);
        float[] rgb255 = colorFormatConversion._float_to_255(rgbFloat);
        float[] ryb255 = rgb2ryb_ryb2rgb.rgb255_to_ryb255(rgb255);
        float[] cmyk255 = rgb2cmyk_cmyk2rgb.rgb255_to_cmyk255(rgb255);

        string rgbString = "";
        for (int i = 0; i < rgb255.Length; i++)
        {
            rgbString += Mathf.RoundToInt(rgb255[i]);
            if (i != (rgb255.Length - 1))
                rgbString += ", ";
        }

        string rybString = "";
        for (int i = 0; i < ryb255.Length; i++)
        {
            rybString += Mathf.RoundToInt(ryb255[i]);
            if (i != (ryb255.Length - 1))
                rybString += ", ";
        }

        string cmykString = "";
        for (int i = 0; i < cmyk255.Length; i++)
        {
            cmykString += Mathf.RoundToInt(cmyk255[i]);
            if (i != (cmyk255.Length - 1))
                cmykString += ", ";
        }

        rgb.GetComponent<InputField>().text = rgbString;
        ryb.GetComponent<InputField>().text = rybString;
        cmyk.GetComponent<InputField>().text = cmykString;
    }
}
