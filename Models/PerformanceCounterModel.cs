using System;
using System.Collections.Generic;

namespace performance_extractor
{
	public class PerformanceCounterModel
	{
		public string UserName { get; set; }
		public Dictionary<string, int> Splits { get; set; }
		public int TotalMs { get; set; }
	}
}
