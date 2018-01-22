namespace LandRush.Cadastre.Russia.Database
{
	public static class Naming
	{
		public static string PkName(string tableName) =>
			$"{tableName}_pk";

		public static string FkName(string tableName) =>
			$"{tableName}_fk";

		public static string FkName(string tableName, string name) =>
			$"{tableName}_fk_{name}";

		public static string IdxName(string tableName, string name) =>
			$"{tableName}_idx_{name}";

		public static string SeqName(string tableName, string name) =>
			$"{tableName}_seq_{name}";
	}
}
