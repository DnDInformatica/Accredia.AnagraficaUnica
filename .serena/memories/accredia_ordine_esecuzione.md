# ACCREDIA SOLUTION - ORDINE DI ESECUZIONE CONFIGURATO

## Configurazione Completata

Il file `Accredia.GestioneAnagrafica.sln` è stato aggiornato con la configurazione di "Multi-Start Projects".

## Ordine di Esecuzione (F5)

```
1. Accredia.GestioneAnagrafica.API (priorità 1)
   └─ https://localhost:5001

2. Accredia.GestioneAnagrafica.Web (priorità 2)
   └─ https://localhost:62412
```

## Come Avviare (3 Opzioni)

### Opzione 1: Visual Studio (Consigliato)
1. Apri la soluzione in Visual Studio
2. Premi F5 o Ctrl+F5
3. API e Web si avviano automaticamente

### Opzione 2: PowerShell
```powershell
./run-solution.ps1
```

### Opzione 3: Batch
```batch
run-solution.bat
```

## URL di Accesso

- API: https://localhost:5001
- Swagger: https://localhost:5001/swagger
- Web: https://localhost:62412

## Configurazione nel .sln

```xml
GlobalSection(StartupProjects) = preSolution
	{0EAA1AD2-FAF8-4CB7-2A1F-AAA4BB60EB4B} = 1    <!-- API -->
	{6D035ACA-53F1-4038-952B-FF26E01A118D} = 2    <!-- Web -->
EndGlobalSection
```

## File Creati

- `ORDINE_ESECUZIONE.md` - Documentazione completa
- `run-solution.ps1` - Script PowerShell
- `run-solution.bat` - Script Batch

## Vantaggi

✓ Esecuzione simultanea di API e Web
✓ Ordine garantito
✓ Debugging facile
✓ Automatico con F5
✓ Professionale e mantenibile
