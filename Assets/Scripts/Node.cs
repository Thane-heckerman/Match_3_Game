using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    // this class is for managing node not cookies aka board
    private int xCordinator;

    private int yCordinator;

    public bool isUsable;

    public string title;

    public  Transform cookies;

    public List<Node> findingAxis;

    public int times = 0;
    // constructor
    public Node(bool _isUsable, Transform _cookies)
    {
        isUsable = _isUsable;
        cookies = _cookies;
    }

    public void SetCookie(Transform _cookie)
    {
        cookies = _cookie;
    }

    private CookieTypeSO GetCookieType()
    {
        CookieTypeSO cookieType = cookies.GetComponent<CookieTypeHolder>().cookieType;
        return cookieType;
    }

    public void SetCordinator(int x, int y)
    {
        xCordinator = x;
        yCordinator = y;
    }

    public int GetXCordinator()
    {
        return xCordinator;
    }

    public int GetYCordinator()
    {
        return yCordinator;
    }
    // có thể node sẽ bị null nên thử viết lại set node dưới để tránh trường hợp đó

    //public Node left => xCordinator > 0 ? GridManager.Instance.GetBoard(xCordinator - 1, yCordinator) : null;
    //public Node right => xCordinator < GridManager.Instance.gridWidth - 1 ? GridManager.Instance.GetBoard(xCordinator + 1, yCordinator) : null;
    //public Node top => yCordinator > 0 ? GridManager.Instance.GetBoard(xCordinator, yCordinator - 1) : null; // chưa get it
    //public Node bottom => yCordinator < GridManager.Instance.gridHeight - 1 ? GridManager.Instance.GetBoard(xCordinator, yCordinator + 1) : null;// chưa get it

    public Node leftNode()
    {
        if (xCordinator > 0)
        {
            Node left = GridManager.Instance.GetBoard(xCordinator - 1, yCordinator);
            left.SetNodeTitle("Horizontal");
            return left;
        }
        else return null;
    }

    public Node rightNode()
    {
        if (xCordinator < GridManager.Instance.gridWidth - 1)
        {
            Node right = GridManager.Instance.GetBoard(xCordinator + 1, yCordinator);
            right.SetNodeTitle("Horizontal");
            return right;
        }
        else return null;
    }

    public Node topNode()
    {
        if (yCordinator > 0)
        {
            Node top = GridManager.Instance.GetBoard(xCordinator, yCordinator - 1);
            top.SetNodeTitle("Vertical");
            return top;
        }
        else return null;
    }

    public Node bottomNode()
    {
        if (yCordinator < GridManager.Instance.gridHeight - 1)
        {
            Node bottom = GridManager.Instance.GetBoard(xCordinator, yCordinator + 1);
            bottom.SetNodeTitle("Vertical");
            return bottom;
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
        List <Node> horizontalNeighborNodes = new List<Node>();
        horizontalNeighborNodes.Add(leftNode());
        horizontalNeighborNodes.Add(rightNode());
        return horizontalNeighborNodes;
    }

    public List<Node> GetAllConnectedNeighborNodes()
    {
        List<Node> horizontalNeighborNodes = GetHorizontalNeigborNodes();
        List<Node> verticalNeighBorNodes = GetVerticalNeighborsNodes();
        List<Node> allNeigborNodes = new List<Node>();
        allNeigborNodes.AddRange(horizontalNeighborNodes);
        allNeigborNodes.AddRange(verticalNeighBorNodes);
        return allNeigborNodes;
    }
    private void SetNodeTitle(string axis)
    {
        title = axis;
    }

    private string GetNodeTitle()
    {
        return this.title;
    }

    public List<Node> GetHorizontalConnectedNodes(List<Node> excludes)//DFS
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
        foreach (Node neighbor in GetHorizontalNeigborNodes())
        {
            if (excludes.Contains(neighbor) || neighbor == null || neighbor.GetCookieType() != GetCookieType()) continue;
            result.AddRange(neighbor.GetHorizontalConnectedNodes(excludes));
        }

        return result;
    }

    public List<Node> GetVerticalConnectedNodes(List<Node> excludes)//DFS
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
        foreach (Node neighbor in GetVerticalNeighborsNodes())
        {
            if (excludes.Contains(neighbor) || neighbor == null || neighbor.GetCookieType() != GetCookieType()) continue;
            result.AddRange(neighbor.GetVerticalConnectedNodes(excludes));
        }

        return result;
    }

    // function set title for next node when call get next node

    // when count > 0 check title of node and check connected node based on title (left and right => horizontal, top
    // and bottom => vertical , first node title will be null => so it will check for both horizontal and vertical
    // set all title in exclude to clear

}
