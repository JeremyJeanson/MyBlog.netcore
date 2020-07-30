/// <reference path="_ajax.ts" />
/// <reference path="_dialog.ts" />
/// <reference path="localizations/localization.ts" />

// Accessibiliy class to allow users to set accessibilty settings
namespace Accessibility {
    export function show(title: string): void {
        Dialog.Post(`<i class="fab fa-accessible-icon"></i> ${L10n.Accessibility}`, "/UserSettings/PostAccessibility", null);
    };

    export function setDefaultFont():void {
        set("/UserSettings/SetDyslexicFont?value=false");
    }
    export function setDysFont():void {
        set("/UserSettings/SetDyslexicFont?value=true");
    }
    export function setTextIsJutified(): void {
        set("/UserSettings/SetTextIsJutified?value=true");
    }
    export function setTextIsNotJutified(): void {
        set("/UserSettings/SetTextIsJutified?value=false");
    }
    export function setLineHeight100(): void {
        set("/UserSettings/SetLineHeight?value=0");
    }
    export function setLineHeight200(): void {
        set("/UserSettings/SetLineHeight?value=1");
    }
    export function setLineHeight300(): void {
        set("/UserSettings/SetLineHeight?value=2");
    }
    export function setSetZoom100(): void {
        set("/UserSettings/SetZoom?value=0");
    }
    export function setSetZoom125(): void {
        set("/UserSettings/SetZoom?value=1");
    }
    export function setSetZoom150(): void {
        set("/UserSettings/SetZoom?value=2");
    }
    export function setThemeDefault(): void {
        set("/UserSettings/SetTheme?value=0");
    }
    export function setThemeDark(): void {
        set("/UserSettings/SetTheme?value=1");
    }
    export function setThemeHighContrast(): void {
        set("/UserSettings/SetTheme?value=2");
    }    
    function set(url: string): void {
        Ajax.Post(url, undefined, (e) => {
            Ajax.Post("/UserSettings/Style", undefined, (response) => {
                // Replace inline style
                const container = document.getElementById("layoutStyle");
                container.outerHTML = response;
            });
        });
    }
}