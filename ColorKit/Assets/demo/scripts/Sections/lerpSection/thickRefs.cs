using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using colorKit;
using extraKit;

[ExecuteInEditMode]
public class thickRefs : MonoBehaviour {

    public GameObject colorSample;

    public GameObject rgb;
    public GameObject ryb;
    public GameObject cmyk;

    public void updateColor(Color newColor)
    {
        colorSample.GetComponent<Image>().color = newColor;

        float[] rgbFloat = typeConversion.color_to_array(newColor);
        float[] rgb255 = formatConversion._float_to_255(rgbFloat);
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
