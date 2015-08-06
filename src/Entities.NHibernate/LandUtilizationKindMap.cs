using FluentNHibernate.Mapping;

namespace LandRush.Cadastre.Russia.NHibernate
{
	public class LandUtilizationKindMap : ClassMap<LandUtilizationKind>
	{
		public LandUtilizationKindMap()
		{
			Schema("cadastre");
			Table("landutilizationkind");

			Id(x => x.Code);
			Map(x => x.Description)
				.Length(512);
		}
	}
}