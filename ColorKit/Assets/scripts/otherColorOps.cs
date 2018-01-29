using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace colorKit
{
    public enum desiredMixtureType { additive, subtractive };

    public enum colorSpace { RGB, RYB, CMYK };
    public enum updateLocation { fixedUpdate, Update };

    public enum guideDistance { distBetween_Other, distBetween_StartAndEnd, distBetween_CurrAndEnd, distBetween_StartAndCurr };
    public enum unitOfTime { frames, seconds };

    public enum mixingMethod { spaceAveraging, colorAveraging, colorComponentAveraging, eachAsPercentOfMax }

    public static class otherColorOps
    {

        //-------------------------Print Functions-------------------------

        //---4 component

        public static void printVector4(Vector4 vect4)
        {
            printVector4("", vect4);
        }

        public static void printVector4(string printLabel, Vector4 vect4)
        {
            printArray(printLabel, colorTypeConversion.vector4_to_array(vect4));
        }

        //---3 component

        public static void printVector3(Vector3 vect3)
        {
            printVector3("", vect3);
        }

        public static void printVector3(string printLabel, Vector3 vect3)
        {
            printArray(printLabel, colorTypeConversion.vector3_to_array(vect3));
        }

        public static void printColor(Color color)
        {
            printColor("", color);
        }

        public static void printColor(string printLabel, Color color)
        {
            printArray(printLabel, colorTypeConversion.color_to_array(color));
        }

        //---2 component

        public static void printVector2(Vector2 vect2)
        {
            printVector2("", vect2);
        }

        public static void printVector2(string printLabel, Vector2 vect2)
        {
            printArray(printLabel, colorTypeConversion.vector2_to_array(vect2));
        }

        //-----BASE

        public static void printArray(string printLabel, float[] array)
        {
            string text = printLabel + " ";

            for (int i = 0; i < array.Length; i++)
            {
                if (i != (array.Length - 1))
                    text += array[i] + ", ";
                else
                    text += array[i];
            }

            UnityEngine.MonoBehaviour.print(text);
        }

        //-------------------------Error Correction-------------------------

        public static float[] nanCheck(float[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = 0;
                if (float.IsNaN(array[i]))
                    UnityEngine.MonoBehaviour.print("is NAN");
                else if (float.IsInfinity(array[i]))
                    UnityEngine.MonoBehaviour.print("is Inf or Neg Inf");
            }

            return array;
        }

        public static float[] clamp(float[] array, float min, float max)
        {
            for (int i = 0; i < array.Length; i++)
                array[i] = Mathf.Clamp(array[i], min, max);
            return array;
        }
    }
}