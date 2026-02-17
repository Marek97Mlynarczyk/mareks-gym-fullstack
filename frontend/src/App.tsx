import { useEffect, useMemo, useState } from "react";
import type { Exercise } from "./types/exercise";
import { getExercises } from "./api/exercisesApi";

function App() {
  // I store the exercises I fetch from the backend
  const [items, setItems] = useState<Exercise[]>([]);

  // I track total count so I can build paging controls
  const [totalCount, setTotalCount] = useState(0);

  // I store UI query inputs
  const [search, setSearch] = useState("");
  const [muscleGroup, setMuscleGroup] = useState("");
  const [equipment, setEquipment] = useState("");

  // I store paging state
  const [page, setPage] = useState(1);
  const [pageSize] = useState(20);

  // I use this to show a simple loading message while waiting for the API
  const [loading, setLoading] = useState(true);

  // I store possible error message if something fails
  const [error, setError] = useState<string | null>(null);

  
  // I compute total pages from backend totalCount
  const totalPages = useMemo(() => {
    if (totalCount <= 0) return 1;
    return Math.max(1, Math.ceil(totalCount / pageSize));
  }, [totalCount, pageSize]);

  // I decide button enabled/disabled states
  const canGoPrev = page > 1;
  const canGoNext = page < totalPages;

  // I use useEffect so this runs once when the component loads
    useEffect(() => {
    const abortController = new AbortController();

    async function loadExercises() {
      try {
        setError(null);
        setLoading(true);

        const result = await getExercises(
          {
            search,
            muscleGroup,
            equipment,
            page,
            pageSize,
          },
          abortController.signal
        );

        setItems(result.items);
        setTotalCount(result.totalCount);
      } catch (e) {
         if (abortController.signal.aborted) return;

        if (e instanceof Error) {
          setError(e.message);
        } else {
          setError("Unknown error occurred.");
        }
      } finally {
        if (!abortController.signal.aborted) {
          setLoading(false);
        }
      }
    }

    loadExercises();

    // I cancel in-flight requests when the query changes quickly
    return () => {
      abortController.abort();
    };
  }, [search, muscleGroup, equipment, page, pageSize]);

  function handleSearchChange(value: string) {
    setSearch(value);
    setPage(1);
  }

  function handleMuscleGroupChange(value: string) {
    setMuscleGroup(value);
    setPage(1);
  }

  function handleEquipmentChange(value: string) {
    setEquipment(value);
    setPage(1);
  }

  function goPrev() {
    if (!canGoPrev) return;
    setPage((p) => p - 1);
  }

  function goNext() {
    if (!canGoNext) return;
    setPage((p) => p + 1);
  }

  return (
    <div style={{ padding: 24 }}>
      <div style={{ maxWidth: 900, margin: "0 auto" }}>
        <h1 style={{ marginBottom: 16 }}>Exercises</h1>

        {/* I keep filters grouped together so the UI stays predictable */}
        <div
          style={{
            display: "flex",
            gap: 12,
            alignItems: "flex-end",
            flexWrap: "wrap",
            marginBottom: 16,
          }}
        >
          <div style={{ display: "flex", flexDirection: "column", gap: 6 }}>
            <label htmlFor="search">Search</label>
            <input
              id="search"
              value={search}
              onChange={(e) => handleSearchChange(e.target.value)}
              placeholder="e.g. bench, row, squat"
              style={{ padding: 8, width: 260 }}
            />
          </div>

          <div style={{ display: "flex", flexDirection: "column", gap: 6 }}>
            <label htmlFor="muscleGroup">Muscle group</label>
            <input
              id="muscleGroup"
              value={muscleGroup}
              onChange={(e) => handleMuscleGroupChange(e.target.value)}
              placeholder="e.g. Chest"
              style={{ padding: 8, width: 180 }}
            />
          </div>

          <div style={{ display: "flex", flexDirection: "column", gap: 6 }}>
            <label htmlFor="equipment">Equipment</label>
            <input
              id="equipment"
              value={equipment}
              onChange={(e) => handleEquipmentChange(e.target.value)}
              placeholder="e.g. Dumbbell"
              style={{ padding: 8, width: 180 }}
            />
          </div>
        </div>

        {/* I show loading only while I'm waiting for the API */}
        {loading && <p>Loading..</p>}

        {/* I show error only if it exists */}
        {error && <p style={{ color: "red" }}>{error}</p>}

        {/* I show the list only when I'm not loading and I don't have an error */}
        {!loading && !error && (
          <>
            <p style={{ marginBottom: 12 }}>
              Showing page {page} of {totalPages} — Total: {totalCount}
            </p>

            <div style={{ display: "flex", gap: 8, marginBottom: 12 }}>
              <button onClick={goPrev} disabled={!canGoPrev}>
                Prev
              </button>
              <button onClick={goNext} disabled={!canGoNext}>
                Next
              </button>
            </div>

            <ul>
              {items.map((exercise) => (
                <li key={exercise.id}>
                  {exercise.name} — {exercise.muscleGroup ?? "N/A"} —{" "}
                  {exercise.equipment ?? "N/A"}
                </li>
              ))}
            </ul>
          </>
        )}
      </div>
    </div>
  );
}

export default App;