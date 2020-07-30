/// <reference path="_ajax.ts" />
/// <reference path="_dialog.ts" />
/// <reference path="localizations/localization.ts" />
// Accessibiliy class to allow users to set accessibilty settings
var Accessibility;
(function (Accessibility) {
    function show(title) {
        Dialog.Post("<i class=\"fab fa-accessible-icon\"></i> " + L10n.Accessibility, "/UserSettings/PostAccessibility", null);
    }
    Accessibility.show = show;
    ;
    function setDefaultFont() {
        set("/UserSettings/SetDyslexicFont?value=false");
    }
    Accessibility.setDefaultFont = setDefaultFont;
    function setDysFont() {
        set("/UserSettings/SetDyslexicFont?value=true");
    }
    Accessibility.setDysFont = setDysFont;
    function setTextIsJutified() {
        set("/UserSettings/SetTextIsJutified?value=true");
    }
    Accessibility.setTextIsJutified = setTextIsJutified;
    function setTextIsNotJutified() {
        set("/UserSettings/SetTextIsJutified?value=false");
    }
    Accessibility.setTextIsNotJutified = setTextIsNotJutified;
    function setLineHeight100() {
        set("/UserSettings/SetLineHeight?value=0");
    }
    Accessibility.setLineHeight100 = setLineHeight100;
    function setLineHeight200() {
        set("/UserSettings/SetLineHeight?value=1");
    }
    Accessibility.setLineHeight200 = setLineHeight200;
    function setLineHeight300() {
        set("/UserSettings/SetLineHeight?value=2");
    }
    Accessibility.setLineHeight300 = setLineHeight300;
    function setSetZoom100() {
        set("/UserSettings/SetZoom?value=0");
    }
    Accessibility.setSetZoom100 = setSetZoom100;
    function setSetZoom125() {
        set("/UserSettings/SetZoom?value=1");
    }
    Accessibility.setSetZoom125 = setSetZoom125;
    function setSetZoom150() {
        set("/UserSettings/SetZoom?value=2");
    }
    Accessibility.setSetZoom150 = setSetZoom150;
    function setThemeDefault() {
        set("/UserSettings/SetTheme?value=0");
    }
    Accessibility.setThemeDefault = setThemeDefault;
    function setThemeDark() {
        set("/UserSettings/SetTheme?value=1");
    }
    Accessibility.setThemeDark = setThemeDark;
    function setThemeHighContrast() {
        set("/UserSettings/SetTheme?value=2");
    }
    Accessibility.setThemeHighContrast = setThemeHighContrast;
    function set(url) {
        Ajax.Post(url, undefined, function (e) {
            Ajax.Post("/UserSettings/Style", undefined, function (response) {
                // Replace inline style
                var container = document.getElementById("layoutStyle");
                container.outerHTML = response;
            });
        });
    }
})(Accessibility || (Accessibility = {}));
//# sourceMappingURL=accessibility.js.map