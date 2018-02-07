using System;

namespace LandRush.Cadastre.Russia
{
	/// <summary>
	/// Собственник
	/// </summary>
	public abstract class Landholder
	{
		private int id;

		public Landholder(int id) =>
			this.id = id;

		protected Landholder() { }

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

		public PersonLandholder(int id, Person person) : base(id) =>
			this.person = person ?? throw new ArgumentNullException(nameof(person));

		protected PersonLandholder() { }

		public override string Name =>
			$"{this.person.FamilyName} {this.person.FirstName} {this.person.Patronymic}";

		public virtual Person Person => this.person;
	}

	/// <summary>
	/// Собственник - организация
	/// </summary>
	public class OrganizationLandholder : Landholder
	{
		private Organization organization;

		public OrganizationLandholder(int id, Organization organization) : base(id) =>
			this.organization = organization ?? throw new ArgumentNullException(nameof(organization));

		protected OrganizationLandholder() { }

		public override string Name =>
			this.organization.Name;

		public virtual Organization Organization =>
			this.organization;
	}

	/// <summary>
	/// Свидетельство о государственной регистрации права
	/// </summary>
	public class ParcelRightsCertificate
	{
		private string series;
		private string number;
		private DateTime date;
		private string registrationRecordNumber;
		private ParcelRight right;

		public ParcelRightsCertificate(string series, string number, DateTime date, ParcelRight right, string registrationRecordNumber)
		{
			this.series = series;
			this.number = number;
			this.date = date;
			this.right = right;
			this.registrationRecordNumber = registrationRecordNumber;
		}

		protected ParcelRightsCertificate() { }

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

		public override bool Equals(object obj) =>
			obj is ParcelRightsCertificate other ?
				this.series == other.series &&
				this.number == other.number :
				false;

		public override int GetHashCode() =>
			this.series.GetHashCode() ^
			this.number.GetHashCode();
	}
}
