﻿using System.Text.Json.Serialization;

namespace Fina.Core.Responses
{
	public abstract class Response<TData>
	{
		private int _code = Configuration.DefaultStatusCode;

        [JsonConstructor]
        protected Response()
            =>  _code = Configuration.DefaultStatusCode;

        protected Response(TData? data, int code = Configuration.DefaultStatusCode, string? message = null)
        {
            Data = data;
            _code = code;
            Message = message;
        }

        public TData? Data { get; set; }
        public string? Message { get; set; }

        [JsonIgnore]
		public bool IsSuccess => _code is >= 200 and <= 299;
    }
}
