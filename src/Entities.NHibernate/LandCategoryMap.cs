using FluentNHibernate.Mapping;

namespace LandRush.Cadastre.Russia.NHibernate
{
	public class LandCategoryMap : ClassMap<LandCategory>
	{
		public LandCategoryMap()
		{
			Schema("cadastre");
			Table("landcategory");

			Id(x => x.Code);
			Map(x => x.Description)
				.Length(100);
		}
	}
}