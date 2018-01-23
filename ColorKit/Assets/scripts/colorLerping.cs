using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace colorKit
{
    //TODO... check function "calculateLerpValueGiven"... create a version of it for all types of lerping

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

    public static class colorLerping
    {

        //-------------------------Color Lerping-------------------------

        public static Color colorLerp(colorSpace csToUse, Color start, Color end, float lerpValue) //value between 0 and 1
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

        static Color colorLerp_inRGB_colorSpace(Color start, Color end, float lerpValue) //value between 0 and 1
        {
            float[] startFloat_rGb = colorTypeConversion.color_to_array(start);
            float[] start255_rGb = colorFormatConversion.colorFloat_to_color255(startFloat_rGb);

            float[] endFloat_rGb = colorTypeConversion.color_to_array(end);
            float[] end255_rGb = colorFormatConversion.colorFloat_to_color255(endFloat_rGb);

            float[] result255_rGb = colorLerp(start255_rGb, end255_rGb, lerpValue);
            float[] resultFloat = colorFormatConversion.color255_to_colorFloat(result255_rGb);

            return colorTypeConversion.array_to_color(resultFloat);
        }

        static Color colorLerp_inRYB_colorSpace(Color start, Color end, float lerpValue) //value between 0 and 1
        {
            float[] startFloat_rGb = colorTypeConversion.color_to_array(start);
            float[] start255_rGb = colorFormatConversion.colorFloat_to_color255(startFloat_rGb);
            float[] start255_rYb = rgb2ryb_ryb2rgb.rgb255_to_ryb255(start255_rGb);

            float[] endFloat_rGb = colorTypeConversion.color_to_array(end);
            float[] end255_rGb = colorFormatConversion.colorFloat_to_color255(endFloat_rGb);
            float[] end255_rYb = rgb2ryb_ryb2rgb.rgb255_to_ryb255(end255_rGb);

            float[] result255_rYb = colorLerp(start255_rYb, end255_rYb, lerpValue);

            float[] result255_rGb = rgb2ryb_ryb2rgb.ryb255_to_rgb255(result255_rYb); //255 rgb
            float[] resultFloat = colorFormatConversion.color255_to_colorFloat(result255_rGb);

            return colorTypeConversion.array_to_color(resultFloat);
        }

        static Color colorLerp_inCMYK_colorSpace(Color start, Color end, float lerpValue) //value between 0 and 1
        {
            float[] startFloat_rGb = colorTypeConversion.color_to_array(start);
            float[] start255_rGb = colorFormatConversion.colorFloat_to_color255(startFloat_rGb);
            float[] start255_CMYK = rgb2cmyk_cmyk2rgb.rgb255_to_cmyk255(start255_rGb);

            float[] endFloat_rGb = colorTypeConversion.color_to_array(end);
            float[] end255_rGb = colorFormatConversion.colorFloat_to_color255(endFloat_rGb);
            float[] end255_CMYK = rgb2cmyk_cmyk2rgb.rgb255_to_cmyk255(end255_rGb);

            float[] result255_CMYK = colorLerp(start255_CMYK, end255_CMYK, lerpValue);

            float[] result255_rGb = rgb2cmyk_cmyk2rgb.cmyk255_to_rgb255(result255_CMYK); //255 rgb
            float[] resultFloat = colorFormatConversion.color255_to_colorFloat(result255_rGb);

            return colorTypeConversion.array_to_color(resultFloat);
        }

        //-----BASE

        public static float[] colorLerp(float[] start, float[] end, float lerpValue) //value between 0 and 1
        {
            if (start.Length == end.Length)
            {
                lerpValue = Mathf.Clamp01(lerpValue);

                if (start.Length == 3)
                {
                    Vector3 startVect = colorTypeConversion.array_to_vector3(start);
                    Vector3 endVect = colorTypeConversion.array_to_vector3(end);

                    Vector3 resultVect = Vector3.Lerp(startVect, endVect, lerpValue);

                    return colorTypeConversion.vector3_to_array(resultVect);
                }
                else if (start.Length == 4)
                {
                    Vector4 startVect = colorTypeConversion.array_to_vector4(start);
                    Vector4 endVect = colorTypeConversion.array_to_vector4(end);

                    Vector4 resultVect = Vector4.Lerp(startVect, endVect, lerpValue);

                    return colorTypeConversion.vector4_to_array(resultVect);
                }
                else
                    return start;
            }
            else
                return start;
        }

        //-------------------------Color Lerping Helpers------------------------- 

        //-----MAX Color Distances (distance between black and white in that particular color space) [assuming all components of the color are in 255 format]

        //TODO... big yet unituitive optimization possible
        //This should be used to calculate the lerpValue For "Color.Lerp(currColor, endColor, lerpValueCalculatedByThisFunction)"
        public static float calculateLerpValueGiven(
            distanceUsedToCalculateLerpValue guideDistance, //if given MAX DIST -> convert to -> this dist
            float timeToTravel_GuideDistance, //units are below
            unitOfTime UnitOfTime_forTimeToTravelGuideDistance, //if given SECONDS -> convert to -> frames

            updateLocation LL, //update | fixedUpdate
            colorSpace LCS, //rgb | ryb | cmyk

            Color startColor,
            Color endColor,
            Color currColor
            )
        {

            //---first calculated how many frames we need to travel our guide distance

            float framesToTravelGuideDistance;
            if (UnitOfTime_forTimeToTravelGuideDistance == unitOfTime.frames)
                framesToTravelGuideDistance = timeToTravel_GuideDistance; //the user knows whether they mean physics (fixedUpdate) or graphics (Update) frames
            else //unitOfTime.seconds
            {
                if (LL == updateLocation.fixedUpdate)
                    framesToTravelGuideDistance = timeToTravel_GuideDistance / Time.fixedDeltaTime;
                else //updateLocation.Update
                    framesToTravelGuideDistance = timeToTravel_GuideDistance / Time.deltaTime;
            }

            //---Calculate our Distances

            float dist_B_2_W = 0;
            float dist_S_2_E = 0;
            float dist_C_2_E = 0;

            switch (LCS)
            {
                case colorSpace.RGB:
                    dist_B_2_W = 441.672956f; // maxDistanceInRGBColorSpace
                    dist_S_2_E = colorDistances.distBetweenColors(colorSpace.RGB, startColor, endColor);
                    dist_C_2_E = colorDistances.distBetweenColors(colorSpace.RGB, currColor, endColor);
                    break;
                case colorSpace.RYB:
                    dist_B_2_W = 441.672956f; //maxDistanceInRYBColorSpace
                    dist_S_2_E = colorDistances.distBetweenColors(colorSpace.RYB, startColor, endColor);
                    dist_C_2_E = colorDistances.distBetweenColors(colorSpace.RYB, currColor, endColor);
                    break;
                case colorSpace.CMYK:
                    dist_B_2_W = 255; //maxDistanceInCMYKColorSpace (because we have no accurate representation for 4D distance)
                    dist_S_2_E = colorDistances.distBetweenColors(colorSpace.CMYK, startColor, endColor);
                    dist_C_2_E = colorDistances.distBetweenColors(colorSpace.CMYK, currColor, endColor);
                    break;
            }

            //---Calculate how many frames we have left to finish

            float framesToFinsih_C_2_E = -1;

            switch (guideDistance)
            {
                case distanceUsedToCalculateLerpValue.distBetween_BlackAndWhite:
                    framesToFinsih_C_2_E = (framesToTravelGuideDistance / dist_B_2_W) * dist_C_2_E;
                    break;
                case distanceUsedToCalculateLerpValue.distBetween_StartAndEndColor:
                    framesToFinsih_C_2_E = (framesToTravelGuideDistance / dist_S_2_E) * dist_C_2_E;
                    break;
                case distanceUsedToCalculateLerpValue.distBetween_CurrentAndEndColor:
                    framesToFinsih_C_2_E = framesToTravelGuideDistance;
                    break;
            }

            //NOTE: I was previously using FloorToInt()... using Ceil is untested
            framesToFinsih_C_2_E = (int)Mathf.CeilToInt(framesToFinsih_C_2_E);

            //NOTE: as of now we know how many frames every distance will take... so now we use said data to lerp

            //NOTE: "distPerFrame" that took so long to calculate WILL NOT CHANGE because we are aiming for a LERP
            //so we travel the same distance every frame AS LONG AS OUR SETTINGS STAY THE SAME
            float distPerFrame = dist_C_2_E / framesToFinsih_C_2_E;

            //TODO... The Implication is that we only need to run this function once  and then just run "the one line" below every frame
            float lerpValue = Mathf.Clamp((distPerFrame / dist_C_2_E), 0, 1);

            return lerpValue;
        }
    }
}