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
			Map(x => x.BookValue)
				.Access.CamelCaseField();
			References(x => x.LandKind)
				.Column("LandKindId")
				.Not.LazyLoad();
			References(x => x.LandUserRightType)
				.Column("LandUserRightTypeId")
				.Not.LazyLoad();
			Map(x => x.LeaseStartDate)
				.Access.CamelCaseField();
			Map(x => x.LeaseEndDate)
				.Access.CamelCaseField();
			Map(x => x.Note);
		}
	}
}