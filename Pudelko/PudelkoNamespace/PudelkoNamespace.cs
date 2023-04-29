using PudelkoNamespace.Enums;
using System.Collections;
using System.Collections.Immutable;
using System.Data.Common;
using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace PudelkoNamespace.PudelkoLib
{
    public sealed class Pudelko : IFormattable, IEquatable<Pudelko>
	{
        private readonly double _a;
        private readonly double _b;
        private readonly double _c;

        public UnitOfMeasure Measure { get; set; }

        // meter returning function "ReturnMeters"
        public static double ReturnMeters(double value, UnitOfMeasure m)
        {
            if (m == UnitOfMeasure.meter)
            {
                return Math.Round(value, 14);
            }
            else if (m == UnitOfMeasure.milimeter)
            {
                if (value < 1) return 0;

                return Math.Round(value / 1000, 14);
            }
            else if (m == UnitOfMeasure.centimeter)
            {
                if (value < 0.1) return 0;

                return Math.Round(value / 100, 14);
            }
            else throw new FormatException("Invalid type for MeasureType enum.");
        }
        // constructor with a,b,c, type of measure and exceptions
        public Pudelko(double a, double b, double c, UnitOfMeasure type)
        {
            if (!Enum.IsDefined(typeof(UnitOfMeasure), type))
            {
                throw new FormatException("Invalid type for MeasureType enum.");
            }
            if (ReturnMeters(a, type) <= 0 || ReturnMeters(b, type) <= 0 || ReturnMeters(c, type) <= 0)
            {
                throw new ArgumentOutOfRangeException("Dimensions cannot be negative or equal to 0");
            }

            if (ReturnMeters(a,type) > 10 || ReturnMeters(b, type) > 10 || ReturnMeters(c, type) > 10) {
                throw new ArgumentOutOfRangeException("The maximum dimensions of the box are 10m x 10m x 10m");
            }
            _a = a;
            _b = b;
            _c = c;
            Measure = type;
        }
        
        // second constructor for tests
        public Pudelko(double A, double B, double C) : this(A, B, C, UnitOfMeasure.meter)
        {
            
        }
        // every measure exceptions for tests
        public Pudelko(double A, double B, UnitOfMeasure type = UnitOfMeasure.meter) : this(A,B,10, type)
        {
            if (type == UnitOfMeasure.meter) _c /= 100;

            if (type == UnitOfMeasure.milimeter) _c *= 10;
        }

        public Pudelko(double A, double B) : this(A, B, 10, UnitOfMeasure.meter)
        {
            _c /= 100;
        }

        public Pudelko(double A) : this(A, 10, 10, UnitOfMeasure.meter)
        {
            _c /= 100;
            _b /= 100;
        }

        public Pudelko(double A, UnitOfMeasure type = UnitOfMeasure.centimeter) : this(A, 10, 10, type)
        {
            if (type == UnitOfMeasure.meter)
            {
                _c /= 100;
                _b /= 100;
            }
            else if (type == UnitOfMeasure.milimeter)
            {
                _c *=  10;
                _b *= 10;
            }
        }

        public Pudelko(UnitOfMeasure type = UnitOfMeasure.centimeter) : this(10, 10, 10, type)
        {
            if (type == UnitOfMeasure.meter)
            {
                _c /= 100;
                _b /= 100;
                _a /= 100;
                
            }
            else if (type == UnitOfMeasure.milimeter)
            {
                _c *= 10;
                _b *= 10;
                _a *= 10;
            }
        }
        // returning A,B,C method
        public  double A => ReturnMeters(_a, Measure);
        public  double B => ReturnMeters(_b, Measure);
        public  double C => ReturnMeters(_c, Measure);
        
        // string inplementation
        public string ToString(string? format = "m", IFormatProvider? provider = default)
        {
            if (format is null)
                format = "m";

			return format.ToLower() switch
			{
				"m" => $"{A:F3} {format} × {B:F3} {format} × {C:F3} {format}",
				("cm") => $"{A * 100:F1} {format} × {B * 100:F1} {format} × {C * 100:F1} {format}",
				("mm") => $"{A * 1000:F0} {format} × {B * 1000:F0} {format} × {C * 1000:F0} {format}",
				_ => throw new FormatException(),
			};
		}
		// equals

		public bool Equals(Pudelko? other)
		{
			if (other is null)
				return false;
			if (Object.ReferenceEquals(this, other))
				return true;

			double[] p1 = { A, B, C };
			Array.Sort(p1);
			double[] p2 = { other.A, other.B, other.C };
			Array.Sort(p2);

			return (p1[0] == p2[0] && p1[1] == p2[1] && p1[2] == p2[2]);
		}

		public override bool Equals(object? obj)
		{
			if (obj is Pudelko)
				return Equals((Pudelko)obj);
			else
				return false;
		}

		public override int GetHashCode() => (A, B, C).GetHashCode();

		public static bool operator ==(Pudelko m1, Pudelko m2)
		{
			return m1.Equals(m2);
		}

		public static bool operator !=(Pudelko m1, Pudelko m2)
		{
			return !m1.Equals(m2);
		}





		// properteis returning pole and objetosc
		public double Objetosc => Math.Round(A * B * C, 9);
        public double Pole => Math.Round((A * B * 2) + (A * C * 2) + (B * C * 2), 6);
        
        
       
    }
}
