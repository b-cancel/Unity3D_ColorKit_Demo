using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script allows us to change the format of the components of the colors
/// 
/// It is meant to work with colors with
///     3 components: ryb, rgb
///     4 components: cmyk
///     
/// Conversions occur between these 3 different types of formats
/// 
/// #  FORMAT | RANGE       | RGB White in different formats
/// 1. Float -> [0 -> 1]   -> 1 1 1
/// 2. 255   -> [0 -> 255] -> 255 255 255
/// 3. Hex   -> [00 -> FF] -> FF FF FF
/// 
/// The Functions always clamp our results between expected values to always return valid input
///     NOTE: although the functions should return valid input already I havent tested for every possible exception so I clamp just in case
/// 
/// </summary>

public class colorFormatConversions : MonoBehaviour {

    //-------------------------convert between different FORMATS-------------------------

    //--- color in float format -> 255 format

    public float[] colorFloat_to_color255(float[] colorFloat)
    {
        float[] color255 = new float[colorFloat.Length];
        for (int i = 0; i < color255.Length; i++)
            color255[i] = _float_to_255(colorFloat[i]);
        return color255;
    }

    //--- color in float format -> hex format

    public string[] colorFloat_to_colorHex(float[] colorFloat) //TODO... proper conversion...
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

    float _float_to_255(float num)
    {
        return Mathf.Clamp(num * 255, 0, 255);
    }

    //--- (Float -> Hex)

    string _float_to_hex(float num)
    {
        string hex = Convert.ToString((int)Mathf.Round(255 * num), 16);
        return (hex.Length == 1) ? "0" + hex : hex;
    }

    //--- (255 -> Float)

    float _255_to_float(float num)
    {
        return Mathf.Clamp(num / 255, 0, 1);
    }

    //--- (255 -> Hex)

    string _255_to_hex(float num)
    {
        string hex = Convert.ToString((int)Mathf.Round(num), 16);
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

    //-------------------------convert between different TYPES-------------------------

    // Vector3 -> Color

    public Color vector3_to_color(Vector3 vect)
    {
        return new Color(vect.x, vect.y, vect.z);
    }

    // Color -> Vector3

    public Vector3 color_to_vector3(Color color)
    {
        return new Vector3(color.r, color.g, color.b);
    }
    
    // Color -> Float[]

    public float[] color_to_array(Color color)
    {
        return new float[] { color.r, color.g, color.b };
    }

    // Float[] -> Color

    public Color array_to_color(float[] floatColor)
    {
        if (floatColor.Length == 3)
            return new Color(floatColor[0], floatColor[1], floatColor[2]);
        else
            return Color.black;
    }

    //---Vector 2 <-> array

    public Vector2 array_to_vector2(float[] floatColor)
    {
        if (floatColor.Length == 2)
            return new Vector2(floatColor[0], floatColor[1]);
        else
            return Vector2.zero;
    }

    public float[] vector2_to_array(Vector2 floatColor)
    {
        return new float[] { floatColor[0], floatColor[1] };
    }

    //---Vector 3 <-> array

    public Vector3 array_to_vector3(float[] floatColor)
    {
        if (floatColor.Length == 3)
            return new Vector3(floatColor[0], floatColor[1], floatColor[2]);
        else
            return Vector3.zero;
    }

    public float[] vector3_to_array(Vector3 floatColor)
    {
        return new float[] { floatColor[0], floatColor[1], floatColor[2] };
    }

    //---Vector 4 <-> array

    public Vector4 array_to_vector4(float[] floatColor)
    {
        if (floatColor.Length == 4)
            return new Vector4(floatColor[0], floatColor[1], floatColor[2], floatColor[3]);
        else
            return Vector4.zero;
    }

    public float[] vector4_to_array(Vector4 floatColor)
    {
        return new float[] { floatColor[0], floatColor[1], floatColor[2], floatColor[3]};
    }

    //----------other----------

    public float[] clamp(float[] values, float min, float max)
    {
        for (int i = 0; i < values.Length; i++)
            values[i] = Mathf.Clamp(values[i], min, max);
        return values;
    }
}