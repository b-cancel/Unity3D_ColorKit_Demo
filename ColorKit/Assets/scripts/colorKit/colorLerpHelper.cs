using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using lerpKit;

namespace colorKit
{
    public static class colorLerpHelper
    {
        public static float calcGuideDistance(colorSpace CS, guideDistance GD, Color startColor, Color currColor, Color endColor)
        {

            switch (CS)
            {
                case colorSpace.RGB:

                    if (GD == guideDistance.distBetween_StartAndCurr)
                        return colorDistances.distBetweenColors(colorSpace.RGB, startColor, currColor);
                    else if (GD == guideDistance.distBetween_StartAndEnd)
                        return colorDistances.distBetweenColors(colorSpace.RGB, startColor, endColor);
                    else if (GD == guideDistance.distBetween_CurrAndEnd)
                        return colorDistances.distBetweenColors(colorSpace.RGB, currColor, endColor);
                    else
                        return 441.672956f; // maxDistanceInRGBColorSpace

                    break;
                case colorSpace.RYB:

                    if (GD == guideDistance.distBetween_StartAndCurr)
                        return colorDistances.distBetweenColors(colorSpace.RYB, startColor, currColor);
                    else if (GD == guideDistance.distBetween_StartAndEnd)
                        return colorDistances.distBetweenColors(colorSpace.RYB, startColor, endColor);
                    else if (GD == guideDistance.distBetween_CurrAndEnd)
                        return colorDistances.distBetweenColors(colorSpace.RYB, currColor, endColor);
                    else
                        return 441.672956f; //maxDistanceInRYBColorSpace

                    break;
                default:

                    if (GD == guideDistance.distBetween_StartAndCurr)
                        return colorDistances.distBetweenColors(colorSpace.CMYK, startColor, currColor);
                    else if (GD == guideDistance.distBetween_StartAndEnd)
                        return colorDistances.distBetweenColors(colorSpace.CMYK, startColor, endColor);
                    else if (GD == guideDistance.distBetween_CurrAndEnd)
                        return colorDistances.distBetweenColors(colorSpace.CMYK, currColor, endColor);
                    else
                        return 255; //maxDistanceInCMYKColorSpace (because we have no accurate representation for 4D distance)

                    break;
            }
        }

        public static float calcLerpValue(colorSpace CS, Color startColor, Color currColor, Color endColor, float guideDistance, float guideTime, unitOfTime UOT_GD, updateLocation UL)
        {
            return calcLerpValue(CS, startColor, currColor, endColor, lerpHelper.calcLerpVelocity(guideDistance, guideTime, UOT_GD, UL));
        }

        public static float calcLerpValue(colorSpace CS, Color startColor, Color currColor, Color endColor, float lerpVelocity_DperF)
        {
            //---calc distance left to travel
            float distToFinish = 0;
            switch (CS)
            {
                case colorSpace.RGB:
                    distToFinish = colorDistances.distBetweenColors(colorSpace.RGB, currColor, endColor);
                    break;
                case colorSpace.RYB:
                    distToFinish = colorDistances.distBetweenColors(colorSpace.RYB, currColor, endColor);
                    break;
                default:
                    distToFinish = colorDistances.distBetweenColors(colorSpace.CMYK, currColor, endColor);
                    break;
            }

            //--- calc lerp value based on this
            return Mathf.Clamp((lerpVelocity_DperF / distToFinish), 0, 1);
        }
    }
}