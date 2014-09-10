using System;

namespace LandRush.Cadastre
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

		public ParcelRightsCertificate(string series, string number, DateTime date, Parcel parcel, Landholder landholder, string registrationRecordNumber)
		{
			this.series = series;
			this.number = number;
			this.date = date;
			this.parcel = parcel;
			this.landholder = landholder;
			this.registrationRecordNumber = registrationRecordNumber;
		}

		private string series;
		private string number;
		private DateTime date;
		private string registrationRecordNumber;
		private Parcel parcel;
		private Landholder landholder;

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
		public virtual Parcel Parcel { get { return parcel; } }
		public virtual Landholder Landholder
		{
			get { return landholder; }
			set { landholder = value; }
		}

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