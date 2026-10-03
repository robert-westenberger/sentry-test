import { dirname as dirOf, resolve } from "node:path";
import { fileURLToPath } from "node:url";
import react from "@vitejs/plugin-react";
import { defineConfig } from "vite";

/**
 * Shared Vite config for the React apps: a self-contained IIFE bundle (React
 * included) written to the app's own directory under the web project.
 * emptyOutDir only clears that app's folder.
 *
 * @param {{ name: string, globalName: string, url: string }} options
 *   name: folder under SentryPlayground.Web/static; globalName: IIFE global;
 *   url: the app config file URL (pass import.meta.url).
 */
export function createViteConfig({ name, globalName, url }) {
  const dirname = dirOf(fileURLToPath(url));
  return defineConfig({
    root: dirname,
    plugins: [react()],
    define: { "process.env.NODE_ENV": JSON.stringify("production") },
    build: {
      outDir: resolve(dirname, "../../SentryPlayground.Web/static", name),
      emptyOutDir: true,
      sourcemap: true,
      cssCodeSplit: false,
      lib: {
        entry: "src/main.tsx",
        formats: ["iife"],
        name: globalName,
        fileName: () => "app.js",
        cssFileName: "app",
      },
    },
  });
}
