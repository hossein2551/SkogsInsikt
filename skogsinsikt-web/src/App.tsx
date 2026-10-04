import { useEffect, useState } from "react";
import "./App.css";

type ForestAnalysis = {
  id: number;
  forestAreaId: number;
  createdAt: string;
  riskLevel: string;
  recommendation: string;
  temperature: number;
  precipitation: number;
  windSpeed: number;
};

type ForestArea = {
  id: number;
  name: string;
  areaHectares: number;
  treeSpecies: string;
  plantingYear: number;
  latitude: number;
  longitude: number;
};

const API_URL = "http://localhost:5113/api";

function App() {
  const [forestAreas, setForestAreas] = useState<ForestArea[]>([]);
  const [analyses, setAnalyses] = useState<ForestAnalysis[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [analyzingId, setAnalyzingId] = useState<number | null>(null);

  const loadData = async () => {
    try {
      setError("");

      const areasResponse = await fetch(`${API_URL}/ForestAreas`);

      if (!areasResponse.ok) {
        throw new Error("Kunde inte hämta skogsområden.");
      }

      const areas: ForestArea[] = await areasResponse.json();
      setForestAreas(areas);

      const historyResponses = await Promise.all(
        areas.map((area) =>
          fetch(`${API_URL}/ForestAnalysis/area/${area.id}`)
        )
      );

      if (historyResponses.some((response) => !response.ok)) {
        throw new Error("Kunde inte hämta analyshistorik.");
      }

      const history = await Promise.all(
        historyResponses.map(
          (response) => response.json() as Promise<ForestAnalysis[]>
        )
      );

      setAnalyses(history.flat());
    } catch {
      setError("Kunde inte ansluta till SkogsInsikt API.");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadData();
  }, []);

  const analyzeForestArea = async (id: number) => {
    try {
      setError("");
      setAnalyzingId(id);

      const response = await fetch(
        `${API_URL}/ForestAnalysis/${id}`,
        {
          method: "POST",
        }
      );

      if (!response.ok) {
        throw new Error("Analysen misslyckades.");
      }

      const newAnalysis: ForestAnalysis = await response.json();

      setAnalyses((current) => [
        newAnalysis,
        ...current.filter((item) => item.id !== newAnalysis.id),
      ]);
    } catch {
      setError("Kunde inte genomföra skogsanalysen.");
    } finally {
      setAnalyzingId(null);
    }
  };

  const totalArea = forestAreas.reduce(
    (sum, area) => sum + area.areaHectares,
    0
  );

  const latestAnalysisForArea = (forestAreaId: number) =>
    analyses
      .filter((item) => item.forestAreaId === forestAreaId)
      .sort(
        (a, b) =>
          new Date(b.createdAt).getTime() -
          new Date(a.createdAt).getTime()
      )[0];

  const currentRiskCount = forestAreas.filter((area) => {
    const latest = latestAnalysisForArea(area.id);

    return (
      latest &&
      (latest.riskLevel === "Medium" ||
        latest.riskLevel === "High")
    );
  }).length;

  const formatDate = (date: string) =>
    new Intl.DateTimeFormat("sv-SE", {
      dateStyle: "short",
      timeStyle: "short",
    }).format(new Date(date));

  return (
    <div className="app">
      <header className="header">
        <div>
          <h1>SkogsInsikt</h1>
          <p>Digitalt beslutsstöd för skogsägare</p>
        </div>

        <div className="status">
          <span></span>
          System online
        </div>
      </header>

      <main className="content">
        <section className="hero">
          <div>
            <p className="eyebrow">ÖVERSIKT</p>
            <h2>Din skog. Bättre beslut.</h2>
            <p>
              Följ dina skogsområden och få aktuella riskbedömningar
              baserade på väderdata.
            </p>
          </div>

          <button>+ Lägg till skogsområde</button>
        </section>

        <section className="stats">
          <article>
            <p>Skogsområden</p>
            <strong>{loading ? "–" : forestAreas.length}</strong>
            <span>Registrerade områden</span>
          </article>

          <article>
            <p>Total areal</p>
            <strong>{loading ? "–" : totalArea}</strong>
            <span>Hektar</span>
          </article>

          <article>
            <p>Aktuella risker</p>
            <strong>{loading ? "–" : currentRiskCount}</strong>
            <span>Områden med förhöjd risk</span>
          </article>
        </section>

        {error && (
          <div className="errorMessage">
            {error}
          </div>
        )}

        <section className="panel">
          <div className="panelHeader">
            <div>
              <p className="eyebrow">SKOGSINNEHAV</p>
              <h2>Mina skogsområden</h2>
            </div>
          </div>

          {loading && (
            <div className="emptyState">
              <h3>Hämtar skogsområden...</h3>
            </div>
          )}

          {!loading && forestAreas.length === 0 && (
            <div className="emptyState">
              <h3>Inga skogsområden registrerade</h3>
            </div>
          )}

          {!loading && forestAreas.length > 0 && (
            <div className="forestGrid">
              {forestAreas.map((area) => {
                const latest = latestAnalysisForArea(area.id);
                const areaHistory = analyses
                  .filter(
                    (item) => item.forestAreaId === area.id
                  )
                  .sort(
                    (a, b) =>
                      new Date(b.createdAt).getTime() -
                      new Date(a.createdAt).getTime()
                  );

                return (
                  <article className="forestCard" key={area.id}>
                    <div className="forestCardTop">
                      <div>
                        <p className="eyebrow">SKOGSOMRÅDE</p>
                        <h3>{area.name}</h3>
                      </div>

                      <span className="species">
                        {area.treeSpecies}
                      </span>
                    </div>

                    <div className="forestDetails">
                      <div>
                        <span>Areal</span>
                        <strong>{area.areaHectares} ha</strong>
                      </div>

                      <div>
                        <span>Trädslag</span>
                        <strong>{area.treeSpecies}</strong>
                      </div>

                      <div>
                        <span>Planteringsår</span>
                        <strong>{area.plantingYear}</strong>
                      </div>
                    </div>

                    <button
                      className="analysisButton"
                      onClick={() => analyzeForestArea(area.id)}
                      disabled={analyzingId === area.id}
                    >
                      {analyzingId === area.id
                        ? "Analyserar..."
                        : "Analysera område"}
                    </button>

                    {latest && (
                      <div
                        className={`analysisResult risk-${latest.riskLevel.toLowerCase()}`}
                      >
                        <div className="analysisHeader">
                          <span>Aktuell risknivå</span>
                          <strong>{latest.riskLevel}</strong>
                        </div>

                        <div className="weatherData">
                          <div>
                            <span>Temperatur</span>
                            <strong>
                              {latest.temperature} °C
                            </strong>
                          </div>

                          <div>
                            <span>Nederbörd</span>
                            <strong>
                              {latest.precipitation} mm
                            </strong>
                          </div>

                          <div>
                            <span>Vind</span>
                            <strong>
                              {latest.windSpeed} km/h
                            </strong>
                          </div>
                        </div>

                        <div className="recommendation">
                          <span>Rekommendation</span>
                          <p>{latest.recommendation}</p>
                        </div>
                      </div>
                    )}

                    {areaHistory.length > 0 && (
                      <div className="history">
                        <h4>Analyshistorik</h4>

                        {areaHistory.map((item) => (
                          <div
                            className="historyItem"
                            key={item.id}
                          >
                            <div>
                              <strong>
                                {formatDate(item.createdAt)}
                              </strong>
                              <span>
                                {item.temperature} °C ·{" "}
                                {item.windSpeed} km/h
                              </span>
                            </div>

                            <span
                              className={`riskBadge riskBadge-${item.riskLevel.toLowerCase()}`}
                            >
                              {item.riskLevel}
                            </span>
                          </div>
                        ))}
                      </div>
                    )}
                  </article>
                );
              })}
            </div>
          )}
        </section>
      </main>
    </div>
  );
}

export default App;