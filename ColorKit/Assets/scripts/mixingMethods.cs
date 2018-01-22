using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace colorKit
{
    //NOTE: the mixingMethods dont care about... (1) what color space the colors are NOW in (2) what format the colors are in

    public static class mixingMethods
    {
        //IF (true) --> use Vector4.Distance()... ELSE (false) --> use some other approximation
        static bool useVect4Dist = true;

        //-------------------------Originally From Color Mixing-------------------------

        //Ignore Quants == true
        public static float[] mixColors(mixingMethod mm, List<float[]> colors)
        {
            float[] colorQuantities = new float[0]; //create it to meet requirements
            return mixColors(mm, colors, colorQuantities, true);
        }

        //Ignore Quants == false
        public static float[] mixColors(mixingMethod mm, List<float[]> colors, float[] colorQuantities)
        {
            return mixColors(mm, colors, colorQuantities, false);
        }

        //BASE CODE
        static float[] mixColors(mixingMethod mm, List<float[]> colors, float[] colorQuantities, bool ignoreQuants)
        {
            if (
                //COLORS MUST BE IN THE SAME COLOR SPACE (rgb,ryb,cmyk) -AND- FORMAT (255,float)... but I CANNOT CHECK THIS...
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
                    case mixingMethod.colorAveraging:
                        if (ignoreQuants)
                            return colorAveraging(colors);
                        else
                            return colorAveraging(colors, colorQuantities);
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
                    default:
                        return colorAveraging(colors, colorQuantities, ignoreQuants);
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
        static float[] spaceAveraging(List<float[]> colors, float[] colorQuantities)
        {
            return spaceAveraging(colors, colorQuantities, false);
        }

        //BASE CODE (tested)
        static float[] spaceAveraging(List<float[]> colors, float[] colorQuants, bool ignoreQuants)
        {
            //NOTE: this algorithm is based on space averaging at a CORE so we don't have multiple ways to determine distance
            //(in reference to color component averaging -AND- color averaging 4D options)... so we just stick to Unity's weird 4D distance

            //start with 1 color and add all colors to it
            float[] baseColor = colors[0];
            float baseQuant = colorQuants[0];

            for (int i = 1; i < colors.Count; i++)
            {
                //the color we are goign to add to our base color
                float[] othercolor = colors[i];
                float otherQuant = colorQuants[i];

                //calculate new color
                float[] newColor;

                //NOTE: this does Unity's strange 4D Distance to Lerp (IF your colors have 4 components or are 4D)

                if (ignoreQuants)
                    newColor = mixColorsGivenRatios(baseColor, 1, othercolor, 1);
                else
                    newColor = mixColorsGivenRatios(baseColor, baseQuant, othercolor, otherQuant);

                baseColor = newColor;
                baseQuant += otherQuant;
            }

            return baseColor;
        }

        //-----Color Averaging

        //Ignore Quants == true
        static float[] colorAveraging(List<float[]> colors)
        {
            float[] colorQuantities = new float[0]; //create it to meet requirements
            return colorAveraging(colors, colorQuantities, true);
        }

        //Ignore Quants == false
        static float[] colorAveraging(List<float[]> colors, float[] colorQuantities)
        {
            return colorAveraging(colors, colorQuantities, false);
        }

        //BASE CODE
        static float[] colorAveraging(List<float[]> colors, float[] colorQuantities, bool ignoreQuants)
        {
            bool _3DSpace = (colors[0].Length == 3) ? true : false;

            //We have 2 ways of determining 4D distance... BOTH FLAWED... so pick one a hope for the best...
            //even better... mix with 3D colors

            if (_3DSpace || useVect4Dist)
            {
                float[] color1 = colors[0];
                float color1Quant = colorQuantities[0];

                //---check all code below

                //SUM
                for (int color2Index = 1; color2Index < colors.Count; color2Index++)
                {
                    float[] color_1_and_2_Mix = new float[colors[0].Length];

                    float color2Quant = colorQuantities[color2Index];
                    bool addInColor2 = false;

                    for (int component = 0; component < color1.Length; component++)
                    {
                        if (Mathf.Approximately(color2Quant, 0) == false)
                        {
                            color_1_and_2_Mix[component] = color1[component] + (colors[color2Index])[component];
                            addInColor2 = true; //if we use one component then we use the color
                        }
                    }

                    //FIND OUT WHY THIS IS NESSESARY
                    if (addInColor2)
                    {
                        //midColor Contains (ColorA + ColorB)... we need their average
                        for (int component = 0; component < color_1_and_2_Mix.Length; component++)
                            color_1_and_2_Mix[component] = color_1_and_2_Mix[component] / 2; //2 colors so we divide by 2

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
                float[] mixedColor = new float[colors[0].Length];
                float mixedColorQuant = 0;

                //For Color Averaging
                //DONT consider the color if the quantity of the color is 0; (quant = 0) && (comp = 0); (quant = 0) && (comp != 0)
                //DO consider the color regardless of the value of the component; (comp = 0) && (quant != 0); (comp != 0) && (quant != 0)

                //SUM
                for (int color = 0; color < colors.Count; color++)
                {
                    if (Mathf.Approximately(colorQuantities[color], 0) == false)
                    {
                        mixedColorQuant += (colorQuantities[color]); //we don't care what the colors components are... we always use its quantity

                        for (int component = 0; component < mixedColor.Length; component++)
                        {
                            if (ignoreQuants)
                                mixedColor[component] += (colors[color])[component];
                            else
                                mixedColor[component] += ((colors[color])[component] * (colorQuantities[color]));
                        }
                    }
                    //ELSE we can't mix a color that isnt there
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
        static float[] colorComponentAveraging(List<float[]> colors, float[] colorQuantities, bool ignoreQuants)
        {
            bool _3DSpace = (colors[0].Length == 3) ? true : false;

            //We have 2 ways of determining 4D distance... BOTH FLAWED... so pick one a hope for the best...
            //even better... mix with 3D colors

            if (_3DSpace || useVect4Dist)
            {
                //This sniplet is used IF we have 3D colors -OR- 4D colors where you are willing to use Unity's flawed vector 4 distance to approximate 4D space badly

                float[] color1 = colors[0];
                float color1Quant = colorQuantities[0];

                //---Check all code below

                //SUM
                for (int color2Index = 1; color2Index < colors.Count; color2Index++)
                {
                    float[] color_1_and_2_Mix = new float[color1.Length];
                    float[] color_1_and_2_MixComponentQuants = new float[color1.Length];

                    float color2Quant = colorQuantities[color2Index];

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

                float[] newColor = new float[colors[0].Length];
                float[] componentQuantities = new float[colors[0].Length];

                //SUM (TODO we have to find out where to count the quantity
                for (int color = 0; color < colors.Count; color++)
                {
                    for (int component = 0; component < colors[color].Length; component++)
                    {
                        if (((colors[color])[component] != 0) && (Mathf.Approximately(colorQuantities[color], 0) == false))
                        {
                            if (ignoreQuants)
                            {
                                componentQuantities[component] += 1;
                                newColor[component] += (colors[color])[component] * 1;
                            }
                            else
                            {
                                //TODO... make sure it makes sense to use colorQuantities twice
                                componentQuantities[component] += colorQuantities[color];
                                newColor[component] += ((colors[color])[component] * colorQuantities[color]);
                            }

                        }
                        //ELSE... comp = 0, quant = 0 (no effect) -or- comp = 0, quant != 0 (undesired effect 1) -or- comp != 0, quant = 0 (undesired effect 2)
                    }
                }

                //AVERAGE
                for (int i = 0; i < newColor.Length; i++)
                {
                    if (componentQuantities[i] != 0)
                        newColor[i] = newColor[i] / componentQuantities[i];
                    else
                        newColor[i] = 0;
                }

                return newColor;
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

        //BASE CODE
        static float[] eachAsPercentOfMax(List<float[]> colors, float[] colorQuantities, bool ignoreQuants)
        {
            float[] newColor = new float[colors[0].Length];

            //get total of all the colors
            for (int color = 0; color < colors.Count; color++) //loop through all the colors
            {
                for (int component = 0; component < newColor.Length; component++) //loop through all the components of this particular color
                {
                    if (ignoreQuants)
                        newColor[component] += (colors[color])[component];
                    else
                    {
                        if (((colors[color])[component] != 0) && (Mathf.Approximately(colorQuantities[color], 0) == false))
                            newColor[component] += ((colors[color])[component] * colorQuantities[color]);
                        //ELSE... comp = 0, quant = 0 (no effect) -or- comp = 0, quant != 0 (undesired effect 1) -or- comp != 0, quant = 0 (undesired effect 2)
                    }
                }
            }

            /*
             * NOTE: the total for each component can be above 255... 
             * another possible implementation is...
             * if the max component is below 255 the color is simply returned...
             * else the max component is now 255 regardless of what it actually was and the other components get reduced by the same factor
             */

            // Calculate the max of all sums for each color component
            float maxComponent = 0;
            for (int i = 0; i < newColor.Length; i++)
                maxComponent = Mathf.Max(maxComponent, newColor[i]);

            // Now calculate each channel as a percentage of the max
            for (int i = 0; i < newColor.Length; i++)
                newColor[i] = Mathf.Floor(newColor[i] / maxComponent * 255);

            return newColor;
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