using PudelkoNamespace.Enums;
using System.Collections;
using System.Collections.Immutable;
using System.Data.Common;
using System.Drawing;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;

namespace PudelkoNamespace.PudelkoLib
{
    public sealed class Pudelko 
    {
        private readonly double _a;
        private readonly double _b;
        private readonly double _c;

        
        public double A => ReturnMeters(_a, Measure);
        public double B => ReturnMeters(_b, Measure);
        public double C => ReturnMeters(_c, Measure);
        public UnitOfMeasure Measure { get; set; }

        public double Objetosc => Math.Round(A * B * C, 9);
        public double Pole => Math.Round((A * B * 2) + (A * C * 2) + (B * C * 2), 6);
        public Pudelko(double a, double b, double c, UnitOfMeasure type)
        {
            if (!Enum.IsDefined(typeof(UnitOfMeasure), type))
            {
                throw new FormatException("Invalid type for MeasureType enum.");
            }

            Measure = type;

            if (ReturnMeters(a, type) <= 0 || ReturnMeters(b, type) <= 0 || ReturnMeters(c, type) <= 0)
            {
                throw new ArgumentOutOfRangeException("Dimensions of the box must be positive!");
            }

            if (ReturnMeters(a, type) > 10 || ReturnMeters(b, type) > 10 || ReturnMeters(c, type) > 10)
            {
                throw new ArgumentOutOfRangeException("Box is too big! Maximum dimensions: 10x10x10 Meters.");
            }

            _a = a;
            _b = b;
            _c = c;
        }

        public Pudelko(double A, double B, double C) : this(A, B, C, UnitOfMeasure.meter)
        {
        }

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


        // reprezentacja tekstowa obiektu i przeciążenia
        public override string ToString()
        {
            if (Measure == UnitOfMeasure.centimeter)
            {
                return string.Format($"{A * 100:F3} cm × {B * 100:F3} cm × {C * 100:F3} cm");

            }
            else if (Measure == UnitOfMeasure.milimeter)
            {
                return string.Format($"{A * 1000:F3} mm × {B * 1000:F3} mm × {C * 1000:F3} mm");

            }
            else if (Measure == UnitOfMeasure.meter)
            {
                return string.Format($"{A:F3} m × {B:F3} m × {C:F3} m");
            }
            else
            {
                throw new FormatException("Bad format, avaible formats: 'mm', 'cm', 'm'.");
            }
        }

        public string ToString(string format)
        {
            if (format == "cm")
            {
                return string.Format($"{A * 100:F1} cm × {B * 100:F1} cm × {C * 100:F1} cm");

            }
            else if (format == "mm")
            {
                return string.Format($"{A * 1000:F0} mm × {B * 1000:F0} mm × {C * 1000:F0} mm");

            }
            else if (format == "m")
            {
                return string.Format($"{A:F3} m × {B:F3} m × {C:F3} m");

            }
            else if (format == null)
            {
                return string.Format($"{A:F3} m × {B:F3} m × {C:F3} m");
            }
            else
            {
                throw new FormatException("Bad format, avaible formats: 'mm', 'cm', 'm'.");
            }
        }

        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            if (string.IsNullOrEmpty(format)) format = "m";
            _ = formatProvider ?? CultureInfo.CurrentCulture;

            return format.ToUpperInvariant() switch
            {
                "mm" => string.Format($"{A * 1000:F0} mm × {B * 1000:F0} mm × {C * 1000:F0} mm"),
                "cm" => string.Format($"{A * 100:F1} cm × {B * 100:F1} cm × {C * 100:F1} cm"),
                "m" => string.Format($"{A:F3} m × {B:F3} m × {C:F3} m"),
                _ => ToString(),
            };
        }

        // funkcja pomocnicza zwracajaca metry
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

       


        public static bool operator ==(Pudelko? a, Pudelko? b)
        {
            if (a is null || b is null) return false;
            return a.Equals(b);
        }

        public static bool operator !=(Pudelko? a, Pudelko? b)
        {
            if (a is null || b is null) return false;
            return !a.Equals(b);
        }
    }
}
