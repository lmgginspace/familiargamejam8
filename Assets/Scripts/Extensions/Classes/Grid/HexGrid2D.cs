using System;
using System.Collections.Generic;
using UnityEngine;

namespace Extensions.UnityEngine
{
    public class HexGrid2D<T> : IEnumerable<T>
    {
        // ---- ---- ---- ---- ---- ---- ---- ----
        // Campos
        // ---- ---- ---- ---- ---- ---- ---- ----
        // Basic grid properties
        private int width;
        private int height;
        private bool wrapping;
        
        // Hexagonal grid properties
        private float edgeSize;
        private HexGridType gridType;
        
        // Projection fields
        private Vector2 offset = Vector2.zero;
        private AxisDirection xAxisDirection = AxisDirection.Positive;
        private AxisDirection yAxisDirection = AxisDirection.Positive;
        
        // Extrapolation fields
        private T outsideValue;
        private bool outsideValueActive;
        
        // Data structures
        private T[,] gridData;
        
        // Constants
        private const float sqrt3 = 1.73205080757f;
        private const float areaFactor = 2.59807621135f; // 3 * sqrt3 / 2
        
        // ---- ---- ---- ---- ---- ---- ---- ----
        // Propiedades
        // ---- ---- ---- ---- ---- ---- ---- ----
        // Propiedades básicas
        public float Area
        {
            get
            {
                return (HexGrid2D<T>.areaFactor * this.edgeSize * this.edgeSize) * (this.height * width);
            }
        }
        
        public int Height
        {
            get { return this.height; }
        }
        
        public int Width
        {
            get { return this.width; }
        }
        
        public bool Wrapping
        {
            get { return this.wrapping; }
            set { this.wrapping = value; }
        }
        
        // Propiedades específicas
        public float EdgeSize
        {
            get { return this.edgeSize; }
            set { this.edgeSize = value > 0.0f ? value : 0.0f; }
        }
        
        public HexGridType GridType
        {
            get { return this.gridType; }
            set { this.gridType = value; }
        }
        
        // Projection properties
        public Vector2 ProjectionOffset
        {
            get { return this.offset; }
            set { this.offset = value; }
        }
        
        public AxisDirection XAxisDirection
        {
            get { return this.xAxisDirection; }
            set { this.xAxisDirection = value; }
        }
        
        public AxisDirection YAxisDirection
        {
            get { return this.yAxisDirection; }
            set { this.yAxisDirection = value; }
        }
        
        // Extrapolation properties
        public bool UsingOutsideValue
        {
            get { return this.outsideValueActive; }
        }
        
        // Indizadores
        public T this[HexPoint2D p]
        {
            get { return this.GetItem(p); }
        }
        
        public T this[Point2D p]
        {
            get { return this.GetItem(p); }
        }
        
        public T this[int x, int y]
        {
            get { return this.GetItem(x, y); }
        }
        
        // ---- ---- ---- ---- ---- ---- ---- ----
        // Constructores
        // ---- ---- ---- ---- ---- ---- ---- ----
        public HexGrid2D(int width, int height, float edgeSize = 0.0f,
                         HexGridType hexGridType = HexGridType.FlatTopEvenOffset)
        {
            if (width <= 0)
                throw new ArgumentOutOfRangeException("width");
            if (height <= 0)
                throw new ArgumentOutOfRangeException("height");
            
            this.width = width;
            this.height = height;
            
            this.gridData = new T[width, height];
            
            this.EdgeSize = edgeSize;
            this.gridType = hexGridType;
        }
        
        // ---- ---- ---- ---- ---- ---- ---- ----
        // Métodos
        // ---- ---- ---- ---- ---- ---- ---- ----
        // Métodos de obtención de elementos
        public T GetItem(HexPoint2D hexPoint)
        {
            Point2D point = this.NormalizedToRectangular(hexPoint);
            return this.GetItem(point.X, point.Y);
        }
        
        public T GetItem(Point2D point)
        {
            return this.GetItem(point.X, point.Y);
        }
        
        private T GetItem(int x, int y)
        {
            if (this.wrapping)
                return this.gridData[this.ModWidth(x), this.ModHeight(y)];
            else
            {
                if (this.outsideValueActive && (x < 0 || x >= this.width || y < 0 || y >= this.height))
                    return this.outsideValue;
                else
                    return this.gridData[x, y];
            }
        }
        
        // Métodos de asignación de elementos
        public void SetItem(HexPoint2D hexPoint, T item)
        {
            Point2D point = this.NormalizedToRectangular(hexPoint);
            this.SetItem(point.X, point.Y, item);
        }
        
        public void SetItem(Point2D point, T item)
        {
            this.SetItem(point.X, point.Y, item);
        }
        
        private void SetItem(int x, int y, T item)
        {
            if (this.wrapping)
                this.gridData[this.ModWidth(x), this.ModHeight(y)] = item;
            else
                this.gridData[x, y] = item;
        }
        
        // Métodos de información sobre adyacencia
        public int CountNeighbours(int x, int y, Predicate<T> predicate)
        {
            int count = 0;
            foreach (T item in this.GetNeighbours(x, y))
                if (predicate(item))
                    count++;
            
            return count;
        }
        
        public T[] GetNeighbours(int x, int y)
        {
            HexPoint2D h = this.RectangularToNormalized(new Point2D(x, y));
            if (wrapping)
            {
                return new T[]
                {
                    this.GetItem(new HexPoint2D(h.X + 1, h.Y - 1)),
                    this.GetItem(new HexPoint2D(h.X + 1, h.Y    )),
                    this.GetItem(new HexPoint2D(h.X - 1, h.Y + 1)),
                    this.GetItem(new HexPoint2D(h.X    , h.Y + 1)),
                    this.GetItem(new HexPoint2D(h.X - 1, h.Y    )),
                    this.GetItem(new HexPoint2D(h.X    , h.Y - 1)),
                };
            }
            else
            {
                if (this.outsideValueActive)
                {
                    return new T[]
                    {
                        this.PointInRange(new HexPoint2D(h.X + 1, h.Y - 1)) ?
                        this.GetItem(new HexPoint2D(h.X + 1, h.Y - 1)) : this.outsideValue,
                        
                        this.PointInRange(new HexPoint2D(h.X + 1, h.Y    )) ?
                        this.GetItem(new HexPoint2D(h.X + 1, h.Y    )) : this.outsideValue,
                        
                        this.PointInRange(new HexPoint2D(h.X - 1, h.Y + 1)) ?
                        this.GetItem(new HexPoint2D(h.X - 1, h.Y + 1)) : this.outsideValue,
                        
                        this.PointInRange(new HexPoint2D(h.X    , h.Y + 1)) ?
                        this.GetItem(new HexPoint2D(h.X    , h.Y + 1)) : this.outsideValue,
                        
                        this.PointInRange(new HexPoint2D(h.X - 1, h.Y    )) ?
                        this.GetItem(new HexPoint2D(h.X - 1, h.Y    )) : this.outsideValue,
                        
                        this.PointInRange(new HexPoint2D(h.X    , h.Y - 1)) ?
                        this.GetItem(new HexPoint2D(h.X    , h.Y - 1)) : this.outsideValue,
                    };
                }
                else
                {
                    List<T> result = new List<T>(6);
                    if (this.PointInRange(new HexPoint2D(h.X + 1, h.Y - 1)))
                        result.Add(this.GetItem(new HexPoint2D(h.X + 1, h.Y - 1)));
                    
                    if (this.PointInRange(new HexPoint2D(h.X + 1, h.Y    )))
                        result.Add(this.GetItem(new HexPoint2D(h.X + 1, h.Y - 1)));
                    
                    if (this.PointInRange(new HexPoint2D(h.X - 1, h.Y + 1)))
                        result.Add(this.GetItem(new HexPoint2D(h.X + 1, h.Y - 1)));
                    
                    if (this.PointInRange(new HexPoint2D(h.X    , h.Y + 1)))
                        result.Add(this.GetItem(new HexPoint2D(h.X + 1, h.Y - 1)));
                    
                    if (this.PointInRange(new HexPoint2D(h.X - 1, h.Y    )))
                        result.Add(this.GetItem(new HexPoint2D(h.X + 1, h.Y - 1)));
                    
                    if (this.PointInRange(new HexPoint2D(h.X    , h.Y - 1)))
                        result.Add(this.GetItem(new HexPoint2D(h.X + 1, h.Y - 1)));
                    
                    return result.ToArray();
                }
            }
        }
        
        // Métodos de configuración de valores fuera de rango
        public void ClearOutsideValue()
        {
            this.outsideValueActive = false;
        }
        
        public void SetOutsideValue(T value)
        {
            this.outsideValue = value;
            this.outsideValueActive = true;
        }
        
        // Métodos de manipulación
        public void Fill(T item)
        {
            for (int i = 0; i < this.width; i++)
                for (int j = 0; j < this.height; j++)
                    this.gridData[i, j] = item;
        }
        
        private void Swap(int sourceX, int sourceY, int targetX, int targetY)
        {
            if (this.wrapping)
            {
                sourceX = this.ModWidth(sourceX);
                sourceY = this.ModHeight(sourceY);
                targetX = this.ModWidth(targetX);
                targetY = this.ModHeight(targetY);
            }
            
            T tempTarget = this.gridData[targetX, targetX];
            this.gridData[targetX, targetY] = this.gridData[sourceX, sourceY];
            this.gridData[sourceX, sourceY] = tempTarget;
        }
        
        // Métodos de transformación de coordenadas
        public Vector2 ProjectHexPointToRectangular(Point2D p)
        {
            return this.ProjectHexPointToRectangular(p.X, p.Y);
        }
        
        public Vector2 ProjectHexPointToRectangular(int x, int y)
        {
            float xValue = 0.0f, yValue = 0.0f;
            
            switch (this.gridType)
            {
                case HexGridType.FlatTopOddOffset:
                    xValue = this.edgeSize * 1.5f * x;
                    yValue = this.edgeSize * HexGrid2D<T>.sqrt3 * (y + (0.5f * (x & 1)));
                    break;
                case HexGridType.FlatTopEvenOffset:
                    xValue = this.edgeSize * 1.5f * x;
                    yValue = this.edgeSize * HexGrid2D<T>.sqrt3 * (y - (0.5f * (x & 1)));
                    break;
                case HexGridType.VertexTopOddOffset:
                    xValue = this.edgeSize * HexGrid2D<T>.sqrt3 * (x + (0.5f * (y & 1)));
                    yValue = this.edgeSize * 1.5f * y;
                    break;
                case HexGridType.VertexTopEvenOffset:
                    xValue = this.edgeSize * HexGrid2D<T>.sqrt3 * (x - (0.5f * (y & 1)));
                    yValue = this.edgeSize * 1.5f * y;
                    break;
                default:
                    throw new InvalidOperationException();
            }
            
            if (this.xAxisDirection == AxisDirection.Negative)
                xValue = -xValue;
            if (this.yAxisDirection == AxisDirection.Negative)
                yValue = -yValue;
            
            return new Vector2(xValue, yValue) + this.offset;
        }
        
        public Point2D ProjectRectangularToHexPoint(Vector2 p)
        {
            float xValue = p.x - this.offset.x;
            float yValue = p.y - this.offset.y;
            
            if (this.xAxisDirection == AxisDirection.Negative)
                xValue = -xValue;
            if (this.yAxisDirection == AxisDirection.Negative)
                yValue = -yValue;
            
            switch (this.gridType)
            {
                case HexGridType.FlatTopOddOffset:
                case HexGridType.FlatTopEvenOffset:
                    return this.NormalizedToRectangular(
                        new HexPoint2D(
                            HexGrid2D<T>.Round(xValue * 0.66666666f / this.edgeSize),
                            HexGrid2D<T>.Round(((HexGrid2D<T>.sqrt3 * 0.33333333f * yValue) - (xValue * 0.33333333f)) /
                                               this.edgeSize))
                    );
                case HexGridType.VertexTopOddOffset:
                case HexGridType.VertexTopEvenOffset:
                    return this.NormalizedToRectangular(
                        new HexPoint2D(
                            HexGrid2D<T>.Round(((HexGrid2D<T>.sqrt3 * 0.33333333f * xValue) - (yValue * 0.33333333f)) /
                                               this.edgeSize),
                            HexGrid2D<T>.Round(yValue * 0.66666666f / this.edgeSize))
                    );
                default:
                    throw new InvalidOperationException();
            }
        }
        
        // Métodos de transformación de coordenadas
        public HexPoint2D RectangularToNormalized(Point2D p)
        {
            switch (this.gridType)
            {
                case HexGridType.FlatTopOddOffset:
                    return new HexPoint2D(p.X, p.Y - (p.X - (p.X & 1)) / 2);
                case HexGridType.FlatTopEvenOffset:
                    return new HexPoint2D(p.X, p.Y - (p.X + (p.X & 1)) / 2);
                case HexGridType.VertexTopOddOffset:
                    return new HexPoint2D(p.X - (p.Y - (p.Y & 1)) / 2, p.Y);
                case HexGridType.VertexTopEvenOffset:
                    return new HexPoint2D(p.X - (p.Y + (p.Y & 1)) / 2, p.Y);
                default:
                    throw new InvalidOperationException();
            }
        }
        
        public Point2D NormalizedToRectangular(HexPoint2D p)
        {
            switch (this.gridType)
            {
                case HexGridType.FlatTopOddOffset: // odd-q v-layout
                    return new Point2D(p.X, p.Y + (p.X - (p.X & 1)) / 2);
                case HexGridType.FlatTopEvenOffset: // even-q v-layout
                    return new Point2D(p.X, p.Y + (p.X + (p.X & 1)) / 2);
                case HexGridType.VertexTopOddOffset: // odd-r h-layout
                    return new Point2D(p.X + (p.Y - (p.Y & 1)) / 2, p.Y);
                case HexGridType.VertexTopEvenOffset: // even-r h-layout
                    return new Point2D(p.X + (p.Y + (p.Y & 1)) / 2, p.Y);
                default:
                    throw new InvalidOperationException();
            }
        }
        
        // Métodos auxiliares
        private int ModHeight(int y)
        {
            int r = y % this.height;
            return r < 0 ? r + this.height : r;
        }
        
        private int ModWidth(int x)
        {
            int r = x % this.width;
            return r < 0 ? r + this.width : r;
        }
        
        private bool PointInRange(int x, int y)
        {
            return (x >= 0) && (x < width) && (y >= 0) && (y < height);
        }
        
        private bool PointInRange(Point2D p)
        {
            return (p.X >= 0) && (p.X < width) && (p.Y >= 0) && (p.Y < height);
        }
        
        private bool PointInRange(HexPoint2D p)
        {
            Point2D point = this.NormalizedToRectangular(p);
            return (point.X >= 0) && (point.X < width) && (point.Y >= 0) && (point.Y < height);
        }
        
        private static int Round(float f)
        {
            #if (UNITY_5 || UNITY_4)
                return Mathf.RoundToInt(f);
            #else
                return (int)Math.Round((double)f);
            #endif
        }
        
        // Métodos de IEnumerable<T>
        public IEnumerator<T> GetEnumerator()
        {
            foreach (T item in this.gridData)
                yield return item;
        }
        
        global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        
        public sealed class HexGrid2DItem<U>
        {
            public Point2D RectangularCoordinates { get; set; }
            public HexPoint2D NormalizedCoordinates { get; set; }
            public U Item { get; set; }
        }
    }
    
}