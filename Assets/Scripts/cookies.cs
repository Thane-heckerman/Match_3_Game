using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cookies : MonoBehaviour
{
    private int xPosition;
    private int yPosition;

    public static Cookies Create(Vector2 position, int xCordinate, int yCordinate, List<Transform> posibleCookieTransform)
    {
        CookieTypeListSO cookieTypeList = Resources.Load<CookieTypeListSO>(typeof(CookieTypeListSO).Name);
        Transform cookieTransform = Instantiate(posibleCookieTransform[Random.Range(0, posibleCookieTransform.Count)],
            position, Quaternion.identity) ;
        Cookies cookies = cookieTransform.GetComponent<Cookies>();
        cookies.SetCordinates(xCordinate, yCordinate);
        return cookies;
    }

    private void SetCordinates(int xPos, int yPos)
    {
        xPosition = xPos;
        yPosition = yPos;
    }
}
