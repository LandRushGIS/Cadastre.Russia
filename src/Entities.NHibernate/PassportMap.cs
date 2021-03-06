using FluentNHibernate.Mapping;

namespace LandRush.Cadastre.Russia.NHibernate
{
	public class PassportMap : ClassMap<Passport>
	{
		public PassportMap()
		{
			Schema("cadastre");
			Table("passport");

			CompositeId()
				.KeyProperty(
					x => x.Series,
					m => m.Access.CamelCaseField())
				.KeyProperty(
					x => x.Number,
					m => m.Access.CamelCaseField());
			Map(x => x.IssueDate)
				.Access.CamelCaseField();
			Map(x => x.AuthorityCode)
				.Access.CamelCaseField()
				.Length(6)
				.Nullable();
			Map(x => x.AuthorityName)
				.Access.CamelCaseField();
			References(x => x.Person)
				.Access.CamelCaseField()
				.Column("PersonId");
		}
	}
}
