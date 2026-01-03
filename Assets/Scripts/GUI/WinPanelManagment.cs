using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPanelManagment : MonoBehaviour
{
    public GameObject WhiteSprite, BlackSprite, PatSprite;
    public void whiteWin()
    {
        this.gameObject.SetActive(true);
        BlackSprite.SetActive(false);
        PatSprite.SetActive(false);
        WhiteSprite.SetActive(true);
    }

    public void blackWin()
    {
        this.gameObject.SetActive(true);
        WhiteSprite.SetActive(false);
        PatSprite.SetActive(false);
        BlackSprite.SetActive(true);
    }

    public void pat()
    {
        this.gameObject.SetActive(true);
        WhiteSprite.SetActive(false);
        BlackSprite.SetActive(false);
        PatSprite.SetActive(true);
    }
}
