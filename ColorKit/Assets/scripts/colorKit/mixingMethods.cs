using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace colorKit
{
    //NOTE: the mixingMethods dont care about... (1) what color space the colors are NOW in (2) what format the colors are in

    public static class mixingMethods
    {
        //IF (true) --> use Vector4.Distance()... ELSE (false) --> use some other approximation
        public static bool useVect4Dist = true;

        //-------------------------Originally From Color Mixing-------------------------

        //Ignore Quants == true
        public static float[] mixColors(List<float[]> colors, mixingMethod mm)
        {
            float[] colorQuantities = new float[0]; //create it to meet requirements
            return mixColors(colors, colorQuantities, mm, true);
        }

        //Ignore Quants == false
        public static float[] mixColors(List<float[]> colors, float[] colorQuantities, mixingMethod mm)
        {
            return mixColors(colors, colorQuantities, mm, false);
        }

        //BASE CODE
        static float[] mixColors(List<float[]> colors, float[] colorQuantities, mixingMethod mm, bool ignoreQuants)
        {
            if (
                //COLOR QUANTITIES SHOULD BE POSITIVE... but I WILL NOT CHECK THIS...
                //COLORS SHOULD BE IN THE SAME COLOR SPACE (rgb,ryb,cmyk) -AND- FORMAT (255,float)... but I CANNOT CHECK THIS...
                (colors[0].Length == 3 || colors[0].Length == 4) //colors can only be in these formats
                && (colors.Count > 1) //we need 2 or more colors to MIX them
                && ((ignoreQuants == false) && (colors.Count == colorQuantities.Length)))
            {
                switch (mm)
                {
                    case mixingMethod.spaceAveraging:
                        if (ignoreQuants)
                            return spaceAveraging(colors);
                        else
                            return spaceAveraging(colors, colorQuantities);
                    case mixingMethod.colorComponentAveraging:
                        if (ignoreQuants)
                            return colorComponentAveraging(colors);
                        else
                            return colorComponentAveraging(colors, colorQuantities);
                    case mixingMethod.eachAsPercentOfMax:
                        if (ignoreQuants)
                            return eachAsPercentOfMax(colors);
                        else
                            return eachAsPercentOfMax(colors, colorQuantities);
                    default: //colorAveraging
                        if (ignoreQuants)
                            return colorAveraging(colors);
                        else
                            return colorAveraging(colors, colorQuantities);
                }
            }
            else
            {
                if (colors.Count == 1)
                    return colors[0];
                else
                    return new float[colors[0].Length];
            }
        }

        //-------------------------All Mixing Methods-------------------------

        //----------Tested Mixing Methods (Used in Demo)----------

        //-----Space Averaging

        //Ignore Quants == true
        static float[] spaceAveraging(List<float[]> colors)
        {
            float[] colorQuantities = new float[0]; //create it to meet requirements
            return spaceAveraging(colors, colorQuantities, true);
        }

        //Ignore Quants == false
        static float[] spaceAveraging(List<float[]> colors, float[] colorQuants)
        {
            return spaceAveraging(colors, colorQuants, false);
        }

        //BASE CODE [CHECKED]
        static float[] spaceAveraging(List<float[]> colors, float[] colorQuants, bool ignoreQuants)
        {
            //NOTE: this algorithm is based on space averaging at a CORE so we don't have multiple ways to determine 4D distance
            //so we just stick to Unity's weird 4D distance

            //start with 1 color in the mix (this must happen)
            float[] mixedColor = colors[0];
            float mixedColorQuant = (ignoreQuants) ? 1 : colorQuants[0];

            for (int color = 1; color < colors.Count; color++) //loop through all of our colors and add them to the mix
            {
                //we MIGHT add this newColor to the mix
                float[] newColor = colors[color];
                float newColorQuant = (ignoreQuants) ? 1 : colorQuants[color];

                if(Mathf.Approximately(newColorQuant, 0) == false)  //we have some newColor to add to the mix
                {
                    //calculate new color
                    //NOTE: this does Unity's strange 4D Distance to Lerp (IF your colors have 4 components or are 4D)
                    mixedColor = mixColorsGivenRatios(mixedColor, mixedColorQuant, newColor, newColorQuant);
                    mixedColorQuant += newColorQuant;
                }
            }

            return mixedColor;
        }

        //-----Color Averaging

        //Ignore Quants == true
        static float[] colorAveraging(List<float[]> colors)
        {
            float[] colorQuantities = new float[0]; //create it to meet requirements
            return colorAveraging(colors, colorQuantities, true);
        }

        //Ignore Quants == false
        static float[] colorAveraging(List<float[]> colors, float[] colorQuants)
        {
            return colorAveraging(colors, colorQuants, false);
        }

        //BASE CODE
        static float[] colorAveraging(List<float[]> colors, float[] colorQuants, bool ignoreQuants)
        {
            bool _3DSpace = (colors[0].Length == 3) ? true : false;

            //We have 2 ways of determining 4D distance... BOTH FLAWED... so pick one a hope for the best...
            //even better... mix with 3D colors

            //IF IM NOT MISTAKE... this first IF is a big modification because the original algorithm could only handle integers (1,2) but not 1.5 as a quantity
            if (_3DSpace || useVect4Dist)
            {

                float[] mixedColor = colors[0];
                float mixedColorQuant = (ignoreQuants) ? 1 : colorQuants[0];

                //---check all code below

                //SUM
                for (int color = 1; color < colors.Count; color++) //loop through every colors
                {
                    float[] color_1_and_2_Mix = new float[colors[0].Length]; //WHY DO I NEED THIS?

                    float[] newColor = colors[color];
                    float newColorQuant = (ignoreQuants) ? 1 : colorQuants[color];

                    if(Mathf.Approximately(newColorQuant, 0) == false) //add in color = true
                    {
                        //add this color to our total
                        for (int comp = 0; comp < mixedColor.Length; comp++)
                            color_1_and_2_Mix[comp] = mixedColor[comp] + newColor[comp];

                        //do something else about the color
                        //midColor Contains (ColorA + ColorB)... we need their average
                        for (int component = 0; component < color_1_and_2_Mix.Length; component++)
                            color_1_and_2_Mix[component] = color_1_and_2_Mix[component] / 2; //2 colors so we divide by 2

                        //midColor now contains the mix of both colors assuming equal porportion
                        mixedColor = mixColorsGivenRatios(mixedColor, mixedColorQuant, newColor, newColorQuant);

                        mixedColorQuant += newColorQuant;
                    }
                }

                //---check all code above

                return mixedColor;
            }
            else //CLOSEST TO THE ORIGINAL CODE
            {
                float[] mixedColor = new float[colors[0].Length];
                float mixedColorQuant = 0;

                //SUM
                for (int color = 0; color < colors.Count; color++) //loop through all the colors
                {
                    float[] newColor = colors[color];
                    float newColorQuant = (ignoreQuants) ? 1 : colorQuants[color];

                    if(Mathf.Approximately(newColorQuant, 0) == false)
                    {
                        for (int component = 0; component < mixedColor.Length; component++) //loop through every component of this particular color
                            mixedColor[component] += (newColor[component] * newColorQuant);

                        mixedColorQuant += newColorQuant;
                    }
                }

                //AVERAGE
                for (int i = 0; i < mixedColor.Length; i++)
                {
                    if (mixedColorQuant != 0)
                        mixedColor[i] = mixedColor[i] / mixedColorQuant;
                    else
                        mixedColor[i] = 0;
                }

                return mixedColor;
            }
        }

        //-----Color Component Averaging

        //Ignore Quants == true
        static float[] colorComponentAveraging(List<float[]> colors)
        {
            float[] colorQuantities = new float[0];
            return colorComponentAveraging(colors, colorQuantities, true);
        }

        //Ignore Quants == false
        static float[] colorComponentAveraging(List<float[]> colors, float[] colorQuantities)
        {
            return colorComponentAveraging(colors, colorQuantities, false);
        }

        //BASE CODE
        static float[] colorComponentAveraging(List<float[]> colors, float[] colorQuants, bool ignoreQuants)
        {
            bool _3DSpace = (colors[0].Length == 3) ? true : false;

            //We have 2 ways of determining 4D distance... BOTH FLAWED... so pick one a hope for the best...
            //even better... mix with 3D colors

            if (_3DSpace || useVect4Dist)
            {
                //This sniplet is used IF we have 3D colors -OR- 4D colors where you are willing to use Unity's flawed vector 4 distance to approximate 4D space badly

                float[] color1 = colors[0];
                float color1Quant = colorQuants[0];

                //---Check all code below

                //SUM
                for (int color2Index = 1; color2Index < colors.Count; color2Index++)
                {
                    float[] color_1_and_2_Mix = new float[color1.Length];
                    float[] color_1_and_2_MixComponentQuants = new float[color1.Length];

                    float color2Quant = colorQuants[color2Index];

                    bool addInColor2 = false;

                    for (int component = 0; component < color1.Length; component++)
                    {
                        if (Mathf.Approximately(color2Quant, 0) == false)
                        {
                            color_1_and_2_Mix[component] = color1[component] + (colors[color2Index])[component];
                            color_1_and_2_MixComponentQuants[component] = (Mathf.Approximately(color1[component], 0) ? 0 : 1) + (Mathf.Approximately((colors[color2Index])[component], 0) ? 0 : 1);
                            addInColor2 = true; //if we use one component then we use the color
                        }
                    }

                    if (addInColor2)
                    {
                        //midColor Contains (ColorA + ColorB)... we need their average
                        for (int component = 0; component < color_1_and_2_Mix.Length; component++)
                        {
                            if (color_1_and_2_MixComponentQuants[component] != 0)
                                color_1_and_2_Mix[component] = color_1_and_2_Mix[component] / color_1_and_2_MixComponentQuants[component];
                            else
                                color_1_and_2_Mix[component] = 0;
                        }

                        //midColor now contains the mix of both colors assuming equal porportion
                        if (ignoreQuants)
                            color1 = mixColorsGivenRatios(color1, 1, (colors[color2Index]), 1);
                        else
                            color1 = mixColorsGivenRatios(color1, color1Quant, (colors[color2Index]), color2Quant);
                    }

                    color1Quant += color2Quant;
                }

                //---check all code above

                return color1;
            }
            else
            {
                //NOTE: This sniplet is used IF we have a 4D color 
                //and we are willing to use a flawed averaging algorithm to determine a "distance" in 4D space

                //---check all code below

                float[] mixedColor = new float[colors[0].Length];
                float[] mixedColorCompQuants = new float[colors[0].Length];

                //SUM
                for (int color = 0; color < colors.Count; color++) //loop through all the colors
                {
                    float[] newColor = colors[color];
                    float newColorQuant = (ignoreQuants) ? 1 : colorQuants[color];

                    for (int component = 0; component < mixedColor.Length; component++) //loop through all the components of this particular color
                    {
                        if ((newColor[component] != 0) && (Mathf.Approximately(newColorQuant, 0) == false))
                        {
                            if (ignoreQuants)
                            {
                                mixedColorCompQuants[component] += 1;
                                mixedColor[component] += newColor[component] * 1;
                            }
                            else
                            {
                                //TODO... make sure it makes sense to use colorQuantities twice
                                mixedColorCompQuants[component] += newColorQuant;
                                mixedColor[component] += (newColor[component] * newColorQuant);
                            }

                        }
                        //ELSE... comp = 0, quant = 0 (no effect) -or- comp = 0, quant != 0 (undesired effect 1) -or- comp != 0, quant = 0 (undesired effect 2)
                    }
                }

                //---check all code above

                //AVERAGE
                for (int i = 0; i < mixedColor.Length; i++)
                {
                    if (mixedColorCompQuants[i] != 0)
                        mixedColor[i] = mixedColor[i] / mixedColorCompQuants[i];
                    else
                        mixedColor[i] = 0;
                }

                return mixedColor;
            }
        }

        //----------Experimental Untested(Unused in Demo)----------

        //Ignore Quants == true
        static float[] eachAsPercentOfMax(List<float[]> colors)
        {
            float[] colorQuantities = new float[0]; //create it to meet requirements
            return eachAsPercentOfMax(colors, colorQuantities, true);
        }

        //Ignore Quants == false
        static float[] eachAsPercentOfMax(List<float[]> colors, float[] colorQuantities)
        {
            return eachAsPercentOfMax(colors, colorQuantities, false);
        }

        //BASE CODE [CHECKED]
        static float[] eachAsPercentOfMax(List<float[]> colors, float[] colorQuantities, bool ignoreQuants)
        {
            float[] mixedColor = new float[colors[0].Length];

            //get total of all the colors
            for (int color = 0; color < colors.Count; color++) //loop through all the colors
            {
                //we MIGHT add this newColor to the mix
                float[] newColor = colors[color];
                float newColorQuant = (ignoreQuants) ? 1 : colorQuantities[color];

                if (Mathf.Approximately(newColorQuant, 0) == false) //we have some newColor to add to the mix
                    for (int comp = 0; comp < newColor.Length; comp++) //loop through all the components of this particular color
                        mixedColor[comp] += (newColor[comp] * newColorQuant);
            }

            // Calculate the max of all sums for each color component
            float maxComponent = 0;
            for (int comp = 0; comp < mixedColor.Length; comp++)
                maxComponent = Mathf.Max(maxComponent, mixedColor[comp]);

            // Now calculate each channel as a percentage of the max
            for (int comp = 0; comp < mixedColor.Length; comp++)
                mixedColor[comp] = Mathf.Floor(mixedColor[comp] / maxComponent * 255);

            return mixedColor;
        }

        //-------------------------the function that makes color quantities affect the final colors-------------------------

        //(tested)
        static float[] mixColorsGivenRatios(float[] color1, float color1Quant, float[] color2, float color2Quant)
        {
            float ratio = 0;
            if (color1Quant != color2Quant)
                ratio = color2Quant / (color1Quant + color2Quant);
            else
                ratio = .5f;
            return colorLerping.colorLerp(color1, color2, ratio);
        }
    }
}