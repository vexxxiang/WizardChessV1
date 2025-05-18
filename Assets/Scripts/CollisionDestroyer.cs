using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDestroyer : MonoBehaviour
{
    public GameObject DestroyModel;
    public GameObject DestroyObject;
    
    public float TRPawn = 1f, TRRook = 0.7f, TRBishop = 2f, TRKnight = 1.5f, TRQueen = 1.16f, TRKing = 0.8f;
    private float PreDelay = 0.05f;


    private void OnCollisionEnter(Collision other)
    {
        Break(other.gameObject);
    }
    private void Update()
    {
        if (ChessGameManager.instance.time >= 3f && ChessGameManager.instance.targetFigure != null)
        {
            Break(ChessGameManager.instance.targetFigure.gameObject);
        }
    }

    public void Break(GameObject other) {

        

        float WaitingTime = 0;
        if (ChessGameManager.instance.selectedPiece != null)
        {

            var objectTag = ChessGameManager.instance.selectedPiece.tag;
            if (objectTag == "Pawn")
            {
                WaitingTime = TRPawn - PreDelay;
            }
            else if (objectTag == "Rook")
            {
                WaitingTime = TRRook - PreDelay;
            }
            else if (objectTag == "Bishop")
            {
                WaitingTime = TRBishop - PreDelay;
            }
            else if (objectTag == "Knight")
            {
                WaitingTime = TRKnight - PreDelay;
            }
            else if (objectTag == "Queen")
            {
                WaitingTime = TRQueen - PreDelay;
            }
            else if (objectTag == "King")
            {
                WaitingTime = TRKing - PreDelay;
            }

        }

        if (ChessGameManager.instance.selectedPiece != this.gameObject.transform.parent && ChessGameManager.instance.selectedPiece != null && ChessGameManager.instance.time >= WaitingTime)
        {
            ChessGameManager.instance.TimerOn = false;
            ChessGameManager.instance.time = 0;
            Debug.Log(other.gameObject.name);
           
            if (other.gameObject.transform.parent.GetComponent<ChessPiece>() == ChessGameManager.instance.selectedPiece)
            {
               
                this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
                
                Quaternion Quat = this.gameObject.transform.parent.transform.rotation;
                Debug.Log("w ifie" + other.name);
                DestroyObject = Instantiate(DestroyModel, new Vector3(this.transform.position.x, 0, this.transform.position.z), Quat);
                StartCoroutine(preEnd());


            }
            else if (other.gameObject.GetComponent<ChessPiece>() == ChessGameManager.instance.selectedPiece)
            {

                this.gameObject.GetComponent<CapsuleCollider>().enabled = false;

                Quaternion Quat = this.gameObject.transform.rotation;
                Debug.Log("w ifie" + other.name);
                DestroyObject = Instantiate(DestroyModel, new Vector3(this.transform.position.x, 0, this.transform.position.z), Quat);
                StartCoroutine(preEnd());


            }
        }
            
        
    }
    IEnumerator preEnd()
    { 
        
        Renderer[] skinnedMeshRenderersTarget = ChessGameManager.instance.targetFigure.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in skinnedMeshRenderersTarget)
        {
            renderer.enabled = false;

        }

        yield return new WaitForSeconds(0.3f);
        Renderer[] skinnedMeshRenderers = this.gameObject.transform.parent.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in skinnedMeshRenderers)
        {
            renderer.enabled = false;
            
        }
       
        Debug.Log("1");
        
        yield return new WaitForSeconds(2f);
        ChessGameManager.instance.end();
        Destroy(ChessGameManager.instance.targetFigure.gameObject);
        Destroy(DestroyObject);
        yield return new WaitForSeconds(1.1f);
        Debug.Log("2");
        if (ChessGameManager.instance.Promocja())
        {
            ChessGameManager.instance.ShowPromoPanel();
        }
        Destroy(this.gameObject);
        
        
      
    }
}
