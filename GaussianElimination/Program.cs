using System;
using System.Linq;
using c = System.Console;
using cc = System.ConsoleColor;

namespace StrubT.Buas.LinAlg.Matrix.GaussianElimination {

	static class Program {

		static void Main() {

			//testFractions();
			TestEliminatePredef();
			//testEliminateRandom();
		}

		static void TestFractions() {

			c.WriteLine("*** FRACTION TEST ***");

			var f1 = new Fraction(5);
			var f2 = new Fraction(-5);
			var f3 = new Fraction(5, 1);
			var f4 = new Fraction(-5, 1);
			var f5 = new Fraction(5, -1);
			var f6 = new Fraction(5, 2);
			var f7 = new Fraction(17, 18);
			var f8 = new Fraction(225, 15);
			var f9 = new Fraction(-99, -18);

			c.WriteLine("{0} == {1}: {2}", f1, f2, f1 == f2);
			c.WriteLine("{0} == -{1}: {2}", f1, f2, f1 == -f2);
			c.WriteLine("{0} == {1}: {2}", f1, f3, f1 == f3);
			c.WriteLine("{0} == {1}: {2}", f2, f4, f2 == f4);
			c.WriteLine("{0} == {1}: {2}", f2, f5, f2 == f5);

			c.WriteLine("{0} + {1}: {2}", f1, f3, f1 + f3);
			c.WriteLine("{0} - {1}: {2}", f1, f3, f1 - f3);

			c.WriteLine("{0} + {1}: {2}", f1, f2, f1 + f2);
			c.WriteLine("{0} - {1}: {2}", f1, f2, f1 - f2);

			c.WriteLine("{0} + {1}: {2}", f1, f6, f1 + f6);
			c.WriteLine("{0} - {1}: {2}", f1, f6, f1 - f6);
			c.WriteLine("{0} * {1}: {2}", f1, f6, f1 * f6);
			c.WriteLine("{0} / {1}: {2}", f1, f6, f1 / f6);

			c.WriteLine("{0}", f7);
			c.WriteLine("{0}", f8);
			c.WriteLine("{0}", f9);
		}

		static void TestEliminatePredef() {

			c.WriteLine("*** PREDEFINED ELIMINATION TEST ***");

			TestEliminate(new Fraction[,] {
				{ 3, 2, -3 },
				{ -2, 8, 4 }
			});

			TestEliminate(new Fraction[,] {
				{ 1, 1, 2, 9 },
				{ 2, 4, -3, 1 },
				{ 3, 6, -5, 0 }
			});

			TestEliminate(new Fraction[,] {
				{ 0, 1, 8, 0, 0, 4 },
				{ 0, 0, 0, 1, 0, 3 },
				{ 0, 0, 0, 0, 1, 1 }
			});

			TestEliminate(new Fraction[,] {
				{ 1, 2, 3, 4 },
				{ 0, 1, 5, 6 },
				{ 0, 0, 1, 7 }
			});

			TestEliminate(new Fraction[,] {
				{ 0, 0, -2, 0, 7, 12 },
				{ 2, 4, -10, 6, 12, 28 },
				{ 2, 4, -5, 6, -5, -1 }
			});

			TestEliminate(new Fraction[,] {
				{ 1, 0, 0, 4, -1 },
				{ 0, 1, 0, 2, 6 },
				{ 0, 0, 1, 3, 2 }
			});

			TestEliminate(new Fraction[,] {
				{ 1, 6, 0, 0, 4, -2 },
				{ 0, 0, 1, 0, 3, 1 },
				{ 0, 0, 0, 1, 5, 2 },
				{ 0, 0, 0, 0, 0, 0 }
			});

			TestEliminate(new Fraction[,] {
				{ 1, 0, 0, 0 },
				{ 0, 1, 2, 0 },
				{ 0, 0, 0, 1 }
			});

			TestEliminate(new Fraction[,] {
				{ 1, 2, 3, 4, 1 },
				{ 0, 0, 1, -1, 1 },
				{ 0, 0, 0, 1, 4 }
			});

			TestEliminate(new Fraction[,] {
				{ 3, 5, 1, 2, 1 },
				{ 2, -4, 3, 7, 2 },
				{ 4, 14, -1, -3, 0 },
				{ 13, 7, 9, 20, 7 }
			});
		}

		static void TestEliminateRandom() {

			c.WriteLine("*** RANDOM ELIMINATION TEST ***");

			const int EQUATIONS_VARIABLES_MIN = 1;
			const int EQUATIONS_VARIABLES_MAX = 10;

			const int VALUE_MIN = 1;
			const int VALUE_MAX = 7;

			var r = new Random();
			var equations = r.Next(EQUATIONS_VARIABLES_MIN, EQUATIONS_VARIABLES_MAX);
			var variables = r.Next(EQUATIONS_VARIABLES_MIN, EQUATIONS_VARIABLES_MAX);

			var m = new Matrix(equations, variables);
			for (int e = 0; e < equations; e++)
				for (int v = 0; v <= variables; v++)
					m[e, v] = new Fraction(r.Next(VALUE_MIN, VALUE_MAX));

			TestEliminate(m);
		}

		static void TestEliminate(Fraction[,] m) {

			TestEliminate(new Matrix(m));
		}

		static void TestEliminate(Matrix m) {

			var f = c.ForegroundColor;
			var l = new object();

			c.ForegroundColor = cc.Yellow;
			c.WriteLine("\n\n\n*** {0} EQUATION{1}, {2} VARIABLE{3}, BEFORE ELIMINATION ***", m.NofEquations, m.NofEquations != 1 ? "S" : null, m.NofVariables, m.NofVariables != 1 ? "S" : null);
			c.ForegroundColor = f;
			Print(m);

			m.ColumnEliminationCompleted += col => {
				lock (l) {
					c.ForegroundColor = cc.DarkGray;
					c.WriteLine("\n*** {0} EQUATION{1}, {2} VARIABLE{3}, AFTER COLUMN {4} ***", m.NofEquations, m.NofEquations != 1 ? "S" : null, m.NofVariables, m.NofVariables != 1 ? "S" : null, col + 1);
					Print(m);
					c.ForegroundColor = f;
				}
			};
			m.Eliminate();

			c.ForegroundColor = cc.Green;
			c.WriteLine("\n*** {0} EQUATION{1}, {2} VARIABLE{3}, AFTER ELIMINATION ***", m.NofEquations, m.NofEquations != 1 ? "S" : null, m.NofVariables, m.NofVariables != 1 ? "S" : null);
			c.ForegroundColor = f;
			Print(m);
		}

		static void Print(Matrix m) {

			var s = new string[m.NofRows][];
			for (int i = 0; i < m.NofRows; i++)
				s[i] = new string[m.NofColumns];

			var l = 0;
			for (int i = 0; i < m.NofRows; i++)
				for (int j = 0; j < m.NofColumns; j++) {
					s[i][j] = m[i, j].ToString();
					if (l < s[i][j].Length) l = s[i][j].Length;
				}

			var f = string.Format("{{0,{0}}}", l);

			c.WriteLine("---{0}---", new string(' ', (l + 2) * m.NofColumns - 2));
			for (int i = 0; i < m.NofRows; i++)
				c.WriteLine("| {0} |", string.Join("  ", s[i].Select((n, j) => string.Format((j == m.NofVariables ? "| " : null) + f, n))));
			c.WriteLine("---{0}---", new string(' ', (l + 2) * m.NofColumns - 2));
		}
	}
}
