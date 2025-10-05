using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class main : MonoBehaviour
{

    public enum Region { Top, Bottom, Left, Right }
    public Region region;

    void OnMouseDown()
    {
        switch (region)
        {
            case Region.Top:
                TopClicked();
                break;
            case Region.Bottom:
                BottomClicked();
                break;
            case Region.Left:
                LeftClicked();
                break;
            case Region.Right:
                RightClicked();
                break;
        }
    }

    void TopClicked() { SceneManager.LoadScene("Circle"); }
    void BottomClicked() { SceneManager.LoadScene("Square Scene"); }
    void LeftClicked() { SceneManager.LoadScene("Star Scene"); }
    void RightClicked() { SceneManager.LoadScene("Triangle"); }
}
