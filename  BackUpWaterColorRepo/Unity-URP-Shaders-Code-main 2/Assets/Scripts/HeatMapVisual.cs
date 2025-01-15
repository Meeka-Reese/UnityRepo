using UnityEngine;

public class HeatMapVisual : MonoBehaviour
{
    private Grids grids;
    private Mesh mesh;
    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   
        public void SetGrid(Grids grids)
        {
            this.grids = grids;
            // UpdateHeatMapVisual();
        }

        // private void UpdateHeatMapVisual()
        // {
        //     ClassTest.CreateEmptyMeshArrays(grids.GetWidth() * grids.GetHeight(), 
        //         out Vector3[] vertices, out Vector2[] uvs, out int[] triangles);
        //     for (int x = 0; x < grids.GetWidth(); x++)
        //     {
        //         for (int y = 0; y < grids.GetHeight(); y++)
        //         {
        //             int index = x * grids.GetHeight() + y;
        //             Vector3 quadSize = new Vector3(1, 1) * grids.GetCellSize();
        //             ClassTest.AddQuad(vertices, uvs, triangles, index, grids.GetWorldPosition(x, y), quadSize, Vector2.zero);
        //             //(Vector3[] vertices, Vector2[] uvs, int[] triangles, int index, Vector3 GridPos, Vector3 QuadSize, Vector2 Uv)
        //         }
        //     }
        //     mesh.vertices = vertices;
        //     mesh.uv = uvs;
        //     mesh.triangles = triangles;
        // }
    }


