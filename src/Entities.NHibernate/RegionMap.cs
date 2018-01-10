using FluentNHibernate.Mapping;

namespace LandRush.Cadastre.Russia.NHibernate
{
	public class RegionMap : ClassMap<Region>
	{
		public RegionMap()
		{
			Schema("cadastre");
			Table("region");

			Id(x => x.LocalNumber)
				.Access.CamelCaseField()
				.Column("Number");
			Map(x => x.Name)
				.Access.CamelCaseField()
				.Length(100);
		}
	}
}
