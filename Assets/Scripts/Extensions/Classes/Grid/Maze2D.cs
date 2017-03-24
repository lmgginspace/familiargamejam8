using System;
using System.Collections.Generic;

namespace Extensions.UnityEngine
{
    public class Maze2D<TCell, TBorder> : Grid2D<TCell>
    {
        // ---- ---- ---- ---- ---- ---- ---- ----
        // Fields
        // ---- ---- ---- ---- ---- ---- ---- ----
        private TBorder[,] horizontalBorderData;
        private TBorder[,] verticalBorderData;
        
        private readonly Grid2DDirection[] directions =
            {
                Grid2DDirection.North,
                Grid2DDirection.West,
                Grid2DDirection.East,
                Grid2DDirection.South
            };
        
        // ---- ---- ---- ---- ---- ---- ---- ----
        // Properties
        // ---- ---- ---- ---- ---- ---- ---- ----
        public IEnumerable<Maze2DItem<TCell, TBorder>> MazeCells
        {
            get
            {
                Maze2DItem<TCell, TBorder>[] result = new Maze2DItem<TCell, TBorder>[this.Area];
                for (int j = 0; j < this.height; j++)
                {
                    for (int i = 0; i < this.width; i++)
                    {
                        result[i + j * this.width] =
                            new Maze2DItem<TCell, TBorder>(i, j, this.gridData[i, j],
                                                           this.GetBorder(i, j, Grid2DDirection.North),
                                                           this.GetBorder(i, j, Grid2DDirection.West),
                                                           this.GetBorder(i, j, Grid2DDirection.East),
                                                           this.GetBorder(i, j, Grid2DDirection.South));
                    }
                }
                return result;
            }
        }
        
        // ---- ---- ---- ---- ---- ---- ---- ----
        // Constructors
        // ---- ---- ---- ---- ---- ---- ---- ----
        public Maze2D(int width, int height) : base(width, height)
        {
            this.horizontalBorderData = new TBorder[width + 1, height];
            this.verticalBorderData = new TBorder[width, height + 1];
        }
        
        // ---- ---- ---- ---- ---- ---- ---- ----
        // Methods
        // ---- ---- ---- ---- ---- ---- ---- ----
        // Wall value management methods.
        public TBorder GetBorder(int x, int y, Grid2DDirection direction)
        {
            if (this.wrapping)
            {
                x = this.ModWidth(x);
                y = this.ModHeight(y);
            }
            
            switch (direction)
            {
                case Grid2DDirection.North:
                    return this.verticalBorderData[x, y];
                
                case Grid2DDirection.South:
                    return this.verticalBorderData[x, this.wrapping ? this.ModHeight(y + 1) : y + 1];
                    
                case Grid2DDirection.West:
                    return this.horizontalBorderData[x, y];
                
                case Grid2DDirection.East:
                    return this.horizontalBorderData[this.wrapping ? this.ModWidth(x + 1) : x + 1, y];
                
                default:
                    throw new ArgumentException();
            }
        }
        
        /// <summary>
        /// Sets the value of all the borders around the specified cell. The adjacent cells will be updated
        /// accordingly, and wrapping is also taken into account.
        /// </summary>
        /// <param name="p">The coordinates of the cell.</param>
        /// <param name="border">The value for the border.</param>
        public void SetAllBorders(Point2D p, TBorder border)
        {
            foreach (var direction in directions)
                this.SetBorder(p.X, p.Y, direction, border);
        }
        
        /// <summary>
        /// Sets the value of all the borders around the specified cell. The adjacent cells will be updated
        /// accordingly, and wrapping is also taken into account.
        /// </summary>
        /// <param name="p">The coordinates of the cell.</param>
        /// <param name="border">The value for the border.</param>
        public void SetAllBorders(int x, int y, TBorder border)
        {
            foreach (var direction in directions)
                this.SetBorder(x, y, direction, border);
        }
        
        /// <summary>
        /// Sets the value at the specified border of the specified cell. The adjacent cell will be updated
        /// accordingly, and wrapping is also taken into account.
        /// </summary>
        /// <param name="p">The coordinates of the cell.</param>
        /// <param name="direction">The direction of the border relative to the specified cell.</param>
        /// <param name="border">The value for the border.</param>
        public void SetBorder(Point2D p, Grid2DDirection direction, TBorder border)
        {
            this.SetBorder(p.X, p.Y, direction, border);
        }
        
        /// <summary>
        /// Sets the value at the specified border of the specified cell. The adjacent cell will be updated
        /// accordingly, and wrapping is also taken into account.
        /// </summary>
        /// <param name="x">The x coordinate of the cell.</param>
        /// <param name="y">The y coordinate of the cell.</param>
        /// <param name="direction">The direction of the border relative to the specified cell.</param>
        /// <param name="border">The value for the border.</param>
        public void SetBorder(int x, int y, Grid2DDirection direction, TBorder border)
        {
            if (this.wrapping)
            {
                x = this.ModWidth(x);
                y = this.ModHeight(y);
            }
            
            switch (direction)
            {
                case Grid2DDirection.North:
                    this.verticalBorderData[x, y] = border;
                    break;
                
                case Grid2DDirection.South:
                    this.verticalBorderData[x, this.wrapping ? this.ModHeight(y + 1) : y + 1] = border;
                    break;
                
                case Grid2DDirection.West:
                    this.horizontalBorderData[x, y] = border;
                    break;
                
                case Grid2DDirection.East:
                    this.horizontalBorderData[this.wrapping ? this.ModWidth(x + 1) : x + 1, y] = border;
                    break;
                
                default:
                    throw new ArgumentException();
            }
        }
        
        /// <summary>
        /// Sets the value at the shared border between two adjacent cells.
        /// </summary>
        /// <param name="p1">The coordinates of one of the cells.</param>
        /// <param name="p2">The coordinates of the other cell.</param>
        /// <param name="border">The value for the border.</param>
        public void SetBorderBetweenCells(Point2D p1, Point2D p2, TBorder border)
        {
            this.SetBorderBetweenCells(p1.X, p1.Y, p2.X, p2.Y, border);
        }
        
        /// <summary>
        /// Sets the value at the shared border between two adjacent cells.
        /// </summary>
        /// <param name="x1">The x coordinate of the first cell.</param>
        /// <param name="y1">The y coordinate of the first cell.</param>
        /// <param name="x2">The x coordinate of the second cell.</param>
        /// <param name="y2">The y coordinate of the second cell.</param>
        /// <param name="border">The value for the border.</param>
        public void SetBorderBetweenCells(int x1, int y1, int x2, int y2, TBorder border)
        {
            if ((x2 == x1 + 1) && (y2 == y1))
                this.SetBorder(x1, y1, Grid2DDirection.East, border);
            else if ((x2 == x1 - 1) && (y2 == y1))
                this.SetBorder(x1, y1, Grid2DDirection.West, border);
            else if ((x2 == x1) && (y2 == y1 + 1))
                this.SetBorder(x1, y1, Grid2DDirection.South, border);
            else if ((x2 == x1) && (y2 == y1 - 1))
                this.SetBorder(x1, y1, Grid2DDirection.North, border);
        }
        
        // Wall information methods.
        public int CountBorders(int x, int y, Predicate<TBorder> predicate)
        {
            int count = 0;
            count += predicate(this.GetBorder(x, y, Grid2DDirection.North)) ? 1 : 0;
            count += predicate(this.GetBorder(x, y, Grid2DDirection.South)) ? 1 : 0;
            count += predicate(this.GetBorder(x, y, Grid2DDirection.West)) ? 1 : 0;
            count += predicate(this.GetBorder(x, y, Grid2DDirection.East)) ? 1 : 0;
            return count;
        }
        
        // Manipulation methods.
        public void Fill(TCell cell, TBorder border)
        {
            for (int j = 0; j < this.height; j++)
            {
                for (int i = 0; i < this.width; i++)
                {
                    this.gridData[i, j] = cell;
                    
                    this.horizontalBorderData[i, j] = border;
                    this.verticalBorderData[i, j] = border;
                }
            }
            
            for (int i = 0; i < this.height; i++)
                this.horizontalBorderData[this.width, i] = border;
            
            for (int i = 0; i < this.width; i++)
                this.verticalBorderData[i, this.height] = border;
        }
        
        public void FillBorders(TBorder wall)
        {
            for (int j = 0; j < this.height; j++)
            {
                for (int i = 0; i < this.width; i++)
                {
                    this.horizontalBorderData[i, j] = wall;
                    this.verticalBorderData[i, j] = wall;
                }
            }
            
            for (int i = 0; i < this.height; i++)
                this.horizontalBorderData[this.width, i] = wall;
            
            for (int i = 0; i < this.width; i++)
                this.verticalBorderData[i, this.height] = wall;
        }
        
        // Subclasses.
        public sealed class Maze2DItem<UCell, UBorder>
        {
            private UBorder north;
            private UBorder west;
            private UBorder east;
            private UBorder south;
            
            public Point2D Coordinates { get; set; }
            public UCell Item { get; set; }
            
            public Maze2DItem(int x, int y, UCell item, UBorder north, UBorder west, UBorder east, UBorder south)
            {
                this.Coordinates = new Point2D(x, y);
                this.Item = item;
                
                this.north = north;
                this.west = west;
                this.east = east;
                this.south = south;
            }
            
            public int CountBorders(Predicate<UBorder> predicate)
            {
                int count = 0;
                count += predicate(this.north) ? 1 : 0;
                count += predicate(this.west) ? 1 : 0;
                count += predicate(this.east) ? 1 : 0;
                count += predicate(this.south) ? 1 : 0;
                return count;
            }
            
            public UBorder GetBorder(Grid2DDirection direction)
            {
                switch (direction)
                {
                    case Grid2DDirection.North: return this.north;
                    case Grid2DDirection.South: return this.south;
                    case Grid2DDirection.West:  return this.west;
                    case Grid2DDirection.East:  return this.east;
                        
                    default: throw new ArgumentException();
                }
            }
        }
    }
    
}