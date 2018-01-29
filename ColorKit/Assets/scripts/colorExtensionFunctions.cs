using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using colorKit;

//allows the colorKit to also be addressible as extension methods of the colors class
//NOTE: that the instance of color that you use to call the method isnt used... its simply a placeholder

public static class colorExtensionFunctions
{

    #region colorFormatConversion

    //Description: Change the Color's Format (255,float,hex)

    //color in float format -> 255 format
    public static float[] colorFloat_to_color255(this Color c, float[] colorFloat)
    {
        return colorFormatConversion.colorFloat_to_color255(colorFloat);
    }

    //color in float format -> hex format
    public static string[] colorFloat_to_colorHex(this Color c, float[] colorFloat)
    {
        return colorFormatConversion.colorFloat_to_colorHex(colorFloat);
    }

    //color in 255 format -> float format
    public static float[] color255_to_colorFloat(this Color c, float[] color255)
    {
        return colorFormatConversion.color255_to_colorFloat(color255);
    }

    //color in 255 format -> hex format
    public static string[] color255_to_colorHex(this Color c, float[] color255)
    {
        return colorFormatConversion.color255_to_colorHex(color255);
    }

    //color in hex format -> float format
    public static float[] colorHex_to_colorFloat(this Color c, string[] colorHex)
    {
        return colorFormatConversion.colorHex_to_colorFloat(colorHex);
    }

    //color in hex format -> 255 format
    public static float[] colorHex_to_color255(this Color c, string[] colorHex)
    {
        return colorFormatConversion.colorHex_to_color255(colorHex);
    }

    #endregion

    #region colorTypeConversion

    //Description: Change the Color's Data Type (Vectors, Arrays, Colors)

    //-----2 component ??? (Vector2 | Array)

    public static float[] vector2_to_array(this Color c, Vector2 vector2)
    {
        return colorTypeConversion.vector2_to_array(vector2);
    }

    public static Vector2 array_to_vector2(this Color c, float[] array)
    {
        return colorTypeConversion.array_to_vector2(array);
    }

    //-----3 Component Color (Vector3 | Array | Color)

    public static float[] vector3_to_array(this Color c, Vector3 vector3)
    {
        return colorTypeConversion.vector3_to_array(vector3);
    }

    public static Color vector3_to_color(this Color c, Vector3 vector3)
    {
        return colorTypeConversion.vector3_to_color(vector3);
    }

    public static Vector3 array_to_vector3(this Color c, float[] array)
    {
        return colorTypeConversion.array_to_vector3(array);
    }

    public static Color array_to_color(this Color c, float[] array)
    {
        return colorTypeConversion.array_to_color(array);
    }

    public static Vector3 color_to_vector3(this Color c, Color color)
    {
        return colorTypeConversion.color_to_vector3(color);
    }

    public static float[] color_to_array(this Color c, Color color)
    {
        return colorTypeConversion.color_to_array(color);
    }

    //-----4 Component Color (Vector4 | Array)

    public static float[] vector4_to_array(this Color c, Vector4 vector4)
    {
        return colorTypeConversion.vector4_to_array(vector4);
    }

    public static Vector4 array_to_vector4(this Color c, float[] array)
    {
        return colorTypeConversion.array_to_vector4(array);
    }

    #endregion

    #region rgb2ryb_ryb2rgb

    //Description: Convert RGB to RYB -and- RYB to RGB

    public static float[] rgb255_to_ryb255(this Color c, float[] rgb255)
    {
        return rgb2ryb_ryb2rgb.rgb255_to_ryb255(rgb255);
    }

    public static float[] ryb255_to_rgb255(this Color c, float[] ryb255)
    {
        return rgb2ryb_ryb2rgb.ryb255_to_rgb255(ryb255);
    }

    #endregion

    #region rgb2cmyk_cmyk2rgb

    //Description: Convert RGB to CMYK -and- CMYK to RGB

    public static float[] rgb255_to_cmyk255(this Color c, float[] rgb255)
    {
        return rgb2cmyk_cmyk2rgb.rgb255_to_cmyk255(rgb255);
    }
    public static float[] cmyk255_to_rgb255(this Color c, float[] cmyk255)
    {
        return rgb2cmyk_cmyk2rgb.cmyk255_to_rgb255(cmyk255);
    }

    #endregion

    #region colorDistances 

    //Description: find the distance Between 2 Colors in 1D, 2D, 3D, and 4D Space

    //NOTE: Vector 4 distance works out to be really strange because we don't really have an accurate version of distance in 4 Dimensional Space

    public static float distBetweenColors(this Color c, colorSpace colorSpaceUsed, Color color1, Color color2)
    {
        return colorDistances.distBetweenColors(colorSpaceUsed, color1, color2);
    }

    public static float distBetweenColors(this Color c, float[] color1, float[] color2)
    {
        return colorDistances.distBetweenColors(color1, color2);
    }

    #endregion

    #region colorCompliments

    //get the complement / inverse of a color

    public static Color complimentary(this Color c, colorSpace csToUse, Color origColor)
    {
        return colorCompliments.complimentary(csToUse, origColor);
    }

    public static float[] complimentary(this Color c, float[] color, int floatLimit)
    {
        return colorCompliments.complimentary(color, floatLimit);
    }

    #endregion

    #region colorLerping

    //Allows you to interpolate between 2 colors

    public static Color colorLerp(this Color c, colorSpace csToUse, Color start, Color end, float lerpValue)
    {
        return colorLerping.colorLerp(csToUse, start, end, lerpValue);
    }

    public static float[] colorLerp(this Color c, float[] start, float[] end, float lerpValue)
    {
        return colorLerping.colorLerp(start, end, lerpValue);
    }

    public static float calculateLerpValueGiven(this Color c,
        guideDistance guideDistance, //if given MAX DIST -> convert to -> this dist
        float timeToTravel_GuideDistance, //units are below
        unitOfTime UnitOfTime_forTimeToTravelGuideDistance, //if given SECONDS -> convert to -> frames

        updateLocation LL, //update | fixedUpdate
        colorSpace LCS, //rgb | ryb | cmyk

        Color startColor,
        Color endColor,
        Color currColor
        )
    {
        return colorLerping.calcLerpVal(startColor, currColor, endColor, guideDistance, timeToTravel_GuideDistance, UnitOfTime_forTimeToTravelGuideDistance, LL, LCS);
    }

    #endregion

    #region colorMixing

    public static Color mixColors(this Color c, colorSpace csToUse, mixingMethod mm, Color[] colors)
    {
        return colorMixing.mixColors(csToUse, mm, colors);
    }

    public static Color mixColors(this Color c, colorSpace csToUse, mixingMethod mm, Color[] colors, float[] colorQuantities)
    {
        return colorMixing.mixColors(csToUse, mm, colors, colorQuantities);
    }

    #endregion

    #region mixingMethods

    public static float[] mixColors(this Color c, mixingMethod mm, List<float[]> colors)
    {
        return mixingMethods.mixColors(mm, colors);
    }

    public static float[] mixColors(this Color c, mixingMethod mm, List<float[]> colors, float[] colorQuantities)
    {
        return mixingMethods.mixColors(mm, colors, colorQuantities);
    }

    #endregion

    #region otherColorOps

    //-------------------------Print Functions-------------------------

    //---4 component

    public static void printVector4(this Color c, Vector4 vect4)
    {
        otherColorOps.printVector4(vect4);
    }

    public static void printVector4(this Color c, string printLabel, Vector4 vect4)
    {
        otherColorOps.printVector4(printLabel, vect4);
    }

    //---3 component

    public static void printVector3(this Color c, Vector3 vect3)
    {
        otherColorOps.printVector3(vect3);
    }

    public static void printVector3(this Color c, string printLabel, Vector3 vect3)
    {
        otherColorOps.printVector3(printLabel, vect3);
    }

    public static void printColor(this Color c, Color color)
    {
        otherColorOps.printColor(color);
    }

    public static void printColor(this Color c, string printLabel, Color color)
    {
        otherColorOps.printColor(printLabel, color);
    }

    //---2 component

    public static void printVector2(this Color c, Vector2 vect2)
    {
        otherColorOps.printVector2(vect2);
    }

    public static void printVector2(this Color c, string printLabel, Vector2 vect2)
    {
        otherColorOps.printVector2(printLabel, vect2);
    }

    //-----BASE

    public static void printArray(this Color c, string printLabel, float[] array)
    {
        otherColorOps.printArray(printLabel, array);
    }

    //-------------------------Error Correction-------------------------

    public static float[] nanCheck(this Color c, float[] array)
    {
        return otherColorOps.nanCheck(array);
    }

    public static float[] clamp(this Color c, float[] array, float min, float max)
    {
        return otherColorOps.clamp(array, min, max);
    }

    #endregion
}