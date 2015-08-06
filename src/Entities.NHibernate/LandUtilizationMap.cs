using FluentNHibernate.Mapping;

namespace LandRush.Cadastre.Russia.NHibernate
{
	public class LandUtilizationMap : ComponentMap<LandUtilization>
	{
		public LandUtilizationMap()
		{
			References(x => x.Kind)
				.Access.CamelCaseField()
				.Column("KindCode");
			Map(x => x.Description)
				.Access.CamelCaseField()
				.Column("Description");
		}
	}
}