using System;
using System.Collections.Generic;

namespace LandRush.Cadastre.Russia
{
	/// <summary>
	/// Часть участка
	/// </summary>
	public class SubParcel
	{
		private Parcel parcel;
		private int localNumber;
		private ParcelState state;
		private int? landPieceNumber;
		private ISet<SubParcelEncumbrance> encumbrances;

		public SubParcel(Parcel parcel, int localNumber, ParcelState state, ParcelLandPiece landPiece)
		{
			this.parcel = parcel;
			this.localNumber = localNumber;
			this.state = state;
			this.LandPiece = landPiece;
			this.encumbrances = new HashSet<SubParcelEncumbrance>();
		}

		protected SubParcel() { }

		public virtual Parcel Parcel => this.parcel;

		public virtual int LocalNumber => this.localNumber;

		public virtual SubParcelNumber Number =>
			new SubParcelNumber(this.parcel.Number, this.localNumber);

		/// <summary>
		/// Статус части земельного участка
		/// </summary>
		public virtual ParcelState State => this.state;

		public virtual ParcelLandPiece LandPiece
		{
			get =>
				this.landPieceNumber.HasValue ?
					this.parcel.NumberedLandPieces[this.landPieceNumber.Value] : null;
			set
			{
				if (value == null)
					this.landPieceNumber = null;
				else if (this.parcel != value.Parcel)
					throw new InvalidOperationException("Land piece belongs to another parcel");
				else
					this.landPieceNumber = value.Number;
			}
		}

		// Ограничения (обременения) прав на часть земельного участка
		public virtual ISet<SubParcelEncumbrance> Encumbrances
		{
			get => this.encumbrances;
			protected set => this.encumbrances = value;
		}

		public override bool Equals(object obj) =>
			obj is SubParcel other ?
				this.parcel == other.parcel &&
				this.localNumber == other.localNumber :
				false;

		public override int GetHashCode() =>
			this.parcel.GetHashCode() ^
			this.localNumber.GetHashCode();
	}
}
