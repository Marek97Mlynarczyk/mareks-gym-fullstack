import { Link, Navigate, Route, Routes } from "react-router-dom";
import ExercisesPage from "./pages/ExercisesPage";
import MacrosPage from "./pages/MacrosPage";

export default function App() {
  return (
    <div style={{ padding: 24 }}>
      <div style={{ maxWidth: 1000, margin: "0 auto" }}>
        <header style={{ marginBottom: 16 }}>
          <h1 style={{ margin: 0 }}>Marek’s Gym Strength & Conditioning</h1>
          <div style={{ opacity: 0.8 }}>
            Exercises library + macro calculator
          </div>
        </header>

        <nav style={{ display: "flex", gap: 12, marginBottom: 24 }}>
          <Link to="/exercises">Exercises</Link>
          <Link to="/macros">Macros</Link>
        </nav>

        <Routes>
          <Route path="/" element={<Navigate to="/exercises" replace />} />
          <Route path="/exercises" element={<ExercisesPage />} />
          <Route path="/macros" element={<MacrosPage />} />
        </Routes>
      </div>
    </div>
  );
}