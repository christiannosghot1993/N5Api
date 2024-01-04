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

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            try
            {
                var _tipoPermiso = new TipoPermiso { Descripcion = "Permiso por calamidad médica 2" };
                IRepository<TipoPermiso> tipoPermiso = _unitOfWork.TipoPermiso;
                tipoPermiso.Add(_tipoPermiso);
                throw new Exception("Forzando error para probar rollback");

                IRepository<Permiso> permiso = _unitOfWork.Permiso;
                permiso.Add(new Permiso
                {
                    NombreEmpleado = "Christian",
                    ApellidoEmpleado = "Suárez",
                    FechaPermiso = DateTime.Now,
                    TipoPermisoNavigation = _tipoPermiso
                });
                _unitOfWork.Save();
            }
            catch (Exception)
            {

                _unitOfWork.RollBack();
            }
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