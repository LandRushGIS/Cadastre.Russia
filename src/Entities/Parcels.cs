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
		public ParcelType(string code, string description) : base(code, description) { }
		protected ParcelType() { }
	}

	/// <summary>
	/// Статус земельного участка
	/// </summary>
	public class ParcelState : DomainValue
	{
		public ParcelState(string code, string description) : base(code, description) { }
		protected ParcelState() { }
	}

	/// <summary>
	/// Земельный участок
	/// </summary>
	public /*abstract*/ class Parcel
	{
		private static IDictionary<Type, ParcelType> types;

		static Parcel() =>
			types = new Dictionary<Type, ParcelType>();

		public static IDictionary<Type, ParcelType> Types =>
			types;
		//

		protected IDictionary<int, ParcelLandPiece> numberedLandPieces;
		private Block block;
		private int localNumber;
		private ParcelType type;
		private ParcelState state;
		private DateTime creationDate;
		private DateTime? removingDate;
		private LandCategory landCategory;
		private LandUtilization landUtilization;
		private double documentedArea;
		private ISet<ParcelRight> rights;
		private ISet<ParcelEncumbrance> encumbrances;
		private IDictionary<int, SubParcel> numberedSubParcels;

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

		protected Parcel() : this(null, 0, default(DateTime)) { }

		public virtual Block Block =>
			this.block;

		public virtual int LocalNumber =>
			this.localNumber;

		// Кадастровый номер участка
		public virtual ParcelNumber Number =>
			new ParcelNumber(this.block.Number, this.localNumber);

		// ???
		public virtual ParcelNumber FullNumber =>
			this.Number;

		// Тип участка
		public virtual ParcelType Type =>
			this.type;

		// Статус земельного участка
		public virtual ParcelState State
		{
			get => this.state;
			set => this.state = value;
		}

		// Дата постановки участка на учет
		public virtual DateTime CreationDate =>
			this.creationDate;

		// Дата снятия участка с учета
		public virtual bool HasRemovingDate =>
			this.removingDate.HasValue;

		public virtual void UnsetRemovingDate() =>
			this.removingDate = null;

		public virtual DateTime RemovingDate =>
			this.removingDate.Value;

		// Категория земель участка
		public virtual LandCategory LandCategory
		{
			get => this.landCategory;
			set => this.landCategory = value;
		}

		// Разрешенное использование земель участка
		public virtual LandUtilization LandUtilization
		{
			get => this.landUtilization;
			set => this.landUtilization = value;
		}

		// Документированная площадь участка
		public virtual double DocumentedArea
		{
			get => this.documentedArea;
			//!! must be removed
			set => this.documentedArea = value;
		}

		// Cadastral value (кадастровая стоимость)
		public virtual double CadastralValue =>
			this.DocumentedArea * 10.0;//!!block.CadastralValueFactors[category];

		// Права на земельный участок
		public virtual ISet<ParcelRight> Rights
		{
			get => this.rights;
			protected set => this.rights = value;
		}

		// Ограничения (обременения) прав на земельный участок
		public virtual ISet<ParcelEncumbrance> Encumbrances
		{
			get => this.encumbrances;
			protected set => this.encumbrances = value;
		}

		// !! Bad idea to publish this to interface
		public virtual IDictionary<int, ParcelLandPiece> NumberedLandPieces =>
			this.numberedLandPieces;

		public virtual IEnumerable<ParcelLandPiece> LandPieces =>
			this.numberedLandPieces.Values;

		public virtual IDictionary<int, SubParcel> NumberedSubParcels =>
			this.numberedSubParcels;

		public virtual IEnumerable<SubParcel> SubParcels =>
			this.numberedSubParcels.Values;

		public override bool Equals(object obj) =>
			obj is Parcel other ?
				this.block == other.block &&
				this.localNumber == other.localNumber :
				false;

		public override int GetHashCode() =>
			this.block.GetHashCode() ^
			this.localNumber.GetHashCode();
	}

	public class SingleLandPieceParcel : Parcel
	{
		public SingleLandPieceParcel(Block block, int localNumber, DateTime creationDate) : base(block, localNumber, creationDate) { }

		protected SingleLandPieceParcel() : this(null, 0, default(DateTime)) { }

		// Контур участка
		public virtual ParcelLandPiece LandPiece
		{
			get =>
				this.LandPieces.Count() > 0 ?
					this.LandPieces.First() : null;
			set
			{
				if (this.numberedLandPieces.Count() > 0) this.numberedLandPieces.Clear();
				this.numberedLandPieces.Add(0, value);
			}
		}
	}

	public class SimpleParcel : SingleLandPieceParcel
	{
		public SimpleParcel(Block block, int localNumber, DateTime creationDate) : base(block, localNumber, creationDate) { }

		protected SimpleParcel() : this(null, 0, default(DateTime)) { }
	}

	public class UnifiedLandUseChildParcel : SingleLandPieceParcel
	{
		private bool hasAnotherParentBlock;
		private int parentParcelLocalNumber;

		public UnifiedLandUseChildParcel(Block block, int localNumber, DateTime creationDate) : base(block, localNumber, creationDate) { }

		protected UnifiedLandUseChildParcel() : this(null, 0, default(DateTime)) { }

		// Является ли квартал родительского в едином землепользовании участка отличным от того, в котором находится данный участок
		public virtual bool HasAnotherParentBlock
		{
			get => this.hasAnotherParentBlock;
			set => this.hasAnotherParentBlock = value;
		}

		// Локальный номер родительского участка в едином землепользовании
		public virtual int ParentParcelLocalNumber
		{
			get => this.parentParcelLocalNumber;
			set => this.parentParcelLocalNumber = value;
		}

		// Номер родительского участка в едином землепользовании
		public virtual ParcelNumber? ParentParcelNumber =>
			this.hasAnotherParentBlock ?
				new ParcelNumber(new BlockNumber(this.Number.DistrictNumber, 0), this.parentParcelLocalNumber) :
				new ParcelNumber(this.Number.BlockNumber, this.parentParcelLocalNumber);
	}

	public class UnifiedLandUseParcel : Parcel
	{
		public UnifiedLandUseParcel(Block block, int localNumber, DateTime creationDate) : base(block, localNumber, creationDate) { }

		protected UnifiedLandUseParcel() : this(null, 0, default(DateTime)) { }
	}

	public class MulticontourParcel : Parcel
	{
		private IDictionary<int, MulticontourParcelContour> numberedContours;

		public MulticontourParcel(Block block, int localNumber, DateTime creationDate)
			: base(block, localNumber, creationDate) =>
			this.numberedContours = new Dictionary<int, MulticontourParcelContour>();

		protected MulticontourParcel() : this(null, 0, default(DateTime)) { }

		// Контуры участка
		public virtual IDictionary<int, MulticontourParcelContour> NumberedContours =>
			this.numberedContours;

		public virtual IEnumerable<MulticontourParcelContour> Contours =>
			this.numberedContours.Values;
	}

	// Контур многоконтурного участка
	public class MulticontourParcelContour
	{
		private MulticontourParcel parcel;
		private int localNumber;

		public MulticontourParcelContour(MulticontourParcel parcel, int localNumber)
		{
			this.parcel = parcel;
			this.localNumber = localNumber;
		}

		protected MulticontourParcelContour() { }

		public virtual MulticontourParcel Parcel =>
			this.parcel;

		public virtual int LocalNumber =>
			this.localNumber;

		public virtual ParcelContourNumber Number =>
			new ParcelContourNumber(this.parcel.Number, this.localNumber);

		public virtual ParcelLandPiece LandPiece =>
			this.Parcel.NumberedLandPieces[this.localNumber];

		public override bool Equals(object obj) =>
			obj is MulticontourParcelContour other ?
				this.parcel == other.parcel &&
				this.localNumber == other.localNumber :
				false;

		public override int GetHashCode() =>
			this.parcel.GetHashCode() ^
			this.localNumber.GetHashCode();
	}
}
