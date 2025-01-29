using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using static Window_Graph;
using System;

public class GraphUpdater : MonoBehaviour
{
    [SerializeField] private int graphLength = 5;
    [SerializeField] private Window_Graph xenonGraph;
    [SerializeField] private Window_Graph rodsGraph;
    [SerializeField] private Window_Graph waterGraph;
    [SerializeField] private FloatVariable waterRunning;
    [SerializeField] private MainDisplayVariablesHandler simVariables;
    [HideInInspector] public List<TMP_Text> timeStampsList = new List<TMP_Text>();
    [SerializeField] private ChangeRodStruct rodSliders;
    [SerializeField] private FloatVariable rodInsertionRate;

    [SerializeField] private float cd = 5f;
    private float t = 0f;

    private Queue<string> timeStampQueue = new Queue<string>();
    private Queue<int> waterPcts = new Queue<int>();
    private Queue<int> rodsPcts = new Queue<int>();
    private Queue<int> xenonPcts = new Queue<int>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < graphLength; i++) 
        {
            rodsPcts.Enqueue(0);
            xenonPcts.Enqueue(0);
            waterPcts.Enqueue(100);
            timeStampQueue.Enqueue(DateTime.Now.ToString("HH:mm:ss"));
        }
        xenonGraph.ShowGraph(xenonPcts.ToList());
        rodsGraph.ShowGraph(rodsPcts.ToList());
        waterGraph.ShowGraph(waterPcts.ToList());
        UpdateTimestamps();
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;

        if (t > cd) 
        {
            t = 0;

            timeStampQueue.Dequeue();
            timeStampQueue.Enqueue(DateTime.Now.ToString("HH:mm:ss"));          

            waterPcts.Dequeue();
            waterPcts.Enqueue(Mathf.RoundToInt(waterRunning.value));

            xenonPcts.Dequeue();
            xenonPcts.Enqueue(Mathf.RoundToInt(simVariables.Xenon() * 100));

            rodsPcts.Dequeue();
            rodsPcts.Enqueue(Mathf.RoundToInt(rodInsertionRate.value * 100));
            rodSliders.SetSliders(1 - rodInsertionRate.value); //invert due to how sliders are set

            xenonGraph.ShowGraph(xenonPcts.ToList());
            rodsGraph.ShowGraph(rodsPcts.ToList());
            waterGraph.ShowGraph(waterPcts.ToList());
            UpdateTimestamps();
        }
    }

    private void UpdateTimestamps()
    {
        for (int i = 0; i < timeStampsList.Count; i++)
        {
            timeStampsList[i].text = timeStampQueue.ElementAt(i);
        }
    }
}
