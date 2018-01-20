using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class demo_updateUI : MonoBehaviour
{
    public GameObject masterPanel;

    List<string> gameObjectsToIgnore;

    void Awake()
    {
        gameObjectsToIgnore = new List<string>();

        gameObjectsToIgnore.Add("mixing panel");
        gameObjectsToIgnore.Add("mixing panel(Clone)");
        gameObjectsToIgnore.Add("color panel");
        gameObjectsToIgnore.Add("color panel(Clone)");
        gameObjectsToIgnore.Add("lerping panel");

        gameObjectsToIgnore.Add("Control Panel");
        gameObjectsToIgnore.Add("CL Horizontal Layout");
        //gameObjectsToIgnore.Add("C Vertical Layout");
        //gameObjectsToIgnore.Add("CM Vertical Layout");
    }

    void Update()
    {
        updateUI_of_N_generation(masterPanel, 3);
    }

    void FixedUpdate()
    {
        updateUI_of_N_generation(masterPanel, 3);
    }

    void updateUI_of_N_generation(GameObject self, int generations)
    {
        if (generations > 1) //you have children -> take care of them
        {
            foreach (Transform child in self.transform)
            {
                if (gameObjectsToIgnore.Contains(child.gameObject.name) == false)
                    updateUI_of_N_generation(child.gameObject, generations - 1);
            }
        }

        //take care of self
        set_size_and_pos_OfPanel_basedOnChildren(self);
    }

    //-----Helper Function

    void set_size_and_pos_OfPanel_basedOnChildren(GameObject panel)
    {
        if (panel.transform.childCount != 0)
        {
            //get values to set size
            float panel_sizeX = panel.GetComponent<RectTransform>().sizeDelta.x;
            float panel_sizeY = 5; //this is the 5 units of space on top

            foreach (Transform child in panel.transform)
                if (Mathf.Approximately(child.GetComponent<RectTransform>().rect.height, 0) == false)
                    panel_sizeY += child.GetComponent<RectTransform>().rect.height + 5;

            //officially set SIZE
            panel.GetComponent<RectTransform>().sizeDelta = new Vector2(panel_sizeX, panel_sizeY);

            //get values to set POSITIONS and set them(afte size has been calculated)
            bool firstTime = true;
            float prevChildY = 0;
            GameObject prevChild = panel;
            foreach (Transform child in panel.transform)
            {
                if (Mathf.Approximately(child.GetComponent<RectTransform>().rect.height, 0) == false)
                {
                    //set the position based on the top of the parent
                    float childX = child.gameObject.GetComponent<RectTransform>().localPosition.x;
                    float childY;

                    if (firstTime)
                    {
                        childY = (prevChild.GetComponent<RectTransform>().rect.height / 2f) - 5 - (child.gameObject.GetComponent<RectTransform>().rect.height / 2f);
                        firstTime = false;
                    }
                    else
                        childY = prevChildY - (prevChild.GetComponent<RectTransform>().rect.height / 2f) - 5 - (child.gameObject.GetComponent<RectTransform>().rect.height / 2f);

                    child.gameObject.GetComponent<RectTransform>().localPosition = new Vector2(childX, childY);

                    prevChildY = childY;
                    prevChild = child.gameObject;
                }
            }
        }
        //ELSE you keep your size and position
    }
}