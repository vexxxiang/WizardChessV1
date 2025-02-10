using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rendering : MonoBehaviour
{
    SkinnedMeshRenderer skinnedMesh;
    MeshCollider meshCollider;
    Mesh bakedMesh;


    void Start()
    {
        skinnedMesh = GetComponent<SkinnedMeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        bakedMesh = new Mesh();
        
    }

    void Update()
    {
        skinnedMesh.BakeMesh(bakedMesh);
        meshCollider.sharedMesh = bakedMesh;
    }
  
}

