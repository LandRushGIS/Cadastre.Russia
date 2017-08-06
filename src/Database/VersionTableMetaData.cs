namespace LandRush.Cadastre.Russia.Database
{
	[FluentMigrator.VersionTableInfo.VersionTableMetaData]
	public class VersionTableMetaData : FluentMigrator.VersionTableInfo.DefaultVersionTableMetaData
	{
		public override string SchemaName => "cadastre";
		public override string TableName => "_version";
		public override string ColumnName => "number";
		public override string UniqueIndexName => Naming.IdxName(this.TableName, this.ColumnName);
		public override string AppliedOnColumnName => "appliedon";
		public override string DescriptionColumnName => "description";
	}
}
