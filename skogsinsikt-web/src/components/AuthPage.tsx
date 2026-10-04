import { useState } from "react";
import { login, register } from "../services/authService";
import type { AuthResponse } from "../types/auth";

type Props = {
  onAuthenticated: (auth: AuthResponse) => void;
};

function AuthPage({ onAuthenticated }: Props) {
  const [mode, setMode] = useState<"login" | "register">("login");
  const [fullName, setFullName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const [submitting, setSubmitting] = useState(false);

  async function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();

    setError("");
    setSubmitting(true);

    try {
      const auth =
        mode === "login"
          ? await login({ email, password })
          : await register({ fullName, email, password });

      onAuthenticated(auth);
    } catch (err) {
      setError(
        err instanceof Error
          ? err.message
          : "Något gick fel. Försök igen."
      );
    } finally {
      setSubmitting(false);
    }
  }

  function changeMode(nextMode: "login" | "register") {
    setMode(nextMode);
    setError("");
    setPassword("");
  }

  return (
    <main className="authPage">
      <section className="authIntro">
        <p className="eyebrow">SKOGSINSIKT</p>
        <h1>Din skog. Bättre beslut.</h1>
        <p>
          Ett digitalt beslutsstöd för skogsägare som kombinerar
          skogsdata, väderinformation och geografisk översikt.
        </p>

        <div className="authFeatures">
          <span>Skogsområden</span>
          <span>Riskanalys</span>
          <span>GIS & väderdata</span>
        </div>
      </section>

      <section className="authCard">
        <div className="authTabs">
          <button
            type="button"
            className={mode === "login" ? "active" : ""}
            onClick={() => changeMode("login")}
          >
            Logga in
          </button>

          <button
            type="button"
            className={mode === "register" ? "active" : ""}
            onClick={() => changeMode("register")}
          >
            Skapa konto
          </button>
        </div>

        <div className="authHeading">
          <h2>
            {mode === "login"
              ? "Välkommen tillbaka"
              : "Skapa ditt konto"}
          </h2>

          <p>
            {mode === "login"
              ? "Logga in för att fortsätta till din skogsöversikt."
              : "Registrera dig för att börja använda SkogsInsikt."}
          </p>
        </div>

        <form className="authForm" onSubmit={handleSubmit}>
          {mode === "register" && (
            <label>
              Namn
              <input
                type="text"
                value={fullName}
                onChange={(event) => setFullName(event.target.value)}
                placeholder="För- och efternamn"
                minLength={2}
                required
              />
            </label>
          )}

          <label>
            E-post
            <input
              type="email"
              value={email}
              onChange={(event) => setEmail(event.target.value)}
              placeholder="namn@example.se"
              required
              autoComplete="email"
            />
          </label>

          <label>
            Lösenord
            <input
              type="password"
              value={password}
              onChange={(event) => setPassword(event.target.value)}
              placeholder="Minst 8 tecken"
              minLength={8}
              required
              autoComplete={
                mode === "login"
                  ? "current-password"
                  : "new-password"
              }
            />
          </label>

          {error && <div className="authError">{error}</div>}

          <button
            className="authSubmit"
            type="submit"
            disabled={submitting}
          >
            {submitting
              ? "Vänta..."
              : mode === "login"
                ? "Logga in"
                : "Skapa konto"}
          </button>
        </form>

        <p className="authFooter">
          {mode === "login"
            ? "Har du inget konto?"
            : "Har du redan ett konto?"}

          <button
            type="button"
            onClick={() =>
              changeMode(mode === "login" ? "register" : "login")
            }
          >
            {mode === "login" ? "Skapa konto" : "Logga in"}
          </button>
        </p>
      </section>
    </main>
  );
}

export default AuthPage;
