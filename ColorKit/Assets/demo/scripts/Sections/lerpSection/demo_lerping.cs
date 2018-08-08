using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using colorKit;
using lerpKit;

public class demo_lerping : MonoBehaviour
{
    //references
    public GameObject controlPanel_CL;
    public GameObject lerps_CL;

    //height of our reference objs
    float controlPanel_CL_height;
    float controlPanel_CL_settings_height;
    float lerps_CL_height;

    //change button obj and (show or hide) panels depending on that button obj

    public GameObject lerpPlayStopBtn;
    public GameObject lerpingBtnIcon;

    public Sprite playSprite;
    public Sprite stopSprite;

    public GameObject noSettingsGO;
    public GameObject settingsGO;

    //Sample of the Color As It Lerp Shown Here
    public GameObject rgbLerpSampleGO;
    public GameObject rybLerpSampleGO;
    public GameObject cmykLerpSampleGO;

    //referances to our objs that can be changable
    public GameObject setting1;
    public GameObject setting2;
    public GameObject setting3;
    public GameObject setting4;

    //Variables for lerping
    bool lerping;

    bool fixedUpdateLerp;
    bool largestDistanceLerp;
    float timeToLerpDistance;
    bool timeTypeSecond;

    //store calculated distances
    public GameObject distanceRGB_GO;
    public GameObject distanceRYB_GO;
    public GameObject distanceCMYK_GO;

    //calculated every time we have a new pair of colors
    float distanceBetweenColorsInRGB;
    float distanceBetweenColorsInRYB;
    float distanceBetweenColorsInCMYK;

    Color startColor;
    Color endColor;

    Color startColor_RGB;
    Color endColor_RGB;
    Color startColor_RYB;
    Color endColor_RYB;
    Color startColor_CMYK;
    Color endColor_CMYK;

    //calculated every frame
    public Color currColor_RGB;
    public Color currColor_RYB;
    public Color currColor_CMYK;

    public void manualAwake()
    {
        controlPanel_CL_height = 150f;
        controlPanel_CL_settings_height = 135f;
        lerps_CL_height = 390f;

        lerping = false;

        fixedUpdateLerp = true;
        largestDistanceLerp = true;
        timeToLerpDistance = 10;
        setting3.GetComponent<InputField>().text = timeToLerpDistance.ToString();
        timeTypeSecond = true;

        //event trigger creation

        setting1.GetComponent<Dropdown>().onValueChanged.AddListener(onUpdateFuncEdit);
        setting2.GetComponent<Dropdown>().onValueChanged.AddListener(onDistEdit);
        setting3.GetComponent<InputField>().onEndEdit.AddListener(onTimeEdit);
        setting4.GetComponent<Dropdown>().onValueChanged.AddListener(onTimeTypeEdit);

        lerpPlayStopBtn.GetComponent<Button>().onClick.AddListener(onClickLerpingBtn);
    }

    //----------Functions Triggered by Changing UI Settings

    void onUpdateFuncEdit(int i)
    {
        if (i == 0)
            fixedUpdateLerp = true;
        else
            fixedUpdateLerp = false;
        updateLerpingOptions();
    }

    void onDistEdit(int i)
    {
        if (i == 0)
            largestDistanceLerp = true;
        else
            largestDistanceLerp = false;
        updateLerpingOptions();
    }

    void onTimeEdit(string newTime)
    {
        timeToLerpDistance = float.Parse(newTime);
        setting3.GetComponent<InputField>().text = timeToLerpDistance.ToString();
        updateLerpingOptions();
    }

    void onTimeTypeEdit(int i)
    {
        if (i == 0)
            timeTypeSecond = true;
        else
            timeTypeSecond = false;
        updateLerpingOptions();
    }

    void onClickLerpingBtn() //this only triggers when we physically click the button
    {
        onClickLerpingBtn(true);
    }

    void onClickLerpingBtn(bool FlipLerpingVar)
    {
        if (FlipLerpingVar)
            lerping = !lerping;

        //handle technical stuff
        currColor_RGB = startColor;
        currColor_RYB = startColor;
        currColor_CMYK = startColor;

        rgbLerpSampleGO.GetComponent<thickRefs>().updateColor(startColor);
        rybLerpSampleGO.GetComponent<thickRefs>().updateColor(startColor);
        cmykLerpSampleGO.GetComponent<thickRefs>().updateColor(startColor);

        if (lerping == false) //we where lerping (we STOP lerping)
        {
            //NOTE: by now our startColor Should Already be Set

            //handle visible stuff
            lerpingBtnIcon.GetComponent<Image>().sprite = playSprite;
            settingsGO.GetComponent<RectTransform>().sizeDelta = new Vector2(settingsGO.GetComponent<RectTransform>().sizeDelta.x, controlPanel_CL_settings_height);
            noSettingsGO.GetComponent<RectTransform>().sizeDelta = new Vector2(noSettingsGO.GetComponent<RectTransform>().sizeDelta.x, 0);
            settingsGO.SetActive(true);
            noSettingsGO.SetActive(false);
        }
        else //we START Lerping
        {
            //NOTE: actual Lerping will happen in update or in fixed update depending on our settings

            //handle visible stuff
            lerpingBtnIcon.GetComponent<Image>().sprite = stopSprite;
            settingsGO.GetComponent<RectTransform>().sizeDelta = new Vector2(settingsGO.GetComponent<RectTransform>().sizeDelta.x, 0);
            noSettingsGO.GetComponent<RectTransform>().sizeDelta = new Vector2(noSettingsGO.GetComponent<RectTransform>().sizeDelta.x, controlPanel_CL_settings_height);
            settingsGO.SetActive(false);
            noSettingsGO.SetActive(true);
        }
    }

    //----------Things That Run Every "Frame"

    void Update()
    {
        if (lerping)
            if (!fixedUpdateLerp)
                lerpColors();
    }

    void FixedUpdate()
    {
        if (lerping)
            if (fixedUpdateLerp)
                lerpColors();
    }

    void lerpColors() //TODO... this must be repaired...
    {
        //----------Update The Colors programatically

        guideDistance GD = (largestDistanceLerp) ? guideDistance.distBetween_Other : guideDistance.distBetween_StartAndEnd;

        //-----RGB

        float guideDistRGB = colorLerpHelper.calcGuideDistance(startColor_RGB, currColor_RGB, endColor_RGB, colorSpace.RGB, GD);
        float lerpValueRGB = colorLerpHelper.calcLerpValue(startColor_RGB, currColor_RGB, endColor_RGB,
            guideDistRGB, 
            timeToLerpDistance,
            (timeTypeSecond) ? unitOfTime.seconds : unitOfTime.frames,
            (fixedUpdateLerp) ? updateLocation.fixedUpdate : updateLocation.update,
            colorSpace.RGB
            );

        currColor_RGB = colorLerping.colorLerp(currColor_RGB, endColor_RGB, lerpValueRGB, colorSpace.RGB);

        //-----RYB

        float guideDistRYB = colorLerpHelper.calcGuideDistance(startColor_RYB, currColor_RYB, endColor_RYB, colorSpace.RYB, GD);
        float lerpValueRYB = colorLerpHelper.calcLerpValue(startColor_RYB, currColor_RYB, endColor_RYB,
            guideDistRYB, //calculate guide distance [(largestDistanceLerp) ? guideDistance.distBetween_Other : guideDistance.distBetween_StartAndEnd]
            timeToLerpDistance,
            (timeTypeSecond) ? unitOfTime.seconds : unitOfTime.frames,
            (fixedUpdateLerp) ? updateLocation.fixedUpdate : updateLocation.update,
            colorSpace.RYB
            );

        currColor_RYB = colorLerping.colorLerp(currColor_RYB, endColor_RYB, lerpValueRYB, colorSpace.RYB);

        //-----CMYK

        float guideDistCMYK = colorLerpHelper.calcGuideDistance(startColor_CMYK, currColor_CMYK, endColor_CMYK, colorSpace.CMYK, GD);
        float lerpValueCMYK = colorLerpHelper.calcLerpValue(startColor_CMYK, currColor_CMYK, endColor_CMYK,
            guideDistCMYK, //calculate guide distance [(largestDistanceLerp) ? guideDistance.distBetween_Other : guideDistance.distBetween_StartAndEnd]
            timeToLerpDistance,
            (timeTypeSecond) ? unitOfTime.seconds : unitOfTime.frames,
            (fixedUpdateLerp) ? updateLocation.fixedUpdate : updateLocation.update,
            colorSpace.CMYK
            );

        currColor_CMYK = colorLerping.colorLerp(currColor_CMYK, endColor_CMYK, lerpValueCMYK, colorSpace.CMYK);

        //----------Update The Colors Visually

        rgbLerpSampleGO.GetComponent<thickRefs>().updateColor(currColor_RGB);
        rybLerpSampleGO.GetComponent<thickRefs>().updateColor(currColor_RYB);
        cmykLerpSampleGO.GetComponent<thickRefs>().updateColor(currColor_CMYK);

        //---------Start and End Inversion because different color lerps will lerp at different paces

        if (Mathf.Approximately(1, lerpValueRGB) || lerpValueRGB > 1) //oscilate back to our other color
        {
            Color startColorCopy = startColor_RGB;
            startColor_RGB = endColor_RGB;
            endColor_RGB = startColorCopy;
        }

        if (Mathf.Approximately(1, lerpValueRYB) || lerpValueRYB > 1) //oscilate back to our other color
        {
            Color startColorCopy = startColor_RYB;
            startColor_RYB = endColor_RYB;
            endColor_RYB = startColorCopy;
        }

        if (Mathf.Approximately(1, lerpValueCMYK) || lerpValueCMYK > 1) //oscilate back to our other color
        {
            Color startColorCopy = startColor_CMYK;
            startColor_CMYK = endColor_CMYK;
            endColor_CMYK = startColorCopy;
        }
    }

    //----------Called by UpdateMixture in Demo Color Tracker (called when color is added, removed, edited)

    public void updateLerpingOptions()
    {
        Dictionary<GameObject, colorData> colorDataList = gameObject.GetComponent<demo_colors>().colorDataList;

        if (colorDataList.Count == 2)
        {
            controlPanel_CL.SetActive(true);
            lerps_CL.SetActive(true);
            controlPanel_CL.GetComponent<RectTransform>().sizeDelta = new Vector2(controlPanel_CL.GetComponent<RectTransform>().sizeDelta.x, controlPanel_CL_height);
            lerps_CL.GetComponent<RectTransform>().sizeDelta = new Vector2(lerps_CL.GetComponent<RectTransform>().sizeDelta.x, lerps_CL_height);

            float youngestTime = float.MaxValue;
            GameObject youngestGO = gameObject;
            foreach (var item in colorDataList)
            {
                if (colorDataList[item.Key].creationTime < youngestTime)
                {
                    youngestGO = item.Key;
                    youngestTime = colorDataList[youngestGO].creationTime;
                }
            }

            if (youngestTime != float.MaxValue)
            {
                foreach (var item in colorDataList)
                {
                    if (item.Key == youngestGO)
                        startColor = colorDataList[item.Key].getColor();
                    else
                        endColor = colorDataList[item.Key].getColor();
                }
                colorDataList[youngestGO].getColor();
            }
            else
            {
                startColor = Color.white;
                endColor = Color.white;
            }

            //copy start and end color for all color spaces
            startColor_RGB = startColor;
            startColor_RYB = startColor;
            startColor_CMYK = startColor;

            endColor_RGB = endColor;
            endColor_RYB = endColor;
            endColor_CMYK = endColor;

            //update the distance between colors in rgb and ryb
            distanceBetweenColorsInRGB = colorDistances.distBetweenColors(startColor, endColor, colorSpace.RGB);
            distanceBetweenColorsInRYB = colorDistances.distBetweenColors(startColor, endColor, colorSpace.RYB);
            distanceBetweenColorsInCMYK = colorDistances.distBetweenColors(startColor, endColor, colorSpace.CMYK);

            distanceRGB_GO.GetComponent<InputField>().text = distanceBetweenColorsInRGB.ToString();
            distanceRYB_GO.GetComponent<InputField>().text = distanceBetweenColorsInRYB.ToString();
            distanceCMYK_GO.GetComponent<InputField>().text = distanceBetweenColorsInCMYK.ToString();
        }
        else
        {
            controlPanel_CL.GetComponent<RectTransform>().sizeDelta = new Vector2(controlPanel_CL.GetComponent<RectTransform>().sizeDelta.x, 0);
            lerps_CL.GetComponent<RectTransform>().sizeDelta = new Vector2(lerps_CL.GetComponent<RectTransform>().sizeDelta.x, 0);
            controlPanel_CL.SetActive(false);
            lerps_CL.SetActive(false);
        }

        lerping = false; //a color was change removed or added... so we stop lerping...
        onClickLerpingBtn(false);
    }
}