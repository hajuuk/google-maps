﻿using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;
using GoogleMapsApi.Test.Utils;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [TestFixture]
    public class DirectionsTests : BaseTestIntegration
    {
        [Test]
        public async Task Directions_ErrorMessageAsync()
        {
            var request = new DirectionsRequest
            {
                ApiKey = "UNIT-TEST-FAKE-KEY", // Wrong API Key
                Origin = "285 Bedford Ave, Brooklyn, NY, USA",
                Destination = "185 Broadway Ave, Manhattan, NY, USA"
            };
            var result = await GoogleMaps.Directions.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(DirectionsStatusCodes.REQUEST_DENIED, result.Status);
            Assert.IsNotNull(result.ErrorMessage);
            Assert.IsNotEmpty(result.ErrorMessage);
        }

        [Test]
        [Obsolete]
        public void Directions_ErrorMessage()
        {
            var request = new DirectionsRequest
            {
                ApiKey = "UNIT-TEST-FAKE-KEY", // Wrong API Key
                Origin = "285 Bedford Ave, Brooklyn, NY, USA",
                Destination = "185 Broadway Ave, Manhattan, NY, USA"
            };
            var result = GoogleMaps.Directions.Query(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(DirectionsStatusCodes.REQUEST_DENIED, result.Status);
            Assert.IsNotNull(result.ErrorMessage);
            Assert.IsNotEmpty(result.ErrorMessage);
        }

        [Test]
        public async Task Directions_WithWayPointsAsync()
        {
            var request = new DirectionsRequest
            {
                Origin = "NYC, USA",
                Destination = "Miami, USA",
                Waypoints = new string[] {"Philadelphia, USA"},
                OptimizeWaypoints = true,
                ApiKey = ApiKey
            };

            var result = await GoogleMaps.Directions.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(DirectionsStatusCodes.OK, result.Status, result.ErrorMessage);
            Assert.AreEqual(156097, result.Routes.First().Legs.First().Steps.Sum(s => s.Distance.Value), 10 * 1000);

            StringAssert.Contains("Philadelphia", result.Routes.First().Legs.First().EndAddress);
        }

        [Test]
        [Obsolete]
        public void Directions_WithWayPoints()
        {
            var request = new DirectionsRequest
            {
                Origin = "NYC, USA",
                Destination = "Miami, USA",
                Waypoints = new string[] {"Philadelphia, USA"},
                OptimizeWaypoints = true,
                ApiKey = ApiKey
            };

            var result = GoogleMaps.Directions.Query(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(DirectionsStatusCodes.OK, result.Status, result.ErrorMessage);
            Assert.AreEqual(156097, result.Routes.First().Legs.First().Steps.Sum(s => s.Distance.Value), 10 * 1000);

            StringAssert.Contains("Philadelphia", result.Routes.First().Legs.First().EndAddress);
        }

        [Test]
        public async Task Directions_Correct_OverviewPathAsync()
        {
            DirectionsRequest request = new DirectionsRequest
            {
                Destination = "maleva 10, Ahtme, Kohtla-Järve, 31025 Ida-Viru County, Estonia",
                Origin = "veski 2, Jõhvi Parish, 41532 Ida-Viru County, Estonia",
                ApiKey = ApiKey
            };

            DirectionsResponse result = await GoogleMaps.Directions.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);

            OverviewPolyline overviewPath = result.Routes.First().OverviewPath;
            OverviewPolyline polyLine = result.Routes.First().Legs.First().Steps.First().PolyLine;
            IEnumerable<Location> points = result.Routes.First().OverviewPath.Points;

            Assert.AreEqual(DirectionsStatusCodes.OK, result.Status, result.ErrorMessage);
            Assert.AreEqual(122, overviewPath.Points.Count(), 30);
            Assert.Greater(polyLine.Points.Count(), 1);
        }

        [Test]
        [Obsolete]
        public void Directions_Correct_OverviewPath()
        {
            DirectionsRequest request = new DirectionsRequest
            {
                Destination = "maleva 10, Ahtme, Kohtla-Järve, 31025 Ida-Viru County, Estonia",
                Origin = "veski 2, Jõhvi Parish, 41532 Ida-Viru County, Estonia",
                ApiKey = ApiKey
            };

            DirectionsResponse result = GoogleMaps.Directions.Query(request);

            AssertInconclusive.NotExceedQuota(result);

            OverviewPolyline overviewPath = result.Routes.First().OverviewPath;
            OverviewPolyline polyLine = result.Routes.First().Legs.First().Steps.First().PolyLine;
            IEnumerable<Location> points = result.Routes.First().OverviewPath.Points;

            Assert.AreEqual(DirectionsStatusCodes.OK, result.Status, result.ErrorMessage);
            Assert.AreEqual(122, overviewPath.Points.Count(), 30);
            Assert.Greater(polyLine.Points.Count(), 1);
        }

        [Test]
        public async Task Directions_SumOfStepDistancesCorrectAsync()
        {
            var request = new DirectionsRequest
            {
                Origin = "285 Bedford Ave, Brooklyn, NY, USA",
                Destination = "185 Broadway Ave, Manhattan, NY, USA",
                ApiKey = ApiKey
            };

            var result = await GoogleMaps.Directions.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(DirectionsStatusCodes.OK, result.Status, result.ErrorMessage);
            Assert.Greater(result.Routes.First().Legs.First().Steps.Sum(s => s.Distance.Value), 100);
        }

        [Test]
        [Obsolete]
        public void Directions_SumOfStepDistancesCorrect()
        {
            var request = new DirectionsRequest
            {
                Origin = "285 Bedford Ave, Brooklyn, NY, USA",
                Destination = "185 Broadway Ave, Manhattan, NY, USA",
                ApiKey = ApiKey
            };

            var result = GoogleMaps.Directions.Query(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(DirectionsStatusCodes.OK, result.Status, result.ErrorMessage);
            Assert.Greater(result.Routes.First().Legs.First().Steps.Sum(s => s.Distance.Value), 100);
        }

        //The sub_steps differes between google docs documentation and implementation. We use it as google implemented, so we have test to make sure it's not broken.
        [Test]
        public async Task Directions_VerifySubStepsAsync()
        {
            var request = new DirectionsRequest
            {
                Origin = "75 9th Ave, New York, NY",
                Destination = "MetLife Stadium Dr East Rutherford, NJ 07073",
                TravelMode = TravelMode.Driving,
                ApiKey = ApiKey
            };

            DirectionsResponse result = await GoogleMaps.Directions.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);

            var route = result.Routes.First();
            var leg = route.Legs.First();
            var step = leg.Steps.First();

            Assert.NotNull(step);
        }

        [Test]
        [Obsolete]
        public void Directions_VerifySubSteps()
        {
            var request = new DirectionsRequest
            {
                Origin = "75 9th Ave, New York, NY",
                Destination = "MetLife Stadium Dr East Rutherford, NJ 07073",
                TravelMode = TravelMode.Driving,
                ApiKey = ApiKey
            };

            DirectionsResponse result = GoogleMaps.Directions.Query(request);

            AssertInconclusive.NotExceedQuota(result);

            var route = result.Routes.First();
            var leg = route.Legs.First();
            var step = leg.Steps.First();

            Assert.NotNull(step);
        }

        [Test]
        public async Task Directions_VerifyBoundsAsync()
        {
            var request = new DirectionsRequest
            {
                Origin = "Genk, Belgium",
                Destination = "Brussels, Belgium",
                TravelMode = TravelMode.Driving,
                ApiKey = ApiKey
            };

            DirectionsResponse result = await GoogleMaps.Directions.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);

            var route = result.Routes.First();

            Assert.NotNull(route);
            Assert.NotNull(route.Bounds);
            Assert.Greater(route.Bounds.NorthEast.Latitude, 50);
            Assert.Greater(route.Bounds.NorthEast.Longitude, 3);
            Assert.Greater(route.Bounds.SouthWest.Latitude, 50);
            Assert.Greater(route.Bounds.SouthWest.Longitude, 3);
            Assert.Greater(route.Bounds.Center.Latitude, 50);
            Assert.Greater(route.Bounds.Center.Longitude, 3);
        }

        [Test]
        [Obsolete]
        public void Directions_VerifyBounds()
        {
            var request = new DirectionsRequest
            {
                Origin = "Genk, Belgium",
                Destination = "Brussels, Belgium",
                TravelMode = TravelMode.Driving,
                ApiKey = ApiKey
            };

            DirectionsResponse result = GoogleMaps.Directions.Query(request);

            AssertInconclusive.NotExceedQuota(result);

            var route = result.Routes.First();

            Assert.NotNull(route);
            Assert.NotNull(route.Bounds);
            Assert.Greater(route.Bounds.NorthEast.Latitude, 50);
            Assert.Greater(route.Bounds.NorthEast.Longitude, 3);
            Assert.Greater(route.Bounds.SouthWest.Latitude, 50);
            Assert.Greater(route.Bounds.SouthWest.Longitude, 3);
            Assert.Greater(route.Bounds.Center.Latitude, 50);
            Assert.Greater(route.Bounds.Center.Longitude, 3);
        }

        [Test]
        public async Task Directions_WithIconsAsync()
        {
            var depTime = DateTime.Today
                .AddDays(1)
                .AddHours(13);

            var request = new DirectionsRequest
            {
                Origin = "T-centralen, Stockholm, Sverige",
                Destination = "Kungsträdgården, Stockholm, Sverige",
                TravelMode = TravelMode.Transit,
                DepartureTime = depTime,
                Language = "sv",
                ApiKey = ApiKey
            };

            DirectionsResponse result = await GoogleMaps.Directions.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);

            var route = result.Routes.First();
            var leg = route.Legs.First();
            var steps = leg.Steps;

            Assert.IsNotEmpty(steps.Where(s =>
                s.TransitDetails?
                    .Lines?
                    .Vehicle?
                    .Icon != null));
        }

        [Test]
        [Obsolete]
        public void Directions_WithIcons()
        {
            var depTime = DateTime.Today
                .AddDays(1)
                .AddHours(13);

            var request = new DirectionsRequest
            {
                Origin = "T-centralen, Stockholm, Sverige",
                Destination = "Kungsträdgården, Stockholm, Sverige",
                TravelMode = TravelMode.Transit,
                DepartureTime = depTime,
                Language = "sv",
                ApiKey = ApiKey
            };

            DirectionsResponse result = GoogleMaps.Directions.Query(request);

            AssertInconclusive.NotExceedQuota(result);

            var route = result.Routes.First();
            var leg = route.Legs.First();
            var steps = leg.Steps;

            Assert.IsNotEmpty(steps.Where(s =>
                s.TransitDetails?
                    .Lines?
                    .Vehicle?
                    .Icon != null));
        }

        [Test]
        public async Task Directions_WithRegionSearchAsync()
        {
            var depTime = DateTime.Today
                .AddDays(1)
                .AddHours(13);

            var request = new DirectionsRequest
            {
                Origin = "Mt Albert",
                Destination = "Parnell",
                TravelMode = TravelMode.Transit,
                DepartureTime = depTime,
                Region = "nz",
                ApiKey = ApiKey
            };

            DirectionsResponse result = await GoogleMaps.Directions.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.IsNotEmpty(result.Routes);
            Assert.True(result.Status.Equals(DirectionsStatusCodes.OK));
        }

        [Test]
        [Obsolete]
        public void Directions_WithRegionSearch()
        {
            var depTime = DateTime.Today
                .AddDays(1)
                .AddHours(13);

            var request = new DirectionsRequest
            {
                Origin = "Mt Albert",
                Destination = "Parnell",
                TravelMode = TravelMode.Transit,
                DepartureTime = depTime,
                Region = "nz",
                ApiKey = ApiKey
            };

            DirectionsResponse result = GoogleMaps.Directions.Query(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.IsNotEmpty(result.Routes);
            Assert.True(result.Status.Equals(DirectionsStatusCodes.OK));
        }

        [Test]
        public async Task Directions_CanGetDurationWithTrafficAsync()
        {
            var request = new DirectionsRequest
            {
                Origin = "285 Bedford Ave, Brooklyn, NY, USA",
                Destination = "185 Broadway Ave, Manhattan, NY, USA",
                DepartureTime = DateTime.Now.Date.AddDays(1).AddHours(8),
                ApiKey = ApiKey //Duration in traffic requires an API key
            };
            var result = await GoogleMaps.Directions.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);

            //All legs have duration
            Assert.IsTrue(result.Routes.First().Legs.All(l => l.DurationInTraffic != null));

            //Duration with traffic is usually longer but is not guaranteed
            Assert.AreNotEqual(result.Routes.First().Legs.Sum(s => s.Duration.Value.TotalSeconds),
                result.Routes.First().Legs.Sum(s => s.DurationInTraffic.Value.TotalSeconds));
        }

        [Test]
        [Obsolete]
        public void Directions_CanGetDurationWithTraffic()
        {
            var request = new DirectionsRequest
            {
                Origin = "285 Bedford Ave, Brooklyn, NY, USA",
                Destination = "185 Broadway Ave, Manhattan, NY, USA",
                DepartureTime = DateTime.Now.Date.AddDays(1).AddHours(8),
                ApiKey = ApiKey //Duration in traffic requires an API key
            };
            var result = GoogleMaps.Directions.Query(request);

            AssertInconclusive.NotExceedQuota(result);

            //All legs have duration
            Assert.IsTrue(result.Routes.First().Legs.All(l => l.DurationInTraffic != null));

            //Duration with traffic is usually longer but is not guaranteed
            Assert.AreNotEqual(result.Routes.First().Legs.Sum(s => s.Duration.Value.TotalSeconds),
                result.Routes.First().Legs.Sum(s => s.DurationInTraffic.Value.TotalSeconds));
        }

        [Test]
        public void Directions_CanGetLongDistanceTrainAsync()
        {
            var request = new DirectionsRequest
            {
                Origin = "zurich airport",
                Destination = "brig",
                TravelMode = TravelMode.Transit,
                DepartureTime = new DateTime(2018, 08, 18, 15, 30, 00)
            };

            Assert.DoesNotThrowAsync(async () => await GoogleMaps.Directions.QueryAsync(request));
        }

        [Test]
        [Obsolete]
        public void Directions_CanGetLongDistanceTrain()
        {
            var request = new DirectionsRequest
            {
                Origin = "zurich airport",
                Destination = "brig",
                TravelMode = TravelMode.Transit,
                DepartureTime = new DateTime(2018, 08, 18, 15, 30, 00)
            };

            Assert.DoesNotThrow(() => GoogleMaps.Directions.Query(request));
        }
    }
}