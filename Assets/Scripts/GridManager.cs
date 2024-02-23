using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using System.Linq;
using System;
using System.Threading.Tasks;

public class GridManager : MonoBehaviour
{
    public int gridHeight;
    public int gridWidth;

    private float xSpacing ;
    private float ySpacing ;

    // tồn tại vấn đề vị trí giữa background của node và cookie
    public enum cookieType
    {
        NORMAL,
        EMPTY,
    };

    [System.Serializable]
    public struct CookieType
    {
        public cookieType type;
        public CookieTypeSO cookieTypeSO;
        public Transform prefab;
    };

    public CookieType[] cookieTypesStruct;

    private Dictionary<cookieType, CookieTypeSO> cookieTypeListDictionary;
    
    [SerializeField] private CookieTypeListSO cookieTypeList;

    [SerializeField] private List<Transform> backGroundPrefab;// hard code should get from cookieTypeListSO

    private Node[,] nodes;

    public GameObject boardGO;

    private List<GameObject> selections;// cookies will access this and get the info for new pos and sprite

    public static GridManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        xSpacing = (float)(gridWidth - 1) / 2;
        ySpacing = (float)(gridHeight - 1) / 2;

        cookieTypeListDictionary = new Dictionary<cookieType, CookieTypeSO>();
        for (int i = 0; i < cookieTypeList.cookieTypeList.Count(); i++)
        {
            if (!cookieTypeListDictionary.ContainsKey(cookieTypeList.cookieTypeList[i].type))
            {
                cookieTypeListDictionary.Add(cookieTypeList.cookieTypeList[i].type, cookieTypeList.cookieTypeList[i]);
            }
        }
        selections = new List<GameObject>();
        Camera mainCamera = Camera.main;
        nodes = new Node[this.gridWidth, this.gridHeight];
        InitializeGrid();
    }

    private void Update()
    {
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        RaycastHit2D ray = Physics2D.Raycast(UtilsClass.GetMouseWorldPosition(), Vector2.zero);
        //        if (ray.collider != null )
        //        {
        //            //Selection(ray);

        //        }
        //    }

    }

    //private void Selection(RaycastHit2D ray)// asynce before void
    //{
    //    if (!selections.Contains(ray.collider.gameObject)) selections.Add(ray.collider.gameObject);//make sure there 2 different object to be added to the list
    //    if (selections.Count < 2) return; // make sure there is nothing happens when have 1 object in selections


    //    if (CanBeSwaped(selections[0].transform.parent.GetComponent<Node>(), selections[1].transform.parent.GetComponent<Node>()))
    //    {
    //        Swap(selections[0].transform.parent.GetComponent<Node>(), selections[1].transform.parent.GetComponent<Node>());
    //        Node selectParent = selections[0].transform.parent.GetComponent<Node>();
    //        Node targetParent = selections[1].transform.parent.GetComponent<Node>();
    //    }
    //    else
    //    {
    //        Swap(selections[0].transform.parent.GetComponent<Node>(), selections[1].transform.parent.GetComponent<Node>());
    //    }
    //    selections.Clear();
    //}

    private void InitializeGrid()
    {
        

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                //Vector2 position = GetWorldPos(x ,y );
                Vector2 position = new Vector2(x - xSpacing, y - ySpacing);
                GameObject backGroundTransform = Instantiate(backGroundPrefab[0].gameObject, position, Quaternion.identity);
                backGroundTransform.transform.parent = this.transform;
                SpawnNewNode(x, y,position, cookieType.EMPTY);
            }

        }
        Fill();
        //for (int x = 0; x < gridWidth; x++)
        //{
        //    for (int y = 0; x < gridHeight; y++)
        //    {
                
        //    }
        //}
        // instantiate board

        
    }

    public Vector2 GetWorldPos(float x, float y)
    {
        return new Vector3(x - xSpacing, y - ySpacing);
    }

    public Node SpawnNewNode(int x, int y,Vector2 position, cookieType type)
    {
        // gọi instantiate trong node hoặc tại grid
        GameObject pieceNodeGameObject = Instantiate(cookieTypeListDictionary[type].prefab, position, Quaternion.identity).gameObject;
        pieceNodeGameObject.name = "Piece(" + x + "," + y + ")";
        pieceNodeGameObject.transform.parent = transform;
        
        nodes[x, y] = pieceNodeGameObject.GetComponent<Node>();
        CookieTypeSO cookieTypeSO = nodes[x, y].GetComponent<CookieTypeHolder>().cookieType;

        nodes[x, y].Init(x, y, cookieTypeSO);
        //set color when normal type
        if (nodes[x, y].IsColored())
        {
            pieceNodeGameObject.GetComponent<ColorTypeCookie>().setCookieSprite();
        }
        return nodes[x, y];
    }


    public void Fill()
    {
        while (FillStep())
        {

        }
    }

    public bool FillStep()
    {
        bool movedPiece = false;
        
        for(int y = gridHeight - 2; y >= 0; y--)
        {
            for(int x = 0; x < gridWidth; x++)
            {
                Debug.Log("(" + x + "," + y + ")");
                Node node = nodes[x, y];
                if (node.IsMoveable())
                {
                    Node belowNode = nodes[x, y + 1];
                    if (belowNode.GetCookieTypeSO() == cookieTypeListDictionary[cookieType.EMPTY])
                    {
                        node.GetComponent<Moveable>().Move(x, y +1);
                        nodes[x, y + 1] = node;
                        SpawnNewNode(x, y,GetWorldPos(x,y), cookieType.EMPTY);
                        movedPiece = true;
                    }
                }
            }
        }

        for (int x = 0; x< gridWidth; x++)
        {
            Node belowNode = nodes[x, 0];
            if(belowNode.GetCookieTypeSO() == cookieTypeListDictionary[cookieType.EMPTY])
            {
                Vector3 position = GetWorldPos(x, -1);
                GameObject pieceNodeGameObject = Instantiate(cookieTypeListDictionary[cookieType.NORMAL].prefab, position, Quaternion.identity).gameObject;
                pieceNodeGameObject.transform.parent = transform;
                nodes[x, 0] = pieceNodeGameObject.GetComponent<Node>();
                nodes[x, 0].Init(x, -1, cookieTypeListDictionary[cookieType.NORMAL]);
                nodes[x, 0].GetComponent<Moveable>().Move(x, 0);
                nodes[x, 0].GetComponent<ColorTypeCookie>().setCookieSprite();
                movedPiece = true;
            }
        }

        return movedPiece;
    }
    // swap function

    //private void Swap(Node selected, Node target)
    //{
    //    Node tempNode = selected;
    //    selected = target;
    //    target = tempNode;

    //    float tweenDuration = .25f;

    //    Transform selectedCookie = selected.cookies;
    //    Transform targetCookie = target.cookies;

    //    Sequence sequence = DOTween.Sequence();

    //    sequence.Join(selected.cookies.transform.DOMove(target.cookies.transform.position, tweenDuration)).
    //        Join(target.cookies.transform.DOMove(selected.cookies.transform.position, tweenDuration));

    //    sequence.Play();
                        
    //    GameObject temp = selected.cookies.gameObject;
    //    selected.cookies = target.cookies;
    //    target.cookies = temp.transform;

    //    selectedCookie.SetParent(target.transform);
    //    targetCookie.SetParent(selected.transform);


    //}

    public Node GetBoard(int x, int y)
    {
        return nodes[x,y];
    }

    private bool CanPop()
    {
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x< gridWidth; x++)
            {
                int verticalCount = nodes[x, y].GetVerticalConnectedNodes().Count;
                int horizontalCount = nodes[x, y].GetHorizontalConnectedNodes().Count;
                if (verticalCount >= 3)
                {
                   
                    return true;
                }
                if (horizontalCount >= 3)
                {
                    
                    return true;
                }
                //if (board[x, y].GetVerticalConnectedNodes().Count >= 3 && board[x, y].GetHorizontalConnectedNodes().Count >= 3)
                //{
                //    int result = board[x, y].GetVerticalConnectedNodes().Count + board[x, y].GetHorizontalConnectedNodes().Count;
                //    if (result >= 5) return true;
                //}
            }
        }

        return false;
    }


    //private void Pop()
    //{
    //    for (int y = 0; y < gridHeight; y++)
    //    {
    //        for (int x = 0; x < gridWidth; x++)
    //        {
    //            Node node = node[x, y];
    //            List<Node> horizontalNode = node.GetHorizontalConnectedNodes();
    //            List<Node> verticalNode = node.GetVerticalConnectedNodes();

    //            if (horizontalNode.Count < 3) continue;

    //            if (verticalNode.Count < 3) continue;

                

                //        Sequence refill = DOTween.Sequence();

                //        foreach (Node  connectedNode in connectedNodes)
                //        {
                //            connectedNode.cookies = cookieList[UnityEngine.Random.Range(0, cookieList.Count)];

                //            refill.Join(connectedNode.cookies.DOScale(Vector3.one, 0.25f));
                //        }


                //    }
    //            //}
    //        }
    //    }
    //}
}   
