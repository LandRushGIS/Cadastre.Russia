using System;

namespace LandRush.Cadastre.Russia
{
	/// <summary>
	/// Собственник/арендатор земли
	/// </summary>
	public class Landholder
	{
		protected Landholder() { }

		public Landholder(int id, string name)
		{
			this.id = id;
			this.name = name;
		}

		private int id;
		public virtual int Id
		{
			get
			{
				return id;
			}
		}

		private string name;
		public virtual string Name
		{
			get
			{
				return name;
			}
		}
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

		public virtual string Series { get { return series; } }
		public virtual string Number { get { return number; } }
		public virtual DateTime Date
		{
			get { return date; }
			set { date = value; }
		}
		public virtual string RegistrationRecordNumber
		{
			get { return registrationRecordNumber; }
			set { registrationRecordNumber = value; }
		}
		public virtual ParcelRight Right { get { return right; } }

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