using System;
using System.Text.RegularExpressions;

namespace LandRush.Cadastre.Russia
{
	/// <summary xml:lang="ru">
	/// Паспорт РФ
	/// </summary>
	public class Passport
	{
		private string series;
		private string number;
		private DateTime issueDate;
		private string issuer;
		private Person person;

		protected Passport() { }

		public Passport(string series, string number, Person person)
		{
			if (!Regex.IsMatch(series, "^[0-9]{4}$"))
			{
				throw new ArgumentException("Series must contain 4 digits", nameof(series));
			}
			if (!Regex.IsMatch(number, "^[0-9]{6}$"))
			{
				throw new ArgumentException("Number must contain 6 digits", nameof(number));
			}

			this.series = series;
			this.number = number;
			this.issueDate = DateTime.Now;
			this.issuer = string.Empty;
			this.person = person ?? throw new ArgumentNullException(nameof(person));
		}

		/// <summary xml:lang="ru">
		/// Серия
		/// </summary>
		public virtual string Series => this.series;

		/// <summary xml:lang="ru">
		/// Номер
		/// </summary>
		public virtual string Number => this.number;

		/// <summary xml:lang="ru">
		/// Дата выдачи
		/// </summary>
		public virtual DateTime IssueDate
		{
			get => this.issueDate;
			set => this.issueDate = value;
		}

		/// <summary xml:lang="ru">
		/// Выдавший орган
		/// </summary>
		public virtual string Issuer
		{
			get => this.issuer;
			set => this.issuer = value;
		}

		/// <summary xml:lang="ru">
		/// Удостоверяемое лицо
		/// </summary>
		public virtual Person Person => this.person;

		public override bool Equals(object obj)
		{
			if (obj != null && obj is Passport)
			{
				var other = obj as Passport;
				return
					this.series == other.series &&
					this.number == other.number;
			}
			else
			{
				return false;
			}
		}

		public override int GetHashCode()
		{
			return
				this.series.GetHashCode() ^
				this.number.GetHashCode();
		}
	}
}
