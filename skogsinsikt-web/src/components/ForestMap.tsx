import {
  MapContainer,
  Marker,
  Popup,
  TileLayer,
} from "react-leaflet";
import L from "leaflet";
import "leaflet/dist/leaflet.css";

type ForestArea = {
  id: number;
  name: string;
  areaHectares: number;
  treeSpecies: string;
  plantingYear: number;
  latitude: number;
  longitude: number;
};

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

type Props = {
  forestAreas: ForestArea[];
  analyses: ForestAnalysis[];
};

const createMarkerIcon = (riskLevel?: string) => {
  const riskClass =
    riskLevel === "High"
      ? "mapMarkerHigh"
      : riskLevel === "Medium"
        ? "mapMarkerMedium"
        : "mapMarkerLow";

  return L.divIcon({
    className: "",
    html: `<div class="mapMarker ${riskClass}"></div>`,
    iconSize: [22, 22],
    iconAnchor: [11, 11],
  });
};

function ForestMap({ forestAreas, analyses }: Props) {
  if (forestAreas.length === 0) {
    return (
      <div className="mapEmpty">
        Lägg till ett skogsområde för att visa det på kartan.
      </div>
    );
  }

  const latestAnalysis = (forestAreaId: number) =>
    analyses
      .filter(
        (analysis) =>
          analysis.forestAreaId === forestAreaId
      )
      .sort(
        (a, b) =>
          new Date(b.createdAt).getTime() -
          new Date(a.createdAt).getTime()
      )[0];

  const firstArea = forestAreas[0];

  return (
    <MapContainer
      center={[
        firstArea.latitude,
        firstArea.longitude,
      ]}
      zoom={11}
      scrollWheelZoom={true}
      className="forestMap"
    >
      <TileLayer
        attribution="&copy; OpenStreetMap contributors"
        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
      />

      {forestAreas.map((area) => {
        const analysis = latestAnalysis(area.id);

        return (
          <Marker
            key={area.id}
            position={[
              area.latitude,
              area.longitude,
            ]}
            icon={createMarkerIcon(
              analysis?.riskLevel
            )}
          >
            <Popup>
              <div className="mapPopup">
                <strong>{area.name}</strong>

                <span>
                  {area.areaHectares} ha ·{" "}
                  {area.treeSpecies}
                </span>

                {analysis ? (
                  <>
                    <span>
                      Risknivå:{" "}
                      <b>{analysis.riskLevel}</b>
                    </span>

                    <span>
                      Vind: {analysis.windSpeed} km/h
                    </span>
                  </>
                ) : (
                  <span>Ingen analys ännu</span>
                )}
              </div>
            </Popup>
          </Marker>
        );
      })}
    </MapContainer>
  );
}

export default ForestMap;