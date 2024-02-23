using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cookies : MonoBehaviour
{
    // cookie script này có thể chính là thằng node vì thằng node sẽ đại diện cho tất cả các loại cookie
    // và cookie chỉ là hình ảnh

    /// <summary>
    ///  script này sẽ phụ trách thay đổi hình ảnh của cookie trong tuturial aka colorGamePiece
    ///     
    /// 
    /// </summary>
    
    private int xPosition;
    private int yPosition;

    public static Cookies Create(Vector2 position, int xCordinate, int yCordinate, List<Transform> posibleCookieTransform )
    {
        CookieTypeListSO cookieTypeList = Resources.Load<CookieTypeListSO>(typeof(CookieTypeListSO).Name);
        Transform cookieTransform = Instantiate(posibleCookieTransform[Random.Range(0, posibleCookieTransform.Count)],
            position, Quaternion.identity) ;
        Cookies cookies = cookieTransform.GetComponent<Cookies>();
        return cookies;
    }


    //public void Init(int _x, int _y, Type _type)
    //{
    //    xPosition = _x;
    //    yPosition = _y;
    //    type = _type;
    //}

    private void SetCordinates(int xPos, int yPos)
    {
        xPosition = xPos;
        yPosition = yPos;
    }
}
