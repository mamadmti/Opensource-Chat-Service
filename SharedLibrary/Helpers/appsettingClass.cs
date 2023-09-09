using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Helpers
{
	public class appSettingClass
	{
		public string? emailServiceUrl { get; set; }
		public Guid apiKey { get; set; }
        public string SwaggerUsername { get; set; }
		public string SwaggerPassword { get; set; }

	}
}
