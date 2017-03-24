using System;
using System.Collections.Generic;

namespace Extensions.UnityEngine
{
    public class Grid2D<T> : IEnumerable<T>
    {
        // ---- ---- ---- ---- ---- ---- ---- ----
        // Campos
        // ---- ---- ---- ---- ---- ---- ---- ----
        protected int width;
        protected int height;
        
        protected bool wrapping;
        
        protected T[,] gridData;
        
        protected T outsideValue;
        protected bool outsideValueActive;
        
        // ---- ---- ---- ---- ---- ---- ---- ----
        // Propiedades
        // ---- ---- ---- ---- ---- ---- ---- ----
        // Propiedades básicas
        public int Area
        {
            get { return this.height * width; }
        }
        
        public int Height
        {
            get { return this.height; }
        }
        
        public int Width
        {
            get { return this.width; }
        }
        
        public bool UsingOutsideValue
        {
            get { return this.outsideValueActive; }
        }
        
        public bool Wrapping
        {
            get { return this.wrapping; }
            set { this.wrapping = value; }
        }
        
        // Indizadores
        public T this[Point2D p]
        {
            get { return this.GetItem(p.X, p.Y); }
        }
        
        public T this[int x, int y]
        {
            get { return this.GetItem(x, y); }
        }
        
        // Custom enumerator
        public IEnumerable<Grid2DItem<T>> GridCells
        {
            get
            {
                Grid2DItem<T>[] result = new Grid2DItem<T>[this.Area];
                for (int j = 0; j < this.height; j++)
                {
                    for (int i = 0; i < this.width; i++)
                    {
                        result[i + j * this.width] = new Grid2DItem<T>(i, j, this.gridData[i, j]);
                    }
                }
                return result;
            }
        }
        
        // ---- ---- ---- ---- ---- ---- ---- ----
        // Constructores
        // ---- ---- ---- ---- ---- ---- ---- ----
        public Grid2D(int width, int height)
        {
            this.width = width;
            this.height = height;
            
            this.gridData = new T[width, height];
        }
        
        // ---- ---- ---- ---- ---- ---- ---- ----
        // Métodos
        // ---- ---- ---- ---- ---- ---- ---- ----
        // Métodos de gestión
        public T GetItem(int x, int y)
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
        
        public T GetNeighbor(int x, int y, Grid2DDirection direction)
        {
            this.DirectionTransform(ref x, ref y, direction);
            return this.GetItem(x, y);
        }
        
        public void SetItem(Point2D p, T item)
        {
            this.SetItem(p.X, p.Y, item);
        }
        
        public void SetItem(int x, int y, T item)
        {
            if (this.wrapping)
                this.gridData[this.ModWidth(x), this.ModHeight(y)] = item;
            else
                this.gridData[x, y] = item;
        }
        
        public void SetNeighbor(int x, int y, T item, Grid2DDirection direction)
        {
            this.DirectionTransform(ref x, ref y, direction);
            this.SetItem(x, y, item);
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
        
        // Métodos de información sobre adyacencia
        public int CountNeighbors(Point2D p, Predicate<T> predicate)
        {
            return this.CountNeighbors(p.X, p.Y, predicate);
        }
        
        public int CountNeighbors(Point2D p, Predicate<T> predicate, Grid2DNeighborhood neighborhoodType)
        {
            return this.CountNeighbors(p.X, p.Y, predicate, neighborhoodType);
        }
        
        public int CountNeighbors(int x, int y, Predicate<T> predicate)
        {
            int count = 0;
            foreach (T item in this.GetManhattanNeighborList(x, y))
                if (predicate(item))
                    count++;
            
            return count;
        }
        
        public int CountNeighbors(int x, int y, Predicate<T> predicate, Grid2DNeighborhood neighborhoodType)
        {
            T[] neighborList = neighborhoodType == Grid2DNeighborhood.Chessboard ?
                               this.GetChessboardNeighborList(x, y) :
                               this.GetManhattanNeighborList(x, y);
            
            int count = 0;
            foreach (T item in neighborList)
                if (predicate(item))
                    count++;
            
            return count;
        }
        
        /// <summary>
        /// Gets the four "Manhattan" neighbors of the designated cell. Grid wrapping is taken into account.
        /// </summary>
        /// <returns>An array containing all found neighbors.</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public T[] GetNeighborList(int x, int y)
        {
            return this.GetManhattanNeighborList(x, y);
        }
        
        /// <summary>
        /// Gets all the neighbors of the designated cell, according to the specified neighborhood type. Grid wrapping
        /// is taken into account.
        /// </summary>
        /// <returns>An array containing all found neighbors.</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="neighborhoodType">The neighborhood type. "Manhattan" applies only to horizontal and vertical
        /// neighbors, and "Chessboard" includes also diagonal neighbors.</param>
        public T[] GetNeighborList(int x, int y, Grid2DNeighborhood neighborhoodType)
        {
            return neighborhoodType == Grid2DNeighborhood.Chessboard ?
                   this.GetChessboardNeighborList(x, y) :
                   this.GetManhattanNeighborList(x, y);
        }
        
        /// <summary>
        /// Gets the coordinates of all the four "Manhattan" neighbors of the designated cell. Grid wrapping is taken
        /// into account.
        /// </summary>
        /// <returns>An array containing the coordinates of all found neighbors.</returns>
        /// <param name="p">The two dimensional point.</param>
        public Point2D[] GetNeighborCoordList(Point2D p)
        {
            return this.GetManhattanNeighborCoordList(p.X, p.Y);
        }
        
        /// <summary>
        /// Gets the coordinates of the four "Manhattan" neighbors of the designated cell that satisfy the specified
        /// predicate. Grid wrapping is taken into account.
        /// </summary>
        /// <returns>An array containing the coordinates of all found neighbors.</returns>
        /// <param name="p">The two dimensional point.</param>
        public Point2D[] GetNeighborCoordList(Point2D p, Predicate<T> predicate)
        {
            return this.GetManhattanNeighborCoordList(p.X, p.Y, predicate);
        }
        
        /// <summary>
        /// Gets the coordinates of all the neighbors of the designated cell, according to the specified neighborhood
        /// type. Grid wrapping is taken into account.
        /// </summary>
        /// <returns>An array containing the coordinates of all found neighbors.</returns>
        /// <param name="p">The two dimensional point.</param>
        /// <param name="neighborhoodType">The neighborhood type. "Manhattan" applies only to horizontal and vertical
        /// neighbors, and "Chessboard" includes also diagonal neighbors.</param>
        public Point2D[] GetNeighborCoordList(Point2D p, Grid2DNeighborhood neighborhoodType)
        {
            return neighborhoodType == Grid2DNeighborhood.Chessboard ?
                   this.GetChessboardNeighborCoordList(p.X, p.Y) :
                   this.GetManhattanNeighborCoordList(p.X, p.Y);
        }
        
        /// <summary>
        /// Gets the coordinates of all the four "Manhattan" neighbors of the designated cell. Grid wrapping is taken
        /// into account.
        /// </summary>
        /// <returns>An array containing the coordinates of all found neighbors.</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public Point2D[] GetNeighborCoordList(int x, int y)
        {
            return this.GetManhattanNeighborCoordList(x, y);
        }
        
        /// <summary>
        /// Gets the coordinates of the four "Manhattan" neighbors of the designated cell that satisfy the specified
        /// predicate. Grid wrapping is taken into account.
        /// </summary>
        /// <returns>An array containing the coordinates of all found neighbors.</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public Point2D[] GetNeighborCoordList(int x, int y, Predicate<T> predicate)
        {
            return this.GetManhattanNeighborCoordList(x, y, predicate);
        }
        
        /// <summary>
        /// Gets the coordinates of all the neighbors of the designated cell, according to the specified neighborhood
        /// type. Grid wrapping is taken into account.
        /// </summary>
        /// <returns>An array containing the coordinates of all found neighbors.</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="neighborhoodType">The neighborhood type. "Manhattan" applies only to horizontal and vertical
        /// neighbors, and "Chessboard" includes also diagonal neighbors.</param>
        public Point2D[] GetNeighborCoordList(int x, int y, Grid2DNeighborhood neighborhoodType)
        {
            return neighborhoodType == Grid2DNeighborhood.Chessboard ?
                   this.GetChessboardNeighborCoordList(x, y) :
                   this.GetManhattanNeighborCoordList(x, y);
        }
        
        // Métodos de manipulación
        public void Fill(T item)
        {
            for (int j = 0; j < this.height; j++)
            {
                for (int i = 0; i < this.width; i++)
                {
                    this.gridData[i, j] = item;
                }
            }
        }
        
        public void Swap(Point2D source, Point2D target)
        {
            this.Swap(source.X, source.Y, target.X, target.Y);
        }
        
        public void Swap(int sourceX, int sourceY, int targetX, int targetY)
        {
            if (this.wrapping)
            {
                sourceX = this.ModWidth(sourceX);
                sourceY = this.ModHeight(sourceY);
                targetX = this.ModWidth(targetX);
                targetY = this.ModHeight(targetY);
            }
            
            T tempTarget = this.gridData[targetX, targetY];
            this.gridData[targetX, targetY] = this.gridData[sourceX, sourceY];
            this.gridData[sourceX, sourceY] = tempTarget;
        }
        
        // Helper neighbor coordinate methods.
        private Point2D[] GetChessboardNeighborCoordList(int x, int y)
        {
            if (wrapping)
            {
                return new Point2D[]
                {
                    new Point2D(this.ModWidth(x - 1), this.ModHeight(y - 1)),
                    new Point2D(this.ModWidth(x), this.ModHeight(y - 1)),
                    new Point2D(this.ModWidth(x + 1), this.ModHeight(y - 1)),
                    new Point2D(this.ModWidth(x - 1), this.ModHeight(y)),
                    new Point2D(this.ModWidth(x + 1), this.ModHeight(y)),
                    new Point2D(this.ModWidth(x - 1), this.ModHeight(y + 1)),
                    new Point2D(this.ModWidth(x), this.ModHeight(y + 1)),
                    new Point2D(this.ModWidth(x + 1), this.ModHeight(y + 1))
                };
            }
            else
            {
                List<Point2D> result = new List<Point2D>(8);
                for (int i = x - 1; i <= x + 1; i++)
                {
                    for (int j = y - 1; j <= y + 1; j++)
                    {
                        if (this.PointInRange(i, j))
                            result.Add(new Point2D(i, j));
                    }
                }
                return result.ToArray();
            }
        }
        
        private Point2D[] GetManhattanNeighborCoordList(int x, int y)
        {
            if (wrapping)
            {
                return new Point2D[]
                {
                    new Point2D(this.ModWidth(x), this.ModHeight(y - 1)),
                    new Point2D(this.ModWidth(x - 1), this.ModHeight(y)),
                    new Point2D(this.ModWidth(x + 1), this.ModHeight(y)),
                    new Point2D(this.ModWidth(x), this.ModHeight(y + 1)),
                };
            }
            else
            {
                List<Point2D> result = new List<Point2D>(4);
                if (y > 0)
                    result.Add(new Point2D(x, y - 1));
                if (x > 0)
                    result.Add(new Point2D(x - 1, y));
                if (x < this.width - 1)
                    result.Add(new Point2D(x + 1, y));
                if (y < this.height - 1)
                    result.Add(new Point2D(x, y + 1));
                return result.ToArray();
            }
        }
        
        private Point2D[] GetManhattanNeighborCoordList(int x, int y, Predicate<T> predicate)
        {
            List<Point2D> result = new List<Point2D>();
            
            if (wrapping)
            {
                if (predicate(this.GetItem(x, y - 1))) 
                   result.Add(new Point2D(this.ModWidth(x), this.ModHeight(y - 1)));
               
                if (predicate(this.GetItem(x - 1, y)))
                    result.Add(new Point2D(this.ModWidth(x - 1), this.ModHeight(y)));
                
                if (predicate(this.GetItem(x + 1, y)))
                    result.Add(new Point2D(this.ModWidth(x + 1), this.ModHeight(y)));
                
                if (predicate(this.GetItem(x, y + 1)))
                    result.Add(new Point2D(this.ModWidth(x), this.ModHeight(y + 1)));
            }
            else
            {
                if (y > 0 && predicate(this.gridData[x    , y - 1])) result.Add(new Point2D(x    , y - 1));
                if (x > 0 && predicate(this.gridData[x - 1, y    ])) result.Add(new Point2D(x - 1, y    ));
                
                if (x < this.width - 1  && predicate(this.gridData[x + 1, y    ])) result.Add(new Point2D(x + 1, y    ));
                if (y < this.height - 1 && predicate(this.gridData[x    , y + 1])) result.Add(new Point2D(x    , y + 1));
            }
            
            return result.ToArray();
        }
        
        // Helper neighbor methods.
        private T[] GetChessboardNeighborList(int x, int y)
        {
            if (wrapping)
            {
                return new T[]
                {
                    this.gridData[this.ModWidth(x - 1), this.ModHeight(y - 1)],
                    this.gridData[this.ModWidth(x), this.ModHeight(y - 1)],
                    this.gridData[this.ModWidth(x + 1), this.ModHeight(y - 1)],
                    this.gridData[this.ModWidth(x - 1), this.ModHeight(y)],
                    this.gridData[this.ModWidth(x + 1), this.ModHeight(y)],
                    this.gridData[this.ModWidth(x - 1), this.ModHeight(y + 1)],
                    this.gridData[this.ModWidth(x), this.ModHeight(y + 1)],
                    this.gridData[this.ModWidth(x + 1), this.ModHeight(y + 1)]
                };
            }
            else
            {
                List<T> result = new List<T>(8);
                for (int i = x - 1; i <= x + 1; i++)
                {
                    for (int j = y - 1; j <= y + 1; j++)
                    {
                        if (this.PointInRange(i, j))
                            result.Add(this.gridData[i, j]);
                        else if (this.outsideValueActive)
                            result.Add(this.outsideValue);
                    }
                }
                return result.ToArray();
            }
        }
        
        private T[] GetManhattanNeighborList(int x, int y)
        {
            if (wrapping)
            {
                return new T[]
                {
                    this.gridData[this.ModWidth(x), this.ModHeight(y - 1)],
                    this.gridData[this.ModWidth(x - 1), this.ModHeight(y)],
                    this.gridData[this.ModWidth(x + 1), this.ModHeight(y)],
                    this.gridData[this.ModWidth(x), this.ModHeight(y + 1)]
                };
            }
            else
            {
                if (this.outsideValueActive)
                {
                    return new T[]
                    {
                        y < 0 ? this.outsideValue : this.gridData[x, y - 1],
                        x < 0 ? this.outsideValue : this.gridData[x - 1, y],
                        x >= this.width ? this.outsideValue : this.gridData[x + 1, y],
                        y >= this.height ? this.outsideValue : this.gridData[x, y + 1]
                    };
                }
                else
                {
                    List<T> result = new List<T>(4);
                    if (y > 0)
                        result.Add(this.gridData[x, y - 1]);
                    if (x > 0)
                        result.Add(this.gridData[x - 1, y]);
                    if (x < this.width - 1)
                        result.Add(this.gridData[x + 1, y]);
                    if (y < this.height - 1)
                        result.Add(this.gridData[x, y + 1]);
                    return result.ToArray();
                }
            }
        }
        
        // Misc helper methods.
        private void DirectionTransform(ref int x, ref int y, Grid2DDirection direction)
        {
            switch (direction)
            {
                case Grid2DDirection.NorthWest:
                    x = x - 1;
                    y = y - 1;
                    break;
                case Grid2DDirection.North:
                    //x = x;
                    y = y - 1;
                    break;
                case Grid2DDirection.NorthEast:
                    x = x + 1;
                    y = y - 1;
                    break;
                case Grid2DDirection.West:
                    x = x - 1;
                    //y = y;
                    break;
                case Grid2DDirection.East:
                    x = x + 1;
                    //y = y;
                    break;
                case Grid2DDirection.SouthWest:
                    x = x - 1;
                    y = y + 1;
                    break;
                case Grid2DDirection.South:
                    //x = x;
                    y = y + 1;
                    break;
                case Grid2DDirection.SouthEast:
                    x = x + 1;
                    y = y + 1;
                    break;
                default:
                    throw new ArgumentException();
            }
        }
        
        public bool PointInRange(Point2D p)
        {
            return (p.X >= 0) && (p.X < width) && (p.Y >= 0) && (p.Y < height);
        }
        
        public bool PointInRange(int x, int y)
        {
            return (x >= 0) && (x < width) && (y >= 0) && (y < height);
        }
        
        protected int ModHeight(int y)
        {
            int r = y % this.height;
            return r < 0 ? r + this.height : r;
        }
        
        protected int ModWidth(int x)
        {
            int r = x % this.width;
            return r < 0 ? r + this.width : r;
        }
        
        // Métodos de IEnumerable<T>
        public IEnumerator<T> GetEnumerator()
        {
            for (int j = 0; j < this.height; j++)
            {
                for (int i = 0; i < this.width; i++)
                {
                    yield return this.gridData[i, j];
                }
            }
        }
        
        global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        
        // Subclasses.
        public sealed class Grid2DItem<U>
        {
            public Point2D Coordinates { get; set; }
            public U Item { get; set; }
            
            public Grid2DItem(int x, int y, U item)
            {
                this.Coordinates = new Point2D(x, y);
                this.Item = item;
            }
        }
    }
    
}