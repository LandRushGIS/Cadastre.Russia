using System;
using System.Collections.Generic;
using System.Linq;

namespace LandRush.Cadastre.Russia
{
	/// <summary>
	/// Вид земельного участка
	/// </summary>
	public class ParcelType : DomainValue
	{
		protected ParcelType() { }
		public ParcelType(string code, string description) : base(code, description) { }
	}

	/// <summary>
	/// Статус земельного участка
	/// </summary>
	public class ParcelState : DomainValue
	{
		protected ParcelState() { }
		public ParcelState(string code, string description) : base(code, description) { }
	}

	/// <summary>
	/// Земельный участок
	/// </summary>
	public /*abstract*/ class Parcel
	{
		static Parcel()
		{
			types = new Dictionary<Type, ParcelType>();
		}

		private static IDictionary<Type, ParcelType> types;
		public static IDictionary<Type, ParcelType> Types
		{
			get
			{
				return types;
			}
		}
		//

		protected Parcel() : this(null, 0, default(DateTime)) { }

		public Parcel(Block block, int localNumber, DateTime creationDate)
		{
			this.block = block;
			this.localNumber = localNumber;
			//this.type = Types[this.GetType()];
			this.type = Types.ContainsKey(this.GetType()) ? Types[this.GetType()] : null;
			this.creationDate = creationDate;
			this.rights = new SortedSet<ParcelRight>();
			this.encumbrances = new SortedSet<ParcelEncumbrance>();
			this.numberedLandPieces = new Dictionary<int, ParcelLandPiece>();
			this.numberedSubParcels = new Dictionary<int, SubParcel>();
		}

		private Block block;
		public virtual Block Block
		{
			get
			{
				return this.block;
			}
		}

		private int localNumber;
		public virtual int LocalNumber
		{
			get
			{
				return this.localNumber;
			}
		}

		// Кадастровый номер участка
		public virtual ParcelNumber Number
		{
			get
			{
				return new ParcelNumber(this.block.Number, this.localNumber);
			}
		}

		// ???
		public virtual ParcelNumber FullNumber
		{
			get
			{
				return this.Number;
			}
		}

		// Тип участка
		private ParcelType type;
		public virtual ParcelType Type
		{
			get
			{
				return this.type;
			}
			//set
			//{
			//	type = value;
			//}
		}

		// Статус земельного участка
		private ParcelState state;
		public virtual ParcelState State
		{
			get
			{
				return this.state;
			}
			set
			{
				this.state = value;
			}
		}

		// Дата постановки участка на учет
		private DateTime creationDate;
		public virtual DateTime CreationDate
		{
			get
			{
				return this.creationDate;
			}
		}

		// Дата снятия участка с учета
		private DateTime? removingDate;
		public virtual bool HasRemovingDate
		{
			get
			{
				return this.removingDate.HasValue;
			}
		}

		public virtual void UnsetRemovingDate()
		{
			this.removingDate = null;
		}

		public virtual DateTime RemovingDate
		{
			get
			{
				return this.removingDate.Value;
			}
			//protected set
			//{
			//	removingDate = value;
			//}
		}

		// Категория земель участка
		private LandCategory landCategory;
		public virtual LandCategory LandCategory
		{
			get
			{
				return this.landCategory;
			}
			set
			{
				this.landCategory = value;
			}
		}

		// Разрешенное использование земель участка
		private LandUtilization landUtilization;
		public virtual LandUtilization LandUtilization
		{
			get
			{
				return this.landUtilization;
			}
			set
			{
				this.landUtilization = value;
			}
		}

		// Документированная площадь участка
		private double documentedArea;
		public virtual double DocumentedArea
		{
			get
			{
				return this.documentedArea;
			}
			set //!! must be removed
			{
				this.documentedArea = value;
			}
		}

		// Cadastral value (кадастровая стоимость)
		public virtual double CadastralValue
		{
			get
			{
				return this.DocumentedArea * 10.0;//!!block.CadastralValueFactors[category];
			}
		}

		// Права на земельный участок
		private ISet<ParcelRight> rights;
		public virtual ISet<ParcelRight> Rights
		{
			get
			{
				return this.rights;
			}
			protected set
			{
				this.rights = value;
			}
		}

		// Ограничения (обременения) прав на земельный участок
		private ISet<ParcelEncumbrance> encumbrances;
		public virtual ISet<ParcelEncumbrance> Encumbrances
		{
			get
			{
				return this.encumbrances;
			}
			protected set
			{
				this.encumbrances = value;
			}
		}

		protected IDictionary<int, ParcelLandPiece> numberedLandPieces;

		// !! Bad idea to publish this to interface
		public virtual IDictionary<int, ParcelLandPiece> NumberedLandPieces
		{
			get
			{
				return this.numberedLandPieces;
			}
		}

		public virtual IEnumerable<ParcelLandPiece> LandPieces
		{
			get
			{
				return this.numberedLandPieces.Values;
			}
		}

		private IDictionary<int, SubParcel> numberedSubParcels;

		public virtual IDictionary<int, SubParcel> NumberedSubParcels
		{
			get
			{
				return this.numberedSubParcels;
			}
		}

		public virtual IEnumerable<SubParcel> SubParcels
		{
			get
			{
				return this.numberedSubParcels.Values;
			}
		}

		public override bool Equals(object obj)
		{
			if ((obj == null) || !(obj is Parcel)) return false;
			else return ((obj as Parcel).block == this.block) && ((obj as Parcel).localNumber == this.localNumber);
		}

		public override int GetHashCode()
		{
			return this.block.GetHashCode() ^ (int)this.localNumber.GetHashCode();
		}
	}

	public class SingleLandPieceParcel : Parcel
	{
		protected SingleLandPieceParcel() : this(null, 0, default(DateTime)) { }

		public SingleLandPieceParcel(Block block, int localNumber, DateTime creationDate) : base(block, localNumber, creationDate) { }

		// Контур участка
		public virtual ParcelLandPiece LandPiece
		{
			get
			{
				return (this.LandPieces.Count() > 0) ? this.LandPieces.First() : null;
			}
			set
			{
				if (this.numberedLandPieces.Count() > 0) this.numberedLandPieces.Clear();
				this.numberedLandPieces.Add(0, value);
			}
		}
	}

	public class SimpleParcel : SingleLandPieceParcel
	{
		protected SimpleParcel() : this(null, 0, default(DateTime)) { }

		public SimpleParcel(Block block, int localNumber, DateTime creationDate) : base(block, localNumber, creationDate) { }
	}

	public class UnifiedLandUseChildParcel : SingleLandPieceParcel
	{
		protected UnifiedLandUseChildParcel() : this(null, 0, default(DateTime)) { }

		public UnifiedLandUseChildParcel(Block block, int localNumber, DateTime creationDate) : base(block, localNumber, creationDate) { }

		// Является ли квартал родительского в едином землепользовании участка отличным от того, в котором находится данный участок
		private bool hasAnotherParentBlock;
		public virtual bool HasAnotherParentBlock
		{
			get
			{
				return this.hasAnotherParentBlock;
			}
			set
			{
				this.hasAnotherParentBlock = value;
			}
		}

		// Локальный номер родительского участка в едином землепользовании
		private int parentParcelLocalNumber;
		public virtual int ParentParcelLocalNumber
		{
			get
			{
				return this.parentParcelLocalNumber;
			}
			set
			{
				this.parentParcelLocalNumber = value;
			}
		}

		// Номер родительского участка в едином землепользовании
		public virtual ParcelNumber? ParentParcelNumber
		{
			get
			{
				return
						(this.hasAnotherParentBlock ?
						new ParcelNumber(new BlockNumber(this.Number.DistrictNumber, 0), this.parentParcelLocalNumber) :
						new ParcelNumber(this.Number.BlockNumber, this.parentParcelLocalNumber));
			}
		}
	}

	public class UnifiedLandUseParcel : Parcel
	{
		protected UnifiedLandUseParcel() : this(null, 0, default(DateTime)) { }

		public UnifiedLandUseParcel(Block block, int localNumber, DateTime creationDate) : base(block, localNumber, creationDate) { }
	}

	public class MulticontourParcel : Parcel
	{
		protected MulticontourParcel() : this(null, 0, default(DateTime)) { }

		public MulticontourParcel(Block block, int localNumber, DateTime creationDate)
			: base(block, localNumber, creationDate)
		{
			this.numberedContours = new Dictionary<int, MulticontourParcelContour>();
		}

		private IDictionary<int, MulticontourParcelContour> numberedContours;

		// Контуры участка
		public virtual IDictionary<int, MulticontourParcelContour> NumberedContours
		{
			get
			{
				return this.numberedContours;
			}
		}

		public virtual IEnumerable<MulticontourParcelContour> Contours
		{
			get
			{
				return this.numberedContours.Values;
			}
		}
	}

	// Контур многоконтурного участка
	public class MulticontourParcelContour
	{
		protected MulticontourParcelContour() { }

		public MulticontourParcelContour(MulticontourParcel parcel, int localNumber)
		{
			this.parcel = parcel;
			this.localNumber = localNumber;
		}

		private MulticontourParcel parcel;
		public virtual MulticontourParcel Parcel
		{
			get
			{
				return this.parcel;
			}
		}

		private int localNumber;
		public virtual int LocalNumber
		{
			get
			{
				return this.localNumber;
			}
		}

		public virtual ParcelContourNumber Number
		{
			get
			{
				return new ParcelContourNumber(this.parcel.Number, this.localNumber);
			}
		}

		public virtual ParcelLandPiece LandPiece
		{
			get
			{
				return this.Parcel.NumberedLandPieces[this.localNumber];
			}
		}

		public override bool Equals(object obj)
		{
			if ((obj == null) || !(obj is MulticontourParcelContour)) return false;
			else return ((obj as MulticontourParcelContour).parcel == this.parcel) && ((obj as MulticontourParcelContour).localNumber == this.localNumber);
		}

		public override int GetHashCode()
		{
			return this.parcel.GetHashCode() ^ (int)this.localNumber.GetHashCode();
		}
	}
}
