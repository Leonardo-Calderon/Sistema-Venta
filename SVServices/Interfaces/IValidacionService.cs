using System.ComponentModel.DataAnnotations;

namespace SVServices.Interfaces
{
    public interface IValidacionService
    {
        /// <summary>
        /// Valida y sanitiza un string para prevenir SQL Injection
        /// </summary>
        /// <param name="input">String de entrada</param>
        /// <param name="maxLength">Longitud máxima permitida</param>
        /// <param name="allowNull">Permitir valores null</param>
        /// <returns>String sanitizado o null si no es válido</returns>
        string? SanitizarString(string? input, int maxLength = 100, bool allowNull = false);

        /// <summary>
        /// Valida y sanitiza un email
        /// </summary>
        /// <param name="email">Email a validar</param>
        /// <returns>Email sanitizado o null si no es válido</returns>
        string? SanitizarEmail(string? email);

        /// <summary>
        /// Valida y sanitiza un número entero
        /// </summary>
        /// <param name="input">Valor de entrada</param>
        /// <param name="minValue">Valor mínimo</param>
        /// <param name="maxValue">Valor máximo</param>
        /// <returns>Número validado o null si no es válido</returns>
        int? SanitizarInt(int input, int minValue = 1, int maxValue = int.MaxValue);

        /// <summary>
        /// Valida y sanitiza un número decimal
        /// </summary>
        /// <param name="input">Valor de entrada</param>
        /// <param name="minValue">Valor mínimo</param>
        /// <param name="maxValue">Valor máximo</param>
        /// <returns>Número validado o null si no es válido</returns>
        decimal? SanitizarDecimal(decimal input, decimal minValue = 0, decimal maxValue = decimal.MaxValue);

        /// <summary>
        /// Valida y sanitiza una URL
        /// </summary>
        /// <param name="url">URL a validar</param>
        /// <returns>URL sanitizada o null si no es válida</returns>
        string? SanitizarUrl(string? url);

        /// <summary>
        /// Valida un objeto usando Data Annotations
        /// </summary>
        /// <param name="obj">Objeto a validar</param>
        /// <returns>Lista de errores de validación</returns>
        List<ValidationResult> ValidarObjeto(object obj);

        /// <summary>
        /// Verifica si un string contiene caracteres peligrosos para SQL
        /// </summary>
        /// <param name="input">String a verificar</param>
        /// <returns>True si contiene caracteres peligrosos</returns>
        bool ContieneCaracteresPeligrosos(string? input);

        /// <summary>
        /// Sanitiza un string removiendo caracteres peligrosos
        /// </summary>
        /// <param name="input">String a sanitizar</param>
        /// <returns>String sanitizado</returns>
        string SanitizarContraSQLInjection(string? input);
    }
} 