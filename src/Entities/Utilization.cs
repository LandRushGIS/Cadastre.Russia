namespace LandRush.Cadastre.Russia
{
	/// <summary>
	/// Вид использования земель
	/// </summary>
	public class LandUtilizationKind : DomainValue
	{
		protected LandUtilizationKind() { }
		public LandUtilizationKind(string code, string description) : base(code, description) { }
	}

	/// <summary>
	/// Использование земель
	/// </summary>
	// TODO: rename to ParcelLandUtilization, make reference type
	public struct LandUtilization
	{
		public LandUtilization(LandUtilizationKind kind, string description)
		{
			this.kind = kind;
			this.description = description;
		}

		private LandUtilizationKind kind;
		private string description;

		/// <summary>
		/// Вид использования земель
		/// </summary>
		public LandUtilizationKind Kind { get { return kind; } }

		/// <summary>
		/// Подробное описание использования земель (по документам)
		/// </summary>
		public string Description { get { return description; } }
	}
}