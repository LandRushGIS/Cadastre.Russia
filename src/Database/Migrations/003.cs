using FluentMigrator;

namespace LandRush.Cadastre.Russia.Database.Migrations
{
	using static Naming;

	[Migration(3)]
	public class AddMorePersonAndPassportInfo : AutoReversingMigration
	{
		private void UpdateTablePerson()
		{
			string name = "person";
			Alter.Table(name).InSchema("cadastre")
				.AddColumn("birthdate").AsDate().Nullable().WithDefaultValue(null)
				.AddColumn("deathdate").AsDate().Nullable().WithDefaultValue(null)
				.AddColumn("tin").AsFixedLengthString(12).Nullable().WithDefaultValue(null)
				.AddColumn("address").AsString(4000).Nullable().WithDefaultValue(null);
		}

		private void CreateTablePersonPhoneNumber()
		{
			string name = "personphonenumber";
			string pkName = PkName(name);
			Create.Table(name).InSchema("cadastre")
				// key columns
				.WithColumn("personid").AsInt32().NotNullable().PrimaryKey(pkName)
					.ForeignKey(FkName(name, "person"), "cadastre", "person", "id")
					.Indexed(IdxName(name, "personid"))
				.WithColumn("phonenumber").AsString().NotNullable().PrimaryKey(pkName);
		}

		private void UpdateTablePassport()
		{
			string name = "passport";
			Rename.Column("issuer").OnTable(name).InSchema("cadastre")
				.To("authorityname");
			Alter.Table(name).InSchema("cadastre")
				.AddColumn("authoritycode").AsFixedLengthString(6).Nullable().WithDefaultValue(null);
		}

		public override void Up()
		{
			UpdateTablePerson();
			CreateTablePersonPhoneNumber();
			UpdateTablePassport();
		}
	}
}
