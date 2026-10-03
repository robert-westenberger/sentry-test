import { createViteConfig } from "@sentry-playground/vite-config";

export default createViteConfig({
  name: "app-one",
  globalName: "SentryPlayground_One",
  url: import.meta.url,
});
