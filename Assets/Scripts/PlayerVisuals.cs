using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    [SerializeField] private MeshRenderer headMeshRender;
    [SerializeField] private MeshRenderer bodyMeshRender;

    private Material material;

    private void Awake()
    {
        material = headMeshRender.material;
        headMeshRender.material = material;
        bodyMeshRender.material = material;
    }
    public void SetColor(Color color)
    {
        material.color = color;
    }

}
