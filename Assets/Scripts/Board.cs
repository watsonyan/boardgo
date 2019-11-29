using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using game.logic;
using Action = game.logic.Action;
using UnityEngine.EventSystems;

public class Board : MonoBehaviour, IPointerClickHandler {
    //public static int radius = 1;
    public Camera UICamera;
    public int gap = 50;//90 and 100
    public int dotSize = 20;//20 30 40
    public int lineWidth = 2;
    public int size = 9;
    public Vector2 lowerLeft = Vector2.zero;
    //public Vector2 upperLeft = new Vector2(-1, 1) * radius;
    //public Vector2 upperRight = new Vector2(1, 1) * radius;
    //public Vector2 lowerLeft = new Vector2(-1, -1) * radius;
    //public Vector2 lowerRight = new Vector2(1, -1) * radius;

    public GameObject linePrefab;
    public GameObject blackStone;
    public GameObject whiteStone;

    public Transform lines;
    public Transform stones;

    SGFTree Tree = null;


    private Field[,] fields;

    private int nextColor;//next turn

    private Dictionary<int, int> linesData = new Dictionary<int, int>();

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnGUI()
    {
        if (GUI.Button(new Rect(100,100,100,60), "load"))
        {
            Load(Application.dataPath + "/Res/sgf/001.sgf");
        }


        if (GUI.Button(new Rect(100, 300, 100, 60), "load"))
        {
            StartNewGame(size);
        }
    }

    void StartNewGame(int size)
    {
        this.size = size;

        Tree = new SGFTree(new TreeNode(1));
        SetInfomation();

        SetupGame(Tree.Root);
        //GenerateBoard(size);
    }

    void SetInfomation()
    {
        if (Tree == null) return;
        Tree.Root.SetAction("AP", "WeGo", true);
        Tree.Root.SetAction("SZ", "" + this.size, true);
        Tree.Root.SetAction("GM", "1", true);
        Tree.Root.SetAction("FF", "4", true);


        Debug.Log(SGFTree.GetSgfString(Tree.Root));
    }

    void GenerateBoard(int size)
    {
        SetInfomation();
        GenerateLinesData(size);
        GenerateBoard();
    }

    void GenerateLinesData(int n)
    {
        if (n == 9)
        {
            gap = 100;
            dotSize = 40;
        }
        else if (n == 13)
        {
            gap = 80;
            dotSize = 30;
        }
        else
        {
            gap = 50;
            dotSize = 20;
        }
        linesData.Clear();
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
        ClearBoard();
        foreach (KeyValuePair<int,int> kvp in linesData)
        {
            GameObject line = GameObject.Instantiate(linePrefab) as GameObject;
            line.transform.parent = lines;
            line.name = "lineH";
            line.transform.localScale = Vector3.one;
            line.SetActive(true);
            RectTransform lineTrans = line.GetComponent<RectTransform>();
            lineTrans.anchoredPosition3D = new Vector3(0, kvp.Value, -1);
            lineTrans.sizeDelta = new Vector2((size - 1) * gap, lineWidth);            

            GameObject lineV = GameObject.Instantiate(linePrefab) as GameObject;
            lineV.transform.parent = lines;
            lineV.name = "lineV";
            lineV.transform.localScale = Vector3.one;
            lineV.SetActive(true);
            RectTransform lineVTrans = lineV.GetComponent<RectTransform>();
            lineVTrans.anchoredPosition3D = new Vector3(kvp.Value, 0, -1);
            lineVTrans.sizeDelta = new Vector2(lineWidth, (size - 1) * gap);
        }
        lowerLeft = Vector2.one * -1 * (size - 1) * gap / 2;
        InitField();
    }

    void InitField()
    {
        fields = new Field[size,size];
        for (int i = 0; i < size; ++i)
        {
            for (int j = 0; j < size; ++j)
            {
                fields[i, j] = new Field();
            }
        }
    }

    void ClearBoard()
    {
        //delete all stones on the board
        for (int i = stones.childCount - 1; i >= 0; --i)
        {
            Transform trans = stones.GetChild(i);
            Destroy(trans.gameObject);
        }
        //delete all lines of the board
        for (int i = lines.childCount - 1; i >= 0; --i)
        {
            Transform trans = lines.GetChild(i);
            Destroy(trans.gameObject);
        }
    }

    Vector2 GetPosition(int i, int j)
    {
        Vector2 result = Vector2.zero;
        result = lowerLeft + new Vector2(i, j) * gap;
        return result;
    }

    GameObject GenerateStone(int i, int j, int color)
    {
        if (i < 1 || i > size || j < 1 || j > size) return null;
        Vector2 pos = GetPosition(i, j);
        GameObject prefab = whiteStone;
        if (color == 1)
        {
            prefab = blackStone;
        }
        GameObject result = GameObject.Instantiate(prefab) as GameObject;
        result.transform.parent = stones;
        result.name = "stone_" + i + "_" + j;
        result.transform.localScale = Vector3.one;
        result.SetActive(true);
        RectTransform stoneTrans = result.GetComponent<RectTransform>();
        stoneTrans.anchoredPosition3D = new Vector3(pos.x, pos.y, -2);
        stoneTrans.sizeDelta = Vector2.one * gap;
        Color(i, j, color);
        return result;
    }

    void Load(string filePath)
    {
        try
        {
            // Create an instance of StreamReader to read from a file.
            // The using statement also closes the StreamReader.
            using (StreamReader sr = new StreamReader(filePath, System.Text.Encoding.GetEncoding("gb2312"), true))
            {
                List<SGFTree> trees = SGFTree.Load(sr);
                Tree = trees[0];
                SetupGame(Tree.Root);
            }
        }
        catch (Exception e)
        {
            // Let the user know what went wrong.
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
        }
    }

    void SetupGame(TreeNode tree)
    {
        LinkedListNode<Action> node = tree.First;
        while (node != null)
        {
            if (node.Value.Type.Equals("SZ"))
            {
                if (tree.Parent == null)
                {
                    if(int.TryParse(node.Value.Arg, out size))
                    {
                        Debug.Log(size);
                        break;
                    }
                }
            }
            node = node.Next;
        }
        GenerateBoard(size);
        node = tree.First;
        Action action = null;
        while (node != null)
        {
            action = node.Value;
            if (action.Type.Equals("B"))
            {
                //setaction(n, a, 1);
            }
            else if (action.Type.Equals("W"))
            {
                //setaction(n, a, -1);
            }
            if (action.Type.Equals("AB"))
            {
                SetAction(tree, action, 1);
            }
            if (action.Type.Equals("AW"))
            {
                SetAction(tree, action, -1);
            }
            else if (action.Type.Equals("AE"))
            {
                //emptyaction(n, a);
            }
            node = node.Next;
        }
    }

    void SetNode(TreeNode tree)
    {
        LinkedListNode<Action> node = tree.First;
        while (node != null)
        {
            if (node.Value.Type.Equals("SZ"))
            {
                if (tree.Parent == null)
                {
                    //int num = 9;
                    if (int.TryParse(node.Value.Arg, out size))
                    {
                        Debug.Log(size);
                        GenerateBoard(size);
                        break;
                    }
                }
            }
            node = node.Next;
        }
        node = tree.First;
        Action action = null;
        while (node != null)
        {
            action = node.Value;
            if (action.Type.Equals("B"))
            {
                //setaction(n, a, 1);
            }
            else if (action.Type.Equals("W"))
            {
                //setaction(n, a, -1);
            }
            if (action.Type.Equals("AB"))
            {
                //placeaction(n, a, 1);
            }
            if (action.Type.Equals("AW"))
            {
                //placeaction(n, a, -1);
            }
            else if (action.Type.Equals("AE"))
            {
                //emptyaction(n, a);
            }
            node = node.Next;
        }
    }

    public void SetAction(TreeNode node, Action action, int c)
    // interpret a set move action, update the last move marker,
    // c being the color of the move.
    {
        int i, j;
        LinkedListNode<string> arg = action.Args;
        while (arg != null)
        {
            String s = arg.Value;
            i = Field.i(s);
            j = Field.j(s);
            if (Valid(i, j))
            {
                GenerateStone(i, j, c);
                //n.addchange(new Change(i, j, P.color(i, j), P.number(i, j)));
                //P.color(i, j, c);
                //update(i, j);
            }
            arg = arg.Next;
        }
    }

    bool Valid(int i, int j)
    {
        return i >= 0 && i < size && j >= 0 && j < size;
    }


    public void Color(int i, int j, int color)
    {
        fields[i, j].Color = color;
    }

    public int GetColor(int i, int j)
    {
        return fields[i, j].Color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Screen.width
        Debug.Log(Input.mousePosition);
        Debug.Log(UICamera.ScreenToWorldPoint(Input.mousePosition));
        Debug.Log(UICamera.WorldToScreenPoint(Input.mousePosition));
    }

    public int GetIndex(float p)
    {
        int rlt = 0;

        return rlt;
    }
}
