const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

// I keep fetch logic in one place so every request behaves the same
export async function httpGet<T>(path: string): Promise<T> {
  const response = await fetch(`${API_BASE_URL}${path}`, {
    method: "GET",
    headers: { Accept: "application/json" },
  });

  // If the server returns an error, I want a useful message in the UI
  if (!response.ok) {
    const text = await response.text();
    throw new Error(`HTTP ${response.status}: ${text}`);
  }

  return (await response.json()) as T;
}