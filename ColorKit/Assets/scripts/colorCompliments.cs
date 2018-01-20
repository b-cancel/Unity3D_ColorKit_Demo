using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorCompliments : MonoBehaviour {

    public Color complimentary(colorSpace csToUse, Color origColor)
    {
        switch (csToUse)
        {
            case colorSpace.RGB:
                return complimentary_inRGB_colorSpace(origColor);
            case colorSpace.RYB:
                return complimentary_inRYB_colorSpace(origColor);
            default:
                return complimentary_inCMYK_colorSpace(origColor);
        }
    }

    Color complimentary_inRGB_colorSpace(Color origColor)
    {
        float[] colorFloat_rGb = gameObject.GetComponent<colorTypeConversion>().color_to_array(origColor);
        float[] color255_rGb = gameObject.GetComponent<colorFormatConversion>().colorFloat_to_color255(colorFloat_rGb);

        float[] result255_rGb = complimentary(color255_rGb, 255);
        float[] resultFloat_rGb = gameObject.GetComponent<colorFormatConversion>().color255_to_colorFloat(result255_rGb);

        return gameObject.GetComponent<colorTypeConversion>().array_to_color(resultFloat_rGb);
    }

    Color complimentary_inRYB_colorSpace(Color origColor)
    {
        float[] colorFloat_rGb = gameObject.GetComponent<colorTypeConversion>().color_to_array(origColor);
        float[] color255_rGb = gameObject.GetComponent<colorFormatConversion>().colorFloat_to_color255(colorFloat_rGb);
        float[] color255_rYb = gameObject.GetComponent<rgb2ryb_ryb2rgb>().rgb255_to_ryb255(color255_rGb);

        float[] result255_rYb = complimentary(color255_rYb, 255);
        float[] result255_rGb = gameObject.GetComponent<rgb2ryb_ryb2rgb>().ryb255_to_rgb255(result255_rYb);
        float[] resultFloat_rGb = gameObject.GetComponent<colorFormatConversion>().color255_to_colorFloat(result255_rGb);

        return gameObject.GetComponent<colorTypeConversion>().array_to_color(resultFloat_rGb);
    }

    Color complimentary_inCMYK_colorSpace(Color origColor)
    {
        float[] colorFloat_rGb = gameObject.GetComponent<colorTypeConversion>().color_to_array(origColor);
        float[] color255_rGb = gameObject.GetComponent<colorFormatConversion>().colorFloat_to_color255(colorFloat_rGb);
        float[] color255_CMYK = gameObject.GetComponent<rgb2cmyk_cmyk2rgb>().rgb255_to_cmyk255(color255_rGb);

        float[] result255_CMYK = complimentary(color255_CMYK, 255);
        float[] result255_rGb = gameObject.GetComponent<rgb2cmyk_cmyk2rgb>().cmyk255_to_rgb255(result255_CMYK);
        float[] resultFloat_rGb = gameObject.GetComponent<colorFormatConversion>().color255_to_colorFloat(result255_rGb);

        return gameObject.GetComponent<colorTypeConversion>().array_to_color(resultFloat_rGb);
    }

    //-----BASE

    public float[] complimentary(float[] color, int floatLimit) //for colors in float format floatLimit = 1 | for colors in 255 format floatLimit = 255
    {
        float[] compColor = new float[color.Length];
        for (int i = 0; i < compColor.Length; i++)
            compColor[i] = floatLimit - color[i];
        return compColor;
    }
}
