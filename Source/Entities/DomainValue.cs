using System;

namespace LandRush.Cadastre
{
	// !!!!
	public class DomainValue : IComparable<DomainValue>
	{
		protected DomainValue() { }
		public DomainValue(string code, string description) // !
		{
			this.code = code;
			this.description = description;
		}

		private string code;
		public virtual string Code
		{
			get
			{
				return code;
			}
			protected set
			{
				code = value;
			}
		}

		private string description;
		public virtual string Description
		{
			get
			{
				return description;
			}
			protected set
			{
				description = value;
			}
		}

		public virtual int CompareTo(DomainValue other)
		{
			return this.code.CompareTo(other.code);
		}

		public override string ToString()
		{
			return description; // !
		}
	}
}