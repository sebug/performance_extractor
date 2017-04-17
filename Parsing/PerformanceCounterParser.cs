using System;
using System.Text.RegularExpressions;
using System.Linq;

namespace performance_extractor
{
	public class PerformanceCounterParser
	{
		private static Regex pcRegex = new Regex(
			"Performance counters for (?<username>\\S+) (?<counters>.*Total: \\d+ ms)",
			RegexOptions.Compiled | RegexOptions.Multiline);

		private static Regex splitEntryRegex = new Regex("(?<counterName>.*): (?<timeMs>\\d+) ms",
								 RegexOptions.Compiled);

		public PerformanceCounterModel Parse(LogEntryModel logEntry)
		{
			if (logEntry == null || String.IsNullOrEmpty(logEntry.Message))
			{
				return null;
			}
			var m = pcRegex.Match(logEntry.Message);
			if (!m.Success)
			{
				return null;
			}
			var splitArr = m.Groups["counters"].Value.Split('|');

			var splitDict = splitArr.Select(l =>
			{
				var m2 = splitEntryRegex.Match(l);
				if (m2.Success)
				{
					return new
					{
						Name = m2.Groups["counterName"].Value,
						TimeMs = int.Parse(m2.Groups["timeMs"].Value),
						Success = true
					};
				}
				else
				{
					return new
					{
						Name = (string)null,
						TimeMs = 0,
						Success = false
					};
				}
			}).Where(o => o.Success).ToDictionary(o => o.Name, o => o.TimeMs);

			// "Total" key has to exist by definition of first Regex
			int totalMs = splitDict["Total"];
			splitDict.Remove("Total");

			return new PerformanceCounterModel
			{
				Splits = splitDict,
				UserName = m.Groups["username"].Value,
				TotalMs = totalMs
			};
		}
	}
}
