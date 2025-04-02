using UnityEngine;

public class TerrainFace
{
    private Mesh mesh;
    private int resolution;
    private Vector3 localUp;
    private Vector3 axisA;
    private Vector3 axisB;

    public TerrainFace(Mesh mesh, int resolution, Vector3 localUp)
    {
        this.mesh = mesh;
        this.resolution = resolution;
        this.localUp = localUp;
        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross(localUp, axisA);
    }

    public void ConstructMesh()
    {
        Vector3[] vertices = new Vector3[resolution * resolution];
        Vector3[] normals = new Vector3[resolution * resolution]; 
        int[] triangles = new int[(resolution- 1) * (resolution - 1) * 6];
        int triIndex = 0;
        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int index = y * resolution + x;
                Vector2 Percent = new Vector2(x,y) / (resolution - 1);
                Vector3 PointOnUnitCube = localUp + (Percent.x -.5f) * 2 * axisA + (Percent.y - .5f) * 2 * axisB;
                Vector3 PointOnUnitSphere = PointOnUnitCube.normalized;
                vertices[index] = PointOnUnitSphere;
                
                //adding vertices on unit cube
                if (x != resolution - 1 && y != resolution - 1) 
                {
                    normals[index] = vertices[index].normalized; 
                    triangles[triIndex] = index;
                    triangles[triIndex + 1] = index + resolution + 1;
                    triangles[triIndex + 2] = index + resolution;
                    
                    triangles[triIndex + 3] = index;
                    triangles[triIndex + 4] = index + 1;
                    triangles[triIndex + 5] = index + resolution + 1;
                    //turning vertices to 2 clockwise triangles
                    triIndex += 6;
                }
            }
        }
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
    }
    
}
