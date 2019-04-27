/// <reference path="_my.ts" />

// Accessibiliy class to allow users to set accessibilty settings
class Accessibility {
    public static setDefaultFont():void {
        this.set("/UserSettings/SetDyslexicFont?value=false");
    }
    public static setDysFont():void {
        this.set("/UserSettings/SetDyslexicFont?value=true");
    }
    public static setTextIsJutified(): void {
        this.set("/UserSettings/SetTextIsJutified?value=true");
    }
    public static setTextIsNotJutified(): void {
        this.set("/UserSettings/SetTextIsJutified?value=false");
    }
    public static setLineHeight100(): void {
        this.set("/UserSettings/SetLineHeight?value=0");
    }
    public static setLineHeight200(): void {
        this.set("/UserSettings/SetLineHeight?value=1");
    }
    public static setLineHeight300(): void {
        this.set("/UserSettings/SetLineHeight?value=2");
    }
    public static setSetZoom100(): void {
        this.set("/UserSettings/SetZoom?value=0");
    }
    public static setSetZoom125(): void {
        this.set("/UserSettings/SetZoom?value=1");
    }
    public static setSetZoom150(): void {
        this.set("/UserSettings/SetZoom?value=2");
    }
    public static setThemeDefault(): void {
        this.set("/UserSettings/SetTheme?value=0");
    }
    public static setThemeDark(): void {
        this.set("/UserSettings/SetTheme?value=1");
    }
    public static setThemeHighContrast(): void {
        this.set("/UserSettings/SetTheme?value=2");
    }    
    private static set(url: string): void {
        My.Post(url, undefined, (e) => {
            My.Post("/UserSettings/Style", undefined, (response) => {
                // Replace inline style
                let container = document.getElementById("layoutStyle");
                container.outerHTML = response;
            });
        });
    }
}