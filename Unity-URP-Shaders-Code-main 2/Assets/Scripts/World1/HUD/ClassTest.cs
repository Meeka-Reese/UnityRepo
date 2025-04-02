using UnityEngine;

public class ClassTest
{
    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize,
        Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }

    public static Vector3 GetMouseWorldPosition(float z)
    {
        Vector3 vec = GetMouseWorldPositionWithz(Input.mousePosition, Camera.main);
        vec.z = z;
        return vec;
    }

    public static Vector3 GetMouseWorldPositionWithz()
    {
        return GetMouseWorldPositionWithz(Input.mousePosition, Camera.main);

    }

    public static Vector3 GetMouseWorldPositionWithz(Camera worldCamera)
    {
        return GetMouseWorldPositionWithz(Input.mousePosition, worldCamera);
    }

    public static Vector3 GetMouseWorldPositionWithz(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }

    public static Vector3 MousePos(float z)
    {
        // Set the z-coordinate relative to the camera's position
        Vector3 screenPosition = Input.mousePosition;
        screenPosition.z = z; // Distance to the grid plane

        // Convert screen position to world position
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        Debug.Log("Wolrd Pos is" + worldPosition);
        return worldPosition;
    }

    public static void AddQuad(Vector3[] vertices, Vector2[] uvs, int[] triangles, int index, Vector3 GridPos,
        Vector3 QuadSize, Vector2 Uv)
    {
        vertices[index * 4] = new Vector3((-0.5f + GridPos.x) * QuadSize.x, (-0.5f + GridPos.y) * QuadSize.y);
        vertices[(index * 4) + 1] = new Vector3((-0.5f + GridPos.x) * QuadSize.x, (+0.5f + GridPos.y) * QuadSize.y);
        vertices[(index * 4) + 2] = new Vector3((+0.5f + GridPos.x) * QuadSize.x, (+0.5f + GridPos.y) * QuadSize.y);
        vertices[(index * 4) + 3] = new Vector3((+0.5f + GridPos.x) * QuadSize.x, (-0.5f + GridPos.y) * QuadSize.y);

        Debug.Log(vertices[0]);
        Debug.Log(vertices[1]);
        Debug.Log(vertices[2]);
        Debug.Log(vertices[3]);

        uvs[(index * 4)] = Uv;
        uvs[(index * 4) + 1] = Uv;
        uvs[(index * 4) + 2] = Uv;
        uvs[(index * 4) + 3] = Uv;

        triangles[(index * 6) + 0] = (index * 4) + 0;
        triangles[(index * 6) + 1] = (index * 4) + 1;
        triangles[(index * 6) + 2] = (index * 4) + 2;
        triangles[(index * 6) + 3] = (index * 4) + 2;
        triangles[(index * 6) + 4] = (index * 4) + 3;
        triangles[(index * 6) + 5] = (index * 4) + 0;
    }
}

//     public static void CreateEmptyMeshArrays(int quadCount, out Vector3[] vertices, out Vector2[] uvs, out int[] triangles)
//     {
//         vertices = new Vector3[quadCount * 4];
//         uvs = new Vector2[quadCount * 4];
//         triangles = new int[quadCount * 6];
//     }
//     public static void AddToMeshArrays(Vector3[] vertices, Vector2[] uvs, int[] triangles, int index, Vector3 pos, float rot, Vector3 baseSize, Vector2 uv00, Vector2 uv11) {
//         //Relocate vertices
//         int vIndex = index*4;
//         int vIndex0 = vIndex;
//         int vIndex1 = vIndex+1;
//         int vIndex2 = vIndex+2;
//         int vIndex3 = vIndex+3;
//
//         baseSize *= .5f;
//
//         bool skewed = baseSize.x != baseSize.y;
//         if (skewed) {
//             vertices[vIndex0] = pos+GetQuaternionEuler(rot)*new Vector3(-baseSize.x,  baseSize.y);
//             vertices[vIndex1] = pos+GetQuaternionEuler(rot)*new Vector3(-baseSize.x, -baseSize.y);
//             vertices[vIndex2] = pos+GetQuaternionEuler(rot)*new Vector3( baseSize.x, -baseSize.y);
//             vertices[vIndex3] = pos+GetQuaternionEuler(rot)*baseSize;
//         } else {
//             vertices[vIndex0] = pos+GetQuaternionEuler(rot-270)*baseSize;
//             vertices[vIndex1] = pos+GetQuaternionEuler(rot-180)*baseSize;
//             vertices[vIndex2] = pos+GetQuaternionEuler(rot- 90)*baseSize;
//             vertices[vIndex3] = pos+GetQuaternionEuler(rot-  0)*baseSize;
//         }
// 		
//         //Relocate UVs
//         uvs[vIndex0] = new Vector2(uv00.x, uv11.y);
//         uvs[vIndex1] = new Vector2(uv00.x, uv00.y);
//         uvs[vIndex2] = new Vector2(uv11.x, uv00.y);
//         uvs[vIndex3] = new Vector2(uv11.x, uv11.y);
// 		
//         //Create triangles
//         int tIndex = index*6;
// 		
//         triangles[tIndex+0] = vIndex0;
//         triangles[tIndex+1] = vIndex3;
//         triangles[tIndex+2] = vIndex1;
// 		
//         triangles[tIndex+3] = vIndex1;
//         triangles[tIndex+4] = vIndex3;
//         triangles[tIndex+5] = vIndex2;
//     }
//     private static Quaternion GetQuaternionEuler(float rotFloat) {
//         int rot = Mathf.RoundToInt(rotFloat);
//         rot = rot % 360;
//         if (rot < 0) rot += 360;
//         //if (rot >= 360) rot -= 360;
//         if (cachedQuaternionEulerArr == null) CacheQuaternionEuler();
//         return cachedQuaternionEulerArr[rot];
//     }
// }
