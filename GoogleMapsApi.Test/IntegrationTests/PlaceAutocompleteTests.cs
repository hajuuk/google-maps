﻿using GoogleMapsApi.Entities.PlaceAutocomplete.Request;
using GoogleMapsApi.Entities.PlaceAutocomplete.Response;
using GoogleMapsApi.Test.Utils;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [TestFixture]
    class PlaceAutocompleteTests : BaseTestIntegration
    {
        [Test]
        public async Task ReturnsNoResultsAsync()
        {
            var request = new PlaceAutocompleteRequest
            {
                ApiKey = base.ApiKey,
                Input = "zxqtrb",
                Location = new Entities.Common.Location(53.4635332, -2.2419169),
                Radius = 30000
            };

            PlaceAutocompleteResponse result = await GoogleMaps.PlaceAutocomplete.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.ZERO_RESULTS, result.Status);
        }

        [Test]
        [System.Obsolete]
        public void ReturnsNoResults()
        {
            var request = new PlaceAutocompleteRequest
            {
                ApiKey = base.ApiKey,
                Input = "zxqtrb",
                Location = new Entities.Common.Location(53.4635332, -2.2419169),
                Radius = 30000
            };

            PlaceAutocompleteResponse result = GoogleMaps.PlaceAutocomplete.Query(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.ZERO_RESULTS, result.Status);
        }

        [Test]
        public async Task OffsetTestAsync()
        {
            var request = new PlaceAutocompleteRequest
            {
                ApiKey = base.ApiKey,
                Input = "abbeyjibberish",
                Location = new Entities.Common.Location(53.4635332, -2.2419169),
                Radius = 30000
            };

            PlaceAutocompleteResponse result = await GoogleMaps.PlaceAutocomplete.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.ZERO_RESULTS, result.Status, "results for jibberish");

            var offsetRequest = new PlaceAutocompleteRequest
            {
                ApiKey = base.ApiKey,
                Input = "abbeyjibberish",
                Offset = 5,
                Location = new Entities.Common.Location(53.4635332, -2.2419169)
            };

            PlaceAutocompleteResponse offsetResult = await GoogleMaps.PlaceAutocomplete.QueryAsync(offsetRequest);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.OK, offsetResult.Status, "results using offset");
        }

        [Test]
        [System.Obsolete]
        public void OffsetTest()
        {
            var request = new PlaceAutocompleteRequest
            {
                ApiKey = base.ApiKey,
                Input = "abbeyjibberish",
                Location = new Entities.Common.Location(53.4635332, -2.2419169),
                Radius = 30000
            };

            PlaceAutocompleteResponse result = GoogleMaps.PlaceAutocomplete.Query(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.ZERO_RESULTS, result.Status, "results for jibberish");

            var offsetRequest = new PlaceAutocompleteRequest
            {
                ApiKey = base.ApiKey,
                Input = "abbeyjibberish",
                Offset = 5,
                Location = new Entities.Common.Location(53.4635332, -2.2419169)
            };

            PlaceAutocompleteResponse offsetResult = GoogleMaps.PlaceAutocomplete.Query(offsetRequest);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.OK, offsetResult.Status, "results using offset");
        }

        [Test]
        public async Task TypeTestAsync()
        {
            var request = new PlaceAutocompleteRequest
            {
                ApiKey = base.ApiKey,
                Input = "abb",
                Type = "geocode",
                Location = new Entities.Common.Location(53.4635332, -2.2419169),
                Radius = 30000
            };

            PlaceAutocompleteResponse result = await GoogleMaps.PlaceAutocomplete.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.OK, result.Status);

            foreach (var oneResult in result.Results)
            {
                Assert.IsNotNull(oneResult.Types, "result with no type classification");
                Assert.IsTrue(new List<string>(oneResult.Types).Contains("geocode"), "non-geocode result");
            }
        }

        [Test]
        [System.Obsolete]
        public void TypeTest()
        {
            var request = new PlaceAutocompleteRequest
            {
                ApiKey = base.ApiKey,
                Input = "abb",
                Type = "geocode",
                Location = new Entities.Common.Location(53.4635332, -2.2419169),
                Radius = 30000
            };

            PlaceAutocompleteResponse result = GoogleMaps.PlaceAutocomplete.Query(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.OK, result.Status);

            foreach (var oneResult in result.Results)
            {
                Assert.IsNotNull(oneResult.Types, "result with no type classification");
                Assert.IsTrue(new List<string>(oneResult.Types).Contains("geocode"), "non-geocode result");
            }
        }


        [TestCase("oakfield road, chea", "CHEADLE")]
        [TestCase("128 abbey r", "MACCLESFIELD")]
        public async Task CheckForExpectedRoadAsync(string aSearch, string anExpected)
        {
            var request = new PlaceAutocompleteRequest
            {
                ApiKey = base.ApiKey,
                Input = aSearch,
                Location = new GoogleMapsApi.Entities.Common.Location(53.4635332, -2.2419169),
                Radius = 30000
            };

            PlaceAutocompleteResponse result = await GoogleMaps.PlaceAutocomplete.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreNotEqual(Status.ZERO_RESULTS, result.Status);

            Assert.That(result.Results.Any(t => t.Description.ToUpper().Contains(anExpected)));
        }

        [TestCase("oakfield road, chea", "CHEADLE")]
        [TestCase("128 abbey r", "MACCLESFIELD")]
        [System.Obsolete]
        public void CheckForExpectedRoad(string aSearch, string anExpected)
        {
            var request = new PlaceAutocompleteRequest
            {
                ApiKey = base.ApiKey,
                Input = aSearch,
                Location = new GoogleMapsApi.Entities.Common.Location(53.4635332, -2.2419169),
                Radius = 30000
            };

            PlaceAutocompleteResponse result = GoogleMaps.PlaceAutocomplete.Query(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreNotEqual(Status.ZERO_RESULTS, result.Status);

            Assert.That(result.Results.Any(t => t.Description.ToUpper().Contains(anExpected)));
        }

        [Test(Description = "Ensures that it is ok to sent 0 as a radius value")]
        public async Task CheckZeroRadiusAsync()
        {
            var request = CreatePlaceAutocompleteRequest("RIX", 0);
            PlaceAutocompleteResponse result = await GoogleMaps.PlaceAutocomplete.QueryAsync(request);
            AssertInconclusive.NotExceedQuota(result);
            Assert.AreNotEqual(Status.ZERO_RESULTS, result.Status);
        }

        [Test(Description = "Ensures that it is ok to sent 0 as a radius value")]
        [System.Obsolete]
        public void CheckZeroRadius()
        {
            var request = CreatePlaceAutocompleteRequest("RIX", 0);
            PlaceAutocompleteResponse result = GoogleMaps.PlaceAutocomplete.Query(request);
            AssertInconclusive.NotExceedQuota(result);
            Assert.AreNotEqual(Status.ZERO_RESULTS, result.Status);
        }

        [Test(Description = "Ensures that it is ok to sent negative value as a radius")]
        public async Task CheckNegativeRadiusAsync()
        {
            var request = CreatePlaceAutocompleteRequest("RIX", -1);
            PlaceAutocompleteResponse result = await GoogleMaps.PlaceAutocomplete.QueryAsync(request);
            AssertInconclusive.NotExceedQuota(result);
            Assert.AreNotEqual(Status.ZERO_RESULTS, result.Status);
        }

        [Test(Description = "Ensures that it is ok to sent negative value as a radius")]
        [System.Obsolete]
        public void CheckNegativeRadius()
        {
            var request = CreatePlaceAutocompleteRequest("RIX", -1);
            PlaceAutocompleteResponse result = GoogleMaps.PlaceAutocomplete.Query(request);
            AssertInconclusive.NotExceedQuota(result);
            Assert.AreNotEqual(Status.ZERO_RESULTS, result.Status);
        }

        [Test(Description = "Ensures that it is ok to sent huge value as a radius")]
        public async Task CheckLargerThenEarthRadiusAsync()
        {
            var request = CreatePlaceAutocompleteRequest("RIX", 30000000);
            PlaceAutocompleteResponse result = await GoogleMaps.PlaceAutocomplete.QueryAsync(request);
            AssertInconclusive.NotExceedQuota(result);
            Assert.AreNotEqual(Status.ZERO_RESULTS, result.Status);
        }

        [Test(Description = "Ensures that it is ok to sent huge value as a radius")]
        [System.Obsolete]
        public void CheckLargerThenEarthRadius()
        {
            var request = CreatePlaceAutocompleteRequest("RIX", 30000000);
            PlaceAutocompleteResponse result = GoogleMaps.PlaceAutocomplete.Query(request);
            AssertInconclusive.NotExceedQuota(result);
            Assert.AreNotEqual(Status.ZERO_RESULTS, result.Status);
        }

        private PlaceAutocompleteRequest CreatePlaceAutocompleteRequest(string query, double? radius)
        {
            return new PlaceAutocompleteRequest
            {
                ApiKey = base.ApiKey,
                Input = query,
                Location = new Entities.Common.Location(0, 0),
                Radius = radius
            };
        }
    }
}