using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDestroyer : MonoBehaviour
{
    public GameObject DestroyModel;

    private void OnCollisionEnter(Collision other)
    {

        if (ChessGameManager.instance.selectedPiece != null)
        {
            Debug.Log(other.gameObject.transform.parent.name);
            if (other.gameObject.transform.parent.GetComponent<ChessPiece>() == ChessGameManager.instance.selectedPiece)
            {
                this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
                Quaternion Quat = this.gameObject.transform.parent.transform.rotation;
                
                Instantiate(DestroyModel, new Vector3(this.transform.position.x,0, this.transform.position.z), Quat);
                StartCoroutine(preEnd());


            }
        }
    }
        
    
    IEnumerator preEnd()
    {
        SkinnedMeshRenderer[] skinnedMeshRenderers = this.gameObject.transform.parent.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer renderer in skinnedMeshRenderers)
        {
            renderer.enabled = false;
        }

        yield return new WaitForSeconds(2f);
        ChessGameManager.instance.end();
        yield return new WaitForSeconds(1.1f);
        if (ChessGameManager.instance.Promocja())
        {
            ChessGameManager.instance.ShowPromoPanel();
        }
        Destroy(this.gameObject);
    }
}
