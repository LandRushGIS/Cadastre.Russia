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
			Map(x => x.BirthDate)
				.Access.CamelCaseField()
				.Nullable();
			Map(x => x.DeathDate)
				.Access.CamelCaseField()
				.Nullable();
			Map(x => x.TIN)
				.Access.LowerCaseField()
				.Length(12)
				.Nullable();
			Map(x => x.Address)
				.Access.CamelCaseField()
				.Length(4000)
				.Nullable();
			HasMany(x => x.PhoneNumbers)
				.Schema("cadastre")
				.Table("personphonenumber")
				.KeyColumn("personid")
				.Element("phonenumber")
				.Cascade.AllDeleteOrphan()
				.LazyLoad()
				.Fetch.Subselect()
				.AsSet();
		}
	}
}
