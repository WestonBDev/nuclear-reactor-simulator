using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using static Window_Graph;
using UnityEngine.UIElements;

public class StaticElements_Graph : MonoBehaviour
{
    private RectTransform graphContainer;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
    private RectTransform dashTemplateX;
    private RectTransform dashTemplateY;
    [SerializeField] GraphUpdater graphUpdater;
    [SerializeField] private int maxVisibleValueAmount;
    [SerializeField] private int xDashAmmount;

    private Func<int, string> getAxisLabelX;
    private Func<float, string> getAxisLabelY;

    private List<TMP_Text> xLabelList = new List<TMP_Text>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();
        dashTemplateX = graphContainer.Find("dashTemplateX").GetComponent<RectTransform>();
        dashTemplateY = graphContainer.Find("dashTemplateY").GetComponent<RectTransform>();
        //tooltipGameObject = graphContainer.Find("tooltip").gameObject;

        labelTemplateX.gameObject.SetActive(false);
        labelTemplateY.gameObject.SetActive(false);
        dashTemplateX.gameObject.SetActive(false);
        dashTemplateY.gameObject.SetActive(false);

        ShowElements(maxVisibleValueAmount, (int _i) => "19:06:04", (float _f) => Mathf.RoundToInt(_f).ToString() + "%");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowElements(int maxVisibleValueAmount, Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null)
    {

        // Test for label defaults
        if (getAxisLabelX == null)
        {
            if (this.getAxisLabelX != null)
            {
                getAxisLabelX = this.getAxisLabelX;
            }
            else
            {
                getAxisLabelX = delegate (int _i) { return _i.ToString(); };
            }
        }
        if (getAxisLabelY == null)
        {
            if (this.getAxisLabelY != null)
            {
                getAxisLabelY = this.getAxisLabelY;
            }
            else
            {
                getAxisLabelY = delegate (float _f) { return Mathf.RoundToInt(_f).ToString(); };
            }
        }

        //Debug.Log(getAxisLabelX);
        this.getAxisLabelX = getAxisLabelX;
        this.getAxisLabelY = getAxisLabelY;
 

        // Grab the width and height from the container
        float graphWidth = graphContainer.sizeDelta.x;
        float graphHeight = graphContainer.sizeDelta.y;

        float yMinimum, yMaximum;
        yMinimum = 0; yMaximum = 100;
        //CalculateYScale(out yMinimum, out yMaximum);

        // Set the distance between each point on the graph 
        float xSize = graphWidth / (maxVisibleValueAmount);
        float xSizeDash = graphWidth / (xDashAmmount);

        // Cycle through all visible data points
        int xIndex = 0;
        for (int i = 0; i < maxVisibleValueAmount; i++)
        {
            float xPosition = xIndex * xSize;
            float yPosition =  (graphHeight / maxVisibleValueAmount) * i;


            // Duplicate the x label template
            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -7f);
            TMP_Text tmp = labelX.GetComponent<TMP_Text>();
            tmp.text = getAxisLabelX(i);
            xLabelList.Add(tmp);

            xIndex++;
        }

        graphUpdater.timeStampsList = xLabelList;
        xIndex = 0;
        for (int i = 0;i < xDashAmmount; i++)
        {
            float xPosition = xIndex * xSizeDash;
            float yPosition = (graphHeight / maxVisibleValueAmount) * i;

            // Duplicate the x dash template
            RectTransform dashX = Instantiate(dashTemplateX);
            dashX.SetParent(graphContainer, false);
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition = new Vector2(xPosition, -3f);
            dashX.sizeDelta = new Vector2(graphHeight, dashX.sizeDelta.y);

            xIndex++;
        }

        // Set up separators on the y axis
        int separatorCount = 10;
        for (int i = 0; i <= separatorCount; i++)
        {
            // Duplicate the label template
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer, false);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i * 1f / separatorCount;
            labelY.anchoredPosition = new Vector2(-50f, normalizedValue * graphHeight);
            labelY.GetComponent<TMP_Text>().text = getAxisLabelY(yMinimum + (normalizedValue * (yMaximum - yMinimum)));

            // Duplicate the dash template
            RectTransform dashY = Instantiate(dashTemplateY);
            dashY.SetParent(graphContainer, false);
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition = new Vector2(-4f, normalizedValue * graphHeight);
            dashY.sizeDelta = new Vector2(graphWidth, dashY.sizeDelta.y);
        }
    }
}
