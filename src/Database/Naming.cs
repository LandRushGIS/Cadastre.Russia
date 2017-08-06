namespace LandRush.Cadastre.Russia.Database
{
	public static class Naming
	{
		public static string PkName(string tableName)
		{
			return $"{tableName}_pk";
		}

		public static string FkName(string tableName)
		{
			return $"{tableName}_fk";
		}

		public static string FkName(string tableName, string name)
		{
			return $"{tableName}_fk_{name}";
		}

		public static string IdxName(string tableName, string name)
		{
			return $"{tableName}_idx_{name}";
		}

		public static string SeqName(string tableName, string name)
		{
			return $"{tableName}_seq_{name}";
		}
	}
}
