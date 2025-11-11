# ⚙️ ORDINE DI ESECUZIONE - ACCREDIA SOLUTION

## ✅ CONFIGURAZIONE COMPLETATA

El archivo `.sln` ha sido actualizado con la configuración de "Multi-Start Projects" para establecer el orden correcto de ejecución.

---

## 📋 ORDINE DI ESECUZIONE CONFIGURATO

### Quando pressi F5 (Debug) o Ctrl+F5 (senza Debug)

```
1️⃣  API (Accredia.GestioneAnagrafica.API)
    └─ Porta: https://localhost:5001 / http://localhost:5000
    └─ Status: Si avvia per PRIMA
    
2️⃣  Web (Accredia.GestioneAnagrafica.Web)
    └─ Porta: https://localhost:62412 / http://localhost:62413
    └─ Status: Si avvia per SECONDA (dopo API)
```

---

## 🎯 COME FUNZIONA

### Scenario: Premi F5

```
Visual Studio
    ↓
Legge il .sln e vede "StartupProjects"
    ↓
Avvia Accredia.GestioneAnagrafica.API (priorità 1)
    ↓
Attende e avvia Accredia.GestioneAnagrafica.Web (priorità 2)
    ↓
✅ ENTRAMBI I PROGETTI SONO IN ESECUZIONE
```

---

## 🔍 CONFIGURAZIONE NEL .SLN

```xml
GlobalSection(StartupProjects) = preSolution
	{0EAA1AD2-FAF8-4CB7-2A1F-AAA4BB60EB4B} = 1    <!-- API (primo) -->
	{6D035ACA-53F1-4038-952B-FF26E01A118D} = 2    <!-- Web (secondo) -->
EndGlobalSection
```

Questi sono i GUID (identificatori universali) dei progetti:
- `{0EAA1AD2...}` = Accredia.GestioneAnagrafica.API
- `{6D035ACA...}` = Accredia.GestioneAnagrafica.Web

---

## 🚀 COME ESEGUIRE

### Opzione 1: Visual Studio (Consigliato)
1. Apri la soluzione in Visual Studio
2. Premi `F5` per avviare con Debug
3. Oppure premi `Ctrl+F5` per avviare senza Debug
4. ✅ Si avvieranno API e Web in automatico

### Opzione 2: Riga di comando (API solo)
Se vuoi eseguire solo l'API:
```powershell
cd C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.API
dotnet run
```

### Opzione 3: Riga di comando (Web solo)
Se vuoi eseguire solo il Web:
```powershell
cd C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.Web
dotnet run
```

---

## 📊 PORTE DI ESECUZIONE

| Progetto | HTTP | HTTPS | Stato |
|----------|------|-------|-------|
| **API** | 5000 | 5001 | ✅ Si avvia per primo |
| **Web** | 62413 | 62412 | ✅ Si avvia per secondo |

---

## 🔗 URL DI ACCESSO

Una volta avviato tutto con F5:

```
API
├─ HTTP:  http://localhost:5000
├─ HTTPS: https://localhost:5001
└─ Swagger: https://localhost:5001/swagger

Web
├─ HTTP:  http://localhost:62413
└─ HTTPS: https://localhost:62412
```

---

## 🛑 COME FERMARE

### In Visual Studio
- Premi `Shift+F5` per fermare il Debug
- Oppure clicca il pulsante "Stop" nella toolbar

### A Riga di Comando
- Premi `Ctrl+C` in PowerShell/Command Prompt per fermare il processo

---

## ⚙️ VARIABILI D'AMBIENTE

### API
```json
ASPNETCORE_ENVIRONMENT = Development
```

### Web
```json
ASPNETCORE_ENVIRONMENT = Development
```

---

## 🎨 PERSONALIZZAZIONE

Se vuoi cambiare il progetto di avvio principale:

### Opzione A: Visual Studio GUI
1. Solution Explorer (destra)
2. Click destro sulla soluzione
3. "Set Startup Projects..."
4. Seleziona "Multiple startup projects"
5. Scegli l'ordine

### Opzione B: File .sln (manuale)
Modifica la sezione:
```xml
GlobalSection(StartupProjects) = preSolution
	{0EAA1AD2-FAF8-4CB7-2A1F-AAA4BB60EB4B} = 1    <!-- Cambia qui -->
	{6D035ACA-53F1-4038-952B-FF26E01A118D} = 2    <!-- Cambia qui -->
EndGlobalSection
```

I numeri (1, 2) rappresentano l'ordine di avvio:
- `1` = primo
- `2` = secondo
- ecc.

---

## 🔄 FLUSSO COMPLETO

```
┌─ Premi F5 in Visual Studio
│
├─ Solution carica le configurazioni
│
├─ Compila Shared (libreria base)
│
├─ Compila API (dipende da Shared)
│
├─ Compila Web (dipende da Shared + API)
│
├─ Avvia API su https://localhost:5001
│
├─ Attende che API sia pronto
│
├─ Avvia Web su https://localhost:62412
│
└─ ✅ SISTEMA COMPLETO IN ESECUZIONE
```

---

## 🐛 TROUBLESHOOTING

### Problema: "Porta già in uso"
```
Errore: "Address already in use"
```
Soluzione:
1. Verifica se un'istanza precedente è ancora in esecuzione
2. Usa netstat per vedere le porte: `netstat -ano | findstr :5001`
3. Termina il processo: `taskkill /PID <numero> /F`
4. Riprova

### Problema: "Errore di compilazione"
```
Errore: "One or more errors occurred"
```
Soluzione:
1. Pulisci il progetto: `dotnet clean`
2. Restaura le dipendenze: `dotnet restore`
3. Ricompila: `dotnet build`
4. Riprova F5

### Problema: "Web non si avvia"
```
Errore: "Failed to start project"
```
Soluzione:
1. Verifica che l'API sia avviato correttamente
2. Controlla che il Web dipenda dall'API
3. Verifica le porte nel launchSettings.json
4. Riavvia Visual Studio

---

## ✨ VANTAGGI DI QUESTA CONFIGURAZIONE

✅ **Esecuzione Simultanea** - API e Web si avviano insieme  
✅ **Ordine Garantito** - API si avvia prima del Web  
✅ **Debugging Facile** - Puoi debuggare entrambi i progetti  
✅ **Dipendenze Rispettate** - Web attende che API sia pronto  
✅ **Automatico** - Basta premere F5  
✅ **Professionale** - Simula l'ambiente di produzione  

---

## 📊 STATO ATTUALE

| Elemento | Configurazione |
|----------|----------------|
| **Progetto 1** | API (priorità 1) |
| **Progetto 2** | Web (priorità 2) |
| **Shared** | Non eseguibile (libreria) |
| **Ordine Compilazione** | ✅ Configurato |
| **Ordine Esecuzione** | ✅ Configurato |

---

## 🎯 PROSSIMI STEP

1. ✅ Ordine di compilazione configurato
2. ✅ Ordine di esecuzione configurato
3. ➡️ Apri la soluzione in Visual Studio
4. ➡️ Premi F5 per iniziare

---

## 📝 RIEPILOGO

```
F5 (Debug) o Ctrl+F5 (No Debug)
    ↓
Visual Studio avvia:
    1. API (https://localhost:5001)
    2. Web (https://localhost:62412)
    ↓
✅ SISTEMA COMPLETO FUNZIONANTE
```

---

**Data Configurazione**: 3 Novembre 2025  
**Status**: ✅ **PRONTO**

Quando premi F5 in Visual Studio, l'API e il Web si avvieranno automaticamente nell'ordine corretto!

