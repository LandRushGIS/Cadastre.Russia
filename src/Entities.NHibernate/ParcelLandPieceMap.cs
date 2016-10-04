using FluentNHibernate.Mapping;
using NHSpatial = NHibernate.Spatial;

namespace LandRush.Cadastre.Russia.NHibernate
{
	public class ParcelLandPieceMap : ClassMap<ParcelLandPiece>
	{
		public ParcelLandPieceMap()
		{
			Schema("cadastre");
			Table("parcellandpiece");

			CompositeId()
				.KeyReference(
					x => x.Parcel,
					m => m
						.Access.CamelCaseField(),
						"RegionNumber", "DistrictNumber", "BlockNumber", "ParcelNumber")
				.KeyProperty(
					x => x.Number,
					m => m
						.Access.CamelCaseField()
						.ColumnName("Number"));

			Map(x => x.Geometry)
				.CustomType<NHSpatial.Type.GeometryType>()
				.Nullable();
			Map(x => x.Address);
			Map(x => x.AssessedValue)
				.Access.CamelCaseField();
			Map(x => x.Note);
		}
	}
}