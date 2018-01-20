using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class colorRefs : MonoBehaviour {

    //reference to prefab
    public GameObject toolBarPrefab;

    //reference to obj sometimes
    public GameObject toolBar;
    float toolBarHeight;

    //references to objs
    public GameObject colorSample;

    public GameObject rgb;
    public GameObject ryb;
    public GameObject cmyk;
    public GameObject quantity;

    public GameObject removeColorBtn;

    public GameObject toolBarBtn;
    public GameObject toolBarBtnIcon;

    public Sprite toolBarOpenedSprite;
    public Sprite toolBarClosedSprite;

    //variables
    public bool toolBarOpen;

	// Use this for initialization
	void Awake ()
    {
        toolBarHeight = 470; 

        toolBarOpen = false;
        toolBar = Instantiate(toolBarPrefab, gameObject.transform);
        toolBar.GetComponent<colorChangeTool>().attachedColorData = gameObject;

        //essentially on click drawer
        toolBarBtnIcon.GetComponent<Image>().sprite = toolBarClosedSprite;

        toolBar.GetComponent<RectTransform>().sizeDelta = new Vector2(toolBar.GetComponent<RectTransform>().sizeDelta.x, 0);
        toolBar.SetActive(false);

        addColorListeners();
    }

    public void addColorListeners()
    {
        //NOTE: currently trying to get the line below working in another way
        toolBarBtn.GetComponent<Button>().onClick.AddListener(this.onClick_ColorDrawer);
        quantity.GetComponent<InputField>().onEndEdit.AddListener(this.onEndEdit_colorQuantity);
        removeColorBtn.GetComponent<Button>().onClick.AddListener(this.onClick_removeColor);
    }

    void Update()
    {
        updateUI();
    }

    void FixedUpdate()
    {
        updateUI();
    }

    void updateUI()
    {
        List<GameObject> children = new List<GameObject>();

        //set our height (get height of all children)
        float totalHeight = 5; //5 for top spacing

        foreach (Transform child in transform)
        {
            totalHeight += child.gameObject.GetComponent<RectTransform>().rect.height + 5;
            children.Add(child.gameObject);
        }

        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(gameObject.GetComponent<RectTransform>().sizeDelta.x, totalHeight);

        //set the position of all the children

        float child1_X = children[0].GetComponent<RectTransform>().localPosition.x;
        float child1_Y = (gameObject.GetComponent<RectTransform>().rect.height / 2f) - 5 - (children[0].GetComponent<RectTransform>().rect.height / 2f);
        children[0].GetComponent<RectTransform>().localPosition = new Vector2(child1_X, child1_Y);

        if(gameObject.transform.childCount != 1)
        {
            float child2_X = children[1].GetComponent<RectTransform>().localPosition.x;
            float child2_Y = child1_Y - (children[0].GetComponent<RectTransform>().rect.height / 2f) - 5 - (children[1].GetComponent<RectTransform>().rect.height / 2f); //5 is extra spacing
            children[1].GetComponent<RectTransform>().localPosition = new Vector2(child2_X, child2_Y);
        }
    }

    //----------Events Triggered by UI elements

    public void onClick_ColorDrawer()
    {
        if (toolBarOpen) //currently OPEN -> Closed
        {
            toolBarBtnIcon.GetComponent<Image>().sprite = toolBarClosedSprite;

            toolBar.GetComponent<RectTransform>().sizeDelta = new Vector2(toolBar.GetComponent<RectTransform>().sizeDelta.x, 0);
            toolBar.SetActive(false);
        }
        else //currently CLOSED -> Open
        {
            toolBarBtnIcon.GetComponent<Image>().sprite = toolBarOpenedSprite;

            toolBar.GetComponent<RectTransform>().sizeDelta = new Vector2(toolBar.GetComponent<RectTransform>().sizeDelta.x, toolBarHeight);
            toolBar.SetActive(true);

            //IF... we remove this line the tool bars will work independantly from each other
            Camera.main.GetComponent<demo>().closeAllOtherDrawers(gameObject);
        }

        toolBarOpen = !toolBarOpen;
    }

    public void onClick_removeColor()
    {
        Camera.main.GetComponent<demo_colors>().removeColor(gameObject);
    }

    public void onEndEdit_colorQuantity(string newQuantityS)
    {
        float oldQuantity = Camera.main.GetComponent<demo_colors>().colorDataList[gameObject].getQuantity();
        float newQuantity = 0;

        try
        {
            newQuantity = float.Parse(newQuantityS);
            quantity.GetComponent<Image>().color = Color.white;
        }
        catch (FormatException)
        {
            newQuantity = oldQuantity;
            quantity.GetComponent<Image>().color = Color.red;
        }
        catch (OverflowException)
        {
            newQuantity = oldQuantity;
            quantity.GetComponent<Image>().color = Color.red;
        }

        Camera.main.GetComponent<demo_colors>().colorDataList[gameObject].setQuantity(newQuantity);
    }

    //----------Edits to the Color

    public void updateColor(Color newColor)
    {
        colorSample.GetComponent<Image>().color = newColor;

        float[] rgbFloat = Camera.main.GetComponent<colorTypeConversion>().color_to_array(newColor);
        float[] rgb255 = Camera.main.GetComponent<colorFormatConversion>().colorFloat_to_color255(rgbFloat);
        float[] ryb255 = Camera.main.GetComponent<rgb2ryb_ryb2rgb>().rgb255_to_ryb255(rgb255);
        float[] cmyk255 = Camera.main.GetComponent<rgb2cmyk_cmyk2rgb>().rgb255_to_cmyk255(rgb255);

        string rgbString = "";
        for (int i = 0; i < rgb255.Length; i++)
        {
            rgbString += Mathf.RoundToInt(rgb255[i]);
            if (i != (rgb255.Length - 1))
                rgbString += ", ";
        }

        string rybString = "";
        for (int i = 0; i < ryb255.Length; i++)
        {
            rybString += Mathf.RoundToInt(ryb255[i]);
            if (i != (ryb255.Length - 1))
                rybString += ", ";
        }

        string cmykString = "";
        for (int i = 0; i < cmyk255.Length; i++)
        {
            cmykString += Mathf.RoundToInt(cmyk255[i]);
            if (i != (cmyk255.Length - 1))
                cmykString += ", ";
        }

        rgb.GetComponent<InputField>().text = rgbString;
        ryb.GetComponent<InputField>().text = rybString;
        cmyk.GetComponent<InputField>().text = cmykString;
    }

    public void updateQuantity(float newQuantity)
    {
        quantity.GetComponent<InputField>().text = newQuantity.ToString();
    }
}
