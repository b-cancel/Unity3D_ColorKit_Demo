using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace colorKit
{
    //Description: Change the Color's Format (255,float,hex)

    public static class colorFormatConversion
    {

        //--- color in float format -> 255 format

        public static float[] colorFloat_to_color255(float[] colorFloat)
        {
            float[] color255 = new float[colorFloat.Length];
            for (int i = 0; i < color255.Length; i++)
                color255[i] = _float_to_255(colorFloat[i]);
            return color255;
        }

        //--- color in float format -> hex format

        public static string[] colorFloat_to_colorHex(float[] colorFloat)
        {
            string[] colorHex = new string[colorFloat.Length];
            for (int i = 0; i < colorHex.Length; i++)
                colorHex[i] = _float_to_hex(colorFloat[i]);
            return colorHex;
        }

        //--- color in 255 format -> float format

        public static float[] color255_to_colorFloat(float[] color255)
        {
            float[] colorFloat = new float[color255.Length];
            for (int i = 0; i < colorFloat.Length; i++)
                colorFloat[i] = _255_to_float(color255[i]);
            return colorFloat;
        }

        //---color in 255 format -> hex format

        public static string[] color255_to_colorHex(float[] color255)
        {
            string[] colorHex = new string[color255.Length];
            for (int i = 0; i < colorHex.Length; i++)
                colorHex[i] = _255_to_hex(color255[i]);
            return colorHex;
        }

        //--- color in hex format -> float format

        public static float[] colorHex_to_colorFloat(string[] colorHex)
        {
            float[] colorFloat = new float[colorHex.Length];
            for (int i = 0; i < colorFloat.Length; i++)
                colorFloat[i] = _hex_to_float(colorHex[i]);
            return colorFloat;
        }

        //--- color in hex format -> 255 format

        public static float[] colorHex_to_color255(string[] colorHex)
        {
            float[] color255 = new float[colorHex.Length];
            for (int i = 0; i < color255.Length; i++)
                color255[i] = _hex_to_255(colorHex[i]);
            return color255;
        }

        //-------------------------helpers of the functions above-------------------------

        //--- (Float -> 255)

        static float _float_to_255(float numFloat)
        {
            return Mathf.Clamp(numFloat * 255, 0, 255);
        }

        //--- (Float -> Hex)

        static string _float_to_hex(float numFloat)
        {
            string hex = Convert.ToString((int)Mathf.Round(255 * numFloat), 16);
            return (hex.Length == 1) ? "0" + hex : hex;
        }

        //--- (255 -> Float)

        static float _255_to_float(float num255)
        {
            return Mathf.Clamp(num255 / 255, 0, 1);
        }

        //--- (255 -> Hex)

        static string _255_to_hex(float num255)
        {
            string hex = Convert.ToString((int)Mathf.Round(num255), 16);
            return (hex.Length == 1) ? "0" + hex : hex;
        }

        //--- (Hex -> Float)

        static float _hex_to_float(string hex)
        {
            return Mathf.Clamp(Mathf.Clamp(Convert.ToInt32(hex, 16), 0, 255) / 255, 0, 1);
        }

        //--- (Hex -> 255)

        static float _hex_to_255(string hex)
        {
            return Mathf.Clamp(Convert.ToInt32(hex, 16), 0, 255);
        }
    }
}