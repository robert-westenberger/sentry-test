<%@ Page Language="C#" %>
<!DOCTYPE html>
<html>
<head>
  <meta charset="utf-8" />
  <title>App One</title>
  <link rel="stylesheet" href="static/app-one/app.css" />
</head>
<body>
  <h1>App One</h1>
  <p><a href="app-two.aspx">Go to app-two</a> | <a href="TestErrorHandler.ashx">Backend test error</a></p>
  <div id="root"></div>
  <%= SentryPlayground.AppConfig.FrontendConfigScript() %>
  <script src="static/app-one/app.js"></script>
</body>
</html>
