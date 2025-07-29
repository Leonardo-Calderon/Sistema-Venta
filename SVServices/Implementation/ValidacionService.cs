using SVServices.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace SVServices.Implementation
{
    public class ValidacionService : IValidacionService
    {
        private readonly ILogger<ValidacionService> _logger;

        // Caracteres peligrosos para SQL Injection
        private static readonly string[] CaracteresPeligrosos = {
            "'", "\"", ";", "--", "/*", "*/", "xp_", "sp_", "exec", "execute", 
            "select", "insert", "update", "delete", "drop", "create", "alter",
            "union", "script", "<script", "javascript:", "onload", "onerror"
        };

        public ValidacionService(ILogger<ValidacionService> logger)
        {
            _logger = logger;
        }

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

        public List<ValidationResult> ValidarObjeto(object obj)
        {
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

        public bool ContieneCaracteresPeligrosos(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            var inputLower = input.ToLowerInvariant();
            return CaracteresPeligrosos.Any(peligroso => inputLower.Contains(peligroso.ToLowerInvariant()));
        }

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