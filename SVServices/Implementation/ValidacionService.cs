using SVServices.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace SVServices.Implementation
{
    /// <summary>
    /// Implementación del servicio para la validación y sanitización de datos de entrada.
    /// </summary>
    /// <remarks>
    /// Esta clase implementa la interfaz IValidacionService y proporciona métodos para
    /// validar y sanitizar diferentes tipos de datos de entrada, incluyendo protección
    /// contra SQL Injection, validación de formatos y normalización de datos.
    /// </remarks>
    public class ValidacionService : IValidacionService
    {
        /// <summary>
        /// Instancia del logger para registrar eventos de validación.
        /// </summary>
        private readonly ILogger<ValidacionService> _logger;

        /// <summary>
        /// Lista de caracteres y patrones peligrosos para SQL Injection.
        /// </summary>
        private static readonly string[] CaracteresPeligrosos = {
            "'", "\"", ";", "--", "/*", "*/", "xp_", "sp_", "exec", "execute", 
            "select", "insert", "update", "delete", "drop", "create", "alter",
            "union", "script", "<script", "javascript:", "onload", "onerror"
        };

        /// <summary>
        /// Inicializa una nueva instancia de la clase ValidacionService.
        /// </summary>
        /// <param name="logger">Instancia del logger para registrar eventos de validación.</param>
        /// <remarks>
        /// El constructor recibe una instancia del logger que será utilizada para
        /// registrar eventos de validación y sanitización.
        /// </remarks>
        public ValidacionService(ILogger<ValidacionService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Valida y sanitiza un string para prevenir SQL Injection.
        /// </summary>
        /// <param name="input">String de entrada a sanitizar.</param>
        /// <param name="maxLength">Longitud máxima permitida para el string.</param>
        /// <param name="allowNull">Indica si se permite retornar null cuando el input está vacío.</param>
        /// <returns>String sanitizado o null si no es válido.</returns>
        /// <remarks>
        /// Este método realiza múltiples validaciones y sanitizaciones:
        /// - Verifica la longitud máxima
        /// - Sanitiza contra SQL Injection
        /// - Remueve caracteres de control
        /// - Normaliza espacios en blanco
        /// </remarks>
        public string? SanitizarString(string? input, int maxLength = 100, bool allowNull = false)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(input))
                {
                    return allowNull ? null : string.Empty;
                }

                // Verificar longitud
                if (input.Length > maxLength)
                {
                    _logger.LogWarning($"String excede longitud máxima: {input.Length} > {maxLength}");
                    return null;
                }

                // Sanitizar contra SQL Injection
                var sanitizado = SanitizarContraSQLInjection(input);

                // Remover caracteres de control
                sanitizado = Regex.Replace(sanitizado, @"[\x00-\x1F\x7F]", "");

                // Trim y normalizar espacios
                sanitizado = Regex.Replace(sanitizado.Trim(), @"\s+", " ");

                return sanitizado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al sanitizar string: {Input}", input);
                return null;
            }
        }

        /// <summary>
        /// Valida y sanitiza un email.
        /// </summary>
        /// <param name="email">Email a validar y sanitizar.</param>
        /// <returns>Email sanitizado o null si no es válido.</returns>
        /// <remarks>
        /// Este método valida el formato de email usando una expresión regular
        /// y convierte el resultado a minúsculas para normalización.
        /// </remarks>
        public string? SanitizarEmail(string? email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return null;
                }

                // Sanitizar string básico
                var sanitizado = SanitizarString(email, 100, false);
                if (sanitizado == null)
                {
                    return null;
                }

                // Validar formato de email
                var emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
                if (!emailRegex.IsMatch(sanitizado))
                {
                    _logger.LogWarning($"Formato de email inválido: {email}");
                    return null;
                }

                return sanitizado.ToLowerInvariant();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al sanitizar email: {Email}", email);
                return null;
            }
        }

        /// <summary>
        /// Valida y sanitiza un número entero.
        /// </summary>
        /// <param name="input">Valor entero a validar.</param>
        /// <param name="minValue">Valor mínimo permitido.</param>
        /// <param name="maxValue">Valor máximo permitido.</param>
        /// <returns>Número validado o null si no está en el rango permitido.</returns>
        /// <remarks>
        /// Este método verifica que el valor esté dentro del rango especificado.
        /// </remarks>
        public int? SanitizarInt(int input, int minValue = 1, int maxValue = int.MaxValue)
        {
            try
            {
                if (input < minValue || input > maxValue)
                {
                    _logger.LogWarning($"Valor entero fuera de rango: {input} (rango: {minValue}-{maxValue})");
                    return null;
                }

                return input;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al sanitizar entero: {Input}", input);
                return null;
            }
        }

        /// <summary>
        /// Valida y sanitiza un número decimal.
        /// </summary>
        /// <param name="input">Valor decimal a validar.</param>
        /// <param name="minValue">Valor mínimo permitido.</param>
        /// <param name="maxValue">Valor máximo permitido.</param>
        /// <returns>Número validado y redondeado a 2 decimales, o null si no está en el rango permitido.</returns>
        /// <remarks>
        /// Este método verifica que el valor esté dentro del rango especificado
        /// y lo redondea a 2 decimales para evitar problemas de precisión.
        /// </remarks>
        public decimal? SanitizarDecimal(decimal input, decimal minValue = 0, decimal maxValue = decimal.MaxValue)
        {
            try
            {
                if (input < minValue || input > maxValue)
                {
                    _logger.LogWarning($"Valor decimal fuera de rango: {input} (rango: {minValue}-{maxValue})");
                    return null;
                }

                // Redondear a 2 decimales para evitar problemas de precisión
                return Math.Round(input, 2);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al sanitizar decimal: {Input}", input);
                return null;
            }
        }

        /// <summary>
        /// Valida y sanitiza una URL.
        /// </summary>
        /// <param name="url">URL a validar y sanitizar.</param>
        /// <returns>URL sanitizada o null si no es válida.</returns>
        /// <remarks>
        /// Este método valida el formato de URL y verifica que use esquemas
        /// HTTP o HTTPS permitidos.
        /// </remarks>
        public string? SanitizarUrl(string? url)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(url))
                {
                    return null;
                }

                // Sanitizar string básico
                var sanitizado = SanitizarString(url, 500, false);
                if (sanitizado == null)
                {
                    return null;
                }

                // Validar formato de URL
                if (!Uri.TryCreate(sanitizado, UriKind.Absolute, out var uri))
                {
                    _logger.LogWarning($"Formato de URL inválido: {url}");
                    return null;
                }

                // Verificar esquema permitido
                if (uri.Scheme != "http" && uri.Scheme != "https")
                {
                    _logger.LogWarning($"Esquema de URL no permitido: {uri.Scheme}");
                    return null;
                }

                return sanitizado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al sanitizar URL: {Url}", url);
                return null;
            }
        }

        /// <summary>
        /// Valida un objeto usando Data Annotations.
        /// </summary>
        /// <param name="obj">Objeto a validar.</param>
        /// <returns>Lista de errores de validación encontrados.</returns>
        /// <remarks>
        /// Este método utiliza el sistema de validación de Data Annotations
        /// para validar todas las propiedades del objeto que tengan atributos
        /// de validación definidos.
        /// </remarks>
        public List<ValidationResult> ValidarObjeto(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            var resultados = new List<ValidationResult>();
            var contexto = new ValidationContext(obj);
            
            try
            {
                Validator.TryValidateObject(obj, contexto, resultados, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al validar objeto: {Tipo}", obj.GetType().Name);
                resultados.Add(new ValidationResult("Error interno de validación"));
            }

            return resultados;
        }

        /// <summary>
        /// Verifica si un string contiene caracteres peligrosos para SQL Injection.
        /// </summary>
        /// <param name="input">String a verificar.</param>
        /// <returns>True si contiene caracteres peligrosos, false en caso contrario.</returns>
        /// <remarks>
        /// Este método verifica si el string contiene alguno de los caracteres
        /// o patrones definidos como peligrosos para SQL Injection.
        /// </remarks>
        public bool ContieneCaracteresPeligrosos(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            var inputLower = input.ToLowerInvariant();
            return CaracteresPeligrosos.Any(peligroso => inputLower.Contains(peligroso.ToLowerInvariant()));
        }

        /// <summary>
        /// Sanitiza un string removiendo caracteres peligrosos para SQL Injection.
        /// </summary>
        /// <param name="input">String a sanitizar.</param>
        /// <returns>String sanitizado sin caracteres peligrosos.</returns>
        /// <remarks>
        /// Este método remueve caracteres y patrones peligrosos para SQL Injection,
        /// incluyendo secuencias de escape SQL y comentarios.
        /// </remarks>
        public string SanitizarContraSQLInjection(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            try
            {
                var sanitizado = input;

                // Remover caracteres peligrosos
                foreach (var peligroso in CaracteresPeligrosos)
                {
                    sanitizado = sanitizado.Replace(peligroso, "", StringComparison.OrdinalIgnoreCase);
                }

                // Remover secuencias de escape SQL
                sanitizado = Regex.Replace(sanitizado, @"\\['""\\]", "", RegexOptions.IgnoreCase);

                // Remover comentarios SQL
                sanitizado = Regex.Replace(sanitizado, @"--.*$", "", RegexOptions.Multiline);
                sanitizado = Regex.Replace(sanitizado, @"/\*.*?\*/", "", RegexOptions.Singleline);

                // Log si se detectaron caracteres peligrosos
                if (sanitizado != input)
                {
                    _logger.LogWarning("Se detectaron y removieron caracteres peligrosos en: {Input}", input);
                }

                return sanitizado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al sanitizar contra SQL Injection: {Input}", input);
                return string.Empty;
            }
        }
    }
} 