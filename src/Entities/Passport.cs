using System;
using Regex = System.Text.RegularExpressions.Regex;

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
		private string authorityCode;
		private string authorityName;
		private Person person;

		public Passport(string series, string number, Person person)
		{
			this.series = Regex.IsMatch(series, "^[0-9]{4}$")?
				series : throw new ArgumentException("Series must contain 4 digits", nameof(series));
			this.number = Regex.IsMatch(number, "^[0-9]{6}$")?
				number : throw new ArgumentException("Number must contain 6 digits", nameof(number));
			this.issueDate = DateTime.Now;
			this.authorityCode = null;
			this.authorityName = string.Empty;
			this.person = person ?? throw new ArgumentNullException(nameof(person));
		}

		protected Passport() { }

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
		/// Код подразделения (без разделителя)
		/// </summary>
		public virtual string AuthorityCode
		{
			get => this.authorityCode;
			set => this.authorityCode = IsAuthorityCodeValid(value)?
				value : throw new ArgumentException("AuthorityCode must contain 6 digits", nameof(value));
		}

		/// <summary xml:lang="ru">
		/// Выдавший орган
		/// </summary>
		public virtual string AuthorityName
		{
			get => this.authorityName;
			set => this.authorityName = value;
		}

		/// <summary xml:lang="ru">
		/// Удостоверяемое лицо
		/// </summary>
		public virtual Person Person => this.person;

		public override bool Equals(object obj) =>
			obj is Passport other ?
				this.series == other.series &&
				this.number == other.number :
				false;

		public override int GetHashCode() =>
			this.series.GetHashCode() ^
			this.number.GetHashCode();

		private static bool IsAuthorityCodeValid(string authorityCode) =>
			Regex.IsMatch(authorityCode, "^[0-9]{6}$");
	}
}
