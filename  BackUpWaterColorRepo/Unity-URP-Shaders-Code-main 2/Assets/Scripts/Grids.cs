    using System;
    using UnityEngine;
    using System.Collections;

    public class Grids
    {
        public const int HEAT_MAP_MAX_SIZE = 100;
        public const int HEAT_MAP_MIN_SIZE = 0;
        private int width;
        private int height;
        private Color Color;
        private float CellSize;
        private int[,] gridArray;
        private GameObject MainCamera;
        private TextMesh[,] debugTextArray;
        Vector3 StartPos;
        
        public event EventHandler<OnGridsValueChangedEventArgs> OnGridsValueChanged;

        public class OnGridsValueChangedEventArgs : EventArgs
        {
            public int x;
            public int y;
        }
        
        public Grids(int width, int height, float CellSize, GameObject MainCamera, Vector3 StartPos, Color Color)
        {
            this.width = width;
            this.height = height;
            this.MainCamera = MainCamera;
            this.Color = Color;
            this.StartPos = StartPos;
            //Only using x and y
            
            this.CellSize = CellSize;
            gridArray = new int[width, height];
            debugTextArray = new TextMesh[width, height];
            
            
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    // Position in world space without being affected by parent scale
                    Vector3 worldPosition = GetWorldPosition(x, y) + new Vector3(CellSize, CellSize) * .5f;

                    // Create text with fixed position in world space
                    debugTextArray[x,y] = ClassTest.CreateWorldText(
                        MainCamera.transform,              // Transform parent (still using camera for position)
                        gridArray[x, y].ToString(),        // Text to display
                        worldPosition,                     // Position in world space
                        20,                                  // Font size
                        Color.black,                        // Text color
                        TextAnchor.MiddleCenter,            // Text anchor
                        TextAlignment.Center,               // Text alignment
                        0                                   // Sorting order
                    );
                    
                }
            }

            
            SetValue(2,1,70);
        }

        public int GetWidth()
        {
           return width;
        }
        public int GetHeight()
        {
            return height;
        }

        public Vector3 GetWorldPosition(int x, int y)
        {
            // Return grid positions in world space
            return new Vector3((x * CellSize) -110, (y * CellSize) -140, 0);
            //-110,-140
        }

        public float GetCellSize()
        {
            return CellSize;
        }

        private void GetXY(Vector3 worldPos, out int x, out int y)
        {
            
            x = Mathf.FloorToInt((worldPos.x +StartPos.x) / CellSize);
            y = Mathf.FloorToInt((worldPos.y + StartPos.y) / CellSize) * - 1;
            Debug.Log("cells" + x +"," + y);
            
        }

        public void SetValue(int x, int y, int value)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                gridArray[x, y] = Mathf.Clamp(value, HEAT_MAP_MIN_SIZE, HEAT_MAP_MAX_SIZE);
                debugTextArray[x,y].text = gridArray[x, y].ToString();
                if (OnGridsValueChanged != null) OnGridsValueChanged(this, new OnGridsValueChangedEventArgs { x = x, y = y });
            }
        }

        public void SetValue2(Vector3 worldPos2, int value)
        {
            int x, y;
            
            Debug.Log(worldPos2);
            GetXY(worldPos2, out x, out y);
            
           
            SetValue(x, y, Mathf.Clamp(value, HEAT_MAP_MIN_SIZE, HEAT_MAP_MAX_SIZE));
        }

        public int GetValue(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                return gridArray[x, y];
            }
            else
            {
                return 0;
            }
        }

        public int GetValue2(Vector3 worldPos)
        {
            int x, y;
            GetXY(worldPos, out x, out y);
            return GetValue(x, y);
        }
    }
//21, 1339