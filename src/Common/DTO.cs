using System;
using System.Collections.Generic;
using IGeometry = GeoAPI.Geometries.IGeometry;
using LandRush.Cadastre.Russia;

namespace LandRush.Cadastre.Russia.DTO
{
	public struct Block
	{
		public string Number;
		public double CadastralValueFactor;

		public Block(string number, double cadastralValueFactor)
		{
			this.Number = number;
			this.CadastralValueFactor = cadastralValueFactor;
		}
	}

	public struct Parcel
	{
		public ParcelNumber Number;
		public DateTime CreationDate;
		public IGeometry Geometry;
		public string TypeCode;
		public string StateCode;
		public string Landholder;
		public string Note;
		public double DocumentedArea;
		public double AssessedValue;
		public double CadastralValue;
		public string Address;
		public string LandCategoryCode;
		public LandUtilization LandUtilization;
		public IList<LandRight> Rights;
		public IList<LandEncumbrance> Encumbrances;
		public IList<SubParcel> SubParcels;
		public bool? HasAnotherParentBlock;
		public int? ParentParcelLocalNumber;

		public Parcel(ParcelNumber number, DateTime creationDate, IGeometry geometry, string typeCode, string stateCode, string landholder, string note, double documentedArea, double assessedValue, double cadastralValue, string address, string landCategoryCode, LandUtilization landUtilization, IList<LandRight> rights, IList<LandEncumbrance> encumbrances, IList<SubParcel> subParcels, bool? hasAnotherParentBlock, int? parentParcelSubnumber)
		{
			this.Number = number;
			this.CreationDate = creationDate;
			this.Geometry = geometry;
			this.TypeCode = typeCode;
			this.StateCode = stateCode;
			this.Landholder = landholder;
			this.Note = note;
			this.DocumentedArea = documentedArea;
			this.AssessedValue = assessedValue;
			this.CadastralValue = cadastralValue;
			this.Address = address;
			this.LandCategoryCode = landCategoryCode;
			this.LandUtilization = landUtilization;
			this.Rights = rights;
			this.Encumbrances = encumbrances;
			this.SubParcels = subParcels;
			this.HasAnotherParentBlock = hasAnotherParentBlock;
			this.ParentParcelLocalNumber = parentParcelSubnumber;
		}

		public override string ToString() =>
			$"Parcel №{this.Number}";
	}

	public struct SubParcel
	{
		public int Number;
		public string StateCode;
		public IList<LandEncumbrance> Encumbrances;

		public SubParcel(int number, string stateCode, IList<LandEncumbrance> encumbrances)
		{
			this.Number = number;
			this.StateCode = stateCode;
			this.Encumbrances = encumbrances;
		}
	}

	public struct LandRight
	{
		public string LandRightTypeCode;
		public string Name;

		public LandRight(string landRightTypeCode, string name)
		{
			this.LandRightTypeCode = landRightTypeCode;
			this.Name = name;
		}
	}

	public struct LandEncumbrance
	{
		public string LandEncumbranceTypeCode;
		public string Name;

		public LandEncumbrance(string landEncumbranceTypeCode, string name)
		{
			this.LandEncumbranceTypeCode = landEncumbranceTypeCode;
			this.Name = name;
		}
	}

	public struct LandUtilization
	{
		public string KindCode;
		public string Description;

		public LandUtilization(string kindCode, string description)
		{
			this.KindCode = kindCode;
			this.Description = description;
		}
	}
}
