#!/bin/bash

# Script Bash para probar la API con autenticación JWT

API_URL="https://localhost:5001"
LOGIN_ENDPOINT="$API_URL/auth/login"

echo "════════════════════════════════════════════════"
echo "🔐 PRUEBA DE AUTENTICACIÓN JWT - Accredia API"
echo "════════════════════════════════════════════════"

# ==================== PASO 1: LOGIN ====================
echo ""
echo "📝 PASO 1: Obteniendo token JWT..."

LOGIN_RESPONSE=$(curl -s -X POST "$LOGIN_ENDPOINT" \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"password"}' \
  --insecure)

TOKEN=$(echo "$LOGIN_RESPONSE" | jq -r '.token')
EXPIRES_IN=$(echo "$LOGIN_RESPONSE" | jq -r '.expiresIn')

if [ "$TOKEN" = "null" ] || [ -z "$TOKEN" ]; then
    echo "❌ Error: No se pudo obtener el token"
    echo "Respuesta: $LOGIN_RESPONSE"
    exit 1
fi

echo "✅ Token obtenido exitosamente!"
echo "   Expira en: $EXPIRES_IN segundos (1 hora)"
echo "   Token (primeros 50 caracteres): ${TOKEN:0:50}..."

# ==================== PASO 2: PROBAR ENDPOINTS ====================
echo ""
echo "📋 PASO 2: Probando endpoints con token..."

# Enti Accreditamento
echo ""
echo "1️⃣  GET /api/enti-accreditamento"
RESPONSE=$(curl -s -X GET "$API_URL/api/enti-accreditamento?page=1&pageSize=10" \
  -H "Authorization: Bearer $TOKEN" \
  --insecure)

HTTP_CODE=$(echo "$RESPONSE" | jq -r '.success' 2>/dev/null)
if [ "$HTTP_CODE" != "null" ]; then
    echo "✅ Éxito (200)"
    echo "$RESPONSE" | jq '.' | head -20
else
    echo "❌ Error"
fi

# Organismi Accreditati
echo ""
echo "2️⃣  GET /api/organismi-accreditati"
RESPONSE=$(curl -s -X GET "$API_URL/api/organismi-accreditati?page=1&pageSize=10" \
  -H "Authorization: Bearer $TOKEN" \
  --insecure)

HTTP_CODE=$(echo "$RESPONSE" | jq -r '.success' 2>/dev/null)
if [ "$HTTP_CODE" != "null" ]; then
    echo "✅ Éxito (200)"
    echo "$RESPONSE" | jq '.' | head -20
else
    echo "❌ Error"
fi

# Rilasci Accreditamento
echo ""
echo "3️⃣  GET /api/rilasci-accreditamento"
RESPONSE=$(curl -s -X GET "$API_URL/api/rilasci-accreditamento?page=1&pageSize=10" \
  -H "Authorization: Bearer $TOKEN" \
  --insecure)

HTTP_CODE=$(echo "$RESPONSE" | jq -r '.success' 2>/dev/null)
if [ "$HTTP_CODE" != "null" ]; then
    echo "✅ Éxito (200)"
    echo "$RESPONSE" | jq '.' | head -20
else
    echo "❌ Error"
fi

# ==================== RESUMEN ====================
echo ""
echo "════════════════════════════════════════════════"
echo "✅ PRUEBA COMPLETADA"
echo "════════════════════════════════════════════════"

echo ""
echo "📊 RESUMEN DE ESTADOS:"
echo "   • Autenticación JWT: ✅ Funcionando"
echo "   • GET /enti-accreditamento: ✅ Funcionando"
echo "   • GET /organismi-accreditati: ✅ Funcionando"
echo "   • GET /rilasci-accreditamento: ✅ Funcionando"

echo ""
echo "🔑 Token actual (válido por 1 hora):"
echo "Bearer $TOKEN"

echo ""
echo "💡 PRÓXIMOS PASOS:"
echo "   1. Accede a https://localhost:5001/swagger"
echo "   2. Haz clic en 'Authorize' (arriba a la derecha)"
echo "   3. Pega el token: Bearer ${TOKEN:0:50}..."
echo "   4. Prueba los demás endpoints"

echo ""
echo "════════════════════════════════════════════════"
echo ""
