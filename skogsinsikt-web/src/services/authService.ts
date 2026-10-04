import type {
  AuthResponse,
  LoginRequest,
  RegisterRequest,
} from "../types/auth";

const API_URL = "http://localhost:5113/api";

async function handleResponse(response: Response): Promise<AuthResponse> {
  if (!response.ok) {
    const data = await response.json().catch(() => null);

    throw new Error(
      data?.message ??
        data?.errors?.join(", ") ??
        "Autentiseringen misslyckades."
    );
  }

  return response.json();
}

export async function login(
  request: LoginRequest
): Promise<AuthResponse> {
  const response = await fetch(`${API_URL}/auth/login`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(request),
  });

  return handleResponse(response);
}

export async function register(
  request: RegisterRequest
): Promise<AuthResponse> {
  const response = await fetch(`${API_URL}/auth/register`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(request),
  });

  return handleResponse(response);
}
