using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{

    public Vector2Int Position;
    public bool WhiteColor;
    public bool Selected;
    public bool Zajety = false;
    GameObject Bierki;

    public void PreRefresh(float czas)
    {
        Invoke("Refresh", czas);
    }
    public void Refresh() 
    {
        
        //Debug.Log("Refersh()" + Position);
        if (Selected == true)
        {
            this.gameObject.GetComponent<Renderer>().material.color = Color.green;
        }
        else 
        {
            if (WhiteColor)
            {
                this.gameObject.GetComponent<Renderer>().material.color = Color.white;
            }
            else
            {
                this.gameObject.GetComponent<Renderer>().material.color = Color.black;
            }
        }
    }
    public void UpdateState()
    {
        Bierki = GameObject.FindGameObjectWithTag("Bierki");
        foreach (ChessPiece i in Bierki.transform.GetComponentsInChildren<ChessPiece>())
        {
            if (i.boardPosition == Position)
            {
                Zajety = true;
                break;
            }
            else
            {
                Zajety = false;
            }
        }
    }
}
