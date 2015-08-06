using FluentNHibernate.Mapping;

namespace LandRush.Cadastre.Russia.NHibernate
{
	public class LandKindMap : ClassMap<LandKind>
	{
		public LandKindMap()
		{
			Schema("cadastre");
			Table("landkind");

			Id(x => x.Id);
			Map(x => x.Description)
				.Length(100);
		}
	}
}