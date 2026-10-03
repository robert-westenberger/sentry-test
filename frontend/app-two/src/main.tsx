import * as Sentry from "@sentry/react";
import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { App } from "./App";
import "./styles.css";

const APP_NAME = "app-two";

// Runtime config is written into the ASPX page from web.config appSettings.
// With no DSN, Sentry stays disabled and the app still runs.
const config = window.__SENTRY_CONFIG__ ?? {};

Sentry.init({
  dsn: config.dsn || undefined,
  // Events go through the backend tunnel instead of straight to Sentry.
  tunnel: "tunnel.ashx",
  release: __SENTRY_RELEASE__ ?? undefined,
  environment: config.environment || undefined,
  // @sentry/react v11 replaced sendDefaultPii with dataCollection; turn everything off explicitly.
  dataCollection: { userInfo: false, cookies: false, httpHeaders: false, httpBodies: [], urlQueryParams: false },
  initialScope: { tags: { app: APP_NAME } },
});

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <Sentry.ErrorBoundary fallback={<p>Something went wrong (reported to Sentry if a DSN is set).</p>}>
      <App />
    </Sentry.ErrorBoundary>
  </StrictMode>,
);
