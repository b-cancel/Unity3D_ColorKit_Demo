using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum desiredMixtureType { additive, subtractive };

public enum colorSpace { RGB, RYB, CMYK };
public enum updateLocation { fixedUpdate, Update };

public enum distanceUsedToCalculateLerpValue { distBetween_BlackAndWhite, distBetween_StartAndEndColor, distBetween_CurrentAndEndColor };
public enum unitOftime { frames, seconds };

public enum mixingMethod { spaceAveraging, colorAveraging, colorComponentAveraging, eachAsPercentOfMax }
public enum _4D_flawToAccept { useFlawedAglo, useWeirdVect4Dist };

public class otherColorOps : MonoBehaviour {

    public void printArray(string begin, float[] arr)
    {
        string text = begin + " ";

        for (int i = 0; i < arr.Length; i++)
        {
            if (i != (arr.Length - 1))
                text += arr[i] + ", ";
            else
                text += arr[i];
        }

        print(text);
    }

    public void printColor(Color col)
    {
        printColor("", col);
    }

    public void printColor(string begin, Color col)
    {
        printArray(begin, gameObject.GetComponent<colorTypeConversion>().color_to_array(col));
    }

    public void printVector4(Vector4 vect)
    {
        printVector4("", vect);
    }

    public void printVector4(string begin, Vector4 vect)
    {
        printArray(begin, gameObject.GetComponent<colorTypeConversion>().vector4_to_array(vect));
    }

    public void printVector3(Vector3 vect)
    {
        printVector3("", vect);
    }

    public void printVector3(string begin, Vector3 vect)
    {
        printArray(begin, gameObject.GetComponent<colorTypeConversion>().vector3_to_array(vect));
    }

    public void printVector2(Vector2 vect)
    {
        printVector2("", vect);
    }

    public void printVector2(string begin, Vector2 vect)
    {
        printArray(begin, gameObject.GetComponent<colorTypeConversion>().vector2_to_array(vect));
    }

    public float[] nanCheck(float[] arr)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            if (float.IsNaN(arr[i]))
                arr[i] = 0;
            else if (float.IsInfinity(arr[i]))
                print("Inf or Neg Inf");
        }  

        return arr;
    }

    //-------------------------Complementary-------------------------

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

    //-------------------------Distance Between Colors-------------------------

    public float distBetweenColors(colorSpace csToUse, Color color1, Color color2)
    {
        switch (csToUse)
        {
            case colorSpace.RGB:
                return distBetweenColors_inRGB_colorSpace(color1, color2);
            case colorSpace.RYB:
                return distBetweenColors_inRYB_colorSpace(color1, color2);
            default:
                return distBetweenColors_inCMYK_colorSpace(color1, color2);
        }
    }

    float distBetweenColors_inRGB_colorSpace(Color color1, Color color2)
    {
        float[] color1_Float_rGb = gameObject.GetComponent<colorTypeConversion>().color_to_array(color1);
        float[] color1_255_rGb = gameObject.GetComponent<colorFormatConversion>().colorFloat_to_color255(color1_Float_rGb);

        float[] color2_Float_rGb = gameObject.GetComponent<colorTypeConversion>().color_to_array(color2);
        float[] color2_255_rGb = gameObject.GetComponent<colorFormatConversion>().colorFloat_to_color255(color2_Float_rGb);

        return distanceBetweenColors(color1_255_rGb, color2_255_rGb);
    }

    float distBetweenColors_inRYB_colorSpace(Color color1, Color color2)
    {
        float[] color1_Float_rGb = gameObject.GetComponent<colorTypeConversion>().color_to_array(color1);
        float[] color1_255_rGb = gameObject.GetComponent<colorFormatConversion>().colorFloat_to_color255(color1_Float_rGb);
        float[] color1_255_rYb = gameObject.GetComponent<rgb2ryb_ryb2rgb>().rgb255_to_ryb255(color1_255_rGb);

        float[] color2_Float_rGb = gameObject.GetComponent<colorTypeConversion>().color_to_array(color2);
        float[] color2_255_rGb = gameObject.GetComponent<colorFormatConversion>().colorFloat_to_color255(color2_Float_rGb);
        float[] color2_255_rYb = gameObject.GetComponent<rgb2ryb_ryb2rgb>().rgb255_to_ryb255(color2_255_rGb);

        return distanceBetweenColors(color1_255_rYb, color2_255_rYb);
    }

    //NOTE: be warned that this version of 4D distance is strange and does not behave the way I would personally except it to
    float distBetweenColors_inCMYK_colorSpace(Color color1, Color color2)
    {
        float[] color1_Float_rGb = gameObject.GetComponent<colorTypeConversion>().color_to_array(color1);
        float[] color1_255_rGb = gameObject.GetComponent<colorFormatConversion>().colorFloat_to_color255(color1_Float_rGb);
        float[] color1_255_CMYK = gameObject.GetComponent<rgb2cmyk_cmyk2rgb>().rgb255_to_cmyk255(color1_255_rGb);

        float[] color2_Float_rGb = gameObject.GetComponent<colorTypeConversion>().color_to_array(color2);
        float[] color2_255_rGb = gameObject.GetComponent<colorFormatConversion>().colorFloat_to_color255(color2_Float_rGb);
        float[] color2_255_CMYK = gameObject.GetComponent<rgb2cmyk_cmyk2rgb>().rgb255_to_cmyk255(color2_255_rGb);

        return distanceBetweenColors(color1_255_CMYK, color2_255_CMYK);
    }

    //-----BASE

    public float distanceBetweenColors(float[] color1, float[] color2)
    {
        if ((color1.Length == color2.Length) && (color1.Length <= 4))
        {
            switch (color1.Length)
            {
                case 1:
                    return Mathf.Abs(color1[0] - color2[0]);
                case 2:
                    Vector2 color1Vect2 = gameObject.GetComponent<colorTypeConversion>().array_to_vector2(color1);
                    Vector2 color2Vect2 = gameObject.GetComponent<colorTypeConversion>().array_to_vector2(color2);

                    return Vector2.Distance(color1Vect2, color2Vect2);
                case 3:
                    Vector3 color1Vect3 = gameObject.GetComponent<colorTypeConversion>().array_to_vector3(color1);
                    Vector3 color2Vect3 = gameObject.GetComponent<colorTypeConversion>().array_to_vector3(color2);

                    return Vector3.Distance(color1Vect3, color2Vect3);
                case 4:
                    Vector4 color1Vect4 = gameObject.GetComponent<colorTypeConversion>().array_to_vector4(color1);
                    Vector4 color2Vect4 = gameObject.GetComponent<colorTypeConversion>().array_to_vector4(color2);

                    return Vector4.Distance(color1Vect4, color2Vect4);
                default:
                    return 0;
            }
        }
        else
            return 0;
    }

    //-------------------------Color Mixing Helper-------------------------

    public Color mixColors(colorSpace csToUse, mixingMethod mm, bool ignoreQuants, Color[] colors, float[] colorQuantities)
    {
        switch (csToUse)
        {
            case colorSpace.RGB:
                return mixColors_inRGB_colorSpace(mm, colors, colorQuantities, ignoreQuants);
            case colorSpace.RYB:
                return mixColors_inRYB_colorSpace(mm, colors, colorQuantities, ignoreQuants);
            default:
                return mixColors_inCMYK_colorSpace(mm, colors, colorQuantities, ignoreQuants);
        }
    }

    Color mixColors_inRGB_colorSpace(mixingMethod mm, Color[] colors, float[] colorQuantities, bool ignoreQuants)
    {
        List<float[]> passedColors = new List<float[]>();

        for (int i = 0; i < colors.Length; i++)
        {
            float[] colorFloat_RGB = gameObject.GetComponent<colorTypeConversion>().color_to_array(colors[i]);
            float[] color255_RGB = gameObject.GetComponent<colorFormatConversion>().colorFloat_to_color255(colorFloat_RGB);
            passedColors.Add(color255_RGB);
        }

        float[] result255_RGB = gameObject.GetComponent<colorMixing>().mixColors(mm, passedColors, colorQuantities, ignoreQuants); //actual Color Mixing

        float[] resultFloat_RGB = gameObject.GetComponent<colorFormatConversion>().color255_to_colorFloat(result255_RGB);

        return gameObject.GetComponent<colorTypeConversion>().array_to_color(resultFloat_RGB);
    }

    Color mixColors_inRYB_colorSpace(mixingMethod mm, Color[] colors, float[] colorQuantities, bool ignoreQuants)
    {
        List<float[]> passedColors = new List<float[]>();

        for (int i = 0; i < colors.Length; i++)
        {
            float[] colorFloat_RGB = gameObject.GetComponent<colorTypeConversion>().color_to_array(colors[i]);
            float[] color255_RGB = gameObject.GetComponent<colorFormatConversion>().colorFloat_to_color255(colorFloat_RGB);
            float[] color255_RYB = gameObject.GetComponent<rgb2ryb_ryb2rgb>().rgb255_to_ryb255(color255_RGB);
            passedColors.Add(color255_RYB);
        }

        float[] result255_RYB = gameObject.GetComponent<colorMixing>().mixColors(mm, passedColors, colorQuantities, ignoreQuants); //actual Color Mixing
        float[] result255_RGB = gameObject.GetComponent<rgb2ryb_ryb2rgb>().ryb255_to_rgb255(result255_RYB);
        float[] resultFloat_RGB = gameObject.GetComponent<colorFormatConversion>().color255_to_colorFloat(result255_RGB);

        return gameObject.GetComponent<colorTypeConversion>().array_to_color(resultFloat_RGB);
    }

    Color mixColors_inCMYK_colorSpace(mixingMethod mm, Color[] colors, float[] colorQuantities, bool ignoreQuants)
    {
        List<float[]> passedColors = new List<float[]>();

        for (int i = 0; i < colors.Length; i++)
        {
            float[] colorFloat_RGB = gameObject.GetComponent<colorTypeConversion>().color_to_array(colors[i]);
            float[] color255_RGB = gameObject.GetComponent<colorFormatConversion>().colorFloat_to_color255(colorFloat_RGB);
            float[] color255_CMYK = gameObject.GetComponent<rgb2cmyk_cmyk2rgb>().rgb255_to_cmyk255(color255_RGB);
            passedColors.Add(color255_CMYK);
        }

        float[] result255_CMYK = gameObject.GetComponent<colorMixing>().mixColors(mm, passedColors, colorQuantities, ignoreQuants); //actual Color Mixing
        float[] result255_RGB = gameObject.GetComponent<rgb2cmyk_cmyk2rgb>().cmyk255_to_rgb255(result255_CMYK);
        float[] resultFloat_RGB = gameObject.GetComponent<colorFormatConversion>().color255_to_colorFloat(result255_RGB);

        return gameObject.GetComponent<colorTypeConversion>().array_to_color(resultFloat_RGB);
    }

    //-----BASE (in color mixing script)

    /// <summary>
    /// These functions are used to lerp between two colors by thinking of them as 2 points in 3 dimensional space. 
    /// All the Colors in the rGb and rYb color space now create a sort of color cube and we simply travel between two points on that cube.
    /// 
    /// NOTE: cmyk has 4 components BUT NOT all of the same type
    /// In essense k is a combination of cmy which is why we cant lerp WELL from color to color within the cmyk color space
    /// EX: black to white -> 255 apart... cyan to magenta -> 255 apart although black and white is on complete ends of the color space spectrum
    /// Eventhough we cant lerp well Im still lerping with the Vector4.Distance && Vector4.Lerp Functions
    /// 
    /// NOTE: I always lerp using the 255 version of the color because since the numbers are larger the errors make less of a difference
    /// </summary>

    //-------------------------Color Lerping-------------------------

    public Color colorLerp(colorSpace csToUse, Color start, Color end, float lerpValue) //value between 0 and 1
    {
        switch (csToUse)
        {
            case colorSpace.RGB:
                return colorLerp_inRGB_colorSpace(start, end, lerpValue);
            case colorSpace.RYB:
                return colorLerp_inRYB_colorSpace(start, end, lerpValue);
            default:
                return colorLerp_inCMYK_colorSpace(start, end, lerpValue);
        }
    }

    Color colorLerp_inRGB_colorSpace(Color start, Color end, float lerpValue) //value between 0 and 1
    {
        float[] startFloat_rGb = gameObject.GetComponent<colorTypeConversion>().color_to_array(start);
        float[] start255_rGb = gameObject.GetComponent<colorFormatConversion>().colorFloat_to_color255(startFloat_rGb);

        float[] endFloat_rGb = gameObject.GetComponent<colorTypeConversion>().color_to_array(end);
        float[] end255_rGb = gameObject.GetComponent<colorFormatConversion>().colorFloat_to_color255(endFloat_rGb);

        float[] result255_rGb = colorLerp(start255_rGb, end255_rGb, lerpValue);
        float[] resultFloat = gameObject.GetComponent<colorFormatConversion>().color255_to_colorFloat(result255_rGb);

        return gameObject.GetComponent<colorTypeConversion>().array_to_color(resultFloat);
    }

    Color colorLerp_inRYB_colorSpace(Color start, Color end, float lerpValue) //value between 0 and 1
    {
        float[] startFloat_rGb = gameObject.GetComponent<colorTypeConversion>().color_to_array(start);
        float[] start255_rGb = gameObject.GetComponent<colorFormatConversion>().colorFloat_to_color255(startFloat_rGb);
        float[] start255_rYb = gameObject.GetComponent<rgb2ryb_ryb2rgb>().rgb255_to_ryb255(start255_rGb);

        float[] endFloat_rGb = gameObject.GetComponent<colorTypeConversion>().color_to_array(end);
        float[] end255_rGb = gameObject.GetComponent<colorFormatConversion>().colorFloat_to_color255(endFloat_rGb);
        float[] end255_rYb = gameObject.GetComponent<rgb2ryb_ryb2rgb>().rgb255_to_ryb255(end255_rGb);

        float[] result255_rYb = colorLerp(start255_rYb, end255_rYb, lerpValue);

        float[] result255_rGb = gameObject.GetComponent<rgb2ryb_ryb2rgb>().ryb255_to_rgb255(result255_rYb); //255 rgb
        float[] resultFloat = gameObject.GetComponent<colorFormatConversion>().color255_to_colorFloat(result255_rGb);

        return gameObject.GetComponent<colorTypeConversion>().array_to_color(resultFloat);
    }

    Color colorLerp_inCMYK_colorSpace(Color start, Color end, float lerpValue) //value between 0 and 1
    {
        float[] startFloat_rGb = gameObject.GetComponent<colorTypeConversion>().color_to_array(start);
        float[] start255_rGb = gameObject.GetComponent<colorFormatConversion>().colorFloat_to_color255(startFloat_rGb);
        float[] start255_CMYK = gameObject.GetComponent<rgb2cmyk_cmyk2rgb>().rgb255_to_cmyk255(start255_rGb);

        float[] endFloat_rGb = gameObject.GetComponent<colorTypeConversion>().color_to_array(end);
        float[] end255_rGb = gameObject.GetComponent<colorFormatConversion>().colorFloat_to_color255(endFloat_rGb);
        float[] end255_CMYK = gameObject.GetComponent<rgb2cmyk_cmyk2rgb>().rgb255_to_cmyk255(end255_rGb);

        float[] result255_CMYK = colorLerp(start255_CMYK, end255_CMYK, lerpValue);

        float[] result255_rGb = gameObject.GetComponent<rgb2cmyk_cmyk2rgb>().cmyk255_to_rgb255(result255_CMYK); //255 rgb
        float[] resultFloat = gameObject.GetComponent<colorFormatConversion>().color255_to_colorFloat(result255_rGb);

        return gameObject.GetComponent<colorTypeConversion>().array_to_color(resultFloat);
    }

    //-----BASE

    public float[] colorLerp(float[] start, float[] end, float lerpValue)
    {
        if (start.Length == end.Length)
        {
            lerpValue = Mathf.Clamp01(lerpValue);

            if (start.Length == 3)
            {
                Vector3 startVect = gameObject.GetComponent<colorTypeConversion>().array_to_vector3(start);
                Vector3 endVect = gameObject.GetComponent<colorTypeConversion>().array_to_vector3(end);

                Vector3 resultVect = Vector3.Lerp(startVect, endVect, lerpValue);

                return gameObject.GetComponent<colorTypeConversion>().vector3_to_array(resultVect);
            }
            else if (start.Length == 4)
            {
                Vector4 startVect = gameObject.GetComponent<colorTypeConversion>().array_to_vector4(start);
                Vector4 endVect = gameObject.GetComponent<colorTypeConversion>().array_to_vector4(end);

                Vector4 resultVect = Vector4.Lerp(startVect, endVect, lerpValue);

                return gameObject.GetComponent<colorTypeConversion>().vector4_to_array(resultVect);
            }
            else
                return start;
        }
        else
            return start;
    }

    //-------------------------Color Lerping Helpers------------------------- 

    //-----MAX Color Distances

    public float maxDistanceInRGBColorSpace { get; private set; }
    public float maxDistanceInRYBColorSpace { get; private set; }
    public float maxDistanceInCMYKColorSpace { get; private set; } //once again becaue of the nature of 4D space this will work strangely...

    void Awake()
    {
        float[] rgb1_255 = new float[] { 0, 0, 0 };
        float[] rgb2_255 = new float[] { 255, 255, 255 };
        maxDistanceInRGBColorSpace = distanceBetweenColors(rgb1_255, rgb2_255);

        float[] ryb1_255 = gameObject.GetComponent<rgb2ryb_ryb2rgb>().rgb255_to_ryb255(rgb1_255);
        float[] ryb2_255 = gameObject.GetComponent<rgb2ryb_ryb2rgb>().rgb255_to_ryb255(rgb2_255);
        maxDistanceInRYBColorSpace = distanceBetweenColors(ryb1_255, ryb2_255);

        float[] cmyk1_255 = gameObject.GetComponent<rgb2cmyk_cmyk2rgb>().rgb255_to_cmyk255(rgb1_255);
        float[] cmyk2_255 = gameObject.GetComponent<rgb2cmyk_cmyk2rgb>().rgb255_to_cmyk255(rgb2_255);
        maxDistanceInCMYKColorSpace = distanceBetweenColors(cmyk1_255, cmyk2_255);
    }

    //This should be used to calculate the lerpValue For "Color.Lerp(currColor, endColor, lerpValueCalculatedByThisFunction)"
    public float calculateLerpValueGiven(
        distanceUsedToCalculateLerpValue guideDistance, //if given MAX DIST -> convert to -> this dist
        float timeToTravel_GuideDistance, //units are below
        unitOftime UnitOfTime_forTimeToTravelGuideDistance, //if given SECONDS -> convert to -> frames

        updateLocation LL, //update | fixedUpdate
        colorSpace LCS, //rgb | ryb | cmyk

        Color startColor,
        Color endColor,
        Color currColor
        )
    {
        float framesToTravelPassedDistance;
        if (UnitOfTime_forTimeToTravelGuideDistance == unitOftime.frames)
            framesToTravelPassedDistance = timeToTravel_GuideDistance;
        else
        {
            if (LL == updateLocation.fixedUpdate)
                framesToTravelPassedDistance = timeToTravel_GuideDistance / Time.fixedDeltaTime;
            else
                framesToTravelPassedDistance = timeToTravel_GuideDistance / Time.deltaTime;
        }

        //---Calculate our Distances

        float dist_B_2_W = 0;
        float dist_S_2_E = 0;
        float dist_C_2_E = 0;

        switch (LCS)
        {
            case colorSpace.RGB:
                dist_B_2_W = maxDistanceInRGBColorSpace;
                dist_S_2_E = distBetweenColors_inRGB_colorSpace(startColor, endColor);
                dist_C_2_E = distBetweenColors_inRGB_colorSpace(currColor, endColor);
                break;
            case colorSpace.RYB:
                dist_B_2_W = maxDistanceInRYBColorSpace;
                dist_S_2_E = distBetweenColors_inRYB_colorSpace(startColor, endColor);
                dist_C_2_E = distBetweenColors_inRYB_colorSpace(currColor, endColor);
                break;
            case colorSpace.CMYK:
                dist_B_2_W = maxDistanceInCMYKColorSpace;
                dist_S_2_E = distBetweenColors_inCMYK_colorSpace(startColor, endColor);
                dist_C_2_E = distBetweenColors_inCMYK_colorSpace(currColor, endColor);
                break;
        }

        //---Calculate the Frames Each Distance Would Take To Complete

        float framesToFinsih_B_2_W = -1;
        float framesToFinsih_S_2_E = -1;
        float framesToFinsih_C_2_E = -1;

        switch (guideDistance)
        {
            case distanceUsedToCalculateLerpValue.distBetween_BlackAndWhite:
                framesToFinsih_B_2_W = framesToTravelPassedDistance;
                framesToFinsih_S_2_E = (framesToTravelPassedDistance / dist_B_2_W) * dist_S_2_E;
                framesToFinsih_C_2_E = (framesToTravelPassedDistance / dist_B_2_W) * dist_C_2_E;
                break;
            case distanceUsedToCalculateLerpValue.distBetween_StartAndEndColor:
                framesToFinsih_B_2_W = (framesToTravelPassedDistance / dist_S_2_E) * dist_B_2_W;
                framesToFinsih_S_2_E = framesToTravelPassedDistance;
                framesToFinsih_C_2_E = (framesToTravelPassedDistance / dist_S_2_E) * dist_C_2_E;
                break;
            case distanceUsedToCalculateLerpValue.distBetween_CurrentAndEndColor:
                framesToFinsih_B_2_W = (framesToTravelPassedDistance / dist_C_2_E) * dist_B_2_W;
                framesToFinsih_S_2_E = (framesToTravelPassedDistance / dist_C_2_E) * dist_S_2_E;
                framesToFinsih_C_2_E = framesToTravelPassedDistance;
                break;
        }

        framesToFinsih_B_2_W = (int)Mathf.CeilToInt(framesToFinsih_B_2_W); //NOTE: I was previously using FloorToInt()... using Ceil is untested
        framesToFinsih_S_2_E = (int)Mathf.CeilToInt(framesToFinsih_S_2_E); //NOTE: I was previously using FloorToInt()... using Ceil is untested
        framesToFinsih_C_2_E = (int)Mathf.CeilToInt(framesToFinsih_C_2_E); //NOTE: I was previously using FloorToInt()... using Ceil is untested

        //NOTE: as of now we know how many frames every distance will take... so now we use said data to lerp

        //NOTE: this value that took so long to calculate will not change because we are aiming for a LERP
        //so we travel the same distance every frame as long as our settings stay the same
        //The Implication is that we only need to run this function once  and then just run "the one line" below every frame
        float distPerFrame = dist_C_2_E / framesToFinsih_C_2_E;

        float lerpValue = Mathf.Clamp((distPerFrame / dist_C_2_E), 0, 1);

        return lerpValue;
    }

    public float[] clamp(float[] values, float min, float max)
    {
        for (int i = 0; i < values.Length; i++)
            values[i] = Mathf.Clamp(values[i], min, max);
        return values;
    }
}