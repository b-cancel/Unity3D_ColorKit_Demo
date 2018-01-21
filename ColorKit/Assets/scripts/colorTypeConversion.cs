using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace colorKit
{
    //Description: Change the Color's Data Type (Vectors, Arrays, Colors)

    public static class colorTypeConversion
    {

        //-----2 component ??? (Vector2 | Array)

        public static float[] vector2_to_array(Vector2 color)
        {
            return new float[] { color[0], color[1] };
        }

        public static Vector2 array_to_vector2(float[] color)
        {
            if (color.Length == 2)
                return new Vector2(color[0], color[1]);
            else
                return Vector2.zero;
        }

        //-----3 Component Color (Vector3 | Array | Color)

        public static float[] vector3_to_array(Vector3 color)
        {
            return new float[] { color[0], color[1], color[2] };
        }

        public static Color vector3_to_color(Vector3 color)
        {
            return new Color(color.x, color.y, color.z);
        }

        public static Vector3 array_to_vector3(float[] color)
        {
            if (color.Length == 3)
                return new Vector3(color[0], color[1], color[2]);
            else
                return Vector3.zero;
        }

        public static Color array_to_color(float[] color)
        {
            if (color.Length == 3)
                return new Color(color[0], color[1], color[2]);
            else
                return Color.black;
        }

        public static Vector3 color_to_vector3(Color color)
        {
            return new Vector3(color.r, color.g, color.b);
        }

        public static float[] color_to_array(Color color)
        {
            return new float[] { color.r, color.g, color.b };
        }

        //-----4 Component Color (Vector4 | Array)

        public static float[] vector4_to_array(Vector4 color)
        {
            return new float[] { color[0], color[1], color[2], color[3] };
        }

        public static Vector4 array_to_vector4(float[] color)
        {
            if (color.Length == 4)
                return new Vector4(color[0], color[1], color[2], color[3]);
            else
                return Vector4.zero;
        }
    }
}