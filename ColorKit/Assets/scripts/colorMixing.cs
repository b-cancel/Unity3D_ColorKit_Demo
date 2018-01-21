using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace colorKit
{
    public class colorMixing : MonoBehaviour
    {
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
                float[] colorFloat_RGB = colorTypeConversion.color_to_array(colors[i]);
                float[] color255_RGB = colorFormatConversion.colorFloat_to_color255(colorFloat_RGB);
                passedColors.Add(color255_RGB);
            }

            float[] result255_RGB = gameObject.GetComponent<colorMixing>().mixColors(mm, passedColors, colorQuantities, ignoreQuants); //actual Color Mixing

            float[] resultFloat_RGB = colorFormatConversion.color255_to_colorFloat(result255_RGB);

            return colorTypeConversion.array_to_color(resultFloat_RGB);
        }

        Color mixColors_inRYB_colorSpace(mixingMethod mm, Color[] colors, float[] colorQuantities, bool ignoreQuants)
        {
            List<float[]> passedColors = new List<float[]>();

            for (int i = 0; i < colors.Length; i++)
            {
                float[] colorFloat_RGB = colorTypeConversion.color_to_array(colors[i]);
                float[] color255_RGB = colorFormatConversion.colorFloat_to_color255(colorFloat_RGB);
                float[] color255_RYB = rgb2ryb_ryb2rgb.rgb255_to_ryb255(color255_RGB);
                passedColors.Add(color255_RYB);
            }

            float[] result255_RYB = gameObject.GetComponent<colorMixing>().mixColors(mm, passedColors, colorQuantities, ignoreQuants); //actual Color Mixing
            float[] result255_RGB = rgb2ryb_ryb2rgb.ryb255_to_rgb255(result255_RYB);
            float[] resultFloat_RGB = colorFormatConversion.color255_to_colorFloat(result255_RGB);

            return colorTypeConversion.array_to_color(resultFloat_RGB);
        }

        Color mixColors_inCMYK_colorSpace(mixingMethod mm, Color[] colors, float[] colorQuantities, bool ignoreQuants)
        {
            List<float[]> passedColors = new List<float[]>();

            for (int i = 0; i < colors.Length; i++)
            {
                float[] colorFloat_RGB = colorTypeConversion.color_to_array(colors[i]);
                float[] color255_RGB = colorFormatConversion.colorFloat_to_color255(colorFloat_RGB);
                float[] color255_CMYK = rgb2cmyk_cmyk2rgb.rgb255_to_cmyk255(color255_RGB);
                passedColors.Add(color255_CMYK);
            }

            float[] result255_CMYK = gameObject.GetComponent<colorMixing>().mixColors(mm, passedColors, colorQuantities, ignoreQuants); //actual Color Mixing
            float[] result255_RGB = rgb2cmyk_cmyk2rgb.cmyk255_to_rgb255(result255_CMYK);
            float[] resultFloat_RGB = colorFormatConversion.color255_to_colorFloat(result255_RGB);

            return colorTypeConversion.array_to_color(resultFloat_RGB);
        }

        //-----BASE (in color mixing script)

        //-------------------------Originally From Color Mixing-------------------------

        _4D_flawToAccept theFlawWeAccept;

        void Awake()
        {
            theFlawWeAccept = _4D_flawToAccept.useWeirdVect4Dist;
        }

        //Ignore Quants == true
        public float[] mixColors(mixingMethod mm, List<float[]> colors)
        {
            float[] colorQuantities = new float[0]; //create it to meet requirements
            return mixColors(mm, colors, colorQuantities, true);
        }

        //Ignore Quants == false
        public float[] mixColors(mixingMethod mm, List<float[]> colors, float[] colorQuantities)
        {
            return mixColors(mm, colors, colorQuantities, false);
        }

        //BASE CODE
        public float[] mixColors(mixingMethod mm, List<float[]> colors, float[] colorQuantities, bool ignoreQuants)
        {
            if (
                //COLORS MUST BE IN THE SAME FORMAT (rgb,ryb,cmyk)... I AM NOT CHECKING THIS...
                (colors[0].Length == 3 || colors[0].Length == 4) //colors can only be in these formats
                && (colors.Count > 1) //we need 2 or more colors to MIX them
                )
            {
                switch (mm)
                {
                    case mixingMethod.spaceAveraging:
                        if (ignoreQuants)
                        {
                            if ((colors.Count == colorQuantities.Length))
                                return spaceAveraging(colors);
                            else
                                return new float[colors[0].Length];
                        }
                        else
                            return spaceAveraging(colors, colorQuantities);
                    case mixingMethod.colorAveraging:
                        if (ignoreQuants)
                        {
                            if ((colors.Count == colorQuantities.Length))
                                return colorAveraging(colors);
                            else
                                return new float[colors[0].Length];
                        }
                        else
                            return colorAveraging(colors, colorQuantities);
                    case mixingMethod.colorComponentAveraging:
                        if (ignoreQuants)
                        {
                            if ((colors.Count == colorQuantities.Length))
                                return colorComponentAveraging(colors);
                            else
                                return new float[colors[0].Length];
                        }
                        else
                            return colorComponentAveraging(colors, colorQuantities);
                    case mixingMethod.eachAsPercentOfMax:
                        if (ignoreQuants)
                        {
                            if ((colors.Count == colorQuantities.Length))
                                return eachAsPercentOfMax(colors);
                            else
                                return new float[colors[0].Length];
                        }
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

        //----------Tested Mixing Methods (Used in Demo)----------

        //-----Space Averaging

        //Ignore Quants == true
        float[] spaceAveraging(List<float[]> colors)
        {
            float[] colorQuantities = new float[0]; //create it to meet requirements
            return spaceAveraging(colors, colorQuantities, true);
        }

        //Ignore Quants == false
        float[] spaceAveraging(List<float[]> colors, float[] colorQuantities)
        {
            return spaceAveraging(colors, colorQuantities, false);
        }

        //BASE CODE
        float[] spaceAveraging(List<float[]> colors, float[] colorQuants, bool ignoreQuants)
        {
            //NOTE: this algorithm is based on space averaging at a CORE so we don't have multiple ways to determine distance
            //(in reference to color component averaging -AND- color averaging 4D options)... so we just stick to Unity's weird 4D distance

            float[] baseColor = colors[0];
            float baseQuant = colorQuants[0];

            for (int i = 1; i < colors.Count; i++)
            {
                float[] othercolor = colors[i];
                float otherQuant = colorQuants[i];

                //calculate new color
                float[] newColor;

                //NOTE: this does Unity's strange 4D Distance to Lerp (IF your colors have 4 components or are 4D)

                if (ignoreQuants)
                    newColor = adjustForColorRatios(baseColor, 1, othercolor, 1);
                else
                    newColor = adjustForColorRatios(baseColor, baseQuant, othercolor, otherQuant);

                baseColor = newColor;
                baseQuant += otherQuant;
            }

            return baseColor;
        }

        //-----Color Averaging

        //Ignore Quants == true
        float[] colorAveraging(List<float[]> colors)
        {
            float[] colorQuantities = new float[0]; //create it to meet requirements
            return colorAveraging(colors, colorQuantities, true);
        }

        //Ignore Quants == false
        float[] colorAveraging(List<float[]> colors, float[] colorQuantities)
        {
            return colorAveraging(colors, colorQuantities, false);
        }

        //BASE CODE
        float[] colorAveraging(List<float[]> colors, float[] colorQuantities, bool ignoreQuants)
        {
            bool _3DSpace = (colors[0].Length == 3) ? true : false;

            //We have 2 ways of determining 4D distance... BOTH FLAWED... so pick one a hope for the best...
            //even better... mix with 3D colors

            if (_3DSpace || (theFlawWeAccept == _4D_flawToAccept.useWeirdVect4Dist))
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
                            color1 = adjustForColorRatios(color1, 1, (colors[color2Index]), 1);
                        else
                            color1 = adjustForColorRatios(color1, color1Quant, (colors[color2Index]), color2Quant);
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
        float[] colorComponentAveraging(List<float[]> colors)
        {
            float[] colorQuantities = new float[0];
            return colorComponentAveraging(colors, colorQuantities, true);
        }

        //Ignore Quants == false
        float[] colorComponentAveraging(List<float[]> colors, float[] colorQuantities)
        {
            return colorComponentAveraging(colors, colorQuantities, false);
        }

        //BASE CODE
        float[] colorComponentAveraging(List<float[]> colors, float[] colorQuantities, bool ignoreQuants)
        {
            bool _3DSpace = (colors[0].Length == 3) ? true : false;

            //We have 2 ways of determining 4D distance... BOTH FLAWED... so pick one a hope for the best...
            //even better... mix with 3D colors

            if (_3DSpace || (theFlawWeAccept == _4D_flawToAccept.useWeirdVect4Dist))
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
                            color1 = adjustForColorRatios(color1, 1, (colors[color2Index]), 1);
                        else
                            color1 = adjustForColorRatios(color1, color1Quant, (colors[color2Index]), color2Quant);
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
        float[] eachAsPercentOfMax(List<float[]> colors)
        {
            float[] colorQuantities = new float[0]; //create it to meet requirements
            return eachAsPercentOfMax(colors, colorQuantities, true);
        }

        //Ignore Quants == false
        float[] eachAsPercentOfMax(List<float[]> colors, float[] colorQuantities)
        {
            return eachAsPercentOfMax(colors, colorQuantities, false);
        }

        //BASE CODE
        float[] eachAsPercentOfMax(List<float[]> colors, float[] colorQuantities, bool ignoreQuants)
        {
            //NOTE: this does not require a 4D distance to mix so we dont have the 2 options we have for color component averaging -AND- color averaging

            float[] newColor = new float[colors[0].Length];

            //get total of all the colors
            for (int color = 0; color < colors.Count; color++)
            {
                for (int component = 0; component < newColor.Length; component++)
                {
                    if (((colors[color])[component] != 0) && (Mathf.Approximately(colorQuantities[color], 0) == false))
                    {
                        if (ignoreQuants)
                            newColor[component] += (colors[color])[component]; //the undesired effect 2 would occur here
                        else
                            newColor[component] += ((colors[color])[component] * colorQuantities[color]);
                    }
                    //ELSE... comp = 0, quant = 0 (no effect) -or- comp = 0, quant != 0 (undesired effect 1) -or- comp != 0, quant = 0 (undesired effect 2)
                }
            }

            /*
             * NOTE: total for each component can most definately go above 255... 
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

        //----------the function that makes color quantities affect the final colors----------

        float[] adjustForColorRatios(float[] color1, float color1Quant, float[] color2, float color2Quant)
        {
            float ratio = 0;
            if (color1Quant != color2Quant)
                ratio = color2Quant / (color1Quant + color2Quant);
            else
                ratio = .5f;
            return Camera.main.GetComponent<colorLerping>().colorLerp(color1, color2, ratio);
        }
    }
}