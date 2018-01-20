using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Description: Change the Color's Format (255,float,hex)

public class colorFormatConversion : MonoBehaviour {

    //--- color in float format -> 255 format

    public float[] colorFloat_to_color255(float[] colorFloat)
    {
        float[] color255 = new float[colorFloat.Length];
        for (int i = 0; i < color255.Length; i++)
            color255[i] = _float_to_255(colorFloat[i]);
        return color255;
    }

    //--- color in float format -> hex format

    public string[] colorFloat_to_colorHex(float[] colorFloat)
    {
        string[] colorHex = new string[colorFloat.Length];
        for (int i = 0; i < colorHex.Length; i++)
            colorHex[i] = _float_to_hex(colorFloat[i]);
        return colorHex;
    }

    //--- color in 255 format -> float format

    public float[] color255_to_colorFloat(float[] color255)
    {
        float[] colorFloat = new float[color255.Length];
        for (int i = 0; i < colorFloat.Length; i++)
            colorFloat[i] = _255_to_float(color255[i]);
        return colorFloat;
    }

    //---color in 255 format -> hex format

    public string[] color255_to_colorHex(float[] color255)
    {
        string[] colorHex = new string[color255.Length];
        for (int i = 0; i < colorHex.Length; i++)
            colorHex[i] = _255_to_hex(color255[i]);
        return colorHex;
    }

    //--- color in hex format -> float format

    public float[] colorHex_to_colorFloat(string[] colorHex)
    {
        float[] colorFloat = new float[colorHex.Length];
        for (int i = 0; i < colorFloat.Length; i++)
            colorFloat[i] = _hex_to_float(colorHex[i]);
        return colorFloat;
    }

    //--- color in hex format -> 255 format

    public float[] colorHex_to_color255(string[] colorHex)
    {
        float[] color255 = new float[colorHex.Length];
        for (int i = 0; i < color255.Length; i++)
            color255[i] = _hex_to_255(colorHex[i]);
        return color255;
    }

    //-------------------------helpers of the functions above-------------------------

    //--- (Float -> 255)

    float _float_to_255(float numFloat)
    {
        return Mathf.Clamp(numFloat * 255, 0, 255);
    }

    //--- (Float -> Hex)

    string _float_to_hex(float numFloat)
    {
        string hex = Convert.ToString((int)Mathf.Round(255 * numFloat), 16);
        return (hex.Length == 1) ? "0" + hex : hex;
    }

    //--- (255 -> Float)

    float _255_to_float(float num255)
    {
        return Mathf.Clamp(num255 / 255, 0, 1);
    }

    //--- (255 -> Hex)

    string _255_to_hex(float num255)
    {
        string hex = Convert.ToString((int)Mathf.Round(num255), 16);
        return (hex.Length == 1) ? "0" + hex : hex;
    }

    //--- (Hex -> Float)

    float _hex_to_float(string hex)
    {
        return Mathf.Clamp(Mathf.Clamp(Convert.ToInt32(hex, 16), 0, 255) / 255, 0, 1);
    }

    //--- (Hex -> 255)

    float _hex_to_255(string hex)
    {
        return Mathf.Clamp(Convert.ToInt32(hex, 16), 0, 255);
    }
}