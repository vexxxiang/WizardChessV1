using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    
    public Vector2Int Position;
    public bool WhiteColor;
    public bool Selected;
    public bool Zajety = false;
    public GameObject Bierki;

    private void Start()
    {

    }
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
            this.GetComponent<MeshRenderer>().enabled = true;
        }
        else 
        {
            if (WhiteColor)
            {
                this.gameObject.GetComponent<Renderer>().material.color = Color.white;
                this.GetComponent<MeshRenderer>().enabled = false;
            }
            else
            {
                this.gameObject.GetComponent<Renderer>().material.color = Color.black;
                this.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }
   
}
