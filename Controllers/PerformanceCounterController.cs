﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace performance_extractor.Controllers
{
	[Route("api/[controller]")]
	public class PerformanceCounterController : Controller
	{
		private readonly IOptions<EntrySource> _optionsAccessor;

		public PerformanceCounterController(IOptions<EntrySource> optionsAccessor)
		{
			this._optionsAccessor = optionsAccessor;
		}

		public async Task<IEnumerable<LogEntryModel>> Get(int start = 0)
		{
			var uri = new Uri(
				this._optionsAccessor.Value.GetUrlTemplate.Replace("{start}", start.ToString()));
			using (var client = new HttpClient())
			{
				string resultString = await client.GetStringAsync(
					this._optionsAccessor.Value.GetUrlTemplate.Replace("{start}", start.ToString()));

				var entries = JsonConvert.DeserializeObject<List<LogEntryModel>>(resultString);

				return entries.Where(le => le.Severity.StartsWith("Information", StringComparison.CurrentCultureIgnoreCase));
			}
		}
	}
}
