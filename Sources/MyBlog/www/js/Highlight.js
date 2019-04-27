// Configure
var hljs = require("highlight.js/lib/highlight.js");
hljs.registerLanguage("cs", require("highlight.js/lib/languages/cs"));
hljs.registerLanguage("cpp", require("highlight.js/lib/languages/cpp"));
hljs.registerLanguage("css", require("highlight.js/lib/languages/css"));
hljs.registerLanguage("http", require("highlight.js/lib/languages/http"));
hljs.registerLanguage("javascript", require("highlight.js/lib/languages/javascript"));
hljs.registerLanguage("json", require("highlight.js/lib/languages/json"));
hljs.registerLanguage("powershell", require("highlight.js/lib/languages/powershell"));
hljs.registerLanguage("scss", require("highlight.js/lib/languages/scss"));
hljs.registerLanguage("xml", require("highlight.js/lib/languages/xml"));

// highlight on load of page
hljs.initHighlightingOnLoad();