// En: SistemaVenta.API/Controllers/MenusController.cs
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

    [HttpGet]
    public async Task<IActionResult> Lista()
    {
        try
        {
            var claims = HttpContext.User.Claims;
            var idRolClaim = claims.FirstOrDefault(c => c.Type == "IdRol");
            if (idRolClaim == null) return Unauthorized("Token inválido.");

            int idRol = int.Parse(idRolClaim.Value);
            var listaPlanaDesdeDb = await _menuRolService.Lista(idRol);

            if (listaPlanaDesdeDb == null) return Ok(new List<MenuDTO>());

            // Convertimos la lista de la BD a una lista de DTOs, añadiendo la info que falta
            var listaConDatosCompletos = listaPlanaDesdeDb.Select(m => MapToMenuDTO(m)).ToList();

            // Construimos la jerarquía
            var menuJerarquico = listaConDatosCompletos
                .Where(m => !listaConDatosCompletos.Any(p => p.IdMenu == m.IdMenuPadre)) // Busca padres
                .Select(m => {
                    m.SubMenus = listaConDatosCompletos.Where(h => h.IdMenuPadre == m.IdMenu).ToList();
                    return m;
                }).ToList();


            return Ok(menuJerarquico);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    // Método privado para "hardcodear" las URLs y los iconos
    private MenuDTO MapToMenuDTO(SVRepository.Entities.MenuRol menuDb)
    {
        var menuDto = new MenuDTO
        {
            IdMenu = menuDb.IdMenu,
            Nombre = menuDb.NombreMenu,
            IdMenuPadre = menuDb.IdMenuPadre
            // Url e Icono se asignan aquí abajo
        };

        // Lógica para asignar URL e Icono basado en el nombre
        switch (menuDb.NombreMenu.ToLower())
        {
            case "ventas":
                menuDto.Url = "/ventas";
                menuDto.Icono = "oi oi-cart"; // Ejemplo con Open Iconic
                break;
            case "nuevo":
                menuDto.Url = "/venta/nueva";
                menuDto.Icono = "oi oi-plus";
                break;
            case "historial":
                menuDto.Url = "/ventas/historial";
                menuDto.Icono = "oi oi-clock";
                break;
            case "inventario":
                menuDto.Url = "/inventario";
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
            // ...Añade un case para cada uno de tus menús
            default:
                menuDto.Url = "#"; // URL por defecto si no se encuentra
                menuDto.Icono = "oi oi-question-mark";
                break;
        }
        return menuDto;
    }
}