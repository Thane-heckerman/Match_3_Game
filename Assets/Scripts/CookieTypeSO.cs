using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CookieTypeSO")]
public class CookieTypeSO : ScriptableObject
{
    public GridManager.cookieType type;
    public List<Sprite> spriteList;
    public Transform prefab;
}
