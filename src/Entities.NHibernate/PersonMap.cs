using FluentNHibernate.Mapping;

namespace LandRush.Cadastre.Russia.NHibernate
{
	public class PersonMap : ClassMap<Person>
	{
		public PersonMap()
		{
			Schema("cadastre");
			Table("person");

			Id(x => x.Id)
				.Access.CamelCaseField();
			Map(x => x.FamilyName)
				.Access.CamelCaseField()
				.Length(100);
			Map(x => x.FirstName)
				.Access.CamelCaseField()
				.Length(100);
			Map(x => x.Patronymic)
				.Access.CamelCaseField()
				.Length(100)
				.Nullable();
		}
	}
}
