using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class colorChangeTool : MonoBehaviour {

    public GameObject attachedColorData; //attached manually through code

    public GameObject sliderR_RGB;
    public GameObject sliderG_RGB;
    public GameObject sliderB_RGB;

    public GameObject sliderR_RYB;
    public GameObject sliderY_RYB;
    public GameObject sliderB_RYB;

    public GameObject sliderC_CMYK;
    public GameObject sliderM_CMYK;
    public GameObject sliderY_CMYK;
    public GameObject sliderK_CMYK;

    void Awake()
    {
        slidersBeingSet = false;

        sliderR_RGB.GetComponent<Slider>().onValueChanged.AddListener(onSlide_sliderR_RGB);
        sliderG_RGB.GetComponent<Slider>().onValueChanged.AddListener(onSlide_sliderG_RGB);
        sliderB_RGB.GetComponent<Slider>().onValueChanged.AddListener(onSlide_sliderB_RGB);

        sliderR_RYB.GetComponent<Slider>().onValueChanged.AddListener(onSlide_sliderR_RYB);
        sliderY_RYB.GetComponent<Slider>().onValueChanged.AddListener(onSlide_sliderY_RYB);
        sliderB_RYB.GetComponent<Slider>().onValueChanged.AddListener(onSlide_sliderB_RYB);

        sliderC_CMYK.GetComponent<Slider>().onValueChanged.AddListener(onSlide_sliderC_CMYK);
        sliderM_CMYK.GetComponent<Slider>().onValueChanged.AddListener(onSlide_sliderM_CMYK);
        sliderY_CMYK.GetComponent<Slider>().onValueChanged.AddListener(onSlide_sliderY_CMYK);
        sliderK_CMYK.GetComponent<Slider>().onValueChanged.AddListener(onSlide_sliderK_CMYK);
    }

    bool slidersBeingSet;

    //NOTE: we have to make sure our value is significantly different because otherwise we run into a loop and consequently a stack overflow
    public void setSliders(Color colorToRep)
    {
        slidersBeingSet = true;

        float[] rgbFloat = Camera.main.GetComponent<colorFormatConversion>().color_to_array(colorToRep);

        float[] rgb255 = Camera.main.GetComponent<colorFormatConversion>().colorFloat_to_color255(rgbFloat);
        float[] ryb255 = Camera.main.GetComponent<rgb2ryb_ryb2rgb>().rgb255_to_ryb255(rgb255);
        float[] cmyk255 = Camera.main.GetComponent<rgb2cmyk_cmyk2rgb>().rgb255_to_cmyk255(rgb255);

        if (Mathf.Approximately(sliderR_RGB.GetComponent<Slider>().value, rgb255[0]) == false)
            sliderR_RGB.GetComponent<Slider>().value = rgb255[0];
        if (Mathf.Approximately(sliderG_RGB.GetComponent<Slider>().value, rgb255[1]) == false)
            sliderG_RGB.GetComponent<Slider>().value = rgb255[1];
        if (Mathf.Approximately(sliderB_RGB.GetComponent<Slider>().value, rgb255[2]) == false)
            sliderB_RGB.GetComponent<Slider>().value = rgb255[2];

        if (Mathf.Approximately(sliderR_RYB.GetComponent<Slider>().value, ryb255[0]) == false)
            sliderR_RYB.GetComponent<Slider>().value = ryb255[0];
        if (Mathf.Approximately(sliderY_RYB.GetComponent<Slider>().value, ryb255[1]) == false)
            sliderY_RYB.GetComponent<Slider>().value = ryb255[1];
        if (Mathf.Approximately(sliderB_RYB.GetComponent<Slider>().value, ryb255[2]) == false)
            sliderB_RYB.GetComponent<Slider>().value = ryb255[2];

        if (Mathf.Approximately(sliderC_CMYK.GetComponent<Slider>().value, cmyk255[0]) == false)
            sliderC_CMYK.GetComponent<Slider>().value = cmyk255[0];
        if (Mathf.Approximately(sliderM_CMYK.GetComponent<Slider>().value, cmyk255[1]) == false)
            sliderM_CMYK.GetComponent<Slider>().value = cmyk255[1];
        if (Mathf.Approximately(sliderY_CMYK.GetComponent<Slider>().value, cmyk255[2]) == false)
            sliderY_CMYK.GetComponent<Slider>().value = cmyk255[2];
        if (Mathf.Approximately(sliderK_CMYK.GetComponent<Slider>().value, cmyk255[3]) == false)
            sliderK_CMYK.GetComponent<Slider>().value = cmyk255[3];

        slidersBeingSet = false;
    }

    //-----------------------Slider Change Event Triggers

    //---RGB

    void onSlide_sliderR_RGB(float val)
    {
        newRGB();
    }
    void onSlide_sliderG_RGB(float val)
    {
        newRGB();
    }
    void onSlide_sliderB_RGB(float val)
    {
        newRGB();
    }

    //---RYB

    void onSlide_sliderR_RYB(float val)
    {
        newRYB();
    }
    void onSlide_sliderY_RYB(float val)
    {
        newRYB();
    }
    void onSlide_sliderB_RYB(float val)
    {
        newRYB();
    }

    //---CMYK

    void onSlide_sliderC_CMYK(float val)
    {
        newCMYK();
    }
    void onSlide_sliderM_CMYK(float val)
    {
        newCMYK();
    }
    void onSlide_sliderY_CMYK(float val)
    {
        newCMYK();
    }
    void onSlide_sliderK_CMYK(float val)
    {
        newCMYK();
    }

    //-----Helpers

    void newRGB()
    {
        if (slidersBeingSet == false)
        {
            float[] new_rgb255 = new float[] { sliderR_RGB.GetComponent<Slider>().value, sliderG_RGB.GetComponent<Slider>().value, sliderB_RGB.GetComponent<Slider>().value };
            newColor(new_rgb255);
        }   
    }

    void newRYB()
    {
        if (slidersBeingSet == false)
        {
            float[] new_ryb255 = new float[] { sliderR_RYB.GetComponent<Slider>().value, sliderY_RYB.GetComponent<Slider>().value, sliderB_RYB.GetComponent<Slider>().value };
            float[] new_rgb255 = Camera.main.GetComponent<rgb2ryb_ryb2rgb>().ryb255_to_rgb255(new_ryb255);
            newColor(new_rgb255);
        }  
    }

    void newCMYK()
    {
        if (slidersBeingSet == false)
        {
            float[] new_cmyk255 = new float[] { sliderC_CMYK.GetComponent<Slider>().value, sliderM_CMYK.GetComponent<Slider>().value, sliderY_CMYK.GetComponent<Slider>().value, sliderK_CMYK.GetComponent<Slider>().value };
            float[] new_rgb255 = Camera.main.GetComponent<rgb2cmyk_cmyk2rgb>().cmyk255_to_rgb255(new_cmyk255);
            newColor(new_rgb255);
        }        
    }

    void newColor(float[] new_rgb255)
    {
        float[] new_rgbFloat = Camera.main.GetComponent<colorFormatConversion>().color255_to_colorFloat(new_rgb255);
        Color newColor = Camera.main.GetComponent<colorFormatConversion>().array_to_color(new_rgbFloat);
        Camera.main.GetComponent<demo_colors>().colorDataList[attachedColorData].setColor(newColor);
    }
}
