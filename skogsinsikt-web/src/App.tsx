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

function App() {
  const [forestAreas, setForestAreas] = useState<ForestArea[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [analysis, setAnalysis] = useState<ForestAnalysis | null>(null);
  const [analyzingId, setAnalyzingId] = useState<number | null>(null);

  useEffect(() => {
    const loadForestAreas = async () => {
      try {
        const response = await fetch(
          "http://localhost:5113/api/ForestAreas"
        );

        if (!response.ok) {
          throw new Error("Kunde inte hämta skogsområden.");
        }

        const data: ForestArea[] = await response.json();
        setForestAreas(data);
      } catch {
        setError("Kunde inte ansluta till SkogsInsikt API.");
      } finally {
        setLoading(false);
      }
    };

    loadForestAreas();
  }, []);

  const analyzeForestArea = async (id: number) => {
    try {
      setAnalyzingId(id);
      setAnalysis(null);

      const response = await fetch(
        `http://localhost:5113/api/ForestAnalysis/${id}`,
        {
          method: "POST",
        }
      );

      if (!response.ok) {
        throw new Error("Analysen misslyckades.");
      }

      const data: ForestAnalysis = await response.json();
      setAnalysis(data);
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
            <strong>–</strong>
            <span>Områden med förhöjd risk</span>
          </article>
        </section>

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

          {error && (
            <div className="emptyState">
              <h3>{error}</h3>
            </div>
          )}

          {!loading && !error && forestAreas.length === 0 && (
            <div className="emptyState">
              <h3>Inga skogsområden registrerade</h3>
            </div>
          )}

          {!loading && !error && forestAreas.length > 0 && (
            <div className="forestGrid">
              {forestAreas.map((area) => (
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

                  {analysis?.forestAreaId === area.id && (
                    <div className={`analysisResult risk-${analysis.riskLevel.toLowerCase()}`}>
                      <div className="analysisHeader">
                        <span>Aktuell risknivå</span>
                        <strong>{analysis.riskLevel}</strong>
                      </div>

                      <div className="weatherData">
                        <div>
                          <span>Temperatur</span>
                          <strong>{analysis.temperature} °C</strong>
                        </div>

                        <div>
                          <span>Nederbörd</span>
                          <strong>{analysis.precipitation} mm</strong>
                        </div>

                        <div>
                          <span>Vind</span>
                          <strong>{analysis.windSpeed} km/h</strong>
                        </div>
                      </div>

                      <div className="recommendation">
                        <span>Rekommendation</span>
                        <p>{analysis.recommendation}</p>
                      </div>
                    </div>
                  )}
                </article>
              ))}
            </div>
          )}
        </section>
      </main>
    </div>
  );
}

export default App;
