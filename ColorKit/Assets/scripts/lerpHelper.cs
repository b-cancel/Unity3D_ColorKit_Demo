using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using colorKit; //for the calcLerpValue that deals with color parameters

/*
         * you can make your lerping not frame rate dependant by using the following code in Update
         *      value = Mathf.Lerp(value, 10f, .5f * Time.deltaTime)
         * or in Fixed Update by using
         *      value = Mathf.Lerp(value, 10f, .5f * Time.fixedDeltaTime)
         * BUT... you still will not know how long the linear interpolation will take
         * 
         * you might want it take a particular quantity of FRAME -or- SECONDS 
         * 
         * To caculate the lerpValue of any linear interpolation we need to know
         * 1. how far along the lerp we are (start, end, curr) --> aka predict our next location
         * 2. how what our lerp velocity is
         *      * using
         * 3. where we are lerping (updateLocation)
         * 
         * ALL UNITY LERP FUNCTIONS
         * Mathf.Lerp //1 D
         * Vector2.Lerp //2 D
         * Vector3.Lerp //3 D
         * Vector4.Lerp //4 D
         * 
         * To Finish LINEARLY Interpolating within a particular time frame
         * you need to initially calculate a lerpVelocity
         * then you need to calculate a lerpValue based on your lerpVelocity and how much distance you have yet to travel
         * NOTE: the unity for the velocity is... DISTANCE PER FRAMES
        */


public static class lerpHelper{

    public enum updateLocation { fixedUpdate, Update };
    public enum unitOfTime { frames, seconds };

    //-------------------------CALCULATE GUIDE DISTANCE-------------------------

    //-------------------------1 Dimensional

    public static float calcGuideDistance(guideDistance GD, float startValue, float currValue, float endValue)
    {
        //NOTE: guideDistance.other has no definition for anything but color

        if (GD == guideDistance.distBetween_StartAndCurr)
            return Mathf.Abs(startValue - currValue);
        else if (GD == guideDistance.distBetween_StartAndEnd)
            return Mathf.Abs(startValue - endValue);
        else //guideDistance.distBetween_CurrAndEnd
            return Mathf.Abs(currValue - endValue);
    }

    //-------------------------2 Dimensional

    public static float calcGuideDistance(guideDistance GD, Vector2 startVect2, Vector2 currVector2, Vector2 endVector2)
    {
        //NOTE: guideDistance.other has no definition for anything but color

        if (GD == guideDistance.distBetween_StartAndCurr)
            return Vector2.Distance(startVect2, currVector2);
        else if (GD == guideDistance.distBetween_StartAndEnd)
            return Vector2.Distance(startVect2, endVector2);
        else //guideDistance.distBetween_CurrAndEnd
            return Vector2.Distance(currVector2, endVector2);
    }

    //-------------------------3 Dimensional

    public static float calcGuideDistance(guideDistance GD, Vector3 startVect3, Vector3 currVector3, Vector3 endVector3)
    {
        //NOTE: guideDistance.other has no definition for anything but color

        if (GD == guideDistance.distBetween_StartAndCurr)
            return Vector3.Distance(startVect3, currVector3);
        else if (GD == guideDistance.distBetween_StartAndEnd)
            return Vector3.Distance(startVect3, endVector3);
        else //guideDistance.distBetween_CurrAndEnd
            return Vector3.Distance(currVector3, endVector3);
    }

    //-------------------------4 Dimensional

    public static float calcGuideDistance(guideDistance GD, Vector4 startVect4, Vector4 currVector4, Vector4 endVector4)
    {
        //NOTE: guideDistance.other has no definition for anything but color

        if (GD == guideDistance.distBetween_StartAndCurr)
            return Vector4.Distance(startVect4, currVector4);
        else if (GD == guideDistance.distBetween_StartAndEnd)
            return Vector4.Distance(startVect4, endVector4);
        else //guideDistance.distBetween_CurrAndEnd
            return Vector4.Distance(currVector4, endVector4);
    }

    //-------------------------BASE

    public static float calcGuideDistance(guideDistance GD, float[] startValues, float[] currValues, float[] endValues)
    {
        //NOTE: guideDistance.other has no definition for anything but color

        if (GD == guideDistance.distBetween_StartAndCurr)
            return euclideanDistance(startValues, currValues);
        else if (GD == guideDistance.distBetween_StartAndEnd)
            return euclideanDistance(startValues, endValues);
        else //guideDistance.distBetween_CurrAndEnd
            return euclideanDistance(currValues, endValues);
    }

    //-------------------------Colors

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

    //-------------------------CALCULATE LERP VELOCITY-------------------------

    //NOTE: the PUBLIC version only requires the "Update Location" when your "Unit of Time" is seconds
    //HOWEVER... if I had a seperate PUBLIC version for seconds... and another for frames... then mistakes are likely to arise
    //this is because eventhough the version that works with seconds would require 1 more parameter... 
    //users could just type in a number and it will be taken as frames when what they really wanted was for it to be taken as seconds...
    //but they wont notice... since unity will assume they want to use the frames function with the same name but 1 less paramter

    public static float calcLerpVelocity(float guideDistance, float timeToTravel_GD, unitOfTime UOT_GD, updateLocation UL)
    {
        return calcLerpVelocity(guideDistance, timeToFrames(timeToTravel_GD, UOT_GD, UL));
    }

    static float calcLerpVelocity(float guideDistance, float framesToTravel_GD)
    {
        return guideDistance / framesToTravel_GD;
    }

    //-------------------------CALCULATE LERP VALUE-------------------------

    //-------------------------1 Dimensional

    public static float calcLerpValue(float startValue, float currValue, float endValue, float lerpVelocity_DperF)
    {
        return 0;
    }

    //-------------------------2 Dimensional

    public static float calcLerpValue(Vector2 startVect2, Vector2 currVector2, Vector2 endVector2, float lerpVelocity_DperF)
    {
        return 0;
    }

    //-------------------------3 Dimensional

    public static float calcLerpValue(Vector3 startVect3, Vector3 currVector3, Vector3 endVector3, float lerpVelocity_DperF)
    {
        return 0;
    }

    //-------------------------4 Dimensional

    public static float calcLerpValue(Vector4 startVect4, Vector4 currVector4, Vector4 endVector4, float lerpVelocity_DperF)
    {
        return 0;
    }

    //-------------------------BASE

    public static float calcLerpValue(float[] startValues, float[] currValues, float[] endValues, float lerpVelocity_DperF)
    {
        return 0;
    }

    //-------------------------Colors

    public static float calcLerpValue(colorSpace CS, Color startColor, Color currColor, Color endColor, float lerpVelocity_DperF) //this uses the calcLerpValue function for vector3s
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

        //---Calculate how many frames we have left to finish (based on our velocity)
        float framesToFinish = distToFinish / lerpVelocity_DperF;

        //frames are integers... not floats sowe convert
        //NOTE: this will overshoot our destination... BUT your result is clamped so we DONT return the overshot value
        framesToFinish = (int)Mathf.CeilToInt(framesToFinish);

        //NOTE: as of now we know how many frames every distance will take... so now we use said data to lerp

        //NOTE: "distPerFrame" that took so long to calculate WILL NOT CHANGE because we are aiming for a LERP
        //so we travel the same distance every frame AS LONG AS OUR SETTINGS STAY THE SAME
        float distPerFrame = distToFinish / framesToFinish;

        //TODO... The Implication is that we only need to run this function once  and then just run "the one line" below every frame
        float lerpValue = Mathf.Clamp((distPerFrame / distToFinish), 0, 1);

        return lerpValue;
    }

    //-------------------------HELPER FUNCTIONS-------------------------

    static float timeToFrames(float time, unitOfTime UOT, updateLocation UL)
    {
        if (UOT == unitOfTime.frames)
            return time;
        else //unitOfTime.seconds
            return secondsToFrames(time, UL);
    }

    static float secondsToFrames(float seconds, updateLocation UL)
    {
        if (UL == updateLocation.fixedUpdate)
            return (seconds / Time.fixedDeltaTime);
        else //updateLocation.Update
            return (seconds / Time.deltaTime);
    }

    static float euclideanDistance(float[] pointA, float[] pointB)
    {
        if (pointA.Length == pointB.Length)
        {
            float sum = 0;
            for (int dim = 0; dim < pointA.Length; dim++)
                sum += Mathf.Pow((pointA[dim] - pointB[dim]), 2);
            return Mathf.Sqrt(sum); //distance is always positive
        }
        else
            return -1;
    }
}