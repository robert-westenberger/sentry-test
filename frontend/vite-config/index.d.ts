import type { UserConfig } from "vite";

export interface ViteConfigOptions {
  /** Folder name under SentryPlayground.Web/static. */
  name: string;
  /** Global variable name for the IIFE bundle. */
  globalName: string;
  /** The app config file URL (pass import.meta.url). */
  url: string;
}

export function createViteConfig(options: ViteConfigOptions): UserConfig;
