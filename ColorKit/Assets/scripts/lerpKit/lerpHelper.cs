using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lerpKit
{
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

    public enum updateLocation { fixedUpdate, Update };
    public enum unitOfTime { frames, seconds };
    public enum guideDistance { distBetween_Other, distBetween_StartAndEnd, distBetween_CurrAndEnd, distBetween_StartAndCurr };

    public static class lerpHelper
    {

        //-------------------------CALCULATE GUIDE DISTANCE-------------------------

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

        //-------------------------CALCULATE LERP VALUE (with Distance, Time, Unit of Time, and Update Location)-------------------------

        public static float calcLerpValue(float startValue, float currValue, float endValue, float guideDistance, float guideTime, unitOfTime UOT_GD, updateLocation UL)
        {
            return calcLerpValue(startValue, currValue, endValue, calcLerpVelocity(guideDistance, guideTime, UOT_GD,UL));
        }

        public static float calcLerpValue(Vector2 startVector2, Vector2 currVector2, Vector2 endVector2, float guideDistance, float guideTime, unitOfTime UOT_GD, updateLocation UL)
        {
            return calcLerpValue(startVector2, currVector2, endVector2, calcLerpVelocity(guideDistance, guideTime, UOT_GD, UL));
        }

        public static float calcLerpValue(Vector3 startVector3, Vector3 currVector3, Vector3 endVector3, float guideDistance, float guideTime, unitOfTime UOT_GD, updateLocation UL)
        {
            return calcLerpValue(startVector3, currVector3, endVector3, calcLerpVelocity(guideDistance, guideTime, UOT_GD, UL));
        }

        public static float calcLerpValue(Vector4 startVector4, Vector4 currVector4, Vector4 endVector4, float guideDistance, float guideTime, unitOfTime UOT_GD, updateLocation UL)
        {
            return calcLerpValue(startVector4, currVector4, endVector4, calcLerpVelocity(guideDistance, guideTime, UOT_GD, UL));
        }

        public static float calcLerpValue(float[] startValues, float[] currValues, float[] endValues, float guideDistance, float guideTime, unitOfTime UOT_GD, updateLocation UL)
        {
            return calcLerpValue(startValues, currValues, endValues, calcLerpVelocity(guideDistance, guideTime, UOT_GD, UL));
        }

        //-------------------------CALCULATE LERP VALUE (with Velocity)-------------------------

        public static float calcLerpValue(float startValue, float currValue, float endValue, float lerpVelocity_DperF)
        {
            //---calc distance left to travel
            float distToFinish = Mathf.Abs(currValue - endValue);

            //--- calc lerp value based on this
            return Mathf.Clamp((lerpVelocity_DperF / distToFinish), 0, 1);
        }

        public static float calcLerpValue(Vector2 startVector2, Vector2 currVector2, Vector2 endVector2, float lerpVelocity_DperF)
        {
            //---calc distance left to travel
            float distToFinish = Vector2.Distance(currVector2, endVector2);

            //--- calc lerp value based on this
            return Mathf.Clamp((lerpVelocity_DperF / distToFinish), 0, 1);
        }

        public static float calcLerpValue(Vector3 startVector3, Vector3 currVector3, Vector3 endVector3, float lerpVelocity_DperF)
        {
            //---calc distance left to travel
            float distToFinish = Vector3.Distance(currVector3, endVector3);

            //--- calc lerp value based on this
            return Mathf.Clamp((lerpVelocity_DperF / distToFinish), 0, 1);
        }

        public static float calcLerpValue(Vector4 startVector4, Vector4 currVector4, Vector4 endVector4, float lerpVelocity_DperF)
        {
            //---calc distance left to travel
            float distToFinish = Vector4.Distance(currVector4, endVector4);

            //--- calc lerp value based on this
            return Mathf.Clamp((lerpVelocity_DperF / distToFinish), 0, 1);
        }

        public static float calcLerpValue(float[] startValues, float[] currValues, float[] endValues, float lerpVelocity_DperF)
        {
            //---calc distance left to travel
            float distToFinish = euclideanDistance(currValues, endValues);

            //--- calc lerp value based on this
            return Mathf.Clamp((lerpVelocity_DperF / distToFinish), 0, 1);
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
}