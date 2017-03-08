using System;
using System.Globalization;
using static System.Math;

namespace StrubT.Buas.LinAlg.Matrix.GaussianElimination {

	/// <summary>
	/// Event fired after every column elimination step
	/// </summary>
	/// <param name="column">Number of the eliminated column</param>
	public delegate void ColumnEliminationCompleted(int column);

	/// <summary>
	/// Mathematical fraction
	/// </summary>
	public class Fraction : IFormattable, IComparable, IComparable<Fraction>, IEquatable<Fraction> {

		#region properties / constructor

		/// <summary>
		/// Numerator of the fraction
		/// </summary>
		public int Numerator { get; }

		/// <summary>
		/// Denominator of the fraction
		/// </summary>
		public int Denominator { get; }

		/// <summary>
		/// Constructor
		/// Create a new fraction with a <c>denominator</c> of 1
		/// </summary>
		/// <param name="numerator">Numerator of the fraction</param>
		public Fraction(int numerator) : this(numerator, 1) { }

		/// <summary>
		/// Constructor
		/// Create a new fraction
		/// </summary>
		/// <param name="numerator">Numerator of the fraction</param>
		/// <param name="denominator">Denominator of the fraction</param>
		public Fraction(int numerator, int denominator) {

			if (denominator == 0) throw new DivideByZeroException("Cannot have a denominator 0.");

			var gcd = Gcd(Abs(numerator), Abs(denominator));
			//use greatest common divisor to simplify fractions

			//use denominator / Abs(denominator) to apply denominators sign to the numerator (prevent negative denominators)
			Numerator = numerator * (denominator / Abs(denominator)) / gcd;
			Denominator = Abs(denominator) / gcd;
		}

		int Gcd(int a, int b) {

			//EUCLID'S ALGORITHM
			//source: http://stackoverflow.com/a/2941224/1325979 (shortened)
			while (b > 0)
				b = a % (a = b);
			return a;
		}
		#endregion

		#region mathematical operators

		/// <summary>
		/// Add two fractions
		/// </summary>
		/// <param name="f1">First fraction / operand</param>
		/// <param name="f2">Second fraction / operand</param>
		/// <returns>Sum of the two fractions as simplified fraction</returns>
		public static Fraction operator +(Fraction f1, Fraction f2) => new Fraction(f1.Numerator * f2.Denominator + f2.Numerator * f1.Denominator, f1.Denominator * f2.Denominator);

		/// <summary>
		/// Negate (change sign of) a fraction
		/// </summary>
		/// <param name="f">Fraction to negate</param>
		/// <returns>Negative (or positive in case input was negative) fraction with same absolute value</returns>
		public static Fraction operator -(Fraction f) => new Fraction(-f.Numerator, f.Denominator);

		/// <summary>
		/// Subtract one fraction from another
		/// </summary>
		/// <param name="f1">First fraction / minuend</param>
		/// <param name="f2">Second fraction / subtrahend</param>
		/// <returns>Difference of the two fractions as simplified fraction</returns>
		public static Fraction operator -(Fraction f1, Fraction f2) => new Fraction(f1.Numerator * f2.Denominator - f2.Numerator * f1.Denominator, f1.Denominator * f2.Denominator);

		/// <summary>
		/// Multiply two fractions
		/// </summary>
		/// <param name="f1">First fraction / factor</param>
		/// <param name="f2">Second fraction / factor</param>
		/// <returns>Product of the two fractions as simplified fraction</returns>
		public static Fraction operator *(Fraction f1, Fraction f2) => new Fraction(f1.Numerator * f2.Numerator, f1.Denominator * f2.Denominator);

		/// <summary>
		/// Divide one fraction by another
		/// </summary>
		/// <param name="f1">First fraction / dividend</param>
		/// <param name="f2">Second fraction / divisor</param>
		/// <returns>Quotient of the two fractions as simplified fraction</returns>
		public static Fraction operator /(Fraction f1, Fraction f2) => new Fraction(f1.Numerator * f2.Denominator, f1.Denominator * f2.Numerator);

		/// <summary>
		/// Check two fractions for equality
		/// </summary>
		/// <param name="f1">First fraction</param>
		/// <param name="f2">Second fraction</param>
		/// <returns><c>true</c> if the to fractions numerators and denominators are equal, <c>false</c> otherwise</returns>
		public static bool operator ==(Fraction f1, Fraction f2) => f1.Numerator == f2.Numerator && f1.Denominator == f2.Denominator;

		/// <summary>
		/// Check two fractions for inequality
		/// </summary>
		/// <param name="f1">First fraction</param>
		/// <param name="f2">Second fraction</param>
		/// <returns><c>true</c> if the to fractions numerators or denominators are different, <c>false</c> otherwise</returns>
		public static bool operator !=(Fraction f1, Fraction f2) => !(f1 == f2);

		/// <summary>
		/// Check if one fraction is smaller or equal another
		/// </summary>
		/// <param name="f1">First fraction</param>
		/// <param name="f2">Second fraction</param>
		/// <returns><c>true</c> if first fraction is smaller or equal the second one, <c>false</c> otherwise</returns>
		public static bool operator <=(Fraction f1, Fraction f2) => f1.Numerator * f2.Denominator <= f2.Numerator * f1.Denominator;

		/// <summary>
		/// Check if one fraction is smaller another
		/// </summary>
		/// <param name="f1">First fraction</param>
		/// <param name="f2">Second fraction</param>
		/// <returns><c>true</c> if first fraction is smaller the second one, <c>false</c> otherwise</returns>
		public static bool operator <(Fraction f1, Fraction f2) => f1.Numerator * f2.Denominator < f2.Numerator * f1.Denominator;

		/// <summary>
		/// Check if one fraction is bigger or equal another
		/// </summary>
		/// <param name="f1">First fraction</param>
		/// <param name="f2">Second fraction</param>
		/// <returns><c>true</c> if first fraction is bigger or equal the second one, <c>false</c> otherwise</returns>
		public static bool operator >=(Fraction f1, Fraction f2) => f1.Numerator * f2.Denominator >= f2.Numerator * f1.Denominator;

		/// <summary>
		/// Check if one fraction is bigger another
		/// </summary>
		/// <param name="f1">First fraction</param>
		/// <param name="f2">Second fraction</param>
		/// <returns><c>true</c> if first fraction is bigger the second one, <c>false</c> otherwise</returns>
		public static bool operator >(Fraction f1, Fraction f2) => f1.Numerator * f2.Denominator > f2.Numerator * f1.Denominator;
		#endregion

		#region casting operators

		/// <summary>
		/// Cast an integer number to a fraction
		/// </summary>
		/// <param name="i">Integer to use as numerator of fraction</param>
		/// <returns>A fraction with <c>i</c> as its numerator and <c>1</c> as its denominator</returns>
		public static implicit operator Fraction(int i) => new Fraction(i);

		/// <summary>
		/// Cast a fraction to a floating point number
		/// </summary>
		/// <param name="f">Fraction to cast</param>
		/// <returns>The quotient of the fraction's numerator and denominator as a floating-point number</returns>
		public static implicit operator double(Fraction f) => (double)f.Numerator / f.Denominator;
		#endregion

		#region comparison

		/// <summary>
		/// Generate a unique hash code for the fraction
		/// </summary>
		/// <returns>A hash code based on the fraction's numerator and denominator</returns>
		public override int GetHashCode() => ((17 + Numerator) * 37 + Denominator) * 37;

		/// <summary>
		/// Check if a second fraction equals this one
		/// </summary>
		/// <param name="obj">Second fraction (or any object, really)</param>
		/// <returns>If second object is a fraction and the fraction's numerator and denominator equal this ones</returns>
		public override bool Equals(object obj) => obj is Fraction && Equals((Fraction)obj);

		/// <summary>
		/// Check if a second fraction equals this one
		/// </summary>
		/// <param name="other">Second fraction</param>
		/// <returns>If second fraction's numerator and denominator equal this ones</returns>
		public bool Equals(Fraction other) => other != null && Numerator == other.Numerator && Denominator == other.Denominator;

		/// <summary>
		/// Compare this fraction to an other one
		/// </summary>
		/// <param name="obj">Second fraction (or any object, really)</param>
		/// <returns>A negative number (<c>-1</c>) if this fraction is smaller the second one, <c>0</c> if they are equal and a positive number (<c>1</c>) if this one is bigger the second one</returns>
		int IComparable.CompareTo(object obj) => obj is Fraction ? CompareTo((Fraction)obj) : -1;

		/// <summary>
		/// Compare this fraction to an other one
		/// </summary>
		/// <param name="other">Second fraction</param>
		/// <returns>A negative number (<c>-1</c>) if this fraction is smaller the second one, <c>0</c> if they are equal and a positive number (<c>1</c>) if this one is bigger the second one</returns>
		public int CompareTo(Fraction other) => other != null ? (Numerator * other.Denominator).CompareTo(other.Numerator * Denominator) : 1;
		#endregion

		#region string representation

		/// <summary>
		/// Get a string representation of this fraction
		/// </summary>
		/// <returns>A string representation of this fraction's numerator and denominator</returns>
		public override string ToString() => ToString(null, null);

		/// <summary>
		/// Get a string representation of this fraction
		/// </summary>
		/// <param name="format">Format of the string representation (ignored)</param>
		/// <returns>A string representation of this fraction's numerator and denominator</returns>
		public string ToString(string format) => ToString(format, null);

		/// <summary>
		/// Get a string representation of this fraction
		/// </summary>
		/// <param name="formatProvider">Format provider (culture) to use when formatting the numbers</param>
		/// <returns>A string representation of this fraction's numerator and denominator</returns>
		public string ToString(IFormatProvider formatProvider) => ToString(null, formatProvider);

		/// <summary>
		/// Get a string representation of this fraction
		/// </summary>
		/// <param name="format">Format of the string representation (ignored)</param>
		/// <param name="formatProvider">Format provider (culture) to use when formatting the numbers</param>
		/// <returns>A string representation of this fraction's numerator and denominator</returns>
		public string ToString(string format, IFormatProvider formatProvider) => string.Format(formatProvider ?? CultureInfo.CurrentCulture, Denominator == 1 ? "{0:#,##0}" : "{0:#,##0}/{1:#,##0}", Numerator, Denominator);
		#endregion
	}

	/// <summary>
	/// Mathematical matrix (as used in the Gaussian elimination)
	/// </summary>
	public class Matrix {

		/// <summary>
		/// Event fired after every column elimination step
		/// </summary>
		public event ColumnEliminationCompleted ColumnEliminationCompleted;

		#region properties

		/// <summary>
		/// Number of rows / equations in the matrix
		/// </summary>
		public int NofRows => matrix.GetLength(0);

		/// <summary>
		/// Number of rows / equations in the matrix
		/// </summary>
		public int NofEquations => NofRows;

		/// <summary>
		/// Number of columns in the matrix
		/// </summary>
		public int NofColumns => matrix.GetLength(0);

		/// <summary>
		/// Number of variables in the matrix
		/// </summary>
		public int NofVariables => NofColumns - 1;

		readonly Fraction[,] matrix;
		#endregion

		#region constructors

		/// <summary>
		/// Constructor
		/// Create an empty matrix of size <c>size</c>
		/// </summary>
		/// <param name="size">Size of the matrix (number of equations or rows)</param>
		public Matrix(int size) : this(size, size) { }

		/// <summary>
		/// Constructor
		/// Create an empty matrix of size <c>nofEquations</c> x <c>nofVariables + 1</c>
		/// </summary>
		/// <param name="nofEquations">Number of rows / equations in the matrix</param>
		/// <param name="nofVariables">Number of variables in the matrix</param>
		public Matrix(int nofEquations, int nofVariables) => matrix = new Fraction[nofEquations, nofVariables + 1];

		/// <summary>
		/// Constructor
		/// Wrap a predefined matrix
		/// </summary>
		/// <param name="matrix">Values of the predefined matrix</param>
		internal Matrix(Fraction[,] matrix) => this.matrix = matrix;
		#endregion

		#region instantiation

		/// <summary>
		/// Value at position <c>row</c>,<c>col</c>
		/// </summary>
		/// <param name="row">Row index (starts with <c>0</c>)</param>
		/// <param name="col">Column index (starts with <c>0</c>)</param>
		/// <returns>Value at position <c>row</c>,<c>col</c></returns>
		public Fraction this[int row, int col] {
			set => matrix[row, col] = value;
			get => matrix[row, col];
		}
		#endregion

		#region elimination

		/// <summary>
		/// Run the Gaussian elimination algorithm
		/// </summary>
		public void Eliminate() {

			for (var rc = 0; rc < NofVariables && rc < NofRows; rc++) {

				//1) bestimme weitesten links stehende Spalte mit min. einer Zeile =/= 0
				var skip = true;
				for (var r = 0; skip && r < NofRows; r++)
					if (this[r, rc] != 0) skip = false;
				if (skip) continue;

				//2) falls nötig, Zeilen vertauschen
				if (this[rc, rc] == 0) {
					skip = true;
					for (var r = rc + 1; skip && r < NofRows; r++)
						if (this[r, rc] != 0) {
							SwapRows(rc, r);
							skip = false;
						}
					if (skip) continue;
				}

				//3) falls das 1. Element der Stufe a ist, mit 1/a multiplizieren
				MultiplyRow(rc, 1 / this[rc, rc]);

				//4) passende Vielfache der 1. Zeile addieren, um unter der führenden Eins Nullen zu erhalten
				for (var j = 0; j < NofRows; j++) {
					if (rc == j) continue;

					AddRow(j, -this[j, rc] / this[rc, rc], rc);
				}

				ColumnEliminationCompleted?.Invoke(rc);
			}
		}

		void SwapRows(int d, int s) {

			for (var c = 0; c < NofColumns; c++) {
				var t = this[d, c];
				this[d, c] = this[s, c];
				this[s, c] = t;
			}
		}

		void MultiplyRow(int r, Fraction f) {

			for (var c = 0; c < NofColumns; c++)
				this[r, c] *= f;
		}

		void AddRow(int d, Fraction f, int s) {

			for (var c = 0; c < NofColumns; c++)
				this[d, c] += f * this[s, c];
		}
		#endregion
	}
}
