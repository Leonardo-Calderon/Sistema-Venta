using SVServices.Interfaces;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text;

namespace SVServices.Implementation
{
    public class SeguridadSQLService : ISeguridadSQLService
    {
        private readonly ILogger<SeguridadSQLService> _logger;

        public SeguridadSQLService(ILogger<SeguridadSQLService> logger)
        {
            _logger = logger;
        }

        public async Task<string> AnalizarConsultaParametrizada(string nombreProcedimiento, Dictionary<string, object> parametros)
        {
            try
            {
                var analisis = new StringBuilder();
                analisis.AppendLine($"## 🔍 ANÁLISIS DE SEGURIDAD: {nombreProcedimiento}");
                analisis.AppendLine();

                // 1. Explicar qué es una consulta parametrizada
                analisis.AppendLine("### 📋 ¿Qué es una Consulta Parametrizada?");
                analisis.AppendLine("Una consulta parametrizada es una consulta SQL donde los valores de entrada del usuario");
                analisis.AppendLine("se tratan como **parámetros separados** del comando SQL, no como parte del texto SQL.");
                analisis.AppendLine();

                // 2. Mostrar cómo se ejecuta en el proyecto
                analisis.AppendLine("### 🔧 Implementación en SistemaVenta:");
                analisis.AppendLine("```csharp");
                analisis.AppendLine($"var cmd = new SqlCommand(\"{nombreProcedimiento}\", con);");
                analisis.AppendLine("cmd.CommandType = CommandType.StoredProcedure;");
                
                foreach (var param in parametros)
                {
                    analisis.AppendLine($"cmd.Parameters.Add(new SqlParameter(\"@{param.Key}\", {param.Value}));");
                }
                analisis.AppendLine("await cmd.ExecuteNonQueryAsync();");
                analisis.AppendLine("```");
                analisis.AppendLine();

                // 3. Explicar por qué es seguro
                analisis.AppendLine("### 🛡️ ¿Por qué es Seguro?");
                analisis.AppendLine("1. **Separación de Comando y Datos**: El comando SQL y los datos del usuario están separados");
                analisis.AppendLine("2. **Parámetros Tipados**: Cada parámetro tiene un tipo de dato específico");
                analisis.AppendLine("3. **Escape Automático**: SQL Server escapa automáticamente los caracteres especiales");
                analisis.AppendLine("4. **Prevención de Inyección**: Los caracteres peligrosos se tratan como datos, no como código");
                analisis.AppendLine();

                // 4. Mostrar el flujo de ejecución
                analisis.AppendLine("### 🔄 Flujo de Ejecución Segura:");
                analisis.AppendLine("1. **Validación**: Los datos se validan en el controlador");
                analisis.AppendLine("2. **Sanitización**: Se remueven caracteres peligrosos");
                analisis.AppendLine("3. **Parametrización**: Los datos se pasan como parámetros");
                analisis.AppendLine("4. **Ejecución**: SQL Server ejecuta el SP con parámetros seguros");
                analisis.AppendLine("5. **Resultado**: Los datos se insertan sin riesgo de inyección");
                analisis.AppendLine();

                // 5. Comparar con consulta vulnerable
                analisis.AppendLine("### ⚠️ Comparación con Consulta Vulnerable:");
                analisis.AppendLine("**❌ CONSULTA VULNERABLE (NO USAR):**");
                analisis.AppendLine("```sql");
                analisis.AppendLine($"INSERT INTO Usuario (NombreCompleto, Correo) VALUES ('{parametros.GetValueOrDefault("NombreCompleto", "test")}', '{parametros.GetValueOrDefault("Correo", "test@test.com")}')");
                analisis.AppendLine("```");
                analisis.AppendLine();
                analisis.AppendLine("**✅ CONSULTA SEGURA (NUESTRA IMPLEMENTACIÓN):**");
                analisis.AppendLine("```sql");
                analisis.AppendLine("EXEC sp_crearUsuario @NombreCompleto, @Correo, @NombreUsuario, @Clave, @IdRol");
                analisis.AppendLine("```");
                analisis.AppendLine();

                // 6. Explicar el stored procedure
                analisis.AppendLine("### 📊 Análisis del Stored Procedure:");
                analisis.AppendLine("```sql");
                analisis.AppendLine("CREATE PROCEDURE sp_crearUsuario(");
                analisis.AppendLine("    @IdRol int,");
                analisis.AppendLine("    @NombreCompleto varchar(50),");
                analisis.AppendLine("    @Correo varchar(50),");
                analisis.AppendLine("    @NombreUsuario varchar(50),");
                analisis.AppendLine("    @Clave varchar(100)");
                analisis.AppendLine(")");
                analisis.AppendLine("AS");
                analisis.AppendLine("BEGIN");
                analisis.AppendLine("    INSERT INTO Usuario(IdRol, NombreCompleto, Correo, NombreUsuario, Clave)");
                analisis.AppendLine("    VALUES(@IdRol, @NombreCompleto, @Correo, @NombreUsuario, @Clave)");
                analisis.AppendLine("END");
                analisis.AppendLine("```");
                analisis.AppendLine();

                // 7. Beneficios de seguridad
                analisis.AppendLine("### 🎯 Beneficios de Seguridad:");
                analisis.AppendLine("- ✅ **Prevención de SQL Injection**: Los datos se tratan como valores, no como código");
                analisis.AppendLine("- ✅ **Validación de Tipos**: SQL Server valida automáticamente los tipos de datos");
                analisis.AppendLine("- ✅ **Escape Automático**: Caracteres especiales se escapan automáticamente");
                analisis.AppendLine("- ✅ **Separación de Responsabilidades**: Lógica de negocio separada de la consulta");
                analisis.AppendLine("- ✅ **Reutilización**: El SP puede ser reutilizado con diferentes parámetros");
                analisis.AppendLine("- ✅ **Mantenibilidad**: Cambios en la lógica sin modificar el código C#");

                _logger.LogInformation("Análisis de seguridad completado para: {Procedimiento}", nombreProcedimiento);
                return analisis.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al analizar consulta parametrizada: {Procedimiento}", nombreProcedimiento);
                return "Error al generar análisis de seguridad.";
            }
        }

        public string SimularConsultaVulnerable(string consultaVulnerable)
        {
            try
            {
                var simulacion = new StringBuilder();
                simulacion.AppendLine("## ⚠️ SIMULACIÓN DE CONSULTA VULNERABLE");
                simulacion.AppendLine();

                simulacion.AppendLine("### 🔴 Consulta Vulnerable:");
                simulacion.AppendLine("```sql");
                simulacion.AppendLine(consultaVulnerable);
                simulacion.AppendLine("```");
                simulacion.AppendLine();

                simulacion.AppendLine("### 🎯 Ataque de SQL Injection:");
                simulacion.AppendLine("Si un atacante ingresa: `'; DROP TABLE Usuarios; --`");
                simulacion.AppendLine("La consulta se convertiría en:");
                simulacion.AppendLine("```sql");
                simulacion.AppendLine(consultaVulnerable.Replace("'test'", "''; DROP TABLE Usuarios; --"));
                simulacion.AppendLine("```");
                simulacion.AppendLine();

                simulacion.AppendLine("### 💥 Consecuencias:");
                simulacion.AppendLine("- ❌ **Eliminación de datos**: Se podría eliminar la tabla Usuarios");
                simulacion.AppendLine("- ❌ **Acceso no autorizado**: Se podría acceder a datos sensibles");
                simulacion.AppendLine("- ❌ **Modificación de datos**: Se podrían modificar registros");
                simulacion.AppendLine("- ❌ **Ejecución de comandos**: Se podrían ejecutar comandos SQL maliciosos");

                _logger.LogWarning("Simulación de consulta vulnerable generada: {Consulta}", consultaVulnerable);
                return simulacion.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al simular consulta vulnerable");
                return "Error al generar simulación de consulta vulnerable.";
            }
        }

        public async Task<object> GenerarReporteSeguridad(string nombreProcedimiento, Dictionary<string, object> parametros)
        {
            try
            {
                var reporte = new
                {
                    Procedimiento = nombreProcedimiento,
                    FechaAnalisis = DateTime.Now,
                    Parametros = parametros,
                    NivelSeguridad = "ALTO",
                    MetodoSeguridad = "Stored Procedure con Parámetros",
                    Vulnerabilidades = new string[0],
                    Recomendaciones = new[]
                    {
                        "✅ Continuar usando stored procedures",
                        "✅ Mantener validación en el controlador",
                        "✅ Mantener sanitización de datos",
                        "✅ Usar siempre parámetros tipados"
                    },
                    FlujoSeguridad = new[]
                    {
                        "Validación de DTO",
                        "Sanitización de datos",
                        "Creación de parámetros tipados",
                        "Ejecución de stored procedure",
                        "Manejo de respuesta"
                    }
                };

                _logger.LogInformation("Reporte de seguridad generado para: {Procedimiento}", nombreProcedimiento);
                return reporte;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar reporte de seguridad: {Procedimiento}", nombreProcedimiento);
                return new { Error = "Error al generar reporte de seguridad." };
            }
        }

        public bool VerificarUsoParametros(IDbCommand comando)
        {
            try
            {
                // Verificar que el comando use parámetros
                if (comando.Parameters.Count == 0)
                {
                    _logger.LogWarning("Comando SQL sin parámetros detectado");
                    return false;
                }

                // Verificar que no haya concatenación de strings en el comando
                var commandText = comando.CommandText.ToLowerInvariant();
                if (commandText.Contains("'") || commandText.Contains("\"") || commandText.Contains("+"))
                {
                    _logger.LogWarning("Posible concatenación de strings en comando SQL: {Comando}", comando.CommandText);
                    return false;
                }

                _logger.LogInformation("Comando SQL verificado como seguro: {Comando}", comando.CommandText);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar uso de parámetros");
                return false;
            }
        }

        public string DocumentarFlujoSeguridad(string operacion, string entidad)
        {
            try
            {
                var documentacion = new StringBuilder();
                documentacion.AppendLine($"## 🔄 FLUJO DE SEGURIDAD: {operacion} {entidad}");
                documentacion.AppendLine();

                documentacion.AppendLine("### 📋 Pasos del Flujo Seguro:");
                documentacion.AppendLine();

                switch (operacion.ToUpper())
                {
                    case "INSERT":
                        documentacion.AppendLine("1. **Recepción de DTO**: Los datos llegan desde el cliente");
                        documentacion.AppendLine("2. **Validación de DTO**: Se validan usando Data Annotations");
                        documentacion.AppendLine("3. **Sanitización**: Se remueven caracteres peligrosos");
                        documentacion.AppendLine("4. **Creación de Entidad**: Se crea la entidad con datos sanitizados");
                        documentacion.AppendLine("5. **Llamada al Servicio**: Se llama al método del servicio");
                        documentacion.AppendLine("6. **Llamada al Repositorio**: Se llama al método del repositorio");
                        documentacion.AppendLine("7. **Creación de Comando**: Se crea SqlCommand con stored procedure");
                        documentacion.AppendLine("8. **Asignación de Parámetros**: Se asignan parámetros tipados");
                        documentacion.AppendLine("9. **Ejecución Segura**: Se ejecuta el stored procedure");
                        documentacion.AppendLine("10. **Manejo de Respuesta**: Se procesa la respuesta del SP");
                        break;

                    case "UPDATE":
                        documentacion.AppendLine("1. **Recepción de DTO**: Los datos llegan desde el cliente");
                        documentacion.AppendLine("2. **Validación de DTO**: Se validan usando Data Annotations");
                        documentacion.AppendLine("3. **Verificación de Propiedad**: Se verifica si el usuario puede modificar");
                        documentacion.AppendLine("4. **Sanitización**: Se remueven caracteres peligrosos");
                        documentacion.AppendLine("5. **Actualización de Entidad**: Se actualiza la entidad");
                        documentacion.AppendLine("6. **Llamada al Servicio**: Se llama al método del servicio");
                        documentacion.AppendLine("7. **Llamada al Repositorio**: Se llama al método del repositorio");
                        documentacion.AppendLine("8. **Creación de Comando**: Se crea SqlCommand con stored procedure");
                        documentacion.AppendLine("9. **Asignación de Parámetros**: Se asignan parámetros tipados");
                        documentacion.AppendLine("10. **Ejecución Segura**: Se ejecuta el stored procedure");
                        break;

                    case "SELECT":
                        documentacion.AppendLine("1. **Recepción de Parámetros**: Los filtros llegan desde el cliente");
                        documentacion.AppendLine("2. **Validación de Parámetros**: Se validan los filtros");
                        documentacion.AppendLine("3. **Sanitización**: Se remueven caracteres peligrosos");
                        documentacion.AppendLine("4. **Llamada al Servicio**: Se llama al método del servicio");
                        documentacion.AppendLine("5. **Llamada al Repositorio**: Se llama al método del repositorio");
                        documentacion.AppendLine("6. **Creación de Comando**: Se crea SqlCommand con stored procedure");
                        documentacion.AppendLine("7. **Asignación de Parámetros**: Se asignan parámetros tipados");
                        documentacion.AppendLine("8. **Ejecución Segura**: Se ejecuta el stored procedure");
                        documentacion.AppendLine("9. **Lectura de Datos**: Se leen los datos del DataReader");
                        documentacion.AppendLine("10. **Mapeo Seguro**: Se mapean a entidades de forma segura");
                        break;

                    case "DELETE":
                        documentacion.AppendLine("1. **Recepción de ID**: El ID llega desde el cliente");
                        documentacion.AppendLine("2. **Validación de ID**: Se valida que sea un número válido");
                        documentacion.AppendLine("3. **Verificación de Permisos**: Se verifica si el usuario puede eliminar");
                        documentacion.AppendLine("4. **Llamada al Servicio**: Se llama al método del servicio");
                        documentacion.AppendLine("5. **Llamada al Repositorio**: Se llama al método del repositorio");
                        documentacion.AppendLine("6. **Creación de Comando**: Se crea SqlCommand con stored procedure");
                        documentacion.AppendLine("7. **Asignación de Parámetros**: Se asignan parámetros tipados");
                        documentacion.AppendLine("8. **Ejecución Segura**: Se ejecuta el stored procedure");
                        documentacion.AppendLine("9. **Verificación de Resultado**: Se verifica si se eliminó correctamente");
                        break;
                }

                documentacion.AppendLine();
                documentacion.AppendLine("### 🛡️ Puntos de Seguridad:");
                documentacion.AppendLine("- ✅ **Validación en Múltiples Niveles**: DTO, controlador, servicio");
                documentacion.AppendLine("- ✅ **Sanitización de Datos**: Remoción de caracteres peligrosos");
                documentacion.AppendLine("- ✅ **Parámetros Tipados**: Uso de SqlParameter con tipos específicos");
                documentacion.AppendLine("- ✅ **Stored Procedures**: Lógica encapsulada en la base de datos");
                documentacion.AppendLine("- ✅ **Manejo de Errores**: Try-catch en todos los niveles");
                documentacion.AppendLine("- ✅ **Logging de Seguridad**: Registro de eventos importantes");

                _logger.LogInformation("Documentación de flujo de seguridad generada: {Operacion} {Entidad}", operacion, entidad);
                return documentacion.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al documentar flujo de seguridad: {Operacion} {Entidad}", operacion, entidad);
                return "Error al generar documentación de flujo de seguridad.";
            }
        }

        /// <summary>
        /// Verifica si una entrada contiene caracteres peligrosos para SQL Injection
        /// </summary>
        /// <param name="entrada">Entrada a verificar</param>
        /// <returns>True si contiene caracteres peligrosos</returns>
        public bool ContieneCaracteresPeligrosos(string entrada)
        {
            if (string.IsNullOrWhiteSpace(entrada))
            {
                return false;
            }

            var entradaLower = entrada.ToLowerInvariant();
            
            // Caracteres peligrosos para SQL Injection
            var caracteresPeligrosos = new[]
            {
                "'", "\"", ";", "--", "/*", "*/", "xp_", "sp_", "exec", "execute", 
                "select", "insert", "update", "delete", "drop", "create", "alter",
                "union", "script", "<script", "javascript:", "onload", "onerror"
            };

            return caracteresPeligrosos.Any(peligroso => entradaLower.Contains(peligroso.ToLowerInvariant()));
        }

        /// <summary>
        /// Sanitiza una entrada contra SQL Injection
        /// </summary>
        /// <param name="entrada">Entrada a sanitizar</param>
        /// <returns>Entrada sanitizada</returns>
        public string SanitizarContraSQLInjection(string entrada)
        {
            if (string.IsNullOrWhiteSpace(entrada))
            {
                return string.Empty;
            }

            var sanitizado = entrada;

            // Caracteres peligrosos para SQL Injection
            var caracteresPeligrosos = new[]
            {
                "'", "\"", ";", "--", "/*", "*/", "xp_", "sp_", "exec", "execute", 
                "select", "insert", "update", "delete", "drop", "create", "alter",
                "union", "script", "<script", "javascript:", "onload", "onerror"
            };

            // Remover caracteres peligrosos
            foreach (var peligroso in caracteresPeligrosos)
            {
                sanitizado = sanitizado.Replace(peligroso, "", StringComparison.OrdinalIgnoreCase);
            }

            return sanitizado;
        }
    }
} 