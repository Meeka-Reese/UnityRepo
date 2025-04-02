using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(MeshFilter))]
public class MeshGen : MonoBehaviour
{
    private Mesh mesh;
    private MeshFilter meshFilter;
    private List<Vector3> vertices;
    private List<int> triangles;
    private int sizex = 500;
    private int sizey = 50;
    private float xScale = 15f;
    private float yScale = 15f;
    private bool debug = false;
   
    void Start()
    {
        vertices = new List<Vector3>();
        triangles = new List<int>();
        mesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
        
        
        
    }

    private void Update()
    {
        CreateMesh();
        ApplyMesh();
    }

    void CreateMesh()
    {
        
        vertices.Clear();
        triangles.Clear();
        
        for (int x = 0; x < sizex; x++)
        {
            for (int y = 0; y < sizey; y++)
            {
                float Scaledx = ((float)x/(sizex - 1)) * xScale;
                float Scaledy = ((float)y/(sizey- 1)) * yScale;
                if (!debug)
                {
                    Debug.Log("x" + x + Scaledx + "y" + y + Scaledy);
                }
                float zcord = Mathf.Sin(((float)y) + Time.realtimeSinceStartup);
                
                vertices.Add(new Vector3(Scaledx, Scaledy, zcord));
                
            }
        }
        debug = true;

        for (int row = 0; row < sizey - 1; row++)
        {
            for (int col = 0; col < sizex - 1; col++)
            {
                int current = row * sizex + col;
                triangles.Add(current);
                triangles.Add(current + 1);
                triangles.Add(current + sizex);  
                triangles.Add(current + sizex + 1);
                triangles.Add(current + sizex);
                triangles.Add(current + 1);
                
            }
        }

    }

    void ApplyMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
    }

   


}
