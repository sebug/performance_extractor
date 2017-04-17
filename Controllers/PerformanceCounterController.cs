using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

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

		public string Get(int start = 0)
		{
			return this._optionsAccessor.Value.GetUrlTemplate;
		}
	}
}
