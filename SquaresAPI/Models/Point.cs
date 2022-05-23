﻿using System.ComponentModel.DataAnnotations;

namespace SquaresAPI.Models
{
    public class Point: IEquatable<Point>
    {
        public int Id { get; set; }
        [Required]
        public double X { get; set; }
        [Required]
        public double Y { get; set; }

        public bool Equals(Point? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Point)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public static bool operator ==(Point? left, Point? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Point? left, Point? right)
        {
            return !Equals(left, right);
        }
    }
}
