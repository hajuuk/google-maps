﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Elevation.Request;

namespace GoogleMapsApi.Entities.Elevation.Response
{
	[DataContract]
	public class ElevationResponse : IResponseFor<ElevationRequest>
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
			return $"ElevationResponse - Status: {Status}, Results count: {Results?.Count() ?? 0}";
		}
	}
}
