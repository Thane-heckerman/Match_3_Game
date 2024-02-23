using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Node : MonoBehaviour
{

    // this class is for managing node not cookies aka board
    private int xCordinator;

    private int yCordinator;

    public SpriteRenderer sr;

    private CookieTypeHolder cookieTypeHolder;
    // instantiate piece tại đây

    // giờ k cần cookie type nữa vì tất cả những SO đã tạo ra chỉ là normal type cookie
    private void Awake()
    {
        if (IsColored())
        {
            sr = transform.Find("sprite").GetComponent<SpriteRenderer>();
        }
        cookieTypeHolder = GetComponent<CookieTypeHolder>();
    }

    private void SetCookieType(CookieTypeSO _cookieType)
    {

        cookieTypeHolder.cookieType = _cookieType;
    }

    public CookieTypeSO GetCookieTypeSO()
    {
        CookieTypeSO cookieType = cookieTypeHolder.cookieType;
        return cookieType;
    }

    public void Init(int x, int y, CookieTypeSO _cookieType)
    {
        xCordinator = x;
        yCordinator = y;
        SetCookieType(_cookieType);
    }
    public void SetCordinator(int x,int y)
    {
        xCordinator = x;
        yCordinator = y;
    }

    public Node leftNode()
    {
        if (xCordinator > 0)
        {
            Node left = GridManager.Instance.GetBoard(xCordinator - 1, yCordinator);
            return left;
        }
        else return null;
    }

    public Node rightNode()
    {
        if (xCordinator < GridManager.Instance.gridWidth - 1)
        {
            Node right = GridManager.Instance.GetBoard(xCordinator + 1, yCordinator);
            return right;
        }
        else return null;
    }

    public Node bottomNode()
    {
        if (yCordinator > 0)
        {
            Node bottomNode = GridManager.Instance.GetBoard(xCordinator, yCordinator - 1);
            return bottomNode;
        }
        else return null;
    }

    public Node topNode()
    {
        if (yCordinator < GridManager.Instance.gridHeight - 1)
        {
            Node topNode = GridManager.Instance.GetBoard(xCordinator, yCordinator + 1);
            return topNode;
        }
        else return null;
    }

    public List<Node> GetVerticalNeighborsNodes()
    {
        List<Node> verticalNeighborNodes = new List<Node>();
        verticalNeighborNodes.Add(topNode());
        verticalNeighborNodes.Add(bottomNode());
        return verticalNeighborNodes;
    }

    public List<Node> GetHorizontalNeigborNodes()
    {
        List<Node> horizontalNeighborNodes = new List<Node>();
        horizontalNeighborNodes.Add(leftNode());
        horizontalNeighborNodes.Add(rightNode());
        return horizontalNeighborNodes;
    }

    // naming get confused and call wrong method
    public List<Node> GetAllConnectedNeighborNodes()
    {
        List<Node> horizontalNeighborNodes = GetHorizontalConnectedNodes();
        List<Node> verticalNeighBorNodes = GetVerticalConnectedNodes();
        List<Node> allNeigborNodes = new List<Node>();

        allNeigborNodes.AddRange(horizontalNeighborNodes);
        allNeigborNodes.AddRange(verticalNeighBorNodes);
        allNeigborNodes.RemoveAt(0);
        Debug.Log(allNeigborNodes.Count);
        return allNeigborNodes;
    }

    public List<Node> GetHorizontalConnectedNodes(List<Node> excludes = null)//DFS
    {
        List<Node> result = new List<Node>();
        result.Add(this);

        if (excludes == null)
        {
            excludes = new List<Node> { this, };
        }
        if (excludes != null)
        {
            excludes.Add(this);
        }

        // chỉ node được chọn mới gọi tìm tất cả 
        //foreach (Node neighbor in GetHorizontalNeigborNodes())
        //{
        //    if (excludes.Contains(neighbor) || neighbor == null || neighbor.GetCookieType() != GetCookieType()) continue;
        //    result.AddRange(neighbor.GetHorizontalConnectedNodes(excludes));
        //}

        return result;
    }

    public List<Node> GetVerticalConnectedNodes(List<Node> excludes = null)//DFS
    {
        List<Node> result = new List<Node>();
        result.Add(this);

        if (excludes == null)
        {
            excludes = new List<Node> { this, };
        }
        if (excludes != null)
        {
            excludes.Add(this);
        }

        // chỉ node được chọn mới gọi tìm tất cả 
        //foreach (Node neighbor in GetVerticalNeighborsNodes())
        //{
        //    if (excludes.Contains(neighbor) || neighbor == null || neighbor.GetCookieType() != GetCookieType()) continue;
        //    result.AddRange(neighbor.GetVerticalConnectedNodes(excludes));
        //}

        return result;
    }

    public bool IsMoveable()
    {
        return GetComponent<Moveable>() != null;
    }

    public bool IsColored()
    {
        return GetComponent<ColorTypeCookie>() != null;
    }
}