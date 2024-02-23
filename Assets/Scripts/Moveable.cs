using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Moveable : MonoBehaviour
{

    private Node node;

    private void Awake()
    {
        node = GetComponent<Node>();

    }

    //gọi move xong sẽ set x và y của node luôn
    public void Move(int _x, int _y)
    {
        node.SetCordinator(_x, _y);
        gameObject.transform.localPosition = GridManager.Instance.GetWorldPos(_x,_y);
    }
}
