using System;
using Pudełko;
using PudelkoNamespace.PudelkoLib;

namespace ExtensionMethod
{
	public static class KompresjaPudelka
	{
		public static Pudelko Kompresuj(this Pudelko pudelko)
		{
			double edge = Math.Cbrt(pudelko.Objetosc);
			return new Pudelko(edge, edge, edge);
		}
	}
}