using FluentNHibernate.Mapping;

namespace LandRush.Cadastre.Russia.NHibernate
{
	public class DistrictMap : ClassMap<District>
	{
		public DistrictMap()
		{
			Schema("cadastre");
			Table("district");

			CompositeId()
				.KeyReference(
					x => x.Region,
					m => m
						.Access.CamelCaseField(),
						"RegionNumber")
				.KeyProperty(
					x => x.LocalNumber,
					m => m
						.Access.CamelCaseField()
						.ColumnName("Number"));
		}
	}
}