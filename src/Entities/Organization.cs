using System.Collections.Generic;
using System.Text.RegularExpressions;
using ArgumentException = System.ArgumentException;

namespace LandRush.Cadastre.Russia
{
	/// <summary xml:lang="ru">
	/// Организация
	/// </summary>
	public class Organization
	{
		private readonly int id;
		private string name;
		private string tin = null;
		private string address = null;
		private ISet<string> phoneNumbers = new SortedSet<string>();

		public Organization(int id, string name)
		{
			this.id = id;
			this.name = name;
		}

		protected Organization() : this(0, string.Empty) { }

		/// <summary xml:lang="ru">
		/// Внутренний идентификатор
		/// </summary>
		public virtual int Id => this.id;

		/// <summary xml:lang="ru">
		/// Название
		/// </summary>
		public virtual string Name
		{
			get => this.name;
			set => this.name = value;
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
			set => this.tin = IsTINValid(value) ?
				value : throw new ArgumentException("Organization TIN must contain 10 digits", nameof(value));
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
			Regex.IsMatch(tin, "^[0-9]{10}$");
	}
}
