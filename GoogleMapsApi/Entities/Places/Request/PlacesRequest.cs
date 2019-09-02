﻿using GoogleMapsApi.Entities.Common;
using System;
using System.Globalization;

namespace GoogleMapsApi.Entities.Places.Request
{
    /// <summary>
    /// The Google Places API is a service that returns information about a "place" (hereafter referred to as a Place) — defined within this API as an establishment, a geographic location, or prominent point of interest — using an HTTP request. Place requests specify locations as latitude/longitude coordinates.
    /// Two basic Place requests are available: a Place Search request and a Place Details request. Generally, a Place Search request is used to return candidate matches, while a Place Details request returns more specific information about a Place.
    /// This service is designed for processing place requests generated by a user for placement of application content on a map; this service is not designed to respond to batch of offline queries, which are a violation of its terms of use.
    /// </summary>
    public class PlacesRequest : MapsBaseRequest
	{
		protected internal override string BaseUrl => "maps.googleapis.com/maps/api/place/search/";

        /// <summary>
		/// location (required) — The textual latitude/longitude value from which you wish to retrieve place information.
		/// </summary>
		public Location Location { get; set; } //Required

		/// <summary>
		/// Defines the distance (in meters) within which to return Place results. The maximum allowed radius is 50 000 meters. 
		/// Note that radius must not be included if rankby=distance (described under Optional parameters below) is specified.
		/// </summary>
		public double? Radius { get; set; }

		/// <summary>
		/// A term to be matched against all content that Google has indexed for this Place, 
		/// including but not limited to name, type, and address, as well as customer reviews and other third-party content.
		/// </summary>
		public string Keyword { get; set; }

		/// <summary>
		/// The language code, indicating in which language the results should be returned, if possible. See the list of supported languages and their codes. 
		/// Note that we often update supported languages so this list may not be exhaustive.
		/// </summary>
		public string Language { get; set; }

		/// <summary>
		///  A term to be matched against the names of Places. Results will be restricted to those containing the passed name value.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Specifies the order in which results are listed
		/// </summary>
		public RankBy RankBy { get; set; }

		/// <summary>
		/// Restricts the results to Places matching the specified type. 
		/// See the list of supported types - https://developers.google.com/maps/documentation/places/supported_types
		/// 
		/// </summary>
		public string Type { get; set; }

        /// <summary>
        /// If there is a next page of results, this token will be supplied by Google in a previous PlacesResponse. PageToken should be set to the previous response value to get the next page of results from Google.
        /// When PageToken is set, all other Request parameters are ignored.
        /// </summary>
        public string PageToken { get; set; }

        protected override bool IsSsl
		{
			get => true;
            set => throw new NotSupportedException("This operation is not supported, PlacesRequest must use SSL");
        }

		protected override QueryStringParametersList GetQueryStringParameters()
		{
			if (Location == null)
				throw new ArgumentException("Location must be provided.");
			if (Radius.HasValue && (Radius > 50000 || Radius < 1))
				throw new ArgumentException("Radius must be greater than or equal to 1 and less than or equal to 50000.");
			if (!Radius.HasValue && RankBy != RankBy.Distance)
				throw new ArgumentException("Radius must be specified unless RankBy is 'Distance'.");
			if (!Enum.IsDefined(typeof(RankBy), RankBy))
				throw new ArgumentException("Invalid RankBy value.");
			if (string.IsNullOrWhiteSpace(ApiKey))
				throw new ArgumentException("ApiKey must be provided");

			var parameters = base.GetQueryStringParameters();
			parameters.Add("location", Location.ToString());
			parameters.Add("key", ApiKey);

			if (Radius.HasValue)
				parameters.Add("radius", Radius.Value.ToString(CultureInfo.InvariantCulture));
			if (!string.IsNullOrWhiteSpace(Keyword))
                parameters.Add("keyword", Keyword);
			if (!string.IsNullOrWhiteSpace(Language))
				parameters.Add("language", Language);
			if (!string.IsNullOrWhiteSpace(Type))
				parameters.Add("type", Type);
			if (!string.IsNullOrWhiteSpace(Name))
				parameters.Add("name", Name);
			if (RankBy == RankBy.Distance)
				parameters.Add("rankby", "distance");
            if (!string.IsNullOrWhiteSpace(PageToken))
                parameters.Add("pagetoken", PageToken);

			return parameters;
		}
	}
}
