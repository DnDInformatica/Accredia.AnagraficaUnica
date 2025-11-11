-- ==========================================
-- SCRIPT PER VERIFICARE STRUTTURA DATABASE
-- ==========================================
-- Esegui questo script sul database Accredia
-- per vedere la struttura delle tabelle
-- ==========================================

USE Accredia;
GO

PRINT '========================================';
PRINT 'TABELLE NEL DATABASE';
PRINT '========================================';

SELECT
    SCHEMA_NAME(t.schema_id) AS [Schema],
    t.name AS [Nome Tabella],
    COUNT(c.column_id) AS [Num Colonne]
FROM sys.tables t
INNER JOIN sys.columns c ON t.object_id = c.object_id
GROUP BY t.schema_id, t.name
ORDER BY SCHEMA_NAME(t.schema_id), t.name;
GO

PRINT '';
PRINT '========================================';
PRINT 'STRUTTURA TABELLA Persona';
PRINT '========================================';

SELECT
    c.name AS [Nome Colonna],
    t.name AS [Tipo],
    c.max_length AS [Lunghezza],
    c.is_nullable AS [Nullable],
    c.is_identity AS [Identity]
FROM sys.columns c
INNER JOIN sys.types t ON c.user_type_id = t.user_type_id
WHERE c.object_id = OBJECT_ID('Persone.Persona')
ORDER BY c.column_id;
GO

PRINT '';
PRINT '========================================';
PRINT 'STRUTTURA TABELLA EntitaAziendale';
PRINT '========================================';

SELECT
    c.name AS [Nome Colonna],
    t.name AS [Tipo],
    c.max_length AS [Lunghezza],
    c.is_nullable AS [Nullable],
    c.is_identity AS [Identity]
FROM sys.columns c
INNER JOIN sys.types t ON c.user_type_id = t.user_type_id
WHERE c.object_id = OBJECT_ID('Persone.EntitaAziendale')
ORDER BY c.column_id;
GO

PRINT '';
PRINT '========================================';
PRINT 'CHIAVI PRIMARIE';
PRINT '========================================';

SELECT
    SCHEMA_NAME(t.schema_id) AS [Schema],
    t.name AS [Tabella],
    i.name AS [Nome Chiave],
    c.name AS [Colonna]
FROM sys.indexes i
INNER JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
INNER JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
INNER JOIN sys.tables t ON i.object_id = t.object_id
WHERE i.is_primary_key = 1
  AND t.name IN ('Persona', 'EntitaAziendale')
ORDER BY t.name, ic.key_ordinal;
GO

PRINT '';
PRINT '========================================';
PRINT 'FOREIGN KEYS';
PRINT '========================================';

SELECT
    OBJECT_SCHEMA_NAME(f.parent_object_id) AS [Schema],
    OBJECT_NAME(f.parent_object_id) AS [Tabella],
    f.name AS [Nome FK],
    COL_NAME(fc.parent_object_id, fc.parent_column_id) AS [Colonna],
    OBJECT_NAME(f.referenced_object_id) AS [Tabella Riferita],
    COL_NAME(fc.referenced_object_id, fc.referenced_column_id) AS [Colonna Riferita]
FROM sys.foreign_keys AS f
INNER JOIN sys.foreign_key_columns AS fc ON f.object_id = fc.constraint_object_id
WHERE OBJECT_NAME(f.parent_object_id) IN ('Persona', 'EntitaAziendale')
ORDER BY OBJECT_NAME(f.parent_object_id);
GO
