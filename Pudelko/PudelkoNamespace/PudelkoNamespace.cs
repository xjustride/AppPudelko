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

        public int UnitOfMeasure { get; set; }
        
        // meter returning function "ReturnMeters"
        public static double ReturnMeters(double value, UnitOfMeasure m)
        {
            if (m == UnitOfMeasure.meter)
            {
                return Math.Round(value,14);
            }
            else if (m == UnitOfMeasure.milimeter)
            {
                if (value < 1) return 0;

                return Math.Round(value / 1000,14);
            }
            else if (m == UnitOfMeasure.centimeter)
            {
                if (value < 0.1) return 0;

                return Math.Round(value / 100, 14);
            }
            else throw new FormatException("Invalid type for MeasureType enum.");
        }
        
        
    }
    




}
