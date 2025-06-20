using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using SVServices.Interfaces;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MenusController : ControllerBase
{
    private readonly IMenuRolService _menuRolService;

    public MenusController(IMenuRolService menuRolService)
    {
        _menuRolService = menuRolService;
    }

    [HttpGet] // El NavMenu llamará a GET /api/menus
    public async Task<IActionResult> ObtenerMenusParaUsuario()
    {
        try
        {
            var claims = HttpContext.User.Claims;
            var idRolClaim = claims.FirstOrDefault(c => c.Type == "IdRol");

            if (idRolClaim == null || !int.TryParse(idRolClaim.Value, out int idRol))
            {
                return Unauthorized("Token inválido o no contiene el IdRol.");
            }

            var listaPlanaDesdeDb = await _menuRolService.Lista(idRol);

            if (listaPlanaDesdeDb == null || !listaPlanaDesdeDb.Any())
            {
                return Ok(new List<MenuDTO>());
            }

            var listaCompletaDto = listaPlanaDesdeDb.Select(MapToMenuDTO).ToList();

            // --- LÓGICA DE JERARQUÍA CORREGIDA ---
            var menusPadre = listaCompletaDto.Where(m => m.IdMenuPadre == 0).ToList();

            foreach (var padre in menusPadre)
            {
                padre.SubMenus = listaCompletaDto.Where(hijo => hijo.IdMenuPadre == padre.IdMenu).ToList();
            }
            // ------------------------------------

            return Ok(menusPadre);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    private MenuDTO MapToMenuDTO(SVRepository.Entities.MenuRol menuDb)
    {
        var menuDto = new MenuDTO
        {
            IdMenu = menuDb.IdMenu,
            Nombre = menuDb.NombreMenu,
            IdMenuPadre = menuDb.IdMenuPadre
        };

        // Lógica para asignar URL e Icono basado en el nombre (en minúsculas para evitar errores)
        switch (menuDb.NombreMenu.ToLower())
        {
            // Grupo Ventas
            case "ventas":
                menuDto.Url = null; // Un padre no necesita URL si solo es un agrupador
                menuDto.Icono = "oi oi-cart";
                break;
            case "nuevo":
                menuDto.Url = "/ventas/nueva";
                menuDto.Icono = "oi oi-plus";
                break;
            case "historial":
                menuDto.Url = "/ventas/historial";
                menuDto.Icono = "oi oi-clock";
                break;

            // Grupo Inventario
            case "inventario":
                menuDto.Url = null;
                menuDto.Icono = "oi oi-box";
                break;
            case "productos":
                menuDto.Url = "/productos";
                menuDto.Icono = "oi oi-list";
                break;
            case "categorias":
                menuDto.Url = "/categorias";
                menuDto.Icono = "oi oi-tags";
                break;

            // Grupo Reportes
            case "reportes":
                menuDto.Url = null;
                menuDto.Icono = "oi oi-document";
                break;
            // El hijo de reportes se llama "ventas", hay que diferenciarlo del padre
            // Nota: Sería ideal tener nombres únicos, pero podemos manejarlo.
            // Si el IdMenuPadre es el del menú "Reportes", es el reporte de ventas.
            // Para simplificar, asumiremos que no hay colisión por ahora.
            // case "ventas" when menuDb.IdMenuPadre == ID_REPORTE: 
            //     menuDto.Url = "/reporte/ventas";
            //     break;

            // Menús sin hijos
            case "usuarios":
                menuDto.Url = "/usuarios";
                menuDto.Icono = "oi oi-people";
                break;
            case "configuracion":
                menuDto.Url = "/configuracion";
                menuDto.Icono = "oi oi-cog";
                break;

            default:
                menuDto.Url = "#";
                menuDto.Icono = "oi oi-question-mark";
                break;
        }
        return menuDto;
    }
}