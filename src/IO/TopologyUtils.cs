using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoAPI;
using GeoAPI.Geometries;
using NetTopologySuite;
using NetTopologySuite.Algorithm;
using NetTopologySuite.Geometries;

namespace LandRush.Cadastre.Russia.IO
{
	public static class TopologyUtils
	{
		// Builds normalized polygon: detects outer and inner rings by areas, orients outer ring clockwise, inner rings - counter-clockwise
		public static Polygon BuildNormalizedPolygon(List<LinearRing> rings)
		{
			Polygon polygon = null;

			switch (rings.Count)
			{
				case 0: polygon = null; break;
				case 1: polygon = new Polygon(rings[0]); break; // Only outer ring specified
				default: // More than one ring specified
					{
						// Find outer ring - it must be oriented clockwise
						int outerRingIndex = -1;
						double maxArea = 0.0;
						for (int i = 0; i < rings.Count; i++)
						{
							double area = Math.Abs(CGAlgorithms.SignedArea(rings[i].Coordinates));
							if (area > maxArea)
							{
								outerRingIndex = i;
								maxArea = area;
							}
						}
						if (outerRingIndex < 0) outerRingIndex = 0; // !!! throw new Exception("Outer ring is not defined");
						LinearRing outerRing = CGAlgorithms.IsCCW(rings[outerRingIndex].Coordinates) ? new LinearRing((rings[outerRingIndex].Reverse() as LineString).Coordinates) : rings[outerRingIndex];

						// Inner rings must be oriented counter-clockwise
						List<LinearRing> innerRings = new List<LinearRing>();
						for (int i = 0; i < rings.Count; i++)
							if (i != outerRingIndex)
							{
								LinearRing innerRing = CGAlgorithms.IsCCW(rings[i].Coordinates) ? rings[i] : new LinearRing((rings[i].Reverse() as LineString).Coordinates);
								if (new Polygon(outerRing).Covers(new Polygon(innerRing)))
									innerRings.Add(innerRing);
								else
								{
									int a = innerRings.Count;// Skip adding inner ring // !!! throw new Exception("Invalid geometry");
								}
							}

						// Build polygon
						polygon = new Polygon(outerRing, innerRings.ToArray());
					}
					break;
			}

			return polygon;
		}
	}
}
