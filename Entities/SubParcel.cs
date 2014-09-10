using System;
using System.Collections.Generic;

namespace LandRush.Cadastre
{
	/// <summary>
	/// Часть участка
	/// </summary>
	public class SubParcel
	{
		protected SubParcel() { }

		public SubParcel(Parcel parcel, int localNumber, ParcelState state, ParcelLandPiece landPiece)
		{
			this.parcel = parcel;
			this.localNumber = localNumber;
			this.state = state;
			this.LandPiece = landPiece;
			this.encumbrances = new HashSet<SubParcelEncumbrance>();
		}

		private Parcel parcel;
		public virtual Parcel Parcel
		{
			get
			{
				return parcel;
			}
		}

		private int localNumber;
		public virtual int LocalNumber
		{
			get
			{
				return localNumber;
			}
		}

		public virtual SubParcelNumber Number
		{
			get
			{
				return new SubParcelNumber(parcel.Number, localNumber);
			}
		}

		/// <summary>
		/// Статус части земельного участка
		/// </summary>
		private ParcelState state;
		public virtual ParcelState State
		{
			get
			{
				return state;
			}
		}

		private int? landPieceNumber;

		public virtual ParcelLandPiece LandPiece
		{
			get
			{
				return landPieceNumber.HasValue ? parcel.NumberedLandPieces[landPieceNumber.Value] : null;
			}
			set
			{
				if (value == null)
					landPieceNumber = null;
				else if (this.parcel != value.Parcel)
					throw new InvalidOperationException("Land piece belongs to another parcel");
				else
					landPieceNumber = value.Number;
			}
		}

		// Ограничения (обременения) прав на часть земельного участка
		private ISet<SubParcelEncumbrance> encumbrances;
		public virtual ISet<SubParcelEncumbrance> Encumbrances
		{
			get
			{
				return encumbrances;
			}
			protected set
			{
				encumbrances = value;
			}
		}

		public override bool Equals(object obj)
		{
			if ((obj == null) || !(obj is SubParcel)) return false;
			else return ((obj as SubParcel).parcel == this.parcel) && ((obj as SubParcel).localNumber == this.localNumber);
		}

		public override int GetHashCode()
		{
			return this.parcel.GetHashCode() ^ (int)this.localNumber.GetHashCode();
		}
	}
}