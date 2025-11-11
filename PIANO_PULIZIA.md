# 🧹 PLAN DE LIMPIEZA - ARCHIVOS NON UTILIZZATI

## ❌ DA ELIMINARE (Cartelle)

### Cartelle Obsolete:
1. **Accredia.GestioneAnagrafica.Shared/** - Non necessario (dipendenza già gestita)
2. **Accredia.GestioneAnagrafica.Web/** - Non necessario (progetto separato Web)
3. **bin/** (nella radice) - File di compilazione obsoleti
4. **obj/** (nella radice) - File intermedi obsoleti
5. **.vs/** - Configurazione Visual Studio locale
6. **.github/** - Configurazione GitHub (opzionale)

### Cartelle Contenitori Obsolete (copie nella radice):
- Config/
- Data/
- DTOs/
- Endpoints/
- Models/
- Properties/
- Responses/
- Services/
- Validators/

*(Questi ora sono nella cartella Accredia.GestioneAnagrafica.API/)*

---

## ❌ DA ELIMINARE (File Documentazione)

### File di Fix e Troubleshooting:
- CAMBIOS_AUTENTICACION.md
- CORREZIONI_APPLICATE.md
- FIX_4_SCHEMA_CONFLICT.md
- FIX_5_PAGERESULT.md
- FIX_PAGERESULT_GENERIC_CONFLICT.md
- FIX_SWAGGER_JSON_ERROR.md
- FIX_SWAGGER_NON_VISIBILE.md
- FIX_SWAGGER_SCHEMA_CONFLICT.md
- RISOLUZIONE_ERRORE_WITHOPENAPI.md

### File di Fasi di Sviluppo (obsoleti):
- FASE2_COMPLETATA.md
- GRUPPO_A_COMPLETATO.md
- GRUPPO_A_SUCCESS.md
- GRUPPO_D_COMPLETATO.md
- GRUPPO_D_F_COMPLETATI.md
- GRUPPO_E_TIPOLOGICHE_COMPLETATO.md
- GRUPPO_F_COMPLETATO.md

### File di Riepilogo/Check Obsoleti:
- CHECKLIST_FINALE.md
- COMPLETAMENTO_FINALE.md
- VERIFICA_FINALE_OK.md
- VERIFICA_MODELS_AGGIORNATI.md
- VERIFICA_FILES_ENDPOINT.md
- MANIFEST_FILE_CREATI.md
- TUTTI_4_FIX_PRONTO.md
- TUTTI_FIX_RIEPILOGO.md
- ULTIMO_FIX_PRONTO.md

### File di Documentazione Generale (duplicati/obsoleti):
- PROGETTO_COMPLETATO.md
- PROYECTO_COMPLETADO.md
- SVILUPPO_COMPLETATO.md
- INDICE_DOCUMENTAZIONE.md
- RIEPILOGO_COMPLETO.md
- QUICK_START_DOPO_FIX.md
- SOLUCION_ERRORES_COMPILACION.md
- PIANO_SVILUPPO.md
- ITERAZIONE_2_README.md

### File di Documentazione di Gruppi:
- GRUPPI_D_E_F_DETTAGLIO.md
- GRUPPO_E_TIPOLOGICHE_SINTESI_VISUALE.md
- README_GRUPPO_E.md
- RIEPILOGO_GRUPPO_E_TIPOLOGICHE.md
- RIEPILOGO_FIX_APPLICATI.md
- TEST_ENDPOINTS_TIPOLOGICHE.md

### File di Esempio/Guida (duplicati):
- CODE_EXAMPLES.md
- GUIA_TOKENS_Y_ENDPOINTS.md
- GUIDA_RAPIDA_BUILD_TEST.md
- IMPLEMENTAZIONE_AMBITI_APPLICAZIONE.md
- AUTHENTICATION.md
- CONFIGURAZIONE_ENV_SQLSERVER.md
- EF_MIGRATIONS_COMMANDS.md

### File di Build:
- build.log
- copy-api-project.bat

### File di Test:
- test-api.ps1
- test-api.sh

### Documento Office:
- Prospetto_Applicazione_Web_Claude_API.docx

---

## ✅ DA MANTENERE (Cartella Principale)

**Essenziale:**
- .env
- .env.example
- .gitignore
- appsettings.json
- appsettings.Development.json
- Program.cs
- Accredia.GestioneAnagrafica.API.csproj
- Accredia.GestioneAnagrafica.sln (se usato per referenze)
- README.md

**Cartella della API:**
- Accredia.GestioneAnagrafica.API/ (con tutto il contenuto)

---

## 📊 STATISTICHE

| Tipo | Quantità | Azione |
|------|----------|--------|
| Cartelle da eliminare | 13 | ❌ DELETE |
| File .md da eliminare | 54 | ❌ DELETE |
| File .bat da eliminare | 1 | ❌ DELETE |
| File .ps1/.sh da eliminare | 2 | ❌ DELETE |
| File .docx da eliminare | 1 | ❌ DELETE |
| **TOTAL DA ELIMINARE** | **71** | |
| **Cartelle da mantenere** | **2** | ✅ KEEP |
| **File da mantenere** | **9** | ✅ KEEP |

---

## 🎯 SPAZIO LIBERATO (Stima)

- Cartelle Code: ~30 MB (bin/, obj/, Config/, etc)
- File .md: ~2 MB (54 file di documentazione)
- **Total Stimato**: ~32 MB

---

## ✨ RISULTATO FINALE

```
Cartella C:\Accredia\Sviluppo\ sarà ridotta a:

Accredia.GestioneAnagrafica.API/     ← Tutto il codice
.env
.env.example
appsettings.json
appsettings.Development.json
.gitignore
Program.cs
Accredia.GestioneAnagrafica.API.csproj
Accredia.GestioneAnagrafica.sln
README.md

Molto più pulita e ordinata! 🧹
```

---

**Pronto a procedere con la pulizia?**

