using System;

namespace LandRush.Cadastre.Russia
{
	/// <summary>
	/// Кадастровый номер округа
	/// </summary>
	public struct RegionNumber : IComparable<RegionNumber>
	{
		public readonly int LocalNumber;

		public RegionNumber(int localNumber) =>
			this.LocalNumber = localNumber;

		public override string ToString() =>
			$"{this.LocalNumber:D2}";

		public static bool operator ==(RegionNumber regionNumber1, RegionNumber regionNumber2) =>
			regionNumber1.LocalNumber == regionNumber2.LocalNumber;

		public static bool operator !=(RegionNumber regionNumber1, RegionNumber regionNumber2) =>
			!(regionNumber1 == regionNumber2);

		public static RegionNumber Parse(string s) =>
			new RegionNumber(int.Parse(s ?? throw new ArgumentNullException(nameof(s))));

		public static bool TryParse(string s, out RegionNumber result)
		{
			int localNumber;
			if (int.TryParse(s, out localNumber))
			{
				result = new RegionNumber(localNumber);
				return true;
			}
			result = default(RegionNumber);
			return false;
		}

		public int CompareTo(RegionNumber other) =>
			this.LocalNumber.CompareTo(other.LocalNumber);
	}

	/// <summary>
	/// Кадастровый номер района
	/// </summary>
	public struct DistrictNumber : IComparable<DistrictNumber>
	{
		public readonly RegionNumber regionNumber;
		public readonly int localNumber;

		public DistrictNumber(RegionNumber regionNumber, int localNumber)
		{
			this.regionNumber = regionNumber;
			this.localNumber = localNumber;
		}

		public RegionNumber RegionNumber =>
			this.regionNumber;

		public int LocalNumber =>
			this.localNumber;

		public override string ToString() =>
			$"{this.RegionNumber}:{this.LocalNumber:D2}";

		public static bool operator ==(DistrictNumber districtNumber1, DistrictNumber districtNumber2) =>
			districtNumber1.RegionNumber == districtNumber2.RegionNumber &&
			districtNumber1.LocalNumber == districtNumber2.LocalNumber;

		public static bool operator !=(DistrictNumber districtNumber1, DistrictNumber districtNumber2) =>
			!(districtNumber1 == districtNumber2);

		public static DistrictNumber Parse(string s)
		{
			int lastDelimiterPosition = s.LastIndexOf(':');
			if (lastDelimiterPosition <= 0) throw new FormatException();
			return new DistrictNumber(
					RegionNumber.Parse(s.Substring(0, lastDelimiterPosition)),
					int.Parse(s.Substring(lastDelimiterPosition + 1, s.Length - lastDelimiterPosition - 1))
				);
		}

		public static bool TryParse(string s, out DistrictNumber result)
		{
			int delimiterPosition = s.LastIndexOf(':');
			if (delimiterPosition >= 1)
			{
				RegionNumber regionNumber;
				if (RegionNumber.TryParse(s.Substring(0, delimiterPosition), out regionNumber))
				{
					int localNumber;
					if (int.TryParse(s.Substring(delimiterPosition + 1, s.Length - delimiterPosition - 2), out localNumber))
					{
						result = new DistrictNumber(regionNumber, localNumber);
						return true;
					}
				}
			}

			result = default(DistrictNumber);
			return false;
		}

		public int CompareTo(DistrictNumber other)
		{
			int regionNumberCompareResult = this.regionNumber.CompareTo(other.regionNumber);
			return (regionNumberCompareResult == 0) ? this.localNumber.CompareTo(this.localNumber) : regionNumberCompareResult;
		}
	}

	/// <summary>
	/// Кадастровый номер квартала
	/// </summary>
	public struct BlockNumber : IComparable<BlockNumber>
	{
		public readonly DistrictNumber DistrictNumber;
		public readonly int LocalNumber;

		public BlockNumber(DistrictNumber districtNumber, int localNumber)
		{
			this.DistrictNumber = districtNumber;
			this.LocalNumber = localNumber;
		}

		public RegionNumber RegionNumber =>
			this.DistrictNumber.RegionNumber;

		public override string ToString() =>
			$"{this.DistrictNumber}:{this.LocalNumber:D7}";

		public static bool operator ==(BlockNumber blockNumber1, BlockNumber blockNumber2) =>
			blockNumber1.DistrictNumber == blockNumber2.DistrictNumber &&
			blockNumber1.LocalNumber == blockNumber2.LocalNumber;

		public static bool operator !=(BlockNumber blockNumber1, BlockNumber blockNumber2) =>
			!(blockNumber1 == blockNumber2);

		public static BlockNumber Parse(string s)
		{
			int lastDelimiterPosition = s.LastIndexOf(':');
			if (lastDelimiterPosition <= 0) throw new FormatException();
			return new BlockNumber(
					DistrictNumber.Parse(s.Substring(0, lastDelimiterPosition)),
					int.Parse(s.Substring(lastDelimiterPosition + 1, s.Length - lastDelimiterPosition - 1))
				);
		}

		public static bool TryParse(string s, out BlockNumber result)
		{
			int delimiterPosition = s.LastIndexOf(':');
			if (delimiterPosition >= 1)
			{
				DistrictNumber districtNumber;
				if (DistrictNumber.TryParse(s.Substring(0, delimiterPosition), out districtNumber))
				{
					int localNumber;
					if (int.TryParse(s.Substring(delimiterPosition + 1, s.Length - delimiterPosition - 2), out localNumber))
					{
						result = new BlockNumber(districtNumber, localNumber);
						return true;
					}
				}
			}

			result = default(BlockNumber);
			return false;
		}

		public int CompareTo(BlockNumber other)
		{
			int districtNumberCompareResult = this.DistrictNumber.CompareTo(other.DistrictNumber);
			return (districtNumberCompareResult == 0) ? this.LocalNumber.CompareTo(this.LocalNumber) : districtNumberCompareResult;
		}
	}

	/// <summary>
	/// Кадастровый номер участка
	/// </summary>
	public struct ParcelNumber : IComparable<ParcelNumber>, IComparable
	{
		public readonly BlockNumber BlockNumber;
		public readonly int LocalNumber;

		public ParcelNumber(BlockNumber blockNumber, int localNumber)
		{
			this.BlockNumber = blockNumber;
			this.LocalNumber = localNumber;
		}

		public RegionNumber RegionNumber =>
			this.DistrictNumber.RegionNumber;

		public DistrictNumber DistrictNumber =>
			this.BlockNumber.DistrictNumber;

		public override string ToString() =>
			$"{this.BlockNumber}:{this.LocalNumber}";

		public static bool operator ==(ParcelNumber parcelNumber1, ParcelNumber parcelNumber2) =>
			parcelNumber1.BlockNumber == parcelNumber2.BlockNumber &&
			parcelNumber1.LocalNumber == parcelNumber2.LocalNumber;

		public static bool operator !=(ParcelNumber parcelNumber1, ParcelNumber parcelNumber2) =>
			!(parcelNumber1 == parcelNumber2);

		public static ParcelNumber Parse(string s)
		{
			int lastDelimiterPosition = s.LastIndexOf(':');
			if (lastDelimiterPosition <= 0) throw new FormatException();
			return new ParcelNumber(
					BlockNumber.Parse(s.Substring(0, lastDelimiterPosition)),
					int.Parse(s.Substring(lastDelimiterPosition + 1, s.Length - lastDelimiterPosition - 1))
				);
		}

		public static bool TryParse(string s, out ParcelNumber result)
		{
			int delimiterPosition = s.LastIndexOf(':');
			if (delimiterPosition >= 1)
			{
				BlockNumber blockNumber;
				if (BlockNumber.TryParse(s.Substring(0, delimiterPosition), out blockNumber))
				{
					int localNumber;
					if (int.TryParse(s.Substring(delimiterPosition + 1, s.Length - delimiterPosition - 2), out localNumber))
					{
						result = new ParcelNumber(blockNumber, localNumber);
						return true;
					}
				}
			}

			result = default(ParcelNumber);
			return false;
		}

		public int CompareTo(ParcelNumber other)
		{
			int blockNumberCompareResult = this.BlockNumber.CompareTo(other.BlockNumber);
			return (blockNumberCompareResult == 0) ? this.LocalNumber.CompareTo(other.LocalNumber) : blockNumberCompareResult;
		}

		int IComparable.CompareTo(object other) =>
			this.CompareTo((ParcelNumber)other);
	}

	/// <summary>
	/// Составной номер контура участка
	/// </summary>
	public struct ParcelContourNumber : IComparable<ParcelContourNumber>
	{
		public readonly ParcelNumber ParcelNumber;
		public readonly int LocalNumber;

		public ParcelContourNumber(ParcelNumber parcelNumber, int localNumber)
		{
			this.ParcelNumber = parcelNumber;
			this.LocalNumber = localNumber;
		}

		public RegionNumber RegionNumber =>
			this.DistrictNumber.RegionNumber;

		public DistrictNumber DistrictNumber =>
			this.BlockNumber.DistrictNumber;

		public BlockNumber BlockNumber =>
			this.ParcelNumber.BlockNumber;

		public override string ToString() =>
			$"{this.ParcelNumber} ({this.LocalNumber})";

		public static bool operator ==(ParcelContourNumber parcelContourNumber1, ParcelContourNumber parcelContourNumber2) =>
			parcelContourNumber1.ParcelNumber == parcelContourNumber2.ParcelNumber &&
			parcelContourNumber1.LocalNumber == parcelContourNumber2.LocalNumber;

		public static bool operator !=(ParcelContourNumber parcelContourNumber1, ParcelContourNumber parcelContourNumber2) =>
			!(parcelContourNumber1 == parcelContourNumber2);

		public static ParcelContourNumber Parse(string s)
		{
			ParcelContourNumber result;
			bool successfulParsing = TryParse(s, out result);
			if (!successfulParsing) throw new FormatException("Contour number delimiter not found");
			else return result;
		}

		public static bool TryParse(string s, out ParcelContourNumber result)
		{
			int delimiterPosition = s.LastIndexOf('(');
			if (delimiterPosition >= 1)
			{
				ParcelNumber parcelNumber;
				if (ParcelNumber.TryParse(s.Substring(0, delimiterPosition), out parcelNumber))
				{
					int localNumber;
					if (int.TryParse(s.Substring(delimiterPosition + 1, s.Length - delimiterPosition - 2), out localNumber))
					{
						result = new ParcelContourNumber(parcelNumber, localNumber);
						return true;
					}
				}
			}

			result = default(ParcelContourNumber);
			return false;
		}

		public int CompareTo(ParcelContourNumber other)
		{
			int parcelNumberCompareResult = this.ParcelNumber.CompareTo(other.ParcelNumber);
			return (parcelNumberCompareResult == 0) ? this.LocalNumber.CompareTo(this.LocalNumber) : parcelNumberCompareResult;
		}
	}

	/// <summary>
	/// Составной номер части участка
	/// </summary>
	public struct SubParcelNumber : IComparable<SubParcelNumber>
	{
		public readonly ParcelNumber ParcelNumber;
		public readonly int LocalNumber;

		public SubParcelNumber(ParcelNumber parcelNumber, int localNumber)
		{
			this.ParcelNumber = parcelNumber;
			this.LocalNumber = localNumber;
		}

		public RegionNumber RegionNumber =>
			this.DistrictNumber.RegionNumber;

		public DistrictNumber DistrictNumber =>
			this.BlockNumber.DistrictNumber;

		public BlockNumber BlockNumber =>
			this.ParcelNumber.BlockNumber;

		public override string ToString() =>
			$"{this.ParcelNumber}/{this.LocalNumber}";

		public static bool operator ==(SubParcelNumber subParcelNumber1, SubParcelNumber subParcelNumber2) =>
			subParcelNumber1.ParcelNumber == subParcelNumber2.ParcelNumber &&
			subParcelNumber1.LocalNumber == subParcelNumber2.LocalNumber;

		public static bool operator !=(SubParcelNumber subParcelNumber1, SubParcelNumber subParcelNumber2) =>
			!(subParcelNumber1 == subParcelNumber2);

		public static SubParcelNumber Parse(string s)
		{
			SubParcelNumber result;
			bool successfulParsing = TryParse(s, out result);
			if (!successfulParsing) throw new FormatException("SubParcel number delimiter not found");
			else return result;
		}

		public static bool TryParse(string s, out SubParcelNumber result)
		{
			int delimiterPosition = s.LastIndexOf('/');
			if (delimiterPosition >= 1)
			{
				ParcelNumber parcelNumber;
				if (ParcelNumber.TryParse(s.Substring(0, delimiterPosition), out parcelNumber))
				{
					int localNumber;
					if (int.TryParse(s.Substring(delimiterPosition + 1, s.Length - delimiterPosition - 2), out localNumber))
					{
						result = new SubParcelNumber(parcelNumber, localNumber);
						return true;
					}
				}
			}

			result = default(SubParcelNumber);
			return false;
		}

		public int CompareTo(SubParcelNumber other)
		{
			int parcelNumberCompareResult = this.ParcelNumber.CompareTo(other.ParcelNumber);
			return (parcelNumberCompareResult == 0) ? this.LocalNumber.CompareTo(this.LocalNumber) : parcelNumberCompareResult;
		}
	}
}
