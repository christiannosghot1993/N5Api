using CqrsPatternDesign.Permisos;
using CqrsPatternDesign.PermisosCommand;
using CqrsPatternDesign.PermisosQuery;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RepositoryPatternDesign.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiRestN5.Controllers
{
    /// <summary>
    /// Hola
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PermisosController : ControllerBase
    {
        private readonly IPermisoCommandHandler _commandHandler;
        private readonly IPermisoQueryHandler _queryHandler;
        public PermisosController(IPermisoCommandHandler commandHandler, IPermisoQueryHandler queryHandler)
        {
            _commandHandler= commandHandler;
            _queryHandler= queryHandler;
        }

        /// <summary>
        /// Obtiene un listado de todos los permisos con su respectivo tipo de permiso
        /// </summary>
        /// <returns>Lista de permisos</returns>
        [HttpGet]
        [Route("GetPermissions")]
        public List<Permiso> Get()
        {
            List<Permiso> permisos=_queryHandler.GetPermissions();
            return permisos;
        }

        /// <summary>
        /// Inserta un permiso en la base de datos
        /// </summary>
        /// <param name="permiso">
        /// 
        /// Se debe enviar siempre cero en el parámetro id 
        /// (En el caso de enviar un valor diferente de cero, el funcionamiento no se alterará).
        /// 
        /// Cuando es una inserción de permiso asociado a un tipo de permiso existente, 
        /// se debe enviar el tipoPermiso con el id del tipo de permiso a asociar (tipo de permiso existente) y
        /// el parámetro descripcionTipoPermiso debe ir vacío. 
        /// Si se llena el parámetro descripcionTipoPermiso, el sistema no alterará su funcionamiento.
        /// 
        /// Por otro lado, cuando la inserción del permiso no se enlaza con un tipo de permiso existente, el parámetro 
        /// tipoPermiso debe ir con cero (0), y descripcionTipoPermiso debe tener la descripción del nuevo tipo de permiso.
        /// </param>
        /// <returns>
        /// Status code 200 (Solicitud Correcta)
        /// Status code 400 (Error en la solicitud)
        /// </returns>
        [HttpPost]
        [Route("RequestPermission")]
        public IActionResult Post([FromBody] PermisoCommand permiso)
        {
            var res = _commandHandler.RequestPermission(permiso);
            if (res.Equals("OK"))
            {
                return Ok("Permiso registrado correctamente");
            }
            else
            {
                return BadRequest("Error al procesar la solicitud: "+res);
            }
        }


        /// <summary>
        /// Actualiza la información de un permiso en la base de datos
        /// </summary>
        /// <param name="permiso">
        /// El parámetro id siempre debe venir con un valor diferente de cero, haciendo referencia al
        /// id del permiso que se desea modificar.
        /// 
        /// Por otro lado, el parámetro tipoPermiso, también siempre debe venir con el id del tipo de 
        /// permiso a relacionar, por lo tanto, el parámetro descripcionTipoPermiso debe venir vacío 
        /// (Si se llena el parámetro descripcionTipoPermiso no afectará en el funcionamiento del API).
        /// </param>
        /// <returns>
        /// Status code 200 (Solicitud Correcta)
        /// Status code 400 (Error en la solicitud) 
        /// </returns>
        [HttpPut]
        [Route("ModifyPermission")]
        public IActionResult Put([FromBody] PermisoCommand permiso)
        {
            var res = _commandHandler.ModifyPermission(permiso);
            if (res.Equals("OK"))
            {
                return Ok("Permiso registrado correctamente");
            }
            else
            {
                return BadRequest("Error al procesar la solicitud: " + res);
            }
        }

        /// <summary>
        /// Obtiene listado de tipos de permisos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPermissionsType")]
        public List<TipoPermiso> GetTipoPermisos()
        {
            List<TipoPermiso> tipoPermisos = _queryHandler.GetPermissionsType();
            return tipoPermisos;
        }

        /// <summary>
        /// Obtiene un permiso en especifico por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPermissionById")]
        public Permiso GetPermisoById(int id)
        {
            return _queryHandler.getPermisoById(id);
        }

    }
}
