using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using System.Linq;

public class GridManager : MonoBehaviour
{
    public int gridHeight;
    public int gridWidth;

    private float xSpacing = 2f;
    private float ySpacing = 2f;
    // tồn tại vấn đề vị trí giữa background của node và cookie

    [SerializeField] private GameObject[] tilePrefab;
    [SerializeField] private List<Transform> cookieList;// hard code should get from cookieTypeListSO

    private Node[,] board;

    public GameObject boardGO;

    private List<GameObject> selections;// cookies will access this and get the info for new pos and sprite

    public static GridManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        selections = new List<GameObject>();
        Camera mainCamera = Camera.main;
        InitializeGrid();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D ray = Physics2D.Raycast(UtilsClass.GetMouseWorldPosition(), Vector2.zero);
            if (ray.collider != null )
            {
                Selection(ray);
            }
        }

    }

    private void Selection(RaycastHit2D ray)// asynce before void
    {
        if (!selections.Contains(ray.collider.gameObject)) selections.Add(ray.collider.gameObject);//make sure there 2 different object to be added to the list
        if (selections.Count < 2) return; // make sure there is nothing happens when have 1 object in selections


        Node selectParent = selections[0].transform.parent.GetComponent<Node>();
        Node targetParent = selections[1].transform.parent.GetComponent<Node>();
        if (CanBeSwaped(selectParent,targetParent))
        {
            Swap(selectParent, targetParent);
            List<Node> excludes = new List<Node>();
            List<Node> excludes1 = new List<Node>();

            
            foreach (Node node in targetParent.GetHorizontalConnectedNodes(excludes))
            {
                node.cookies.DOScale(1.25f, 0.5f).Play();
            }
            foreach (Node node in targetParent.GetVerticalConnectedNodes(excludes1))
            {
                node.cookies.DOScale(1.25f, 0.5f).Play();
            }

            Debug.Log("horizontal node " + excludes.Count);
            Debug.Log("vertical node " + excludes1.Count);
        }
        selections.Clear();
    }

    private void InitializeGrid()
    {
        board = new Node[this.gridWidth, this.gridHeight];

        // calculate the offset spacing for balance the grid between the screen game

        xSpacing = (float)(gridWidth - 1) / 2;
        ySpacing = (float)(gridHeight - 1) / 2;

        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                Vector2 position = new Vector2(x - xSpacing, y - ySpacing);
                GameObject nodeGameObject = Instantiate(tilePrefab[Random.Range(0, tilePrefab.Length)], position, Quaternion.identity);
                nodeGameObject.name = x.ToString() + ", " + y.ToString();
                GameObject cookies =  CheckForNearbyNodeCookiesTypeAndCreateCookiePrefab(x, y,position);
                cookies.transform.SetParent(nodeGameObject.transform);
                cookies.transform.Find("sprite").localScale = nodeGameObject.transform.localScale;
                board[x, y] = nodeGameObject.GetComponent<Node>();
                board[x, y].cookies = cookies.transform;
                board[x, y].isUsable = true;
                board[x, y].SetCordinator(x, y);
            }
        }

    }

    

    private GameObject CheckForNearbyNodeCookiesTypeAndCreateCookiePrefab(int x, int y,Vector2 position)
    {
        List<Transform> posibleCookie = new List<Transform>(cookieList);

        if (x >= 2 )
        {
            CookieTypeSO leftNode1Cookie = GetCookieType(board[x - 1, y]);
            CookieTypeSO leftNode2Cookie = GetCookieType(board[x - 2, y]);
            if (leftNode2Cookie != null && leftNode1Cookie == leftNode2Cookie)
            {
                posibleCookie.Remove(leftNode1Cookie.prefab);
            }
            
        }
        if (y >= 2)
        {
            CookieTypeSO downNode1Cookie = GetCookieType(board[x, y - 1]);
            CookieTypeSO downNode2Cookie = GetCookieType(board[x, y - 2]);
            if (downNode2Cookie != null && downNode2Cookie == downNode1Cookie)
            {
                posibleCookie.Remove(downNode1Cookie.prefab);
            }
        }
        GameObject cookie = Cookies.Create(position, x, y,posibleCookie).gameObject;
        return cookie;
    }


    // swap function

    private void Swap(Node selected, Node target)
    {
        float tweenDuration = .25f;

        Transform selectedCookie = selected.cookies;
        Transform targetCookie = target.cookies;

        Sequence sequence = DOTween.Sequence();

        sequence.Join(selected.cookies.transform.DOMove(target.cookies.transform.position, tweenDuration)).
            Join(target.cookies.transform.DOMove(selected.cookies.transform.position, tweenDuration));

        GameObject temp = selected.cookies.gameObject;
        selected.cookies = target.cookies;
        target.cookies = temp.transform;

        selectedCookie.SetParent(target.transform);
        targetCookie.SetParent(selected.transform);

    }

    private bool CanBeSwaped(Node node1, Node node2)
    {
        int x1 = node1.GetXCordinator();
        int y1 = node1.GetYCordinator();

        int x2 = node2.GetXCordinator();
        int y2 = node2.GetYCordinator();

        if (x1 == x2 || y1 == y2) {
            if(node1.cookies.GetComponent<CookieTypeHolder>().cookieType
                != node2.cookies.GetComponent<CookieTypeHolder>().cookieType)
                return true;
        }
        return false;
    }
    // check match function horizontal and vertical


    private CookieTypeSO GetCookieType(Node node)
    {
        CookieTypeSO cookieType = node.cookies.GetComponent<CookieTypeHolder>().cookieType;
        return cookieType;
    }

    public Node GetBoard(int x, int y)
    {
        return board[x,y];
    }

    private void CanPop()
    {

    }

    public enum status
    {
        isChecking,
        isNotChecking
    }
}   
