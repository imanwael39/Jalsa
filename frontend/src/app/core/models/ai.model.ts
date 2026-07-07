export interface RagSource {
    sessionId: string;
    score: number;
    source: string | null;
    textPreview: string;
}

export interface AiGenerationDiagnostics {
    output: string;
    systemPrompt: string;
    userPrompt: string;
    model: string;
    latencyMs: number;
    embeddingCount: number;
    ragChunkCount: number;
    inputTokens: number | null;
    outputTokens: number | null;
    totalTokens: number | null;
    ragSources: RagSource[];
}
