using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionScript : MonoBehaviour
{
    public bool fightMode = true;
    public GameObject weapon;
    public MeshCollider meshCollider;

    public void Start()
    {
        meshCollider = weapon.GetComponent<MeshCollider>();
        SwichMeshCollider();
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
