using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorData
{
    //public
    public float creationTime;
    public GameObject theGO;
    //private
    Color theColor;
    float theQuantity;

    public colorData(GameObject permanentGO, Color starterColor, float starterQuantity)
    {
        creationTime = Time.realtimeSinceStartup;

        theGO = permanentGO;

        setColor(starterColor);
        setQuantity(starterQuantity);
    }

    //-----Setters

    public void setColor(Color newColor)
    {
        theColor = newColor;

        theGO.GetComponent<colorRefs>().updateColor(newColor);

        Camera.main.GetComponent<demo>().updateColorMixtures();

        theGO.GetComponent<colorRefs>().toolBar.GetComponent<colorChangeTool>().setSliders(newColor);
    }

    public void setQuantity(float newQuantity)
    {
        theQuantity = newQuantity;

        theGO.GetComponent<colorRefs>().updateQuantity(newQuantity);

        Camera.main.GetComponent<demo>().updateColorMixtures();
    }

    //-----Getters

    public Color getColor()
    {
        return theColor;
    }

    public float getQuantity()
    {
        return theQuantity;
    }
}
