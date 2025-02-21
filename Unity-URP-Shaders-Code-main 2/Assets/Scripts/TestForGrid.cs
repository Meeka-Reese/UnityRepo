// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;
//
// public class TestForGrid : MonoBehaviour
// {
//     [SerializeField] private HeatMapVisual heatMapVisual;
//     [SerializeField] private GameObject ParentDummy;
//     [SerializeField] private GameObject Paper;
//     
//     private Camera Camera;
//     private float Addedz;
//     private Color Color = Color.black;
//     
//     private int value;
//     
//     private Vector3 StartPos = new Vector3(89, -1475, 0);
//
//     
//     //only using x and y
//     
//     
//
//     private Grids grids;
//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
//         value = Mathf.FloorToInt(255 * Color.r);
//         Camera = Camera.main;
//         grids = new Grids(70, 90, 3.1f, ParentDummy, StartPos, Color);
//         heatMapVisual.SetGrid(grids);   
//         
//     }
//
//     private void Update()
//     {
//         Vector3 PaperPos = new Vector3(Paper.transform.position.x, Paper.transform.position.y, Paper.transform.position.z);
//         // PaperPos.z = 295;
//         Addedz = PaperPos.z - Camera.transform.position.z;
//         
//         
//         
//         if (Input.GetMouseButtonDown(0))
//         {
//             Vector3 Position =(ClassTest.MousePos(Addedz));
//             
//
//             int value = grids.GetValue2(Position);
//             grids.SetValue2(Position, value + 5);
//             
//             Debug.Log("This is Position" + Position);
//         }
//
//         if (Input.GetMouseButtonDown(1))
//         {
//             Debug.Log(grids.GetValue2(ClassTest.MousePos(Addedz)));
//         }
//     }
//
//    
//     }
//
//   
//
