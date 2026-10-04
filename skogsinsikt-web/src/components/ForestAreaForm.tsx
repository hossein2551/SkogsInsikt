import { useState } from "react";

export type CreateForestAreaData = {
  name: string;
  areaHectares: number;
  treeSpecies: string;
  plantingYear: number;
  latitude: number;
  longitude: number;
};

type Props = {
  onSave: (data: CreateForestAreaData) => Promise<void>;
  onCancel: () => void;
};

function ForestAreaForm({ onSave, onCancel }: Props) {
  const [name, setName] = useState("");
  const [areaHectares, setAreaHectares] = useState("");
  const [treeSpecies, setTreeSpecies] = useState("");
  const [plantingYear, setPlantingYear] = useState("");
  const [latitude, setLatitude] = useState("");
  const [longitude, setLongitude] = useState("");
  const [saving, setSaving] = useState(false);

  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();

    try {
      setSaving(true);

      await onSave({
        name,
        areaHectares: Number(areaHectares),
        treeSpecies,
        plantingYear: Number(plantingYear),
        latitude: Number(latitude),
        longitude: Number(longitude),
      });
    } finally {
      setSaving(false);
    }
  };

  return (
    <div className="formOverlay">
      <div className="formModal">
        <div className="formHeader">
          <div>
            <p className="eyebrow">NYTT OMRÅDE</p>
            <h2>Lägg till skogsområde</h2>
          </div>

          <button
            type="button"
            className="closeButton"
            onClick={onCancel}
          >
            ×
          </button>
        </div>

        <form onSubmit={handleSubmit}>
          <label>
            Namn
            <input
              required
              minLength={2}
              maxLength={100}
              value={name}
              onChange={(e) => setName(e.target.value)}
              placeholder="Exempel: Norra skogen"
            />
          </label>

          <div className="formRow">
            <label>
              Areal (hektar)
              <input
                required
                type="number"
                min="0.1"
                step="0.1"
                value={areaHectares}
                onChange={(e) => setAreaHectares(e.target.value)}
              />
            </label>

            <label>
              Trädslag
              <input
                required
                value={treeSpecies}
                onChange={(e) => setTreeSpecies(e.target.value)}
                placeholder="Exempel: Gran"
              />
            </label>
          </div>

          <label>
            Planteringsår
            <input
              required
              type="number"
              min="1800"
              max="2100"
              value={plantingYear}
              onChange={(e) => setPlantingYear(e.target.value)}
            />
          </label>

          <div className="formRow">
            <label>
              Latitud
              <input
                required
                type="number"
                min="-90"
                max="90"
                step="any"
                value={latitude}
                onChange={(e) => setLatitude(e.target.value)}
              />
            </label>

            <label>
              Longitud
              <input
                required
                type="number"
                min="-180"
                max="180"
                step="any"
                value={longitude}
                onChange={(e) => setLongitude(e.target.value)}
              />
            </label>
          </div>

          <div className="formActions">
            <button
              type="button"
              className="cancelButton"
              onClick={onCancel}
            >
              Avbryt
            </button>

            <button
              type="submit"
              className="saveButton"
              disabled={saving}
            >
              {saving ? "Sparar..." : "Spara skogsområde"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

export default ForestAreaForm;