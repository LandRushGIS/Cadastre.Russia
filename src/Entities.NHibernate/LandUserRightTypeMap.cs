using FluentNHibernate.Mapping;

namespace LandRush.Cadastre.Russia.NHibernate
{
	public class LandUserRightTypeMap : ClassMap<LandUserRightType>
	{
		public LandUserRightTypeMap()
		{
			Schema("cadastre");
			Table("landuserrighttype");

			Id(x => x.Id);
			Map(x => x.Description)
				.Length(100);
		}
	}
}