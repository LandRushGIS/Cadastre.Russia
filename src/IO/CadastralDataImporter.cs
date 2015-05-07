using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using BlockDTO = LandRush.Cadastre.Russia.DTO.Block;
using ParcelDTO = LandRush.Cadastre.Russia.DTO.Parcel;
using LandUtilizationDTO = LandRush.Cadastre.Russia.DTO.LandUtilization;
using LandRightDTO = LandRush.Cadastre.Russia.DTO.LandRight;
using LandEncumbranceDTO = LandRush.Cadastre.Russia.DTO.LandEncumbrance;
using SubParcelDTO = LandRush.Cadastre.Russia.DTO.SubParcel;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using NetTopologySuite.Algorithm;

namespace LandRush.Cadastre.Russia.IO
{
	public static class CadastralDataImporter
	{
		private static List<ParcelDTO> ReadCadastralBlocks(XmlReader xmlReader, uint version)
		{
			List<ParcelDTO> parcels = new List<ParcelDTO>();

			xmlReader.ReadStartElement("Cadastral_Blocks");
			while (xmlReader.IsStartElement("Cadastral_Block"))
			{
				BlockNumber blockCadastralNumber = BlockNumber.Parse(xmlReader.GetAttribute("CadastralNumber"));
				//xmlReader.ReadStartElement("Cadastral_Block");
				//xmlReader.Skip();
				xmlReader.ReadToDescendant("Parcels");
				xmlReader.ReadStartElement("Parcels");
				while (xmlReader.IsStartElement("Parcel"))
				{
					string cadastralNumber = xmlReader.GetAttribute("CadastralNumber");
					ParcelNumber parcelCadastralNumber = cadastralNumber[0] == ':' ? ParcelNumber.Parse(blockCadastralNumber.ToString() + cadastralNumber) : ParcelNumber.Parse(cadastralNumber);
					string parcelTypeCode = xmlReader.GetAttribute("Name");
					string parcelStateCode = xmlReader.GetAttribute("State");
					DateTime parcelCreationDate = DateTime.Parse(xmlReader.GetAttribute("DateCreated"));
					DateTime? parcelRemovingDate = xmlReader.GetAttribute("DateRemoved") != null ? new DateTime?(DateTime.Parse(xmlReader.GetAttribute("DateRemoved"))) : null;
					Geometry parcelGeometry = null;
					double parcelDocumentedArea = double.NegativeInfinity;
					string parcelAddress = null;
					string landCategoryCode = null;
					LandUtilizationDTO landUtilization = default(LandUtilizationDTO);
					IList<LandRightDTO> rights = null;
					IList<LandEncumbranceDTO> encumbrances = null;
					IList<SubParcelDTO> subParcels = null;
					ParcelNumber? parentParcelNumber = null;
					xmlReader.ReadStartElement("Parcel");
					while (xmlReader.IsStartElement())
					{
						if ((version < 8) && xmlReader.IsStartElement("Areas"))
							parcelDocumentedArea = ReadAreas(xmlReader);
						else if ((version >= 8) && xmlReader.IsStartElement("Area"))
						{
							AreaInfo areaInfo = ReadArea(xmlReader);
							parcelDocumentedArea = areaInfo.AreaValue; // TODO: check units
						}
						else if (xmlReader.IsStartElement("Location"))
							parcelAddress = ReadLocation(xmlReader);
						else if (xmlReader.IsStartElement("Category"))
							landCategoryCode = ReadCategory(xmlReader);
						else if (xmlReader.IsStartElement("Utilization"))
							landUtilization = ReadUtilization(xmlReader);
						else if (xmlReader.IsStartElement("Rights"))
							rights = ReadRights(xmlReader);
						else if (xmlReader.IsStartElement("Encumbrances"))
							encumbrances = ReadEncumbrances(xmlReader);
						else if (xmlReader.IsStartElement("SubParcels"))
							subParcels = ReadSubParcels(xmlReader, version);
						else if (xmlReader.IsStartElement("Unified_Land_Unit"))
							parentParcelNumber = ReadParentParcelNumber(xmlReader);
						else if (xmlReader.IsStartElement("Contours"))
							parcelGeometry = ReadContours(xmlReader);
						else if (xmlReader.IsStartElement("Entity_Spatial"))
							parcelGeometry = ReadEntitySpatial(xmlReader);
						else xmlReader.Skip();
					}
					xmlReader.ReadEndElement(); // Parcel

					bool? hasAnotherParentBlock = null;
					int? parentParcelSubnumber = null;
					if (parentParcelNumber.HasValue)
					{
						if (parentParcelNumber.Value.BlockNumber.LocalNumber == 0)
							hasAnotherParentBlock = true;
						else
							hasAnotherParentBlock = false;
						//
						parentParcelSubnumber = parentParcelNumber.Value.LocalNumber;
					}

					parcels.Add(new ParcelDTO(parcelCadastralNumber, parcelCreationDate, parcelGeometry, parcelTypeCode, parcelStateCode, "", "", parcelDocumentedArea, 0.0, 0.0, parcelAddress, landCategoryCode, landUtilization, rights, encumbrances, subParcels, hasAnotherParentBlock, parentParcelSubnumber));
				}
				xmlReader.ReadEndElement(); // Parcels
				while (xmlReader.IsStartElement())
				{
					xmlReader.Skip();
				}
				xmlReader.ReadEndElement(); // Cadastral_Block
			}
			xmlReader.ReadEndElement(); // Cadastral_Blocks

			return parcels;
		}

		public static List<ParcelDTO> ReadFederal(XmlReader xmlReader, uint version)
		{
			List<ParcelDTO> parcels = null;

			xmlReader.ReadStartElement("Federal");
			xmlReader.ReadStartElement("Cadastral_Regions");
			while (xmlReader.IsStartElement("Cadastral_Region"))
			{
				RegionNumber regionCadastralNumber = RegionNumber.Parse(xmlReader.GetAttribute("CadastralNumber"));
				xmlReader.ReadToDescendant("Cadastral_Districts");
				xmlReader.ReadStartElement("Cadastral_Districts");
				while (xmlReader.IsStartElement("Cadastral_District"))
				{
					DistrictNumber districtCadastralNumber = DistrictNumber.Parse(xmlReader.GetAttribute("CadastralNumber"));
					xmlReader.ReadToDescendant("Cadastral_Blocks");
					parcels = ReadCadastralBlocks(xmlReader, version);
					xmlReader.ReadEndElement(); // Cadastral_District
				}
				xmlReader.ReadEndElement(); // Cadastral_Districts
			}
			xmlReader.ReadEndElement(); // Cadastral_Region
			xmlReader.ReadEndElement(); // Cadastral_Regions
			xmlReader.ReadEndElement(); // Federal

			return parcels;
		}

		public static List<ParcelDTO> ReadCadastralPlan(XmlReader xmlReader)
		{
			List<ParcelDTO> parcels = null;
			//
			// ... skip unnessesary items
			//
			xmlReader.ReadToFollowing("Region_Cadastr");
			// now located in Region_Cadastr
			uint version = 0;
			if (xmlReader.GetAttribute("Version") != null)
			{
				version = uint.Parse(xmlReader.GetAttribute("Version"));
				xmlReader.ReadStartElement("Region_Cadastr");
			}
			else
			{
				xmlReader.ReadStartElement("Region_Cadastr");
				xmlReader.ReadToFollowing("eDocument");
				xmlReader.ReadStartElement("eDocument");
				version = uint.Parse(xmlReader.GetAttribute("Version"));
				xmlReader.Skip();
				xmlReader.ReadEndElement(); // eDocument
			}

			if (!xmlReader.IsStartElement("Package"))
				xmlReader.ReadToFollowing("Package");
			xmlReader.ReadStartElement("Package");
			if (version < 8)
				parcels = ReadFederal(xmlReader, version);
			else // version >= 8
				parcels = ReadCadastralBlocks(xmlReader, version);
			xmlReader.Skip();
			xmlReader.ReadEndElement(); // Package
			xmlReader.ReadEndElement(); // Region_Cadastr
			return parcels;
		}

		public struct AreaInfo
		{
			public string AreaCode;
			public double AreaValue;
			public string UnitCode;
			public double Innccuracy;
		}

		public static AreaInfo ReadArea(XmlReader xmlReader)
		{
			AreaInfo areaInfo = new AreaInfo();
			xmlReader.ReadStartElement("Area");
			while (xmlReader.IsStartElement())
			{
				if (xmlReader.IsStartElement("AreaCode"))
				{
					xmlReader.ReadStartElement("AreaCode");
					areaInfo.AreaCode = xmlReader.ReadContentAsString();
					xmlReader.ReadEndElement(); // AreaCode
				}
				else if (xmlReader.IsStartElement("Area"))
				{
					xmlReader.ReadStartElement("Area");
					areaInfo.AreaValue = double.Parse(xmlReader.ReadContentAsString().Replace('.', ','));
					xmlReader.ReadEndElement(); // Area
				}
				else if (xmlReader.IsStartElement("Unit"))
				{
					xmlReader.ReadStartElement("Unit");
					areaInfo.UnitCode = xmlReader.ReadContentAsString();
					xmlReader.ReadEndElement(); // Unit
				}
				else if (xmlReader.IsStartElement("Innccuracy"))
				{
					xmlReader.ReadStartElement("Innccuracy");
					areaInfo.Innccuracy = double.Parse(xmlReader.ReadContentAsString().Replace('.', ','));
					xmlReader.ReadEndElement(); // Innccuracy
				}
			}
			xmlReader.ReadEndElement(); // Area
			return areaInfo;
		}

		public static double ReadAreas(XmlReader xmlReader)
		{
			List<AreaInfo> areas = new List<AreaInfo>();
			xmlReader.ReadStartElement("Areas");
			while (xmlReader.IsStartElement("Area"))
			{
				areas.Add(ReadArea(xmlReader));
			}
			xmlReader.ReadEndElement(); // Areas
			string[] areaCodes = { "009", "008" };
			for (int i = 0; i < areaCodes.Length; i++)
			{
				var foundAreas = areas.Where(area => area.AreaCode == areaCodes[i]);
				if (foundAreas.Count() > 0) return foundAreas.First().AreaValue;
			}
			// If not found any areas of known types, use first acceptable, if any
			return (areas.Count > 0) ? areas[0].AreaValue : 0.0;
		}

		public static string ReadLocation(XmlReader xmlReader)
		{
			xmlReader.ReadStartElement("Location");
			while (xmlReader.IsStartElement())
				xmlReader.Skip();
			xmlReader.ReadEndElement(); // Location
			return "";
		}

		public static string ReadCategory(XmlReader xmlReader)
		{
			string landCategoryCode = xmlReader.GetAttribute("Category");
			xmlReader.ReadFullElement("Category");
			return landCategoryCode;
		}

		public static LandUtilizationDTO ReadUtilization(XmlReader xmlReader)
		{
			string landUtilizationKindCode = xmlReader.GetAttribute("Kind");
			string landUtilizationDescription = xmlReader.GetAttribute("ByDoc");
			xmlReader.ReadFullElement("Utilization");
			return new LandUtilizationDTO(landUtilizationKindCode, landUtilizationDescription);
		}

		public static IList<LandRightDTO> ReadRights(XmlReader xmlReader)
		{
			List<LandRightDTO> rights = new List<LandRightDTO>();
			xmlReader.ReadStartElement("Rights");
			while (xmlReader.IsStartElement("Right"))
			{
				xmlReader.ReadStartElement("Right");
				xmlReader.ReadStartElement("Name");
				string name = xmlReader.ReadContentAsString();
				xmlReader.ReadEndElement(); // Name
				xmlReader.ReadStartElement("Type");
				string landRightTypeCode = xmlReader.ReadContentAsString();
				xmlReader.ReadEndElement(); // Type
				xmlReader.ReadEndElement(); // Right
				rights.Add(new LandRightDTO(landRightTypeCode, name));
			}
			xmlReader.ReadEndElement(); // Rights
			return rights;
		}

		public static LandEncumbranceDTO ReadEncumbrance(XmlReader xmlReader)
		{
			xmlReader.ReadStartElement("Encumbrance");
			xmlReader.ReadStartElement("Name");
			string name = xmlReader.ReadContentAsString();
			xmlReader.ReadEndElement(); // Name
			xmlReader.ReadStartElement("Type");
			string landEncumbranceTypeCode = xmlReader.ReadContentAsString();
			xmlReader.ReadEndElement(); // Type
			xmlReader.ReadEndElement(); // Encumbrance
			return new LandEncumbranceDTO(landEncumbranceTypeCode, name);
		}

		public static IList<LandEncumbranceDTO> ReadEncumbrances(XmlReader xmlReader)
		{
			List<LandEncumbranceDTO> encumbrances = new List<LandEncumbranceDTO>();
			xmlReader.ReadStartElement("Encumbrances");
			while (xmlReader.IsStartElement("Encumbrance"))
			{
				encumbrances.Add(ReadEncumbrance(xmlReader));
			}
			xmlReader.ReadEndElement(); // Encumbrances
			return encumbrances;
		}

		public static IList<SubParcelDTO> ReadSubParcels(XmlReader xmlReader, uint version)
		{
			List<SubParcelDTO> subParcels = new List<SubParcelDTO>();
			xmlReader.ReadStartElement("SubParcels");
			while (xmlReader.IsStartElement("SubParcel"))
			{
				int number = int.Parse(xmlReader.GetAttribute(version < 8 ? "Number_PP" : "Number_Record"));
				string stateCode = xmlReader.GetAttribute("State");
				IList<LandEncumbranceDTO> encumbrances = null;
				xmlReader.ReadStartElement("SubParcel");
				if (version < 8)
				{
					if (xmlReader.IsStartElement("Encumbrances"))
						encumbrances = ReadEncumbrances(xmlReader);
				}
				else
				{
					if (xmlReader.IsStartElement("Encumbrance"))
						encumbrances = new List<LandEncumbranceDTO> { ReadEncumbrance(xmlReader) };
				}
				xmlReader.ReadEndElement(); // SubParcel
				subParcels.Add(new SubParcelDTO(number, stateCode, encumbrances));
			}
			xmlReader.ReadEndElement(); // SubParcels
			return subParcels;
		}

		public static ParcelNumber ReadParentParcelNumber(XmlReader xmlReader)
		{
			xmlReader.ReadStartElement("Unified_Land_Unit");
			xmlReader.ReadStartElement("Preceding_Land_Unit");
			ParcelNumber parcelNumber = ParcelNumber.Parse(xmlReader.ReadContentAsString());
			xmlReader.ReadEndElement(); // Preceding_Land_Unit
			xmlReader.ReadEndElement(); // Unified_Land_Unit
			return parcelNumber;
		}

		public static MultiPolygon ReadContours(XmlReader xmlReader)
		{
			List<Polygon> polygons = new List<Polygon>();
			xmlReader.ReadStartElement("Contours");
			while (xmlReader.IsStartElement("Contour"))
			{
				xmlReader.ReadStartElement("Contour");
				polygons.Add(ReadEntitySpatial(xmlReader));
				xmlReader.ReadEndElement(); // Contour
			}
			xmlReader.ReadEndElement(); // Contours
			return new MultiPolygon(polygons.ToArray());
		}

		public static Polygon ReadEntitySpatial(XmlReader xmlReader)
		{
			Polygon polygon = null;
			xmlReader.ReadStartElement("Entity_Spatial");
			//// Read outer polygon ring
			//if (xmlReader.IsStartElement("Spatial_Element"))
			//{
			//    outerRing = ReadSpatialElement(xmlReader);
			//}
			//else return null; // !
			
			//// Read inner polygon rings, if exist
			//if (xmlReader.IsStartElement("Spatial_Element"))
			//{
			//    List<LinearRing> innerRings = new List<LinearRing>();
			//    while (xmlReader.IsStartElement("Spatial_Element"))
			//        innerRings.Add(ReadSpatialElement(xmlReader));
			//    polygon = new Polygon(outerRing, innerRings.ToArray());
			//}
			//else
			//{
			//    polygon = new Polygon(outerRing);
			//}

			List<LinearRing> rings = new List<LinearRing>();
			while (xmlReader.IsStartElement("Spatial_Element"))
				rings.Add(ReadSpatialElement(xmlReader));

			polygon = TopologyUtils.BuildNormalizedPolygon(rings);

			polygon.SRID = 995611; // TODO: Read coordinate system from Ent_Sys attrbiute and use coordinate system table for this XML

			xmlReader.ReadEndElement(); // Entity_Spatial
			return polygon;
		}

		public static LinearRing ReadSpatialElement(XmlReader xmlReader)
		{
			//
			IDictionary<uint, Coordinate> coordinates = new Dictionary<uint, Coordinate>();
			xmlReader.ReadStartElement("Spatial_Element");
			while (xmlReader.IsStartElement("Spelement_Unit"))
			{
				string unitType = xmlReader.GetAttribute("Type_Unit");
				uint unitNumber = uint.Parse(xmlReader.GetAttribute("Su_Nmb"));
				xmlReader.ReadStartElement("Spelement_Unit");
				/// !!! Use switch
				if (unitType == "Точка")
				{
					Coordinate coordinate = ReadOrdinate(xmlReader);
					if (coordinates.ContainsKey(unitNumber - 1))
					{
						if (!coordinates[unitNumber - 1].Equals(coordinate)) throw new Exception("Invalid spatial element");
					}
					else
					{
						coordinates[unitNumber - 1] = coordinate;
					}
				}
				else if (unitType == "Линия")
				{
					Coordinate coordinate1 = ReadOrdinate(xmlReader);
					Coordinate coordinate2 = ReadOrdinate(xmlReader);
					if (coordinates.ContainsKey(unitNumber - 1))
					{
						if (!coordinates[unitNumber - 1].Equals(coordinate1)) throw new Exception("Invalid spatial element");
					}
					else
					{
						coordinates[unitNumber - 1] = coordinate1;
					}

					if (coordinates.ContainsKey(unitNumber))
					{
						if (!coordinates[unitNumber].Equals(coordinate2)) throw new Exception("Invalid spatial element");
					}
					else
					{
						coordinates[unitNumber] = coordinate2;
					}
				}
				else throw new Exception("Unsupported spatial element type");
				xmlReader.ReadEndElement(); // Spelement_Unit
			}
			xmlReader.ReadEndElement(); // Spatial_Element

			// Manually close the contour, if needed
			uint minCoordinateIndex = coordinates.Keys.Min();
			uint maxCoordinateIndex = coordinates.Keys.Max();
			if (coordinates[minCoordinateIndex] != coordinates[maxCoordinateIndex])
				coordinates.Add(maxCoordinateIndex + 1, coordinates[minCoordinateIndex]);

			return new LinearRing(coordinates.OrderBy(kv => kv.Key).Select(kv => kv.Value).ToArray()); // !!! More intelligent conversion required
		}

		public static Coordinate ReadOrdinate(XmlReader xmlReader)
		{
			if (!xmlReader.IsStartElement("Ordinate")) throw new Exception("<Ordinate> element expected");
			double x = double.Parse(xmlReader.GetAttribute("X").Replace('.', ','));
			double y = double.Parse(xmlReader.GetAttribute("Y").Replace('.', ','));
			xmlReader.ReadStartElement("Ordinate");
			return new Coordinate(x, y);
		}

		public static void ReadFullElement(this XmlReader xmlReader, string name)
		{
			bool isEmptyElement = xmlReader.IsEmptyElement;
			xmlReader.ReadStartElement(name);
			if (!isEmptyElement)
				xmlReader.ReadEndElement();
		}
	}
}
