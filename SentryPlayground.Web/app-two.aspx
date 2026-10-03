<%@ Page Language="C#" %>
<!DOCTYPE html>
<html>
<head>
  <meta charset="utf-8" />
  <title>App Two</title>
  <link rel="stylesheet" href="static/app-two/app.css" />
</head>
<body>
  <h1>App Two</h1>
  <p><a href="app-one.aspx">Go to app-one</a> | <a href="TestErrorHandler.ashx">Backend test error</a></p>
  <div id="root"></div>
  <%= SentryPlayground.AppConfig.FrontendConfigScript() %>
  <script src="static/app-two/app.js"></script>
</body>
</html>
