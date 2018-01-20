using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mixtureData
{
    //public
    public GameObject theGO;
    //private
    desiredMixtureType aimingFor; //TODO... use this in our mixture calculations
    bool ignoreQuants;
    colorSpace csUsed;
    mixingMethod mixAlgo;

    public mixtureData(GameObject GO, desiredMixtureType AF, bool IQ, colorSpace CSU, mixingMethod MM)
    {
        theGO = GO;

        setAimingFor(AF);
        setIgnoreQuants(IQ);
        setCSUsed(CSU);
        setMixAlgo(MM);
    }

    public void updateMixture(Color[] colorsToMix, float[] quantsToMix)
    {
        //IF we only have 1 color space to pick && 1 color mixing method to pick... 
        //change aiming for to what we know it will end up looking like based on those 2 settings
        //lock aiming for (drop down not selectable)
        //NOTE: this will cause update color to run twice...

        //ELSE
        //unlock aim for

        //create a set the new mixture
        Color newColor = Camera.main.GetComponent<otherColorOps>().mixColors(csUsed, mixAlgo, ignoreQuants, colorsToMix, quantsToMix);
        theGO.GetComponent<mixtureRefs>().updateColor(newColor);
    }

    //-----Setters

    public void setAimingFor(desiredMixtureType newAimingFor)
    {
        int newIndex = 0;

        switch (newAimingFor)
        {
            case desiredMixtureType.subtractive:
                newIndex = 0;
                break;
            case desiredMixtureType.additive:
                newIndex = 1;
                break;
        }

        setAimingFor(newAimingFor, newIndex);
    }

    public void setAimingFor(desiredMixtureType newAimingFor, int i)
    {
        aimingFor = newAimingFor;
        updateMixture(Camera.main.GetComponent<demo_colors>().getAllColors(), Camera.main.GetComponent<demo_colors>().getAllQuants());

        theGO.GetComponent<mixtureRefs>().justSet = true;
        theGO.GetComponent<mixtureRefs>().aimingForTypeGO.GetComponent<Dropdown>().value = i;
        theGO.GetComponent<mixtureRefs>().justSet = false;
    }

    public void setCSUsed(colorSpace newCSUsed)
    {
        int newIndex = 0;

        switch (newCSUsed)
        {
            case colorSpace.RGB:
                newIndex = 0;
                break;
            case colorSpace.RYB:
                newIndex = 1;
                break;
            case colorSpace.CMYK:
                newIndex = 2;
                break;
        }

        setCSUsed(newCSUsed, newIndex);
    }

    public void setCSUsed(colorSpace newCSUsed, int i)
    {
        csUsed = newCSUsed;
        updateMixture(Camera.main.GetComponent<demo_colors>().getAllColors(), Camera.main.GetComponent<demo_colors>().getAllQuants());

        theGO.GetComponent<mixtureRefs>().justSet = true;
        theGO.GetComponent<mixtureRefs>().colorspaceGO.GetComponent<Dropdown>().value = i;
        theGO.GetComponent<mixtureRefs>().justSet = false;
    }

    public void setIgnoreQuants(bool newIgnoreQuants)
    {
        int newIndex = 0;

        switch (newIgnoreQuants)
        {
            case false:
                newIndex = 0;
                break;
            case true:
                newIndex = 1;
                break;
        }

        setIgnoreQuants(newIgnoreQuants, newIndex);
    }

    public void setIgnoreQuants(bool newIgnoreQuants, int i)
    {
        ignoreQuants = newIgnoreQuants;
        updateMixture(Camera.main.GetComponent<demo_colors>().getAllColors(), Camera.main.GetComponent<demo_colors>().getAllQuants());

        theGO.GetComponent<mixtureRefs>().justSet = true;
        theGO.GetComponent<mixtureRefs>().ignoreQuantsGO.GetComponent<Dropdown>().value = i;
        theGO.GetComponent<mixtureRefs>().justSet = false;
    }

    public void setMixAlgo(mixingMethod newMixAlgo)
    {
        int newIndex = 0;

        switch (newMixAlgo)
        {
            case mixingMethod.colorComponentAveraging:
                newIndex = 0;
                break;
            case mixingMethod.colorAveraging:
                newIndex = 1;
                break;
            case mixingMethod.spaceAveraging:
                newIndex = 2;
                break;
        }

        setMixAlgo(newMixAlgo, newIndex);
    }

    public void setMixAlgo(mixingMethod newMixAlgo, int i)
    {
        mixAlgo = newMixAlgo;
        updateMixture(Camera.main.GetComponent<demo_colors>().getAllColors(), Camera.main.GetComponent<demo_colors>().getAllQuants());

        theGO.GetComponent<mixtureRefs>().justSet = true;
        theGO.GetComponent<mixtureRefs>().mixingGO.GetComponent<Dropdown>().value = i;
        theGO.GetComponent<mixtureRefs>().justSet = false;
    }

    //-----Getters

    public desiredMixtureType getAimingFor()
    {
        return aimingFor;
    }

    public bool getIgnoreQuants()
    {
        return ignoreQuants;
    }

    public colorSpace getCSUsed()
    {
        return csUsed;
    }

    public mixingMethod getMixAlgo()
    {
        return mixAlgo;
    }
}
