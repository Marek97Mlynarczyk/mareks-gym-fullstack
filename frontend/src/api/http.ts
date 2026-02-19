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

  // Cast the JSON response to the expected type
  return (await response.json()) as T;
}

// I use this helper for POST requests so I handle JSON and errors consistently
export async function httpPost<TResponse, TRequest>(
  path: string,
  body: TRequest
): Promise<TResponse> {
  const response = await fetch(`${API_BASE_URL}${path}`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Accept: "application/json",
    },
    body: JSON.stringify(body),
  });

  // I throw that JSON so the UI can display field-level errors
  if (!response.ok) {
    const data = await response.json().catch(() => null);
    throw data ?? new Error(`HTTP ${response.status}`);
  }

  return (await response.json()) as TResponse;
}

// I use this helper for DELETE requests so delete behavior is consistent
export async function httpDelete(path: string): Promise<void> {
  const response = await fetch(`${API_BASE_URL}${path}`, {
    method: "DELETE",
  });
  
  if (!response.ok) {
    throw new Error(`HTTP ${response.status}`);
  }
}