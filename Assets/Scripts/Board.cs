using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {
    public static int radius = 1;
    public int gap = 50;//90 and 100
    public int lineWidth = 2;
    public int lineNum = 9;
    public Vector2 upperLeft = new Vector2(-1, 1) * radius;
    public Vector2 upperRight = new Vector2(1, 1) * radius;
    public Vector2 lowerLeft = new Vector2(-1, -1) * radius;
    public Vector2 lowerRight = new Vector2(1, -1) * radius;

    public GameObject linePrefab;
    public Transform lines;


    private Vector2[] posData;

    private Dictionary<int, int> linesData = new Dictionary<int, int>();

    // Use this for initialization
    void Start () {
        GenerateLinesData(lineNum);
        GenerateBoard();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void GenerateLinesData(int n)
    {
        if (n == 9)
            gap = 100;
        else if (n == 11)
            gap = 90;
        else
            gap = 50;

        for (int i = 0; i <= n / 2; ++i)
        {
            linesData.Add(i, i * gap);
            if(i > 0)
            {
                linesData.Add(-i, -i * gap);
            }
        }
    }

    void GenerateBoard()
    {
        foreach (KeyValuePair<int,int> kvp in linesData)
        {
            GameObject line = GameObject.Instantiate(linePrefab) as GameObject;
            line.transform.parent = lines;
            line.name = "lineH";
            line.transform.localScale = Vector3.one;
            RectTransform lineTrans = line.GetComponent<RectTransform>();
            lineTrans.anchoredPosition = new Vector2(0, kvp.Value);
            lineTrans.sizeDelta = new Vector2((lineNum - 1) * gap, lineWidth);

            GameObject lineV = GameObject.Instantiate(linePrefab) as GameObject;
            lineV.transform.parent = lines;
            lineV.name = "lineV";
            lineV.transform.localScale = Vector3.one;
            RectTransform lineVTrans = lineV.GetComponent<RectTransform>();
            lineVTrans.anchoredPosition = new Vector2(kvp.Value, 0);
            lineVTrans.sizeDelta = new Vector2(lineWidth, (lineNum - 1) * gap);
        }
    }
}
