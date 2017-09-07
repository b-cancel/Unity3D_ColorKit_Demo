using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The function names indicate 
///     INPUT[color space][format of components in input]_2_OUTPUT[color space][format of components in output]
/// If you pass an array of improper length, you will get an array of the expected length returned... but it will be filled with "-1"
/// Details on more accurate SUBTRACTIVE color mixing methods here: https://gist.github.com/b-cancel/70de2dfa0705e94045574931b5c8e664 
///     
/// NOTE: converting white and black to a different color space can cause errors which is why we added the if and else if statements on both functions
/// </summary>

public class rgb2cmyk_cmyk2rgb : MonoBehaviour {

    //-------------------------RGB -> CMKY-------------------------

    public float[] rgbFloat_to_cmykFloat(float[] rgbFloat)
    {
        if (rgbFloat.Length != 3)
            return new float[] { -1, -1, -1 };
        else
        {
            float[] rgb255 = gameObject.GetComponent<colorFormatConversions>().colorFloat_to_color255(rgbFloat);
            return rgb255_to_cmykFloat(rgb255);
        }
    }

    public float[] rgbFloat_to_cmyk255(float[] rgbFloat)
    {
        if (rgbFloat.Length != 3)
            return new float[] { -1, -1, -1 };
        else
        {
            float[] cmykFloat = rgbFloat_to_cmykFloat(rgbFloat);
            return gameObject.GetComponent<colorFormatConversions>().colorFloat_to_color255(cmykFloat);
        }
    }

    public float[] rgb255_to_cmyk255(float[] rgb255)
    {
        if (rgb255.Length != 3)
            return new float[] { -1, -1, -1 };
        else
        {
            float[] cmykFloat = rgb255_to_cmykFloat(rgb255);
            return gameObject.GetComponent<colorFormatConversions>().colorFloat_to_color255(cmykFloat);
        }
    }

    public float[] rgb255_to_cmykFloat(float[] rgb255) //NOTE: all different format conversion types use this function
    {
        if (rgb255.Length != 3)
            return new float[] { -1, -1, -1 };
        else
        {
            if (rgb255[0] == 0 && rgb255[1] == 0 && rgb255[2] == 0) //black
                return new float[] { 0, 0, 0, 1 }; //black
            else if (rgb255[0] == 255 && rgb255[1] == 255 && rgb255[2] == 255) //white
                return new float[] { 0, 0, 0, 0 }; //white
            else
            {
                float cyan = 255 - rgb255[0];
                float magenta = 255 - rgb255[1];
                float yellow = 255 - rgb255[2];
                float black = Mathf.Min(cyan, magenta, yellow);
                cyan = ((cyan - black) / (255 - black));
                magenta = ((magenta - black) / (255 - black));
                yellow = ((yellow - black) / (255 - black));

                // And return back the cmyk typed accordingly.
                float[] cmykFloat = new float[] { cyan, magenta, yellow, black };
                cmykFloat = gameObject.GetComponent<colorFormatConversions>().clamp(cmykFloat, 0, 1);
                return gameObject.GetComponent<otherColorOps>().nanCheck(cmykFloat);
            }
        }
    }

    //-------------------------CMKY -> RGB-------------------------

    public float[] cmykFloat_to_rgbFloat(float[] cmykFloat)
    {
        if (cmykFloat.Length != 4)
            return new float[] { -1, -1, -1 };
        else
        {
            float[] rgb255 = cmykFloat_to_rgb255(cmykFloat);
            return gameObject.GetComponent<colorFormatConversions>().color255_to_colorFloat(rgb255);
        }
    }

    public float[] cmykFloat_to_rgb255(float[] cmykFloat) //NOTE: all different format conversion types use this function
    {
        if (cmykFloat.Length != 4)
            return new float[] { -1, -1, -1 };
        else
        {
            if (cmykFloat[0] == 0 && cmykFloat[1] == 0 && cmykFloat[2] == 0 && cmykFloat[3] == 1) //black
                return new float[] { 0, 0, 0 }; //black
            else if (cmykFloat[0] == 0 && cmykFloat[1] == 0 && cmykFloat[2] == 0 && cmykFloat[3] == 0) //white
                return new float[] { 255, 255, 255 }; //white
            else
            {
                float red = (float)((cmykFloat[0] * (255 - cmykFloat[3])) + cmykFloat[3]);
                float green = (float)((cmykFloat[1] * (255 - cmykFloat[3])) + cmykFloat[3]);
                float blue = (float)((cmykFloat[2] * (255 - cmykFloat[3])) + cmykFloat[3]);
                red = 255 - red;
                green = 255 - green;
                blue = 255 - blue;

                // And return back the rgb typed accordingly.
                float[] rgb255 = new float[] { red, green, blue };
                rgb255 = gameObject.GetComponent<colorFormatConversions>().clamp(rgb255, 0, 255);
                return gameObject.GetComponent<otherColorOps>().nanCheck(rgb255);
            }
        }
    }

    public float[] cmyk255_to_rgb255(float[] cmyk255)
    {
        if (cmyk255.Length != 4)
            return new float[] { -1, -1, -1 };
        else
        {
            float[] cmykFloat = gameObject.GetComponent<colorFormatConversions>().color255_to_colorFloat(cmyk255);
            return cmykFloat_to_rgb255(cmykFloat);
        }
    }

    public float[] cmyk255_rgbFloat(float[] cmyk255)
    {
        if (cmyk255.Length != 4)
            return new float[] { -1, -1, -1};
        else
        {
            float[] rgb255 = cmyk255_to_rgb255(cmyk255);
            return gameObject.GetComponent<colorFormatConversions>().color255_to_colorFloat(rgb255);
        }
    }
}
