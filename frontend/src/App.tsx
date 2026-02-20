import { useEffect, useMemo, useState } from "react";
import type { Exercise } from "./types/exercise";
import { createExercise, deleteExercise, getExercises } from "./api/exercisesApi";
import { useDebouncedValue } from "./hooks/useDebouncedValue";

function App() {
  // I store the exercises I fetch from the backend
  const [items, setItems] = useState<Exercise[]>([]);

  // I track total count so I can build paging controls
  const [totalCount, setTotalCount] = useState(0);
  
  // I store UI query inputs
  const [search, setSearch] = useState("");
  const [muscleGroup, setMuscleGroup] = useState("");
  const [equipment, setEquipment] = useState("");
  
  // I debounce search so I do not hit the API on every keypress
  const debouncedSearch = useDebouncedValue(search, 400);

  // I store paging state
  const [page, setPage] = useState(1);
  const [pageSize] = useState(20);

  // I use this to show a simple loading message while waiting for the API
  const [loading, setLoading] = useState(true);

  // I store possible error message if something fails
  const [error, setError] = useState<string | null>(null);

  // I use this to force a reload after create/delete without changing filters
  const [refreshKey, setRefreshKey] = useState(0);

  // I compute total pages from backend totalCount
  const totalPages = useMemo(() => {
    if (totalCount <= 0) return 1;
    return Math.max(1, Math.ceil(totalCount / pageSize));
  }, [totalCount, pageSize]);

  // I decide button enabled/disabled states
  const canGoPrev = page > 1;
  const canGoNext = page < totalPages;

  // I store create form inputs
  const [newName, setNewName] = useState("");
  const [newDescription, setNewDescription] = useState("");
  const [newMuscleGroup, setNewMuscleGroup] = useState("");
  const [newEquipment, setNewEquipment] = useState("");

  // I store backend validation errors from create
  const [createErrors, setCreateErrors] = useState<Record<string, string[]> | null>(null);

  // I track create request state so I can disable the button
  const [creating, setCreating] = useState(false);

  // I create a new exercise using the API and refresh the list
  async function handleCreate() {
  try {
    setCreateErrors(null);
    setCreating(true);

    await createExercise({
      name: newName,
      description: newDescription.trim().length > 0 ? newDescription : null,
      muscleGroup: newMuscleGroup.trim().length > 0 ? newMuscleGroup : null,
      equipment: newEquipment.trim().length > 0 ? newEquipment : null,
    });
    
    // I clear errors after successful create
    setCreateErrors(null);

    // I reset the form after successful create
    setNewName("");
    setNewDescription("");
    setNewMuscleGroup("");
    setNewEquipment("");

    setRefreshKey((k) => k + 1);

  } catch (err) {
    const maybeErr = err as any;
    if (maybeErr?.errors) {
      setCreateErrors(maybeErr.errors as Record<string, string[]>);
    } else if (maybeErr instanceof Error) {
      setCreateErrors({ general: [maybeErr.message] });
    } else {
      setCreateErrors({ general: ["Unknown error occurred."] });
    }
  } finally {
    setCreating(false);
  }
}

  // I delete an exercise and refresh the list
  async function handleDelete(id: number) {
    const confirmed = window.confirm("Do you want to delete this exercise?");
    if (!confirmed) return;

    try {
      await deleteExercise(id);

      // I refresh the list after delete
      setRefreshKey((k) => k + 1);
    } catch (e) {
      if (e instanceof Error) {
        alert(e.message);
      } else {
        alert("Unknown error occurred.");
      }
    }
  }

    // I fetch exercises when query or paging changes
    useEffect(() => {
    const abortController = new AbortController();

    async function loadExercises() {
      try {
        setError(null);
        setLoading(true);

        const result = await getExercises(
          {
            search: debouncedSearch,
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
  }, [debouncedSearch, muscleGroup, equipment, page, pageSize, refreshKey]);

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

        <h2 style={{ marginBottom: 12 }}>Create Exercise</h2>

<div
  style={{
    display: "flex",
    gap: 12,
    flexWrap: "wrap",
    alignItems: "flex-end",
    marginBottom: 12,
  }}
>
  <div style={{ display: "flex", flexDirection: "column", gap: 6 }}>
    <label htmlFor="newName">Name</label>
    <input
      id="newName"
      value={newName}
      onChange={(e) => setNewName(e.target.value)}
      placeholder="e.g. Barbell Bench Press"
      style={{ padding: 8, width: 260 }}
    />
  </div>

  <div style={{ display: "flex", flexDirection: "column", gap: 6 }}>
    <label htmlFor="newMuscleGroup">Muscle group</label>
    <input
      id="newMuscleGroup"
      value={newMuscleGroup}
      onChange={(e) => setNewMuscleGroup(e.target.value)}
      placeholder="e.g. Chest"
      style={{ padding: 8, width: 180 }}
    />
  </div>

  <div style={{ display: "flex", flexDirection: "column", gap: 6 }}>
    <label htmlFor="newEquipment">Equipment</label>
    <input
      id="newEquipment"
      value={newEquipment}
      onChange={(e) => setNewEquipment(e.target.value)}
      placeholder="e.g. Barbell"
      style={{ padding: 8, width: 180 }}
    />
  </div>

  <div style={{ display: "flex", flexDirection: "column", gap: 6 }}>
    <label htmlFor="newDescription">Description</label>
    <input
      id="newDescription"
      value={newDescription}
      onChange={(e) => setNewDescription(e.target.value)}
      placeholder="optional"
      style={{ padding: 8, width: 260 }}
    />
  </div>

  <button onClick={handleCreate} disabled={creating || newName.trim().length === 0}>
    {creating ? "Creating..." : "Create"}
  </button>
</div>

{createErrors && (
  <div style={{ color: "red", marginBottom: 16 }}>
    {Object.entries(createErrors).map(([field, messages]) => (
      <div key={field}>
        <strong>{field}</strong>: {messages.join(", ")}
      </div>
    ))}
  </div>
)}


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
                  {exercise.name} - {exercise.muscleGroup ?? "N/A"} - {exercise.equipment ?? "N/A"}
                   <button onClick={() => handleDelete(exercise.id)} disabled={loading} style={{ marginLeft: 8 }}>
                    Delete
                    </button>
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