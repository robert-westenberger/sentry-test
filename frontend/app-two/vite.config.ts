import { createViteConfig } from "@sentry-playground/vite-config";

export default createViteConfig({
  name: "app-two",
  globalName: "SentryPlayground_Two",
  url: import.meta.url,
});
