const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

// I keep fetch logic in one place so every request behaves the same
export async function httpGet<T>(path: string, signal?: AbortSignal): Promise<T> {
  const response = await fetch(`${API_BASE_URL}${path}`, {
    method: "GET",
    headers: { Accept: "application/json" },
    signal,
  });

  // If the request was cancelled I stop immediately
  if (signal?.aborted) {
    throw new Error("Request cancelled.");
  }

  // If the server returns an error, I want a useful message in the UI
  if (!response.ok) {
    const text = await response.text();
    throw new Error(`HTTP ${response.status}: ${text}`);
  }

  return (await response.json()) as T;
}