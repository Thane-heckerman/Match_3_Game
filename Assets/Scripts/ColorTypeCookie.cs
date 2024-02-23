using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTypeCookie : MonoBehaviour
{
    private Sprite cookieSprite;

    private CookieTypeSO cookieType;

    private SpriteRenderer sR;

    private void Awake()
    {
        sR = transform.Find("sprite").GetComponent<SpriteRenderer>();

        cookieType = GetComponent<CookieTypeHolder>().cookieType;
    }
    private void Start()
    {
        
    }

    public void setCookieSprite()
    {
        sR.sprite = cookieType.spriteList[Random.Range(0, cookieType.spriteList.Count)];
        cookieSprite = sR.sprite;
    }

    public Sprite GetCookieSprite()
    {
        return cookieSprite;
    }
}
