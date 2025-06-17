-- Versi�n M�nima Requerida de sp_obtenerMenus
ALTER PROC sp_obtenerMenus(
    @IdRol int
)
as
begin
    select
        m.IdMenu, -- << A�ADE ESTA L�NEA
        m.NombreMenu,
        m.IdMenuPadre,
        mr.Activo
    from MenuRol mr
    inner join Menu m on m.IdMenu = mr.IdMenu
    where mr.IdRol = @IdRol AND mr.Activo = 1
end