# -*- coding: utf-8 -*-
import os
import shutil
from pathlib import Path

# Directorio base
base_dir = r"C:\Accredia\Sviluppo"
os.chdir(base_dir)

# Carpetas a eliminar
folders_to_delete = [
    "Accredia.GestioneAnagrafica.Shared",
    "Accredia.GestioneAnagrafica.Web",
    "bin",
    "obj",
    "Config",
    "Data",
    "DTOs",
    "Endpoints",
    "Models",
    "Properties",
    "Responses",
    "Services",
    "Validators",
    ".vs"
]

# Archivos a eliminar
files_to_delete = [
    "CAMBIOS_AUTENTICACION.md",
    "CORREZIONI_APPLICATE.md",
    "FIX_4_SCHEMA_CONFLICT.md",
    "FIX_5_PAGERESULT.md",
    "FIX_PAGERESULT_GENERIC_CONFLICT.md",
    "FIX_SWAGGER_JSON_ERROR.md",
    "FIX_SWAGGER_NON_VISIBILE.md",
    "FIX_SWAGGER_SCHEMA_CONFLICT.md",
    "RISOLUZIONE_ERRORE_WITHOPENAPI.md",
    "FASE2_COMPLETATA.md",
    "GRUPPO_A_COMPLETATO.md",
    "GRUPPO_A_SUCCESS.md",
    "GRUPPO_D_COMPLETATO.md",
    "GRUPPO_D_F_COMPLETATI.md",
    "GRUPPO_E_TIPOLOGICHE_COMPLETATO.md",
    "GRUPPO_F_COMPLETATO.md",
    "CHECKLIST_FINALE.md",
    "COMPLETAMENTO_FINALE.md",
    "VERIFICA_FINALE_OK.md",
    "VERIFICA_MODELS_AGGIORNATI.md",
    "VERIFICA_FILES_ENDPOINT.md",
    "MANIFEST_FILE_CREATI.md",
    "TUTTI_4_FIX_PRONTO.md",
    "TUTTI_FIX_RIEPILOGO.md",
    "ULTIMO_FIX_PRONTO.md",
    "PROGETTO_COMPLETATO.md",
    "PROYECTO_COMPLETADO.md",
    "SVILUPPO_COMPLETATO.md",
    "INDICE_DOCUMENTAZIONE.md",
    "RIEPILOGO_COMPLETO.md",
    "QUICK_START_DOPO_FIX.md",
    "SOLUCION_ERRORES_COMPILACION.md",
    "PIANO_SVILUPPO.md",
    "ITERAZIONE_2_README.md",
    "GRUPPI_D_E_F_DETTAGLIO.md",
    "GRUPPO_E_TIPOLOGICHE_SINTESI_VISUALE.md",
    "README_GRUPPO_E.md",
    "RIEPILOGO_GRUPPO_E_TIPOLOGICHE.md",
    "RIEPILOGO_FIX_APPLICATI.md",
    "TEST_ENDPOINTS_TIPOLOGICHE.md",
    "CODE_EXAMPLES.md",
    "GUIA_TOKENS_Y_ENDPOINTS.md",
    "GUIDA_RAPIDA_BUILD_TEST.md",
    "IMPLEMENTAZIONE_AMBITI_APPLICAZIONE.md",
    "AUTHENTICATION.md",
    "CONFIGURAZIONE_ENV_SQLSERVER.md",
    "EF_MIGRATIONS_COMMANDS.md",
    "build.log",
    "copy-api-project.bat",
    "test-api.ps1",
    "test-api.sh",
    "Prospetto_Applicazione_Web_Claude_API.docx"
]

print("Eliminando carpetas...")
deleted_folders = 0
for folder in folders_to_delete:
    path = Path(folder)
    if path.exists():
        try:
            shutil.rmtree(folder)
            print("[OK] Carpeta eliminada: " + folder)
            deleted_folders += 1
        except Exception as e:
            print("[ERROR] Error al eliminar " + folder + ": " + str(e))

print("\nEliminando archivos...")
deleted_files = 0
for file in files_to_delete:
    path = Path(file)
    if path.exists():
        try:
            os.remove(file)
            print("[OK] Archivo eliminado: " + file)
            deleted_files += 1
        except Exception as e:
            print("[ERROR] Error al eliminar " + file + ": " + str(e))

print("\n[DONE] Limpieza completada!")
print("Carpetas eliminadas: " + str(deleted_folders))
print("Archivos eliminados: " + str(deleted_files))
print("\nArchivos y carpetas restantes:")
remaining = sorted(os.listdir('.'))
for item in remaining:
    if not item.startswith('.'):
        prefix = "[DIR]" if os.path.isdir(item) else "[FILE]"
        print(prefix + " " + item)
