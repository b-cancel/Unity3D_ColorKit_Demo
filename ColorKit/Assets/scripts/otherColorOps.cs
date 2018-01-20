using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum desiredMixtureType { additive, subtractive };

public enum colorSpace { RGB, RYB, CMYK };
public enum updateLocation { fixedUpdate, Update };

public enum distanceUsedToCalculateLerpValue { distBetween_BlackAndWhite, distBetween_StartAndEndColor, distBetween_CurrentAndEndColor };
public enum unitOftime { frames, seconds };

public enum mixingMethod { spaceAveraging, colorAveraging, colorComponentAveraging, eachAsPercentOfMax }
public enum _4D_flawToAccept { useFlawedAglo, useWeirdVect4Dist };

public class otherColorOps : MonoBehaviour {

    //-------------------------Print Functions-------------------------

    //---4 component

    public void printVector4(Vector4 vect)
    {
        printVector4("", vect);
    }

    public void printVector4(string printLabel, Vector4 vect)
    {
        printArray(printLabel, gameObject.GetComponent<colorTypeConversion>().vector4_to_array(vect));
    }

    //---3 component

    public void printVector3(Vector3 vect)
    {
        printVector3("", vect);
    }

    public void printVector3(string printLabel, Vector3 vect)
    {
        printArray(printLabel, gameObject.GetComponent<colorTypeConversion>().vector3_to_array(vect));
    }

    public void printColor(Color col)
    {
        printColor("", col);
    }

    public void printColor(string printLabel, Color col)
    {
        printArray(printLabel, gameObject.GetComponent<colorTypeConversion>().color_to_array(col));
    }

    //---2 component

    public void printVector2(Vector2 vect)
    {
        printVector2("", vect);
    }

    public void printVector2(string printLabel, Vector2 vect)
    {
        printArray(printLabel, gameObject.GetComponent<colorTypeConversion>().vector2_to_array(vect));
    }

    //-----BASE

    public void printArray(string printLabel, float[] arr)
    {
        string text = printLabel + " ";

        for (int i = 0; i < arr.Length; i++)
        {
            if (i != (arr.Length - 1))
                text += arr[i] + ", ";
            else
                text += arr[i];
        }

        print(text);
    }

    //-------------------------Error Correction-------------------------

    public float[] nanCheck(float[] arr)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = 0;
            if (float.IsNaN(arr[i]))
                print("is NAN");
            else if (float.IsInfinity(arr[i]))
                print("is Inf or Neg Inf");
        }  

        return arr;
    }

    public float[] clamp(float[] values, float min, float max)
    {
        for (int i = 0; i < values.Length; i++)
            values[i] = Mathf.Clamp(values[i], min, max);
        return values;
    }
}