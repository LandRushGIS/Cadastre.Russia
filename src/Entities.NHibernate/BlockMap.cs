using FluentNHibernate.Mapping;
using NHSpatial = NHibernate.Spatial;

namespace LandRush.Cadastre.Russia.NHibernate
{
	public class BlockMap : ClassMap<Block>
	{
		public BlockMap()
		{
			Schema("cadastre");
			Table("block");

			CompositeId()
				.KeyReference(
					x => x.District,
					m => m
						.Access.CamelCaseField(),
						"RegionNumber", "DistrictNumber")
				.KeyProperty(
					x => x.LocalNumber,
					m => m
						.Access.CamelCaseField()
						.ColumnName("Number"));

			Map(x => x.Name);
			Map(x => x.Note);
			Map(x => x.DocumentedArea)
				.Access.CamelCaseField();
			Map(x => x.Geometry)
				.CustomType<NHSpatial.Type.GeometryType>()
				.Nullable();
		}
	}
}
