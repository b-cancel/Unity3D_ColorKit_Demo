using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using colorKit;
using lerpKit;

namespace colorKit
{
    public enum desiredMixtureType { additive, subtractive };
    public enum colorSpace { RGB, RYB, CMYK };
    public enum mixingMethod { spaceAveraging, colorAveraging, colorComponentAveraging, eachAsPercentOfMax }
}

//DESCRIPTION: this script allows the colorKit to also be addressible as extension methods of Unity's Color class (and others)

public static class colorEXT
{
    //-----FUNCTION VERSIONS that use a dummy (non used) instance of 'Color'

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

    public static float distBetweenColors(this Color c, Color color1, Color color2, colorSpace colorSpaceUsed)
    {
        return colorDistances.distBetweenColors(color1, color2, colorSpaceUsed);
    }

    public static float distBetweenColors(this Color c, float[] color1, float[] color2)
    {
        return colorDistances.distBetweenColors(color1, color2);
    }

    #endregion

    #region colorCompliments

    //get the complement / inverse of a color

    public static Color complimentary(this Color c, Color origColor, colorSpace csToUse)
    {
        return colorCompliments.complimentary(origColor, csToUse);
    }

    public static float[] complimentary(this Color c, float[] color, int floatLimit)
    {
        return colorCompliments.complimentary(color, floatLimit);
    }

    #endregion

    #region colorLerping

    //Allows you to interpolate between 2 colors

    public static Color colorLerp(this Color c, Color start, Color end, float lerpValue, colorSpace csToUse)
    {
        return colorLerping.colorLerp(start, end, lerpValue, csToUse);
    }

    public static float[] colorLerp(this Color c, float[] start, float[] end, float lerpValue)
    {
        return colorLerping.colorLerp(start, end, lerpValue);
    }

    #endregion

    #region colorLerpHelper

    public static float calcGuideDistance(this Color c, Color startColor, Color currColor, Color endColor, colorSpace CS, guideDistance GD)
    {
        return colorLerpHelper.calcGuideDistance(startColor, currColor, endColor, CS, GD);
    }

    public static float calcLerpValue(this Color c, Color startColor, Color currColor, Color endColor, float guideDistance, float guideTime, unitOfTime UOT_GD, updateLocation UL, colorSpace CS)
    {
        return colorLerpHelper.calcLerpValue(startColor, currColor, endColor, guideDistance, guideTime, UOT_GD, UL, CS);
    }

    public static float calcLerpValue(this Color c, Color startColor, Color currColor, Color endColor, float lerpVelocity_DperF, colorSpace CS)
    {
        return colorLerpHelper.calcLerpValue(startColor, currColor, endColor, lerpVelocity_DperF, CS);
    }

    #endregion

    #region colorMixing

    public static Color mixColors(this Color c, Color[] colors, colorSpace csToUse, mixingMethod mm)
    {
        return colorMixing.mixColors(colors, csToUse, mm);
    }

    public static Color mixColors(this Color c, Color[] colors, float[] colorQuantities, colorSpace csToUse, mixingMethod mm)
    {
        return colorMixing.mixColors(colors, colorQuantities, csToUse, mm);
    }

    #endregion

    #region mixingMethods

    public static float[] mixColors(this Color c, List<float[]> colors, mixingMethod mm)
    {
        return mixingMethods.mixColors(colors, mm);
    }

    public static float[] mixColors(this Color c, List<float[]> colors, float[] colorQuantities, mixingMethod mm)
    {
        return mixingMethods.mixColors(colors, colorQuantities, mm);
    }

    #endregion

    //-----FUNCTION VERSIONS that use the instance of whatever type they extend (the instance will be of the same type as the first parameter)

    #region rgb2ryb_ryb2rgb

    //Description: Convert RGB to RYB -and- RYB to RGB

    public static float[] rgb255_to_ryb255(this float[] rgb255)
    {
        return rgb2ryb_ryb2rgb.rgb255_to_ryb255(rgb255);
    }

    public static float[] ryb255_to_rgb255(this float[] ryb255)
    {
        return rgb2ryb_ryb2rgb.ryb255_to_rgb255(ryb255);
    }

    #endregion

    #region rgb2cmyk_cmyk2rgb

    //Description: Convert RGB to CMYK -and- CMYK to RGB

    public static float[] rgb255_to_cmyk255(this float[] rgb255)
    {
        return rgb2cmyk_cmyk2rgb.rgb255_to_cmyk255(rgb255);
    }
    public static float[] cmyk255_to_rgb255(this float[] cmyk255)
    {
        return rgb2cmyk_cmyk2rgb.cmyk255_to_rgb255(cmyk255);
    }

    #endregion

    #region colorDistances 

    //Description: find the distance Between 2 Colors in 1D, 2D, 3D, and 4D Space

    //NOTE: Vector 4 distance works out to be really strange because we don't really have an accurate version of distance in 4 Dimensional Space

    public static float distBetweenColors(this Color color1, Color color2, colorSpace colorSpaceUsed)
    {
        return colorDistances.distBetweenColors(color1, color2, colorSpaceUsed);
    }

    public static float distBetweenColors(this float[] color1, float[] color2)
    {
        return colorDistances.distBetweenColors(color1, color2);
    }

    #endregion

    #region colorCompliments

    //get the complement / inverse of a color

    public static Color complimentary(this Color color, colorSpace csToUse)
    {
        return colorCompliments.complimentary(color, csToUse);
    }

    public static float[] complimentary(this float[] color, int floatLimit)
    {
        return colorCompliments.complimentary(color, floatLimit);
    }

    #endregion

    #region colorLerping

    //Allows you to interpolate between 2 colors

    public static Color colorLerp(this Color startColor, Color endColor, float lerpValue, colorSpace csToUse)
    {
        return colorLerping.colorLerp(startColor, endColor, lerpValue, csToUse);
    }

    public static float[] colorLerp(this float[] startValues, float[] endValues, float lerpValue)
    {
        return colorLerping.colorLerp(startValues, endValues, lerpValue);
    }

    #endregion

    #region colorLerpHelper

    public static float calcGuideDistance(this Color startColor, Color currColor, Color endColor, colorSpace CS, guideDistance GD)
    {
        return colorLerpHelper.calcGuideDistance(startColor, currColor, endColor, CS, GD);
    }

    public static float calcLerpValue(this Color startColor, Color currColor, Color endColor, float guideDistance, float guideTime, unitOfTime UOT_GD, updateLocation UL, colorSpace CS)
    {
        return colorLerpHelper.calcLerpValue(startColor, currColor, endColor, guideDistance, guideTime, UOT_GD, UL, CS);
    }

    public static float calcLerpValue(this Color startColor, Color currColor, Color endColor, float lerpVelocity_DperF, colorSpace CS)
    {
        return colorLerpHelper.calcLerpValue(startColor, currColor, endColor, lerpVelocity_DperF, CS);
    }

    #endregion

    #region colorMixing

    public static Color mixColors(this Color[] colors, colorSpace csToUse, mixingMethod mm)
    {
        return colorMixing.mixColors(colors, csToUse, mm);
    }

    public static Color mixColors(this Color[] colors, float[] colorQuantities, colorSpace csToUse, mixingMethod mm)
    {
        return colorMixing.mixColors(colors, colorQuantities, csToUse, mm);
    }

    #endregion

    #region mixingMethods

    public static float[] mixColors(this List<float[]> colors, mixingMethod mm)
    {
        return mixingMethods.mixColors(colors, mm);
    }

    public static float[] mixColors(this List<float[]> colors, float[] colorQuantities, mixingMethod mm)
    {
        return mixingMethods.mixColors(colors, colorQuantities, mm);
    }

    #endregion
}