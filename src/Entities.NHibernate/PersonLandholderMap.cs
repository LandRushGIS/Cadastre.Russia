using FluentNHibernate.Mapping;

namespace LandRush.Cadastre.Russia.NHibernate
{
	public class PersonLandholderMap : SubclassMap<PersonLandholder>
	{
		public PersonLandholderMap()
		{
			Schema("cadastre");
			Table("personlandholder");

			KeyColumn("id");
			References(x => x.Person)
				.Access.CamelCaseField()
				.Column("PersonId");
		}
	}
}
