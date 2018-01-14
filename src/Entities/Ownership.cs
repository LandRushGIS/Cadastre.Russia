using System;

namespace LandRush.Cadastre.Russia
{
	/// <summary>
	/// Собственник
	/// </summary>
	public abstract class Landholder
	{
		private int id;

		protected Landholder() { }

		public Landholder(int id)
		{
			this.id = id;
		}

		/// <summary xml:lang="ru">
		/// Внутренний идентификатор
		/// </summary>
		public virtual int Id => this.id;

		/// <summary xml:lang="ru">
		/// Имя / название
		/// </summary>
		public abstract string Name { get; }
	}

	/// <summary>
	/// Собственник - физическое лицо
	/// </summary>
	public class PersonLandholder : Landholder
	{
		private Person person;

		protected PersonLandholder() { }

		public PersonLandholder(int id, Person person) : base(id)
		{
			this.person = person ?? throw new ArgumentNullException(nameof(person));
		}

		public override string Name =>
			$"{this.person.FamilyName} {this.person.FirstName} {this.person.Patronymic}";

		public virtual Person Person => this.person;
	}

	/// <summary>
	/// Собственник - организация
	/// </summary>
	public class OrganizationLandholder : Landholder
	{
		private string name;

		protected OrganizationLandholder() { }

		public OrganizationLandholder(int id, string name) : base(id)
		{
			this.name = name ?? throw new ArgumentNullException(nameof(name));
		}

		public override string Name =>
			this.name;
	}

	/// <summary>
	/// Свидетельство о государственной регистрации права
	/// </summary>
	public class ParcelRightsCertificate
	{
		protected ParcelRightsCertificate() { }

		public ParcelRightsCertificate(string series, string number, DateTime date, ParcelRight right, string registrationRecordNumber)
		{
			this.series = series;
			this.number = number;
			this.date = date;
			this.right = right;
			this.registrationRecordNumber = registrationRecordNumber;
		}

		private string series;
		private string number;
		private DateTime date;
		private string registrationRecordNumber;
		private ParcelRight right;

		public virtual string Series =>
			this.series;

		public virtual string Number =>
			this.number;

		public virtual DateTime Date
		{
			get => this.date;
			set => this.date = value;
		}
		public virtual string RegistrationRecordNumber
		{
			get => this.registrationRecordNumber;
			set => this.registrationRecordNumber = value;
		}

		public virtual ParcelRight Right =>
			this.right;

		public override bool Equals(object obj)
		{
			if ((obj == null) || !(obj is ParcelRightsCertificate)) return false;
			else return ((obj as ParcelRightsCertificate).series == this.series) && ((obj as ParcelRightsCertificate).number == this.number);
		}

		public override int GetHashCode()
		{
			return this.series.GetHashCode() ^ (int)this.number.GetHashCode();
		}
	}
}
