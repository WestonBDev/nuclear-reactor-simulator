/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using TMPro;
using Unity.Entities.UniversalDelegates;
using Unity.VisualScripting;

public class Window_Graph : MonoBehaviour {

    [SerializeField] private Sprite dotSprite;
    [SerializeField] private String graphContainerName = "graphContainer";
    [SerializeField] private Color circleColor = Color.white;
    [SerializeField] private string graphLabel = "Default";
    private RectTransform graphContainer;
    private RectTransform labelTemplateGraph;
    private List<GameObject> gameObjectList;
    private List<IGraphVisualObject> graphVisualObjectList;
    private GameObject tooltipGameObject;
    private List<RectTransform> yLabelList;

    public IGraphVisual lineGraphVisual;
    public IGraphVisual barChartVisual;

    // Cached values
    [SerializeField] private List<int> valueList;
    private IGraphVisual graphVisual;
    private int maxVisibleValueAmount;
    private Func<int, string> getAxisLabelX;
    private Func<float, string> getAxisLabelY;
    private float xSize;
    private bool startYScaleAtZero;

    private void Awake() {
        // Grab base objects references
        graphContainer = transform.Find(graphContainerName).GetComponent<RectTransform>();
        labelTemplateGraph = graphContainer.Find("labelTemplateGraph").GetComponent<RectTransform>();

        startYScaleAtZero = true;
        gameObjectList = new List<GameObject>();
        yLabelList = new List<RectTransform>();
        graphVisualObjectList = new List<IGraphVisualObject>();
        
        lineGraphVisual = new LineGraphVisual(graphContainer, dotSprite, circleColor, new Color(1, 1, 1, .5f), this);
        
        
        //HideTooltip();
    }

    private void ShowTooltip(string tooltipTMP_Text, Vector2 anchoredPosition) {
        // Show Tooltip GameObject
        //tooltipGameObject.SetActive(true);

        //tooltipGameObject.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;

        //TMP_Text tooltipUITMP_Text = tooltipGameObject.transform.Find("text").GetComponent<TMP_Text>();
        //tooltipUITMP_Text.text = tooltipTMP_Text;

        //float textPaddingSize = 4f;
        //Vector2 backgroundSize = new Vector2(
        //    tooltipUITMP_Text.preferredWidth + textPaddingSize * 2f, 
        //    tooltipUITMP_Text.preferredHeight + textPaddingSize * 2f
        //);

        //tooltipGameObject.transform.Find("background").GetComponent<RectTransform>().sizeDelta = backgroundSize;

        //// UI Visibility Sorting based on Hierarchy, SetAsLastSibling in order to show up on top
        //tooltipGameObject.transform.SetAsLastSibling();
    }

    private void HideTooltip() {
        //tooltipGameObject.SetActive(false);
    }

    public void SetGetAxisLabelX(Func<int, string> getAxisLabelX) {
        ShowGraph(this.valueList, this.graphVisual, this.maxVisibleValueAmount, getAxisLabelX, this.getAxisLabelY);
    }

    public void SetGetAxisLabelY(Func<float, string> getAxisLabelY) {
        ShowGraph(this.valueList, this.graphVisual, this.maxVisibleValueAmount, this.getAxisLabelX, getAxisLabelY);
    }

    public void IncreaseVisibleAmount() {
        ShowGraph(this.valueList, this.graphVisual, this.maxVisibleValueAmount + 1, this.getAxisLabelX, this.getAxisLabelY);
    }

    public void DecreaseVisibleAmount() {
        ShowGraph(this.valueList, this.graphVisual, this.maxVisibleValueAmount - 1, this.getAxisLabelX, this.getAxisLabelY);
    }

    private void SetGraphVisual(IGraphVisual graphVisual) {
        ShowGraph(this.valueList, graphVisual, this.maxVisibleValueAmount, this.getAxisLabelX, this.getAxisLabelY);
    }

    public void ShowGraph(List<int> valueList, IGraphVisual graphVisual = null, int maxVisibleValueAmount = -1, Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null) {
        if (valueList == null) {
            Debug.LogError("valueList is null!");
            return;
        }
        this.valueList = valueList;

        if (graphVisual == null) {
            graphVisual = lineGraphVisual;
        }
        this.graphVisual = graphVisual;

        if (maxVisibleValueAmount <= 0) {
            // Show all if no amount specified
            maxVisibleValueAmount = valueList.Count;
        }
        if (maxVisibleValueAmount > valueList.Count) {
            // Validate the amount to show the maximum
            maxVisibleValueAmount = valueList.Count;
        }

        this.maxVisibleValueAmount = maxVisibleValueAmount;

        // Test for label defaults
        if (getAxisLabelX == null) {
            if (this.getAxisLabelX != null) {
                getAxisLabelX = this.getAxisLabelX;
            } else {
                getAxisLabelX = delegate (int _i) { return _i.ToString(); };
            }
        }
        if (getAxisLabelY == null) {
            if (this.getAxisLabelY != null) {
                getAxisLabelY = this.getAxisLabelY;
            } else {
                getAxisLabelY = delegate (float _f) { return Mathf.RoundToInt(_f).ToString(); };
            }
        }

        //Debug.Log(getAxisLabelX);
        this.getAxisLabelX = getAxisLabelX;
        this.getAxisLabelY = getAxisLabelY;

        // Clean up previous graph
        foreach (GameObject gameObject in gameObjectList) {
            Destroy(gameObject);
        }
        gameObjectList.Clear();
        yLabelList.Clear();

        foreach (IGraphVisualObject graphVisualObject in graphVisualObjectList) {
            graphVisualObject.CleanUp();
        }
        graphVisualObjectList.Clear();

        graphVisual.CleanUp();
        
        // Grab the width and height from the container
        float graphWidth = graphContainer.sizeDelta.x;
        float graphHeight = graphContainer.sizeDelta.y;

        float yMinimum, yMaximum;
        CalculateYScale(out yMinimum, out yMaximum);

        // Set the distance between each point on the graph 
        xSize = graphWidth / (maxVisibleValueAmount);

        // Cycle through all visible data points
        int xIndex = 0;
        for (int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i < valueList.Count; i++) {
            float xPosition =  xIndex * xSize;
            float yPosition = (valueList[i] / 100f) * graphHeight;

            // Add data point visual
            string tooltipTMP_Text = getAxisLabelY(valueList[i]);
            IGraphVisualObject graphVisualObject = graphVisual.CreateGraphVisualObject(new Vector2(xPosition, yPosition), xSize, tooltipTMP_Text, i);
            graphVisualObjectList.Add(graphVisualObject);

            xIndex++;
        }
    }

    public void UpdateLastIndexValue(int value) {
        UpdateValue(valueList.Count - 1, value);
    }

    public void UpdateValue(int index, int value) {
        float yMinimumBefore, yMaximumBefore;
        CalculateYScale(out yMinimumBefore, out yMaximumBefore);

        valueList[index] = value;

        float graphWidth = graphContainer.sizeDelta.x;
        float graphHeight = graphContainer.sizeDelta.y;
        
        float yMinimum, yMaximum;
        CalculateYScale(out yMinimum, out yMaximum);

        bool yScaleChanged = yMinimumBefore != yMinimum || yMaximumBefore != yMaximum;

        if (!yScaleChanged) {
            // Y Scale did not change, update only this value
            int xIndex = index - Mathf.Max(valueList.Count - maxVisibleValueAmount, 0);
            float xPosition = xSize + xIndex * xSize;
            float yPosition = ((value - yMinimum) / (yMaximum - yMinimum)) * graphHeight;

            // Add data point visual
            string tooltipTMP_Text = getAxisLabelY(value);
            graphVisualObjectList[xIndex].SetGraphVisualObjectInfo(new Vector2(xPosition, yPosition), xSize, tooltipTMP_Text);
        } else {
            // Y scale changed, update whole graph and y axis labels
            // Cycle through all visible data points
            int xIndex = 0;
            for (int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i < valueList.Count; i++) {
                float xPosition = xSize + xIndex * xSize;
                float yPosition = ((valueList[i] - yMinimum) / (yMaximum - yMinimum)) * graphHeight;

                // Add data point visual
                string tooltipTMP_Text = getAxisLabelY(valueList[i]);
                graphVisualObjectList[xIndex].SetGraphVisualObjectInfo(new Vector2(xPosition, yPosition), xSize, tooltipTMP_Text);

                xIndex++;
            }

            for (int i = 0; i < yLabelList.Count; i++) {
                float normalizedValue = i * 1f / yLabelList.Count;
                yLabelList[i].GetComponent<TMP_Text>().text = getAxisLabelY(yMinimum + (normalizedValue * (yMaximum - yMinimum)));
            }
        }
    }

    private void CalculateYScale(out float yMinimum, out float yMaximum) {
        // Identify y Min and Max values
        yMaximum = valueList[0];
        yMinimum = valueList[0];
        
        for (int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i < valueList.Count; i++) {
            int value = valueList[i];
            if (value > yMaximum) {
                yMaximum = value;
            }
            if (value < yMinimum) {
                yMinimum = value;
            }
        }

        float yDifference = yMaximum - yMinimum;
        if (yDifference <= 0) {
            yDifference = 5f;
        }
        yMaximum = yMaximum + (yDifference * 0.2f);
        yMinimum = yMinimum - (yDifference * 0.2f);

        if (startYScaleAtZero) {
            yMinimum = 0f; // Start the graph at zero
        }
    }



    /*
     * Interface definition for showing visual for a data point
     * */
    public interface IGraphVisual {

        IGraphVisualObject CreateGraphVisualObject(Vector2 graphPosition, float graphPositionWidth, string tooltipTMP_Text, int index);
        void CleanUp();

    }

    /*
     * Represents a single Visual Object in the graph
     * */
    public interface IGraphVisualObject {

        void SetGraphVisualObjectInfo(Vector2 graphPosition, float graphPositionWidth, string tooltipTMP_Text);
        void CleanUp();

    }


    /*
     * Displays data points as a Bar Chart
     * */
    /*
    private class BarChartVisual : IGraphVisual {

        private RectTransform graphContainer;
        private Color barColor;
        private float barWidthMultiplier;
        private Window_Graph windowGraph;

        public BarChartVisual(RectTransform graphContainer, Color barColor, float barWidthMultiplier, Window_Graph windowGraph) {
            this.graphContainer = graphContainer;
            this.barColor = barColor;
            this.barWidthMultiplier = barWidthMultiplier;
            this.windowGraph = windowGraph;
        }

        public void CleanUp() {
        }

        public IGraphVisualObject CreateGraphVisualObject(Vector2 graphPosition, float graphPositionWidth, string tooltipTMP_Text) {
            GameObject barGameObject = CreateBar(graphPosition, graphPositionWidth);

            BarChartVisualObject barChartVisualObject = new BarChartVisualObject(barGameObject, barWidthMultiplier, windowGraph);
            barChartVisualObject.SetGraphVisualObjectInfo(graphPosition, graphPositionWidth, tooltipTMP_Text);

            return barChartVisualObject;
        }

        private GameObject CreateBar(Vector2 graphPosition, float barWidth) {
            GameObject gameObject = new GameObject("bar", typeof(Image));
            gameObject.transform.SetParent(graphContainer, false);
            gameObject.GetComponent<Image>().color = barColor;
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(graphPosition.x, 0f);
            rectTransform.sizeDelta = new Vector2(barWidth * barWidthMultiplier, graphPosition.y);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            rectTransform.pivot = new Vector2(.5f, 0f);
            
            // Add Button_UI Component which captures UI Mouse Events
            Button_UI barButtonUI = gameObject.AddComponent<Button_UI>();

            return gameObject;
        }


        public class BarChartVisualObject : IGraphVisualObject {

            private GameObject barGameObject;
            private float barWidthMultiplier;
            private Window_Graph windowGraph;

            public BarChartVisualObject(GameObject barGameObject, float barWidthMultiplier, Window_Graph windowGraph) {
                this.barGameObject = barGameObject;
                this.barWidthMultiplier = barWidthMultiplier;
                this.windowGraph = windowGraph;
            }

            public void SetGraphVisualObjectInfo(Vector2 graphPosition, float graphPositionWidth, string tooltipTMP_Text) {
                RectTransform rectTransform = barGameObject.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(graphPosition.x, 0f);
                rectTransform.sizeDelta = new Vector2(graphPositionWidth * barWidthMultiplier, graphPosition.y);

                Button_UI barButtonUI = barGameObject.GetComponent<Button_UI>();

                // Show Tooltip on Mouse Over
                barButtonUI.MouseOverOnceFunc = () => {
                    windowGraph.ShowTooltip(tooltipTMP_Text, graphPosition);
                };

                // Hide Tooltip on Mouse Out
                barButtonUI.MouseOutOnceFunc = () => {
                    windowGraph.HideTooltip();
                };
            }

            public void CleanUp() {
                Destroy(barGameObject);
            }


        }

    }
    */

    /*
     * Displays data points as a Line Graph
     * */
    private class LineGraphVisual : IGraphVisual {

        private RectTransform graphContainer;
        private Sprite dotSprite;
        private LineGraphVisualObject lastLineGraphVisualObject;
        private Color dotColor;
        private Color dotConnectionColor;
        private Window_Graph windowGraph;

        public LineGraphVisual(RectTransform graphContainer, Sprite dotSprite, Color dotColor, Color dotConnectionColor, Window_Graph windowGraph) {
            this.graphContainer = graphContainer;
            this.dotSprite = dotSprite;
            this.dotColor = dotColor;
            this.dotConnectionColor = dotConnectionColor;
            this.windowGraph = windowGraph;
            lastLineGraphVisualObject = null;
        }

        public void CleanUp() {
            lastLineGraphVisualObject = null;
        }


        public IGraphVisualObject CreateGraphVisualObject(Vector2 graphPosition, float graphPositionWidth, string tooltipTMP_Text, int index) {
            
            GameObject dotGameObject = CreateDot(graphPosition, index);

            GameObject labelGameObject = CreateLabel(graphPosition, index);

            GameObject dotConnectionGameObject = null;
            if (lastLineGraphVisualObject != null) {
                dotConnectionGameObject = CreateDotConnection(lastLineGraphVisualObject.GetGraphPosition(), dotGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            
            LineGraphVisualObject lineGraphVisualObject = new LineGraphVisualObject(dotGameObject, dotConnectionGameObject, labelGameObject, lastLineGraphVisualObject, windowGraph);
            lineGraphVisualObject.SetGraphVisualObjectInfo(graphPosition, graphPositionWidth, tooltipTMP_Text);
            
            lastLineGraphVisualObject = lineGraphVisualObject;

            return lineGraphVisualObject;
        }

        private GameObject CreateDot(Vector2 anchoredPosition, int index) {
            GameObject gameObject;
            if(index == windowGraph.valueList.Count - 1)
            {
                gameObject = new GameObject("dot", typeof(Image));
                gameObject.transform.SetParent(graphContainer, false);
                gameObject.GetComponent<Image>().sprite = dotSprite;
                gameObject.GetComponent<Image>().color = dotColor;
            }
            else
            {
                gameObject = new GameObject("dot", typeof(RectTransform));
            }
           
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.sizeDelta = new Vector2(11, 11);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            
            // Add Button_UI Component which captures UI Mouse Events
            Button_UI dotButtonUI = gameObject.AddComponent<Button_UI>();

            return gameObject;
        }

        private GameObject CreateLabel(Vector2 anchoredPosition, int index)
        {
            if (index == windowGraph.valueList.Count - 1)
            {
                RectTransform labelY = Instantiate(windowGraph.labelTemplateGraph);
                labelY.transform.SetParent(graphContainer, false);
                labelY.anchoredPosition = anchoredPosition;
                labelY.gameObject.SetActive(true);
                labelY.GetComponent<TMP_Text>().text = windowGraph.graphLabel;

                return labelY.gameObject;
            }

            return null;
        }

        private GameObject CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB) {
            GameObject gameObject = new GameObject("dotConnection", typeof(Image));
            gameObject.transform.SetParent(graphContainer, false);
            gameObject.GetComponent<Image>().color = dotConnectionColor;
            gameObject.GetComponent<Image>().raycastTarget = false;
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            Vector2 dir = (dotPositionB - dotPositionA).normalized;
            float distance = Vector2.Distance(dotPositionA, dotPositionB);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            rectTransform.sizeDelta = new Vector2(distance, 3f);
            rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
            rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
            return gameObject;
        }




        public class LineGraphVisualObject : IGraphVisualObject {

            public event EventHandler OnChangedGraphVisualObjectInfo;

            private GameObject dotGameObject;
            private GameObject dotConnectionGameObject;
            private GameObject labelGameObject;
            private LineGraphVisualObject lastVisualObject;
            private Window_Graph windowGraph;

            public LineGraphVisualObject(GameObject dotGameObject, GameObject dotConnectionGameObject, GameObject labelGameObject, LineGraphVisualObject lastVisualObject, Window_Graph windowGraph) {
                this.dotGameObject = dotGameObject;
                this.dotConnectionGameObject = dotConnectionGameObject;
                this.lastVisualObject = lastVisualObject;
                this.labelGameObject = labelGameObject;
                this.windowGraph = windowGraph;

                if (lastVisualObject != null) {
                    lastVisualObject.OnChangedGraphVisualObjectInfo += LastVisualObject_OnChangedGraphVisualObjectInfo;
                }
            }

            private void LastVisualObject_OnChangedGraphVisualObjectInfo(object sender, EventArgs e) {
                UpdateDotConnection();
            }

            public void SetGraphVisualObjectInfo(Vector2 graphPosition, float graphPositionWidth, string tooltipTMP_Text) {
                RectTransform rectTransform = dotGameObject.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = graphPosition;

                UpdateDotConnection();

                Button_UI dotButtonUI = dotGameObject.GetComponent<Button_UI>();

                // Show Tooltip on Mouse Over
                dotButtonUI.MouseOverOnceFunc = () => {
                    windowGraph.ShowTooltip(tooltipTMP_Text, graphPosition);
                };
            
                // Hide Tooltip on Mouse Out
                dotButtonUI.MouseOutOnceFunc = () => {
                    windowGraph.HideTooltip();
                };

                if (OnChangedGraphVisualObjectInfo != null) OnChangedGraphVisualObjectInfo(this, EventArgs.Empty);
            }

            public void CleanUp() {
                if(dotGameObject != null)
                Destroy(dotGameObject);
                if(dotConnectionGameObject != null)
                Destroy(dotConnectionGameObject);
                if (labelGameObject != null)
                Destroy(labelGameObject);
            }

            public Vector2 GetGraphPosition() {
                RectTransform rectTransform = dotGameObject.GetComponent<RectTransform>();
                return rectTransform.anchoredPosition;
            }

            private void UpdateDotConnection() {
                if (dotConnectionGameObject != null) {
                    RectTransform dotConnectionRectTransform = dotConnectionGameObject.GetComponent<RectTransform>();
                    Vector2 dir = (lastVisualObject.GetGraphPosition() - GetGraphPosition()).normalized;
                    float distance = Vector2.Distance(GetGraphPosition(), lastVisualObject.GetGraphPosition());
                    dotConnectionRectTransform.sizeDelta = new Vector2(distance, 3f);
                    dotConnectionRectTransform.anchoredPosition = GetGraphPosition() + dir * distance * .5f;
                    dotConnectionRectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
                }
            }

        }

    }

}
