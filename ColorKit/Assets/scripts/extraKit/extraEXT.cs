using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extraKit;

public static class extraEXT{
    
    //-----FUNCTION VERSIONS that use a dummy (non used) instance of "whatever type you are (converting format, converting type, printing)"

    #region formatConversion

    //Description: Change the Color's Format (255,float,hex)

    public static float[] _float_to_255(this float[] f, float[] colorFloat)
    {
        return formatConversion._float_to_255(colorFloat);
    }

    public static string[] _float_to_hex(this float[] f, float[] colorFloat)
    {
        return formatConversion._float_to_hex(colorFloat);
    }

    public static float[] _255_to_float(this float[] f, float[] color255)
    {
        return formatConversion._255_to_float(color255);
    }

    public static string[] _255_to_hex(this float[] f, float[] color255)
    {
        return formatConversion._255_to_hex(color255);
    }

    public static float[] _hex_to_float(this string[] s, string[] colorHex)
    {
        return formatConversion._hex_to_float(colorHex);
    }

    public static float[] _hex_to_255(this string[] s, string[] colorHex)
    {
        return formatConversion._hex_to_255(colorHex);
    }

    //-------------------------helpers of the functions above-------------------------

    public static float _float_to_255(this float f, float numFloat)
    {
        return formatConversion._float_to_255(numFloat);
    }

    public static string _float_to_hex(this float f, float numFloat)
    {
        return formatConversion._float_to_hex(numFloat);
    }

    public static float _255_to_float(this float f, float num255)
    {
        return formatConversion._255_to_float(num255);
    }

    public static string _255_to_hex(this float f, float num255)
    {
        return formatConversion._255_to_hex(num255);
    }

    public static float _hex_to_float(this string s, string numHex)
    {
        return formatConversion._hex_to_float(numHex);
    }

    public static float _hex_to_255(this string s, string numHex)
    {
        return formatConversion._hex_to_255(numHex);
    }

    #endregion

    #region colorTypeConversion

    //Description: Change the Color's Data Type (Vectors, Arrays, Colors)

    //-----2 component ??? (Vector2 | Array)

    public static float[] vector2_to_array(this Vector2 v2, Vector2 vector2)
    {
        return typeConversion.vector2_to_array(vector2);
    }

    public static Vector2 array_to_vector2(this float[] fa, float[] array)
    {
        return typeConversion.array_to_vector2(array);
    }

    //-----3 Component Color (Vector3 | Array | Color)

    public static float[] vector3_to_array(this Vector3 v3, Vector3 vector3)
    {
        return typeConversion.vector3_to_array(vector3);
    }

    public static Color vector3_to_color(this Vector3 v3, Vector3 vector3)
    {
        return typeConversion.vector3_to_color(vector3);
    }

    public static Vector3 array_to_vector3(this float[] fa, float[] array)
    {
        return typeConversion.array_to_vector3(array);
    }

    public static Color array_to_color(this float[] fa, float[] array)
    {
        return typeConversion.array_to_color(array);
    }

    public static Vector3 color_to_vector3(this Color c, Color color)
    {
        return typeConversion.color_to_vector3(color);
    }

    public static float[] color_to_array(this Color c, Color color)
    {
        return typeConversion.color_to_array(color);
    }

    //-----4 Component Color (Vector4 | Array)

    public static float[] vector4_to_array(this Vector4 v4, Vector4 vector4)
    {
        return typeConversion.vector4_to_array(vector4);
    }

    public static Vector4 array_to_vector4(this float[] fa, float[] array)
    {
        return typeConversion.array_to_vector4(array);
    }

    #endregion

    #region otherColorOps

    //-------------------------Print Functions-------------------------

    //---4 component

    public static void print(this Vector4 v4, Vector4 vect4)
    {
        otherOps.print(vect4);
    }

    public static void print(this Vector4 v4, string printLabel, Vector4 vect4)
    {
        otherOps.print(vect4, printLabel);
    }

    //---3 component

    public static void print(this Vector3 v3, Vector3 vect3)
    {
        otherOps.print(vect3);
    }

    public static void print(this Vector3 v3, string printLabel, Vector3 vect3)
    {
        otherOps.print(vect3, printLabel);
    }

    public static void print(this Color c, Color color)
    {
        otherOps.print(color);
    }

    public static void print(this Color c, string printLabel, Color color)
    {
        otherOps.print(color, printLabel);
    }

    //---2 component

    public static void print(this Vector2 v2, Vector2 vect2)
    {
        otherOps.print(vect2);
    }

    public static void print(this Vector2 v2, string printLabel, Vector2 vect2)
    {
        otherOps.print(vect2, printLabel);
    }

    //-----BASE

    public static void print(this float[] fa, string printLabel, float[] array)
    {
        otherOps.print(array, printLabel);
    }

    //-------------------------Error Correction-------------------------

    public static float[] nanCheck(this float[] fa, float[] array)
    {
        return otherOps.nanCheck(array);
    }

    public static float[] clamp(this float[] fa, float[] array, float min, float max)
    {
        return otherOps.clamp(array, min, max);
    }

    #endregion

    //-----FUNCTION VERSIONS that use the instance of whatever type they extend (the instance will be of the same type as the first parameter)

    #region formatConversion

    //Description: Change the Color's Format (255,float,hex)

    public static float[] _float_to_255(this float[] colorFloat)
    {
        return formatConversion._float_to_255(colorFloat);
    }

    public static string[] _float_to_hex(this float[] colorFloat)
    {
        return formatConversion._float_to_hex(colorFloat);
    }

    public static float[] _255_to_float(this float[] color255)
    {
        return formatConversion._255_to_float(color255);
    }

    public static string[] _255_to_hex(this float[] color255)
    {
        return formatConversion._255_to_hex(color255);
    }

    public static float[] _hex_to_float(this string[] colorHex)
    {
        return formatConversion._hex_to_float(colorHex);
    }

    public static float[] _hex_to_255(this string[] colorHex)
    {
        return formatConversion._hex_to_255(colorHex);
    }

    //-------------------------helpers of the functions above-------------------------

    public static float _float_to_255(this float numFloat)
    {
        return formatConversion._float_to_255(numFloat);
    }

    public static string _float_to_hex(this float numFloat)
    {
        return formatConversion._float_to_hex(numFloat);
    }

    public static float _255_to_float(this float num255)
    {
        return formatConversion._255_to_float(num255);
    }

    public static string _255_to_hex(this float num255)
    {
        return formatConversion._255_to_hex(num255);
    }

    public static float _hex_to_float(this string numHex)
    {
        return formatConversion._hex_to_float(numHex);
    }

    public static float _hex_to_255(this string numHex)
    {
        return formatConversion._hex_to_255(numHex);
    }

    #endregion

    #region colorTypeConversion

    //Description: Change the Color's Data Type (Vectors, Arrays, Colors)

    //-----2 component ??? (Vector2 | Array)

    public static float[] vector2_to_array(this Vector2 vector2)
    {
        return typeConversion.vector2_to_array(vector2);
    }

    public static Vector2 array_to_vector2(this float[] array)
    {
        return typeConversion.array_to_vector2(array);
    }

    //-----3 Component Color (Vector3 | Array | Color)

    public static float[] vector3_to_array(this Vector3 vector3)
    {
        return typeConversion.vector3_to_array(vector3);
    }

    public static Color vector3_to_color(this Vector3 vector3)
    {
        return typeConversion.vector3_to_color(vector3);
    }

    public static Vector3 array_to_vector3(this float[] array)
    {
        return typeConversion.array_to_vector3(array);
    }

    public static Color array_to_color(this float[] array)
    {
        return typeConversion.array_to_color(array);
    }

    public static Vector3 color_to_vector3(this Color color)
    {
        return typeConversion.color_to_vector3(color);
    }

    public static float[] color_to_array(this Color color)
    {
        return typeConversion.color_to_array(color);
    }

    //-----4 Component Color (Vector4 | Array)

    public static float[] vector4_to_array(this Vector4 vector4)
    {
        return typeConversion.vector4_to_array(vector4);
    }

    public static Vector4 array_to_vector4(this float[] array)
    {
        return typeConversion.array_to_vector4(array);
    }

    #endregion

    #region otherColorOps

    //-------------------------Print Functions-------------------------

    //---4 component

    public static void print(this Vector4 vect4)
    {
        otherOps.print(vect4);
    }

    public static void print(this Vector4 vect4, string printLabel)
    {
        otherOps.print(vect4, printLabel);
    }

    //---3 component

    public static void print(this Vector3 vect3)
    {
        otherOps.print(vect3);
    }

    public static void print(this Vector3 vect3, string printLabel)
    {
        otherOps.print(vect3, printLabel);
    }

    public static void print(this Color color)
    {
        otherOps.print(color);
    }

    public static void print(this Color color, string printLabel)
    {
        otherOps.print(color, printLabel);
    }

    //---2 component

    public static void print(this Vector2 vect2)
    {
        otherOps.print(vect2);
    }

    public static void print(this Vector2 vect2, string printLabel)
    {
        otherOps.print(vect2, printLabel);
    }

    //-----BASE

    public static void print(this float[] array, string printLabel)
    {
        otherOps.print(array, printLabel);
    }

    //-------------------------Error Correction-------------------------

    public static float[] nanCheck(this float[] array)
    {
        return otherOps.nanCheck(array);
    }

    public static float[] clamp(this float[] array, float min, float max)
    {
        return otherOps.clamp(array, min, max);
    }

    #endregion
}
