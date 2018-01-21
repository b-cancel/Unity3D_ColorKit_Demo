using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace colorKit
{
    public enum desiredMixtureType { additive, subtractive };

    public enum colorSpace { RGB, RYB, CMYK };
    public enum updateLocation { fixedUpdate, Update };

    public enum distanceUsedToCalculateLerpValue { distBetween_BlackAndWhite, distBetween_StartAndEndColor, distBetween_CurrentAndEndColor };
    public enum unitOftime { frames, seconds };

    public enum mixingMethod { spaceAveraging, colorAveraging, colorComponentAveraging, eachAsPercentOfMax }
    public enum _4D_flawToAccept { useFlawedAglo, useWeirdVect4Dist };

    public static class otherColorOps
    {

        //-------------------------Print Functions-------------------------

        //---4 component

        public static void printVector4(Vector4 vect)
        {
            printVector4("", vect);
        }

        public static void printVector4(string printLabel, Vector4 vect)
        {
            printArray(printLabel, colorTypeConversion.vector4_to_array(vect));
        }

        //---3 component

        public static void printVector3(Vector3 vect)
        {
            printVector3("", vect);
        }

        public static void printVector3(string printLabel, Vector3 vect)
        {
            printArray(printLabel, colorTypeConversion.vector3_to_array(vect));
        }

        public static void printColor(Color col)
        {
            printColor("", col);
        }

        public static void printColor(string printLabel, Color col)
        {
            printArray(printLabel, colorTypeConversion.color_to_array(col));
        }

        //---2 component

        public static void printVector2(Vector2 vect)
        {
            printVector2("", vect);
        }

        public static void printVector2(string printLabel, Vector2 vect)
        {
            printArray(printLabel, colorTypeConversion.vector2_to_array(vect));
        }

        //-----BASE

        public static void printArray(string printLabel, float[] arr)
        {
            string text = printLabel + " ";

            for (int i = 0; i < arr.Length; i++)
            {
                if (i != (arr.Length - 1))
                    text += arr[i] + ", ";
                else
                    text += arr[i];
            }

            UnityEngine.MonoBehaviour.print(text);
        }

        //-------------------------Error Correction-------------------------

        public static float[] nanCheck(float[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = 0;
                if (float.IsNaN(arr[i]))
                    UnityEngine.MonoBehaviour.print("is NAN");
                else if (float.IsInfinity(arr[i]))
                    UnityEngine.MonoBehaviour.print("is Inf or Neg Inf");
            }

            return arr;
        }

        public static float[] clamp(float[] values, float min, float max)
        {
            for (int i = 0; i < values.Length; i++)
                values[i] = Mathf.Clamp(values[i], min, max);
            return values;
        }
    }
}