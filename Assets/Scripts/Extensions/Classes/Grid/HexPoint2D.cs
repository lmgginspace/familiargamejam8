using System;

namespace Extensions.UnityEngine
{
    [Serializable]
    public struct HexPoint2D
    {
        // ---- ---- ---- ---- ---- ---- ---- ----
        // Campos
        // ---- ---- ---- ---- ---- ---- ---- ----
        private int x;
        private int y;
        
        public static readonly HexPoint2D Zero = new HexPoint2D();
        
        // ---- ---- ---- ---- ---- ---- ---- ----
        // Propiedades
        // ---- ---- ---- ---- ---- ---- ---- ----
        public bool IsZero
        { 
            get { return this.x == 0 && this.y == 0; }
        }
        
        public int X
        {
            get { return this.x; }
            set { this.x = value; }
        }
        
        public int Y
        { 
            get { return this.y; }
            set { this.y = value; }
        }
        
        public int Z
        { 
            get { return -this.x - this.y; }
        }
        
        // ---- ---- ---- ---- ---- ---- ---- ----
        // Constructores
        // ---- ---- ---- ---- ---- ---- ---- ----
        public HexPoint2D(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        
        // ---- ---- ---- ---- ---- ---- ---- ----
        // Métodos
        // ---- ---- ---- ---- ---- ---- ---- ----
        // Operadores sobrecargados
        public static bool operator ==(HexPoint2D left, HexPoint2D right)
        {
            return left.X == right.X && left.Y == right.Y;
        }
        
        public static bool operator !=(HexPoint2D left, HexPoint2D right)
        { 
            return left.X != right.X || left.Y != right.Y;
        }
        
        // Métodos reemplazados
        public override bool Equals(object obj)
        {
            if (!(obj is HexPoint2D))
                return false;
            
            HexPoint2D comp = (HexPoint2D)obj;
            return comp.X == this.X && comp.Y == this.Y; 
        }
        
        public override int GetHashCode()
        { 
            return x ^ y;
        }
        
        public override string ToString()
        {
            return string.Format("{{X: {0}, Y: {1}, Z: {2}}}", this.x, this.y, this.Z);
        }
    }
    
}