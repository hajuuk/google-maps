﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Geocoding.Request;

namespace GoogleMapsApi.Entities.Geocoding.Response
{
	[DataContract]
	public class GeocodingResponse : IResponseFor<GeocodingRequest>
	{
		[DataMember(Name = "status")]
		internal string StatusStr
		{
			get => Status.ToString();
            set => Status = (Status)Enum.Parse(typeof(Status), value);
        }

		public Status Status { get; set; }

		[DataMember(Name = "results")]
		public IEnumerable<Result> Results { get; set; }

		public override string ToString()
		{
			return $"GeocodingResponse - Status: {Status}, Results count: {(Results != null ? Results.Count() : 0)}";
		}
	}
}