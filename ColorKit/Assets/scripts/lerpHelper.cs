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
        return 0;
    }

    //-------------------------2 Dimensional

    public static float calcGuideDistance(guideDistance GD, Vector2 startVect2, Vector2 currVector2, Vector2 endVector2)
    {
        return 0;
    }

    //-------------------------3 Dimensional

    public static float calcGuideDistance(guideDistance GD, Vector3 startVect3, Vector3 currVector3, Vector3 endVector3)
    {
        return 0;
    }

    //-------------------------4 Dimensional

    public static float calcGuideDistance(guideDistance GD, Vector4 startVect4, Vector4 currVector4, Vector4 endVector4)
    {
        return 0;
    }

    //-------------------------BASE

    public static float calcGuideDistance(guideDistance GD, float[] startValues, float[] currValues, float[] endValues)
    {
        return 0;
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
        //calc vel
        float velocity = 0;

        //VEL = distance / framesToTravelGD;

        //distance left to travel

        float dist_C_2_E = 0;
        switch (CS)
        {
            case colorSpace.RGB:
                dist_C_2_E = colorDistances.distBetweenColors(colorSpace.RGB, currColor, endColor);
                break;
            case colorSpace.RYB:
                dist_C_2_E = colorDistances.distBetweenColors(colorSpace.RYB, currColor, endColor);
                break;
            default:
                dist_C_2_E = colorDistances.distBetweenColors(colorSpace.CMYK, currColor, endColor);
                break;
        }

        //---Calculate how many frames we have left to finish (based on our velocity)
        float framesToFinsih_C_2_E = velocity * dist_C_2_E;

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