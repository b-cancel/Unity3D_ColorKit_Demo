using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NOTES....
/// -The input colors MUST be in the same (1) Color Space (2) Format
/// -its best to mix colors when they are in their 255 format because since the number is larger our floating point errors are less significant
/// -All colors return an array in the 255 format
///     
/// -Space Based Quantity Mixing[SBQM]: for our color mixing quantities to work properly and intuitively we always mix colors in pairs of 2s
///     1st: we mix the 2 colors assuming same porportion
///     2nd: we get a range of colors (IOW a line of colors from color A to color B)
///     3rd: we used some form of linear interpolation to grab the color inbetween A and B given the ratio of A to B
///     
/// -RBG and RYB will use [SBQM] because a representation of Space in 3 Dimension is Well Defined
/// -For CMYK to use [SBQM] we would need a good representation of distance in 4 Dimensional Space... which I dont have...
///     -So for now we will either
///         1. use our flawed method of mixing with quantities
///         2. use the strange version of distance that Vector4.Distance Provides
///     -NOTE: Eventually we will use liner interpolation based on what some predefined "Base" mixtures are
/// 
/// Color Mixing Function Sources:
/// 1. My Idea //space averaging
/// 2. https://github.com/AndreasSoiron/Color_mixer //color averaging
/// 3. My Idea //color component Averaging
///
/// The Rest are Experimental
/// https://github.com/camme/ryb-color-mixer/blob/master/ryb-color-mixer.js //combineColors_thenCorrectEachChannelAsPercentOfTheMax
/// 
/// Details on more accurate SUBTRACTIVE color mixing methods here: https://gist.github.com/b-cancel/70de2dfa0705e94045574931b5c8e664 
///  
/// </summary>

public class colorMixing : MonoBehaviour
{
    _4D_flawToAccept theFlawWeAccept;

    void Awake()
    {
        theFlawWeAccept = _4D_flawToAccept.useWeirdVect4Dist;
    }

    public float[] mixColors(mixingMethod mm, List<float[]> colors, float[] colorQuantities, bool ignoreQuants)
    {
        if (
            (colors[0].Length == 3 || colors[0].Length == 4) //we need colors in these formats (due to what quantity effects using some sort of lerping)
            && (colors.Count == colorQuantities.Length) //we need a quantity for every color
            && (colors.Count > 1) //we need 2 or more colors
            )
        {
            switch (mm)
            {
                case mixingMethod.spaceAveraging: return spaceAveraging(colors, colorQuantities, ignoreQuants);
                case mixingMethod.colorAveraging: return colorAveraging(colors, colorQuantities, ignoreQuants);
                case mixingMethod.colorComponentAveraging: return colorComponentAveraging(colors, colorQuantities, ignoreQuants);
                default: return colorAveraging(colors, colorQuantities, ignoreQuants);
            }
        }
        else
        {
            if (colors.Count == 0)
                return new float[colors[0].Length];
            else
                return colors[0];
        }
    }

    float[] spaceAveraging(List<float[]> colors, float[] colorQuants, bool ignoreQuants)
    {
        float[] baseColor = colors[0];
        float baseQuant = colorQuants[0];

        for (int i = 1; i < colors.Count; i++)
        {
            float[] othercolor = colors[i];
            float otherQuant = colorQuants[i];

            //calculate new color
            float[] newColor;

            if (ignoreQuants)
                newColor = Camera.main.GetComponent<otherColorOps>().colorLerp(baseColor, othercolor, .5f);
            else
                newColor = adjustForColorRatios(baseColor, baseQuant, othercolor, otherQuant, Camera.main.GetComponent<otherColorOps>().colorLerp(baseColor, othercolor, .5f));

            baseColor = newColor;
            baseQuant += otherQuant;
        }

        return baseColor;
    }

    float[] colorAveraging(List<float[]> colors, float[] colorQuants, bool ignoreQuants)
    {
        bool _3DSpace = (colors[0].Length == 3) ? true : false;

        if (_3DSpace || (theFlawWeAccept == _4D_flawToAccept.useWeirdVect4Dist))
        {
            float[] color1 = colors[0];
            float color1Quant = colorQuants[0];

            //SUM
            for (int color2Index = 1; color2Index < colors.Count; color2Index++)
            {
                float[] color_1_and_2_Mix = new float[colors[0].Length];

                float color2Quant = colorQuants[color2Index];
                bool addInColor2 = false;

                for (int component = 0; component < color1.Length; component++)
                {
                    if (Mathf.Approximately(color2Quant, 0) == false)
                    {
                        color_1_and_2_Mix[component] = color1[component] + (colors[color2Index])[component];
                        addInColor2 = true; //if we use one component then we use the color
                    }
                }

                if (addInColor2)
                {
                    //midColor Contains (ColorA + ColorB)... we need their average
                    for (int component = 0; component < color_1_and_2_Mix.Length; component++)
                        color_1_and_2_Mix[component] = color_1_and_2_Mix[component] / 2; //2 colors so we divide by 2

                    //midColor now contains the mix of both colors assuming equal porportion
                    if (ignoreQuants)
                        color1 = color_1_and_2_Mix;
                    else
                        color1 = adjustForColorRatios(color1, color1Quant, (colors[color2Index]), color2Quant, color_1_and_2_Mix);
                }

                color1Quant += color2Quant;
            }

            return color1;
        }
        else
        {
            float[] mixedColor = new float[colors[0].Length];
            float mixedColorQuant = 0;

            //SUM
            for (int color = 0; color < colors.Count; color++)
            {
                for (int component = 0; component < mixedColor.Length; component++)
                    mixedColor[component] += ((colors[color])[component] * (colorQuants[color]));

                mixedColorQuant += (colorQuants[color]);
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

    float[] colorComponentAveraging(List<float[]> colors, float[] colorQuantities, bool ignoreQuants)
    {
        bool _3DSpace = (colors[0].Length == 3) ? true : false;

        if (_3DSpace || (theFlawWeAccept == _4D_flawToAccept.useWeirdVect4Dist))
        {
            float[] color1 = colors[0];
            float color1Quant = colorQuantities[0];

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
                        color1 = color_1_and_2_Mix;
                    else
                        color1 = adjustForColorRatios(color1, color1Quant, (colors[color2Index]), color2Quant, color_1_and_2_Mix);
                }

                color1Quant += color2Quant;
            }

            return color1;
        }
        else
        {
            float[] newColor = new float[colors[0].Length];
            float[] componentQuantities = new float[colors[0].Length];

            //SUM
            for (int color = 0; color < colors.Count; color++)
            {
                for (int component = 0; component < colors[color].Length; component++)
                {
                    if ((colors[color])[component] != 0 && Mathf.Approximately(colorQuantities[color], 0) == false)
                    {
                        componentQuantities[component] += colorQuantities[color];
                        newColor[component] += ((colors[color])[component] * colorQuantities[color]);
                    }
                    //ELSE... comp = 0, quant = 0 (no effect) -or- comp = 0, quant != 0 (undesired effect) -or- comp != 0, quant = 0 (no effect)
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

    //---the function that makes color quantities affect the final colors

    float[] adjustForColorRatios(float[] color1, float color1Quant, float[] color2, float color2Quant, float[] mix)
    {
        float ratio = color2Quant / (color1Quant + color2Quant);

        if (ratio == .5f)
            return mix;
        else if (ratio < .5f)
        {
            ratio = (2 * (ratio));
            return Camera.main.GetComponent<otherColorOps>().colorLerp(color1, mix, ratio);
        }
        else
        {
            ratio = (2 * (ratio)) - 1;
            return Camera.main.GetComponent<otherColorOps>().colorLerp(mix, color2, ratio);
        }
    }

    //----------Experimental Untested(Unused in Demo)----------

    float[] combineColors_thenCalculateEachChannelAsAPercentageOfTheMax(List<float[]> colors, float[] colorQuantities)
    {
        float[] newColor = new float[colors[0].Length];

        for (int color = 0; color < colors.Count; color++)
        {
            for (int component = 0; component < newColor.Length; component++)
            {
                if ((colors[color])[component] != 0 && Mathf.Approximately(colorQuantities[color], 0) == false)
                    newColor[component] += (colors[color])[component] * colorQuantities[color];
                //ELSE... comp = 0, quant = 0 (no effect) -or- comp = 0, quant != 0 (undesired effect) -or- comp != 0, quant = 0 (no effect)
            }

        }

        // Calculate the max of all sums for each color component
        float maxComponent = 0;
        for (int i = 0; i < newColor.Length; i++)
            maxComponent = Mathf.Max(maxComponent, newColor[i]);

        // Now calculate each channel as a percentage of the max
        for (int i = 0; i < newColor.Length; i++)
            newColor[i] = Mathf.Floor(newColor[i] / maxComponent * 255);

        return newColor;
    }
}