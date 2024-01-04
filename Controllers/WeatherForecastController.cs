using CqrsPatternDesign.Permisos;
using CqrsPatternDesign.PermisosCommand;
using CqrsPatternDesign.PermisosQuery;
using Microsoft.AspNetCore.Mvc;
using RepositoryPatternDesign;
using RepositoryPatternDesign.Models;
using UnitOfWorkPatternDesign;

namespace ApiRestN5.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPermisoCommandHandler _commandHandler;
        private readonly IPermisoQueryHandler _queryHandler;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IPermisoCommandHandler commandHandler, IPermisoQueryHandler queryHandler, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _commandHandler = commandHandler;
            _queryHandler = queryHandler;
            _unitOfWork = unitOfWork;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            var command = new PermisoCommand
            {
                NombreEmpleado = "Hernán",
                ApellidoEmpleado = "Suárez",
                FechaPermiso = DateTime.Now,
                TipoPermisoNavigation = new TipoPermiso { Descripcion = "Permiso enfermedad hijos" }
            };
            var resp = _commandHandler.RequestPermission(command);
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}