using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(MeshFilter))]
public class MeshGen2 : MonoBehaviour
{
    private Mesh mesh;
    private MeshFilter meshFilter;
    private List<Vector3> vertices;
    private List<int> triangles;
    private int horizontalLines = 20;
    private int verticalLines = 20;
    private float xScale = 15f;
    private float yScale = 15f;
    private bool debug = false;
    private Vector2 StartPos = new Vector2(0, 0);
    public float radius = 4f;
   
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
        
        for (int m = 0; m < horizontalLines; m++)
        {
            for (int n = 0; n < verticalLines; n++)
            {
                float x = Mathf.Sin(Mathf.PI * m/horizontalLines) * Mathf.Cos(2 * Mathf.PI * n/verticalLines);
                float y = Mathf.Sin(Mathf.PI * m/horizontalLines) * Mathf.Sin(2 * Mathf.PI * n/verticalLines);
                float z = Mathf.Cos(Mathf.PI * m / horizontalLines);
                vertices.Add(new Vector3(x, y, z) * radius); 
            }   
        }   
        vertices.Add(new Vector3(xScale, 0, 0));
        debug = true;

        for (int row = 0; row < verticalLines - 1; row++)
        {
            for (int col = 0; col < horizontalLines - 1; col++)
            {
                
                int current = row * horizontalLines + col;
                
                triangles.Add(current + horizontalLines);  
                triangles.Add(current);
                triangles.Add(current + 1);
                triangles.Add(current + 1);
                triangles.Add(current + horizontalLines + 1);
                triangles.Add(current + horizontalLines);
                
                
                
            }
        }

    }

    void ApplyMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        // Debug.Log(triangles.Count + " Triangles count");
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
    }

   


}
