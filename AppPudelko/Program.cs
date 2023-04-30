using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System;
using PudelkoNamespace.Enums;
using PudelkoNamespace.PudelkoLib;
using System.Globalization;

namespace Pudełko
{

	class Program
	{
		public static void Main(string[] args)
		{
			try
			{
				//dla pudełka o wymiarach kolejno 2.5, 9.321 i 0.1, ToString("mm") zwraca napis "2500 mm × 9321 mm × 100 mm"

				CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");

				Pudelko pudlo = new();

				List<Pudelko> list = new();
				list.Add(new Pudelko(5, 3, 2, UnitOfMeasure.meter));
				list.Add(new Pudelko(0.03, 0.01, 0.01, UnitOfMeasure.meter));





				foreach (Pudelko p in list)
				{
					Console.WriteLine($"{p}, obj: {p.Objetosc}, pole: {p.Pole}, sumakrawedzi: {p.SumaKrawedzi}.");
				}

				list.Sort(new Pudelko());

				Console.WriteLine();
				foreach (Pudelko p in list)
				{
					Console.WriteLine($"{p}, obj: {p.Objetosc}, pole: {p.Pole}, sumakrawedzi: {p.SumaKrawedzi}.");
				}

				Pudelko ss = Pudelko.Parse("1 m x 1 m x 1 m");
				Pudelko sss = new(8, 1, 1);
				Console.WriteLine(ss + sss);


				Console.WriteLine(sss.ToString("cm"));

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}

	}
		
}