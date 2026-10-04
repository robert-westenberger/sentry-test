import { useState } from "react";

function Thrower(): never {
  throw new Error("Synthetic frontend test error (app-two)");
}

export function App() {
  const [broken, setBroken] = useState(false);
  if (broken) return <Thrower />;
  return (
    <div>
      <h2>Hello World from app-two (pipeline test)</h2>
      <button onClick={() => setBroken(true)}>Throw test error</button>
    </div>
  );
}
