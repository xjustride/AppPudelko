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
    public sealed class Pudelko
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



    }

}
