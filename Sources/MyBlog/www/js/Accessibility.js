var Accessibility = (function () {
    function Accessibility() {
    }
    Accessibility.setDefaultFont = function () {
        this.set("/UserSettings/SetDyslexicFont?value=false");
    };
    Accessibility.setDysFont = function () {
        this.set("/UserSettings/SetDyslexicFont?value=true");
    };
    Accessibility.setTextIsJutified = function () {
        this.set("/UserSettings/SetTextIsJutified?value=true");
    };
    Accessibility.setTextIsNotJutified = function () {
        this.set("/UserSettings/SetTextIsJutified?value=false");
    };
    Accessibility.setLineHeight100 = function () {
        this.set("/UserSettings/SetLineHeight?value=0");
    };
    Accessibility.setLineHeight200 = function () {
        this.set("/UserSettings/SetLineHeight?value=1");
    };
    Accessibility.setLineHeight300 = function () {
        this.set("/UserSettings/SetLineHeight?value=2");
    };
    Accessibility.setSetZoom100 = function () {
        this.set("/UserSettings/SetZoom?value=0");
    };
    Accessibility.setSetZoom125 = function () {
        this.set("/UserSettings/SetZoom?value=1");
    };
    Accessibility.setSetZoom150 = function () {
        this.set("/UserSettings/SetZoom?value=2");
    };
    Accessibility.setThemeDefault = function () {
        this.set("/UserSettings/SetTheme?value=0");
    };
    Accessibility.setThemeDark = function () {
        this.set("/UserSettings/SetTheme?value=1");
    };
    Accessibility.setThemeHighContrast = function () {
        this.set("/UserSettings/SetTheme?value=2");
    };
    Accessibility.set = function (url) {
        My.Post(url, undefined, function (e) {
            My.Post("/UserSettings/Style", undefined, function (response) {
                var container = document.getElementById("layoutStyle");
                container.outerHTML = response;
            });
        });
    };
    return Accessibility;
}());
