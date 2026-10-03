import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

// Self-contained IIFE bundle (React included) written to this app's own
// directory under the web project. emptyOutDir only clears this app's folder.
export default defineConfig({
  plugins: [react()],
  define: { "process.env.NODE_ENV": JSON.stringify("production") },
  build: {
    outDir: "../../SentryPlayground.Web/static/app-one",
    emptyOutDir: true,
    sourcemap: true,
    cssCodeSplit: false,
    lib: {
      entry: "src/main.tsx",
      formats: ["iife"],
      name: "SentryPlayground_One",
      fileName: () => "app.js",
      cssFileName: "app",
    },
  },
});
