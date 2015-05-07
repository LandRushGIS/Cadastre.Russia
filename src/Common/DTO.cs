using System;
using System.Collections.Generic;
using IGeometry = GeoAPI.Geometries.IGeometry;
using LandRush.Cadastre.Russia;

namespace LandRush.Cadastre.Russia.DTO
{
	//[DataContract]
	public struct Block
	{
		public Block(string number, double cadastralValueFactor)
		{
			this.Number = number;
			this.CadastralValueFactor = cadastralValueFactor;
		}

		//[DataMember]
		public string Number;
		//[DataMember]
		public double CadastralValueFactor;
	}

	//[DataContract]
	public struct Parcel
	{
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

		//[DataMember]
		public ParcelNumber Number;
		//[DataMember]
		public DateTime CreationDate;
		//[DataMember]
		public IGeometry Geometry;
		//[DataMember]
		public string TypeCode;
		//[DataMember]
		public string StateCode;
		//[DataMember]
		public string Landholder;
		//[DataMember]
		public string Note;
		//[DataMember]
		public double DocumentedArea;
		//[DataMember]
		public double AssessedValue;
		//[DataMember]
		public double CadastralValue;
		//[DataMember]
		public string Address;
		//[DataMember]
		public string LandCategoryCode;
		//[DataMember]
		public LandUtilization LandUtilization;
		//[DataMember]
		public IList<LandRight> Rights;
		//[DataMember]
		public IList<LandEncumbrance> Encumbrances;
		//[DataMember]
		public IList<SubParcel> SubParcels;
		//[DataMember]
		public bool? HasAnotherParentBlock;
		//[DataMember]
		public int? ParentParcelLocalNumber;

		public override string ToString()
		{
			return "Parcel №" + Number.ToString();
		}
	}

	//[DataContract]
	public struct SubParcel
	{
		public SubParcel(int number, string stateCode, IList<LandEncumbrance> encumbrances)
		{
			this.Number = number;
			this.StateCode = stateCode;
			this.Encumbrances = encumbrances;
		}

		//[DataMember]
		public int Number;
		//[DataMember]
		public string StateCode;
		//[DataMember]
		public IList<LandEncumbrance> Encumbrances;
	}

	//[DataContract]
	public struct LandRight
	{
		public LandRight(string landRightTypeCode, string name)
		{
			this.LandRightTypeCode = landRightTypeCode;
			this.Name = name;
		}

		//[DataMember]
		public string LandRightTypeCode;

		//[DataMember]
		public string Name;
	}

	//[DataContract]
	public struct LandEncumbrance
	{
		public LandEncumbrance(string landEncumbranceTypeCode, string name)
		{
			this.LandEncumbranceTypeCode = landEncumbranceTypeCode;
			this.Name = name;
		}

		//[DataMember]
		public string LandEncumbranceTypeCode;

		//[DataMember]
		public string Name;
	}

	//[DataContract]
	public struct LandUtilization
	{
		public LandUtilization(string kindCode, string description)
		{
			this.KindCode = kindCode;
			this.Description = description;
		}

		//[DataMember]
		public string KindCode;
		//[DataMember]
		public string Description;
	}
}
