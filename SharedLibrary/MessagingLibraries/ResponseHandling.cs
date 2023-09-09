using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.MessagingLibraries
{
	public class ResponseHandling
	{
		public HttpStatusCode? StatusCode { get; set; }
		public string? Response { get; set; }
		public object? ReturnedData { get; set; }
		

		public ResponseHandling(HttpStatusCode? statusCode = null, string? response = null, object? returnedData = null )
		{
			StatusCode = statusCode;
			Response = response;
			ReturnedData = returnedData;
		}

	}

}
