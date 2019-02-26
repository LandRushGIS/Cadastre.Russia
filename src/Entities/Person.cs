using System.Collections.Generic;
using Regex = System.Text.RegularExpressions.Regex;
using ArgumentException = System.ArgumentException;
using DateTime = System.DateTime;

namespace LandRush.Cadastre.Russia
{
	/// <summary xml:lang="ru">
	/// Физическое лицо
	/// </summary>
	public class Person
	{
		private readonly int id;
		private string familyName;
		private string firstName;
		private string patronymic;
		private DateTime? birthDate = null;
		private DateTime? deathDate = null;
		private string tin = null;
		private string address = null;
		private ISet<string> phoneNumbers = new SortedSet<string>();

		public Person(int id, string familyName, string firstName, string patronymic)
		{
			this.id = id;
			this.familyName = familyName;
			this.firstName = firstName;
			this.patronymic = patronymic;
		}

		protected Person() : this(0, string.Empty, string.Empty, string.Empty) { }

		/// <summary xml:lang="ru">
		/// Внутренний идентификатор
		/// </summary>
		public virtual int Id => this.id;

		/// <summary xml:lang="ru">
		/// Фамилия
		/// </summary>
		public virtual string FamilyName
		{
			get => this.familyName;
			set => this.familyName = value;
		}

		/// <summary xml:lang="ru">
		/// Имя
		/// </summary>
		public virtual string FirstName
		{
			get => this.firstName;
			set => this.firstName = value;
		}

		/// <summary xml:lang="ru">
		/// Отчество
		/// </summary>
		public virtual string Patronymic
		{
			get => this.patronymic;
			set => this.patronymic = value;
		}

		/// <summary xml:lang="ru">
		/// Известна ли дата рождения?
		/// </summary>
		public virtual bool HasBirthDate =>
			this.birthDate.HasValue;

		/// <summary xml:lang="ru">
		/// Дата рождения
		/// </summary>
		public virtual DateTime? BirthDate
		{
			get => this.birthDate;
			set => this.birthDate = value;
		}

		/// <summary xml:lang="ru">
		/// Задана ли дата смерти?
		/// </summary>
		public virtual bool HasDeathDate =>
			this.deathDate.HasValue;

		/// <summary xml:lang="ru">
		/// Дата смерти
		/// </summary>
		public virtual DateTime? DeathDate
		{
			get => this.deathDate;
			set => this.deathDate = value;
		}

		/// <summary xml:lang="ru">
		/// Задан ли ИНН?
		/// </summary>
		public virtual bool HasTIN =>
			this.tin != null;

		/// <summary>
		/// Taxpayer identification number (TIN)
		/// </summary>
		/// <summary xml:lang="ru">
		/// Идентификационный номер налогоплательщика (ИНН)
		/// </summary>
		public virtual string TIN
		{
			get => this.tin;
			set => this.tin = IsTINValid(value)?
				value : throw new ArgumentException("Person TIN must contain 12 digits", nameof(value));
		}

		/// <summary xml:lang="ru">
		/// Известен ли адрес?
		/// </summary>
		public virtual bool HasAddress =>
			this.address != null;

		/// <summary xml:lang="ru">
		/// Адрес
		/// </summary>
		public virtual string Address
		{
			get => this.address;
			set => this.address = value;
		}

		/// <summary xml:lang="ru">
		/// Номера телефонов
		/// </summary>
		public virtual ISet<string> PhoneNumbers =>
			this.phoneNumbers;

		private static bool IsTINValid(string tin) =>
			// TODO: add control sum checks
			tin == null || Regex.IsMatch(tin, "^[0-9]{12}$");
	}
}
