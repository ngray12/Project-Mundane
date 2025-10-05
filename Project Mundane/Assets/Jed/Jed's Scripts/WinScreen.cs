using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
    // Start is called before the first frame update

    public void MainMenu() 
    {
        Debug.Log("Load Main"); 
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
