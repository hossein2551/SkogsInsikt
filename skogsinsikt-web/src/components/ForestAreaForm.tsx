import { useState } from "react";

export type ForestAreaFormData = {
  name: string;
  areaHectares: number;
  treeSpecies: string;
  plantingYear: number;
  latitude: number;
  longitude: number;
};

type Props = {
  onSave: (data: ForestAreaFormData) => Promise<void>;
  onCancel: () => void;
  initialData?: ForestAreaFormData;
};

function ForestAreaForm({
  onSave,
  onCancel,
  initialData,
}: Props) {
  const isEditing = Boolean(initialData);

  const [name, setName] = useState(initialData?.name ?? "");
  const [areaHectares, setAreaHectares] = useState(
    initialData?.areaHectares.toString() ?? ""
  );
  const [treeSpecies, setTreeSpecies] = useState(
    initialData?.treeSpecies ?? ""
  );
  const [plantingYear, setPlantingYear] = useState(
    initialData?.plantingYear.toString() ?? ""
  );
  const [latitude, setLatitude] = useState(
    initialData?.latitude.toString() ?? ""
  );
  const [longitude, setLongitude] = useState(
    initialData?.longitude.toString() ?? ""
  );
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
            <p className="eyebrow">
              {isEditing ? "REDIGERA OMRÅDE" : "NYTT OMRÅDE"}
            </p>
            <h2>
              {isEditing
                ? "Redigera skogsområde"
                : "Lägg till skogsområde"}
            </h2>
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
              {saving
                ? "Sparar..."
                : isEditing
                  ? "Spara ändringar"
                  : "Spara skogsområde"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

export default ForestAreaForm;