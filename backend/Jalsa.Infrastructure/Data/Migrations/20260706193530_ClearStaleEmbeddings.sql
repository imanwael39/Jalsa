-- Migration: ClearStaleEmbeddings (20260706193530)
-- Reason: switching embedding model from amazon.titan-embed-text-v2:0:8k (1024-dim)
--         to amazon.titan-embed-text-v1 (1536-dim). Old vectors are dimension-mismatched
--         and would corrupt cosine similarity scores / throw at deserialize time.
--
-- Run this against your dev/staging/prod DB.
-- Idempotent: safe to re-run.

UPDATE [AiArtifacts]
SET    [EmbeddingVector] = NULL
WHERE  [EmbeddingVector] IS NOT NULL;

DELETE FROM [SessionEmbeddings];

-- Optional: record that the migration was applied (so EF doesn't try to re-apply it)
IF NOT EXISTS (
    SELECT 1 FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260706193530_ClearStaleEmbeddings'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260706193530_ClearStaleEmbeddings', N'8.0.0');
END
