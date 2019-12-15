// Configure
var prism = require("prismjs/prism.js");

// Add languages
require("prismjs/components/prism-clike"); // Extended by csharp, c, javascript
require("prismjs/components/prism-csharp");
require("prismjs/components/prism-c"); //Extended by cpp
require("prismjs/components/prism-cpp");
require("prismjs/components/prism-css"); // Extended by scss
require("prismjs/components/prism-javascript");
require("prismjs/components/prism-json");
require("prismjs/components/prism-markup");
require("prismjs/components/prism-powershell");
require("prismjs/components/prism-scss");
require("prismjs/components/prism-sql");
require("prismjs/components/prism-yaml");

// Apply
prism.highlightAll();