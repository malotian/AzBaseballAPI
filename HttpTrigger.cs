using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace AzBaseballAPI {
    public class HttpTrigger {
        private readonly ILogger<HttpTrigger> _logger;

        public HttpTrigger(ILogger<HttpTrigger> logger) {
            _logger = logger;
        }

        [Function("GetInstructors")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "GetInstructors" }, Summary = "GetInstructors", Description = "GetInstructors", Visibility = OpenApiVisibilityType.Important)]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req) {
            var instructors = new Helper29248().Get(); // Assuming GetInstructors() returns a list of instructors or similar
            return new OkObjectResult(instructors);
        }

        [Function("GetAlumniCommitments")]
        [OpenApiOperation(operationId: "GetAlumniCommitments", tags: new[] { "GetAlumniCommitments" }, Summary = "GetAlumniCommitments", Description = "GetAlumniCommitments", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "currentYear", In = ParameterLocation.Query, Required = true, Type = typeof(int))]
        public IActionResult GetAlumniCommitments([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req, [FromQuery] int currentYear) {
            FactoryAlumni factoryAlumni = new FactoryAlumni();
            factoryAlumni.SetCommitmentsData(currentYear);
            return new OkObjectResult(factoryAlumni.Commitments);
        }

        [Function("GetAlumniCollege")]
        [OpenApiOperation(operationId: "GetAlumniCollege", tags: new[] { "GetAlumniCollege" }, Summary = "GetAlumniCollege", Description = "GetAlumniCollege", Visibility = OpenApiVisibilityType.Important)]
        public IActionResult GetAlumniCollege([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req) {
            FactoryAlumni factoryAlumni = new FactoryAlumni();
            factoryAlumni.SetCollegeData();
            return new OkObjectResult(factoryAlumni.College);
        }

        [Function("GetBaseballIsBackEvents")]
        [OpenApiOperation(operationId: "GetBaseballIsBackEvents", tags: new[] { "GetBaseballIsBackEvents" }, Summary = "GetBaseballIsBackEvents", Description = "GetBaseballIsBackEvents", Visibility = OpenApiVisibilityType.Important)]
        public IActionResult GetBaseballIsBackEvents([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req) {
            FactoryAlumni factoryAlumni = new FactoryAlumni();
            factoryAlumni.SetCollegeData();
            return new OkObjectResult(factoryAlumni.College);
        }

        [Function("GetDraftedPlayers")]
        [OpenApiOperation(operationId: "GetDraftedPlayers", tags: new[] { "GetDraftedPlayers" }, Summary = "GetDraftedPlayers", Description = "GetDraftedPlayers", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "draftYear", In = ParameterLocation.Query, Required = true, Type = typeof(int))]
        public IActionResult GetDraftedPlayers([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req, [FromQuery] int draftYear) {
            FactoryAlumni factoryAlumni = new FactoryAlumni();
            factoryAlumni.SetDraftedPlayers(draftYear);
            return new OkObjectResult(factoryAlumni.Drafted);
        }

        [Function("GetEventGeoLocation")]
        [OpenApiOperation(operationId: "GetEventGeoLocation", tags: new[] { "GetEventGeoLocation" }, Summary = "GetEventGeoLocation", Description = "GetEventGeoLocation", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "zip", In = ParameterLocation.Query, Required = true, Type = typeof(string))]
        [OpenApiParameter(name: "lat", In = ParameterLocation.Query, Required = true, Type = typeof(string))]
        [OpenApiParameter(name: "lng", In = ParameterLocation.Query, Required = true, Type = typeof(string))]
        public IActionResult GetEventGeoLocation([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req, [FromQuery] string zip, string lat, string lng) {
            FactoryEvents factoryEvents = new FactoryEvents();
            var result = factoryEvents.GetEventZipsByLocation(zip, lat, lng);
            return new OkObjectResult(result);
        }

        [Function("GetEventRoster")]
        [OpenApiOperation(operationId: "GetEventRoster", tags: new[] { "GetEventRoster" }, Summary = "GetEventRoster", Description = "GetEventRoster", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "eventID", In = ParameterLocation.Query, Required = true, Type = typeof(string))]
        public IActionResult GetEventRoster([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req, [FromQuery] string eventID) {
            FactoryRosters factoryRosters = new FactoryRosters();
            factoryRosters.SetPreseasonRoster(eventID);
            return new OkObjectResult(factoryRosters.PreSeasonRoster);
        }

        [Function("GetEventSchedule")]
        [OpenApiOperation(operationId: "GetEventSchedule", tags: new[] { "GetEventSchedule" }, Summary = "GetEventSchedule", Description = "GetEventSchedule", Visibility = OpenApiVisibilityType.Important)]
        public IActionResult GetEventSchedule([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req) {
            HelperEventSchedule helperEventSchedule = new HelperEventSchedule();
            return new OkObjectResult(helperEventSchedule.GetEvents());
        }

        [Function("GetEvents")]
        [OpenApiOperation(operationId: "GetEvents", tags: new[] { "GetEvents" }, Summary = "GetEvents", Description = "GetEvents", Visibility = OpenApiVisibilityType.Important)]
        public IActionResult GetEvents([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req) {
            FactoryEvents factoryEvents = new FactoryEvents();
            factoryEvents.SetAllEvents();
            return new OkObjectResult(factoryEvents.EventsByMonth);
        }

        [Function("GetNationalShowcases")]
        [OpenApiOperation(operationId: "GetNationalShowcases", tags: new[] { "GetNationalShowcases" }, Summary = "GetNationalShowcases", Description = "GetNationalShowcases", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "ageGroup", In = ParameterLocation.Query, Required = true, Type = typeof(string))]
        [OpenApiParameter(name: "st", In = ParameterLocation.Query, Required = true, Type = typeof(string))]
        public IActionResult GetNationalShowcases([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req, [FromQuery] string ageGroup, [FromQuery] string st) {
            HelperNationalShowcases helperNationalShowcases = new HelperNationalShowcases();
            return new OkObjectResult(helperNationalShowcases.GetShowcases(ageGroup, st));
        }
    }
}
