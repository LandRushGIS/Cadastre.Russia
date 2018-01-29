namespace LandRush.Cadastre.Russia
{
	/// <summary>
	/// Вид использования земель
	/// </summary>
	public class LandUtilizationKind : DomainValue
	{
		public LandUtilizationKind(string code, string description) : base(code, description) { }
		protected LandUtilizationKind() { }
	}

	/// <summary>
	/// Использование земель
	/// </summary>
	// TODO: rename to ParcelLandUtilization, make reference type
	public struct LandUtilization
	{
		private LandUtilizationKind kind;
		private string description;

		public LandUtilization(LandUtilizationKind kind, string description)
		{
			this.kind = kind;
			this.description = description;
		}

		/// <summary>
		/// Вид использования земель
		/// </summary>
		public LandUtilizationKind Kind => this.kind;

		/// <summary>
		/// Подробное описание использования земель (по документам)
		/// </summary>
		public string Description => this.description;
	}
}
