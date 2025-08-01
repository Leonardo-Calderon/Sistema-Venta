# Script de prueba para la API de login
# Ejecutar en PowerShell

Write-Host "=== Prueba de API SistemaVenta ===" -ForegroundColor Green

# URL de la API (ajustar según tu configuración)
$apiUrl = "https://localhost:7206"

Write-Host "1. Verificando si la API está ejecutándose..." -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "$apiUrl/swagger" -Method Get -TimeoutSec 5
    Write-Host "✓ API está ejecutándose" -ForegroundColor Green
} catch {
    Write-Host "✗ API no está ejecutándose. Asegúrate de ejecutar 'dotnet run' en SistemaVenta.API" -ForegroundColor Red
    exit 1
}

Write-Host "`n2. Probando endpoint de login..." -ForegroundColor Yellow

# Datos de prueba
$loginData = @{
    NombreUsuario = "admin"
    Clave = "admin"
} | ConvertTo-Json

try {
    $headers = @{
        "Content-Type" = "application/json"
    }
    
    $response = Invoke-RestMethod -Uri "$apiUrl/api/auth/login" -Method Post -Body $loginData -Headers $headers
    
    Write-Host "✓ Login exitoso!" -ForegroundColor Green
    Write-Host "Usuario: $($response.NombreUsuario)" -ForegroundColor Cyan
    Write-Host "Rol: $($response.Rol)" -ForegroundColor Cyan
    Write-Host "Token recibido: $($response.Token.Substring(0, 50))..." -ForegroundColor Cyan
    
} catch {
    Write-Host "✗ Error en login:" -ForegroundColor Red
    Write-Host "Status Code: $($_.Exception.Response.StatusCode)" -ForegroundColor Red
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
    
    if ($_.Exception.Response) {
        $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
        $responseBody = $reader.ReadToEnd()
        Write-Host "Response Body: $responseBody" -ForegroundColor Red
    }
}

Write-Host "`n3. Verificando configuración JWT..." -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "$apiUrl/api/auth/login" -Method Post -Body $loginData -Headers $headers
    $token = $response.Token
    
    # Decodificar el token JWT (solo para verificar que se generó correctamente)
    $tokenParts = $token.Split('.')
    if ($tokenParts.Length -eq 3) {
        Write-Host "✓ Token JWT generado correctamente" -ForegroundColor Green
    } else {
        Write-Host "✗ Token JWT malformado" -ForegroundColor Red
    }
    
} catch {
    Write-Host "✗ Error verificando JWT: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`n=== Fin de la prueba ===" -ForegroundColor Green 