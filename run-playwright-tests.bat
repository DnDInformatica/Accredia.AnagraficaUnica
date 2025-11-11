@echo off
REM Script Batch per eseguire Playwright tests - Accredia Corporate Skill
REM ====================================================================

setlocal enabledelayedexpansion

echo.
echo ╔══════════════════════════════════════════════════════╗
echo ║  Accredia - Playwright Tests                         ║
echo ║  Corporate Skill Verification                        ║
echo ╚══════════════════════════════════════════════════════╝
echo.

REM Verifica Node.js
echo [INFO] Verifica Node.js...
node --version >nul 2>&1
if errorlevel 1 (
    echo [ERROR] Node.js non trovato. Installalo da https://nodejs.org/
    pause
    exit /b 1
)
for /f "tokens=*" %%i in ('node --version') do set NODE_VERSION=%%i
echo [OK] %NODE_VERSION%

REM Verifica npm
echo [INFO] Verifica npm...
npm --version >nul 2>&1
if errorlevel 1 (
    echo [ERROR] npm non trovato
    pause
    exit /b 1
)

REM Installa dipendenze se non presenti
if not exist "node_modules" (
    echo [INFO] Installazione dipendenze...
    call npm install
    if errorlevel 1 (
        echo [ERROR] Errore durante installazione
        pause
        exit /b 1
    )
)

REM Installa browser Playwright se non presenti
if not exist "node_modules/.bin/playwright" (
    echo [INFO] Installazione browser Playwright...
    call npx playwright install
    if errorlevel 1 (
        echo [ERROR] Errore durante installazione browser
        pause
        exit /b 1
    )
)

echo.
echo ╔══════════════════════════════════════════════════════╗
echo ║  ESECUZIONE TEST - MODALITA HEADLESS               ║
echo ╚══════════════════════════════════════════════════════╝
echo.

echo [INFO] Verifica connessione Web app su https://localhost:7412...
echo        (Ignorando errori certificato)
echo.

echo [INFO] Esecuzione test Corporate Skill...
call npx playwright test accredia-corporate-skill.spec.ts

if errorlevel 1 (
    echo.
    echo [WARN] Alcuni test potrebbero essere falliti
    echo [INFO] Apertura report per visualizzare i dettagli...
    call npx playwright show-report
    pause
    exit /b 1
) else (
    echo.
    echo [OK] Tutti i test completati con successo!
    echo.
    echo [INFO] Apertura report HTML...
    call npx playwright show-report
)

pause
