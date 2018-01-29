using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using lerpKit;

public static class lerpEXT{

    #region Calculate Guide Distance

    public static float calcGuideDistance(this float f, guideDistance GD, float startValue, float currValue, float endValue)
    {
        return lerpHelper.calcGuideDistance(GD, startValue, currValue, endValue);
    }

    public static float calcGuideDistance(this Vector2 v2, guideDistance GD, Vector2 startVect2, Vector2 currVector2, Vector2 endVector2)
    {
        return lerpHelper.calcGuideDistance(GD, startVect2, currVector2, endVector2);
    }

    public static float calcGuideDistance(this Vector3 v3, guideDistance GD, Vector3 startVect3, Vector3 currVector3, Vector3 endVector3)
    {
        return lerpHelper.calcGuideDistance(GD, startVect3, currVector3, endVector3);
    }

    public static float calcGuideDistance(this Vector4 v4, guideDistance GD, Vector4 startVect4, Vector4 currVector4, Vector4 endVector4)
    {
        return lerpHelper.calcGuideDistance(GD, startVect4, currVector4, endVector4);
    }

    public static float calcGuideDistance(this float[] fa, guideDistance GD, float[] startValues, float[] currValues, float[] endValues)
    {
        return lerpHelper.calcGuideDistance(GD, startValues, currValues, endValues);
    }

    #endregion

    #region Calculate Lerp Value (with Distance, Time, Unit of Time, and Update Location)

    public static float calcLerpValue(this float f, float startValue, float currValue, float endValue, float guideDistance, float guideTime, unitOfTime UOT_GD, updateLocation UL)
    {
        return lerpHelper.calcLerpValue(startValue, currValue, endValue, guideDistance, guideTime, UOT_GD, UL);
    }

    public static float calcLerpValue(this Vector2 v2, Vector2 startVector2, Vector2 currVector2, Vector2 endVector2, float guideDistance, float guideTime, unitOfTime UOT_GD, updateLocation UL)
    {
        return lerpHelper.calcLerpValue(startVector2, currVector2, endVector2, guideDistance, guideTime, UOT_GD, UL);
    }

    public static float calcLerpValue(this Vector3 v3, Vector3 startVector3, Vector3 currVector3, Vector3 endVector3, float guideDistance, float guideTime, unitOfTime UOT_GD, updateLocation UL)
    {
        return lerpHelper.calcLerpValue(startVector3, currVector3, endVector3, guideDistance, guideTime, UOT_GD, UL);
    }

    public static float calcLerpValue(this Vector4 v4, Vector4 startVector4, Vector4 currVector4, Vector4 endVector4, float guideDistance, float guideTime, unitOfTime UOT_GD, updateLocation UL)
    {
        return lerpHelper.calcLerpValue(startVector4, currVector4, endVector4, guideDistance, guideTime, UOT_GD, UL);
    }

    public static float calcLerpValue(this float[] fa, float[] startValues, float[] currValues, float[] endValues, float guideDistance, float guideTime, unitOfTime UOT_GD, updateLocation UL)
    {
        return lerpHelper.calcLerpValue(startValues, currValues, endValues, guideDistance, guideTime, UOT_GD, UL);
    }

    #endregion

    #region Calculate Lerp Value (with Velocity)

    public static float calcLerpValue(this float f, float startValue, float currValue, float endValue, float lerpVelocity_DperF)
    {
        return lerpHelper.calcLerpValue(startValue, currValue, endValue, lerpVelocity_DperF);
    }

    public static float calcLerpValue(this Vector2 v2, Vector2 startVector2, Vector2 currVector2, Vector2 endVector2, float lerpVelocity_DperF)
    {
        return lerpHelper.calcLerpValue(startVector2, currVector2, endVector2, lerpVelocity_DperF);
    }

    public static float calcLerpValue(this Vector3 v3, Vector3 startVector3, Vector3 currVector3, Vector3 endVector3, float lerpVelocity_DperF)
    {
        return lerpHelper.calcLerpValue(startVector3, currVector3, endVector3, lerpVelocity_DperF);
    }

    public static float calcLerpValue(this Vector4 v4, Vector4 startVector4, Vector4 currVector4, Vector4 endVector4, float lerpVelocity_DperF)
    {
        return lerpHelper.calcLerpValue(startVector4, currVector4, endVector4, lerpVelocity_DperF);
    }

    public static float calcLerpValue(this float[] fa, float[] startValues, float[] currValues, float[] endValues, float lerpVelocity_DperF)
    {
        return lerpHelper.calcLerpValue(startValues, currValues, endValues, lerpVelocity_DperF);
    }
    
    #endregion
}
