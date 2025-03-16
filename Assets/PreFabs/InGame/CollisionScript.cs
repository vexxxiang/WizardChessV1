using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionScript : MonoBehaviour
{
    public bool fightMode = false;
    public GameObject weapon;
    public GameObject capsule;
    public MeshCollider meshCollider;

    public void Start()
    {
        meshCollider = weapon.GetComponent<MeshCollider>();
        //SwichMeshCollider();
    }
    public void SwichMeshColliderCapsule()
    {
        capsule.GetComponent<CapsuleCollider>().enabled = true ;
    }
    public void SwichMeshCollider()
    {
        
        if (fightMode)
        {
            fightMode = false;
            meshCollider.enabled = false;
        }
        else
        {
            fightMode = true;
            meshCollider.enabled = true;
        }
    }
}
