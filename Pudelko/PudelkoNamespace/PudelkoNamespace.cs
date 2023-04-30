using PudelkoNamespace.Enums;
using System;
using System.Collections;
using System.Collections.Immutable;
using System.Data.Common;
using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace PudelkoNamespace.PudelkoLib
{
	public sealed class Pudelko : IFormattable, IEquatable<Pudelko>, IEnumerable<double>, IComparer<Pudelko>
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

			if (ReturnMeters(a, type) > 10 || ReturnMeters(b, type) > 10 || ReturnMeters(c, type) > 10)
			{
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
		public Pudelko(double A, double B, UnitOfMeasure type = UnitOfMeasure.meter) : this(A, B, 10, type)
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
				_c *= 10;
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
		public double A => ReturnMeters(_a, Measure);
		public double B => ReturnMeters(_b, Measure);
		public double C => ReturnMeters(_c, Measure);

		// string inplementation
		public string ToString(string? format = "m", IFormatProvider? provider = default)
		{
			if (format is null)
				format = "m";

			switch (format.ToLower())
			{
				case "m":
					return $"{A.ToString("F3")} {format} × {B.ToString("F3")} {format} × {C.ToString("F3")} {format}";

				case ("cm"):
					return $"{A * 100:F1} {format} × {B * 100:F1} {format} × {C * 100:F1} {format}";

				case ("mm"):
					return $"{A * 1000:F0} {format} × {B * 1000:F0} {format} × {C * 1000:F0} {format}";

				default:
					throw new FormatException();
			}
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

		// box stacking operator
		public static Pudelko operator +(Pudelko p1, Pudelko p2)
		{
			double[] pudelko1 = { p1.A, p1.B, p1.C };
			double[] pudelko2 = { p2.A, p2.B, p2.C };
			Array.Sort(pudelko1);
			Array.Sort(pudelko2);

			double x, y, z;
			x = pudelko1[0] + pudelko2[0];
			if (pudelko1[1] > pudelko2[1])
				y = pudelko1[1];
			else
				y = pudelko2[1];
			if (pudelko1[2] > pudelko2[2])
				z = pudelko1[2];
			else
				z = pudelko2[2];

			return new Pudelko(x, y, z);
		}

		// conversion method 
		public static explicit operator double[](Pudelko pudelko) => new double[] { pudelko.A, pudelko.B, pudelko.C };
		public static implicit operator Pudelko((int a, int b, int c) krawedzie) => new Pudelko((double)krawedzie.a / 1000, (double)krawedzie.b / 1000, (double)krawedzie.c / 1000);

		// parsing method 
		public static Pudelko Parse(string text)
		{
			var values = text.Split(' ', 'x');
			string[] valuesTemp = new string[6];

			int i = 0;
			foreach (var val in values)
			{
				if (!string.IsNullOrWhiteSpace(val))
				{
					valuesTemp[i++] = val;
				}
			}

			if (valuesTemp.Length != 6)
			{
				throw new ArgumentException("Invalid text format", nameof(text));
			}

			double.TryParse(valuesTemp[0], out double a);
			double.TryParse(valuesTemp[2], out double b);
			double.TryParse(valuesTemp[4], out double c);

			string unitA = valuesTemp[1];
			string unitB = valuesTemp[3];
			string unitC = valuesTemp[5];

			return new Pudelko(
				a * ParseUnit(unitA),
				b * ParseUnit(unitB),
				c * ParseUnit(unitC)
			);
		}
		// enumerator
		public IEnumerator<double> GetEnumerator()
		{
			yield return A;
			yield return B;
			yield return C;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		private static double ParseUnit(string unit)
		{
			return unit switch
			{
				"m" => 1.0,
				"cm" => 0.01,
				"mm" => 0.001,
				_ => throw new ArgumentException($"Unknown unit: {unit}")
			};
		}

		// adding compare method
		public double SumaKrawedzi => A + B + C;
		public int CompareBox(Pudelko x, Pudelko y)
		{
			if (x.Objetosc > y.Objetosc)
				return 1;
			else if (x.Objetosc < y.Objetosc)
				return -1;
			else
			{
				if (x.Pole > y.Pole) return 1;
				else if (x.Pole < y.Pole) return -1;
				else
				{
					if (x.SumaKrawedzi > y.SumaKrawedzi) return 1;
					else if (x.SumaKrawedzi < y.SumaKrawedzi) return -1;
					else return 0;
				}
			}
		}

		int IComparer<Pudelko>.Compare(Pudelko? x, Pudelko? y)
		{
			if (x.Objetosc > y.Objetosc)
				return 1;
			else if (x.Objetosc < y.Objetosc)
				return -1;
			else
			{
				if (x.Pole > y.Pole) return 1;
				else if (x.Pole < y.Pole) return -1;
				else
				{
					if (x.SumaKrawedzi > y.SumaKrawedzi) return 1;
					else if (x.SumaKrawedzi < y.SumaKrawedzi) return -1;
					else return 0;
				}
			}
		}
		public double this[int index]
		{
			get
			{
				return index switch
				{
					0 => A,
					1 => B,
					2 => C,
					_ => throw new IndexOutOfRangeException(),
				};
			}
		}
		
	}

}
