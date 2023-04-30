using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System;
using ExtensionMethod;
using PudelkoNamespace.Enums;
using PudelkoNamespace.PudelkoLib;

namespace Pudełko
{
	public delegate int Comparison<Pudelko>(Pudelko p1, Pudelko p2);

	class Program
	{
		static void Main(string[] args)
		{
			//Tworzenie obiektów klasy Pudelko
			Pudelko p1 = new Pudelko(54, 54, 54, UnitOfMeasure.centimeter);
			Pudelko p2 = new Pudelko(0.7, 42, UnitOfMeasure.centimeter);
			Pudelko p3 = new Pudelko(19.3, 9, 15, UnitOfMeasure.centimeter);
			Pudelko p4 = new Pudelko(4, 5, 6);
			Pudelko p5 = new Pudelko(2, 3, 1, UnitOfMeasure.meter);
			Pudelko p10 = new Pudelko(3, 1, 2, UnitOfMeasure.meter);

			//Wyświetlanie informacji o pudełkach
			Console.WriteLine(p1.ToString());
			Console.WriteLine(p2.ToString());
			Console.WriteLine(p3.ToString());
			Console.WriteLine(p4.ToString());
			Console.WriteLine(p5.ToString());
			Console.WriteLine(p10.ToString());

			//Wyświetlanie informacji o pudełkach w różnych jednostkach
			Console.WriteLine(p5.ToString("cm"));
			Console.WriteLine(p5.ToString("mm"));
			Console.WriteLine(p1.ToString(null));

			//Wyświetlanie pola i objętości pudełek
			Console.WriteLine("P = " + p1.Pole);
			Console.WriteLine("V = " + p1.Objetosc);

			//Porównywanie pudełek
			Console.WriteLine(p5 == p10);
			Console.WriteLine(p1.Equals(p2));

			//Dodawanie pudełek
			Console.WriteLine(p1 + p2);

			//Indexer i iterator
			Console.WriteLine("p2[1] = " + p2[1]);
			Console.WriteLine("p3: ");
			foreach (var item in p3)
				Console.WriteLine(item);

			//Parsowanie
			Pudelko ss = Pudelko.Parse("1 m x 1 m x 1 m");
			Pudelko sss = new(8, 1, 1);
			Console.WriteLine(ss + sss);

			//Kompresja
			Pudelko p8 = ss.Kompresuj();
			Console.WriteLine(p8.ToString());

			//Sortowanie pudełek
			List<Pudelko> boxes = new List<Pudelko>();
			boxes.Add(new Pudelko(2, 3, 9));
			boxes.Add(new Pudelko(1.5, 2, 3));
			boxes.Add(new Pudelko(8, 4, 5));
			boxes.Add(new Pudelko(1, 2, 2.5));
			boxes.Add(new Pudelko(3, 6, 4));


			Console.WriteLine("Nieposortowane");
			foreach (Pudelko box in boxes)
				Console.WriteLine(box);

			boxes.Sort(delegate (Pudelko p1, Pudelko p2)
			{
				if (p1.Objetosc != p2.Objetosc)
					return (p1.Objetosc.CompareTo(p2.Objetosc));
				else
				{
					if (p1.Pole != p2.Pole)
						return (p1.Pole.CompareTo(p2.Pole));
					else
					{
						if ((p1.A + p1.B + p1.C) != (p2.A + p2.B + p2.C))
							return (p1.A + p1.B + p1.C).CompareTo(p2.A + p2.B + p2.C);
						else return 0;
					}
				}
			});

			Console.WriteLine("Posortowane");
			foreach (Pudelko box in boxes)
				Console.WriteLine(box);

			Console.ReadKey();
		}

	}
}