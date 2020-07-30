/// <binding AfterBuild='build' Clean='clean' ProjectOpened='watch' />
"use strict";

// Common 
const { src, dest, parallel, watch, series } = require("gulp");
const del = require("del");
const concat = require("gulp-concat");

// Plugin for scripts
var terser = require("gulp-terser");
const browserify = require("browserify");
const source = require("vinyl-source-stream");
const streamify = require("gulp-streamify");

// Plugin for RESX files
const resx_out = require("gulp-resx-out");
const ext_replace = require('gulp-ext-replace');

// Plugins for styles
const sass = require('gulp-sass');
const cleanCss = require('gulp-clean-css');
// Clean css options
const cleanCssOptions = {
    level: {
        1: { specialComments: 0 },
        2: {}
    }
};

// Paths
const paths = {
    npm: "./node_modules/",
    input: "./_www/",
    outputJs: "./wwwroot/js",
    outputCss: "./wwwroot/css",
    outputFonts: "./wwwroot/fonts"
};

/*--------------------------------------------------------------------------------------------------------------------------------------------
 * Resx files
 * --------------------------------------------------------------------------------------------------------------------------------------------*/
function convertResx() {
    //return string that should be written to file
    function onwrite(result, file) {
        return `var L10n = ${JSON.stringify(result, null, "\t")};`;
    }

    function onparse(item, element, result, file) {
        return {
            name: item.name,
            value: item.value
        };
    }

    return src(paths.input + "js/Localizations/*.resx")
        .pipe(resx_out({
            delimiter: '.',
            onwrite: onwrite,
            onparse: onparse
        }))
        .pipe(ext_replace(".ts"))
        .pipe(dest(paths.input + "js/Localizations/"));
}

function copyLocalizations() {
    return src(paths.input + "js/Localizations/*.ts")
        .pipe(ext_replace(".js"))
        .pipe(terser())
        .pipe(dest(paths.outputJs));
}

exports.buildResx = series(convertResx, copyLocalizations);


/*--------------------------------------------------------------------------------------------------------------------------------------------
 * Scripts
 * --------------------------------------------------------------------------------------------------------------------------------------------*/
// common.js
function jsCommon() {
    return src([
        paths.npm + "jquery/dist/jquery.js",
        paths.npm + "bootstrap/dist/js/bootstrap.bundle.js",
        paths.npm + "clipboard/dist/clipboard.min.js",
        paths.input + "js/_ajax.js",
        paths.input + "js/_dialog.js",
        paths.input + "js/_my.js"])
        .pipe(concat("common.js"))
        .pipe(terser())
        .pipe(dest(paths.outputJs));
}

// Layout
function jsLayout() {
    return src([
        paths.input + "js/App.js",
        paths.input + "js/Get-More-Items.js",
        paths.input + "js/Accessibility.js"])
        .pipe(concat("layout.js"))
        .pipe(terser())
        .pipe(dest(paths.outputJs));
}

// Post
function jsPost() {
    return browserify([
        paths.input + "js/Prism.js",
        paths.input + "js/Prism-Clipboard.js"
    ])
        .bundle()
        .pipe(source("post.js"))
        .pipe(streamify(terser()))
        .pipe(dest(paths.outputJs));
}

// Post-Details
function jsPostDetail() {
    return src([
        paths.input + "js/Post-Details.js"])
        .pipe(concat("post-details.js"))
        .pipe(terser())
        .pipe(dest(paths.outputJs));
}

// Account edit
function jsAccountEdit() {
    return src([
        paths.input + "js/Account-Edit.js"])
        .pipe(concat("account-edit.js"))
        .pipe(terser())
        .pipe(dest(paths.outputJs));
}

exports.buildScripts = parallel(jsCommon, jsLayout, jsPost, jsPostDetail, jsAccountEdit);

/*--------------------------------------------------------------------------------------------------------------------------------------------
 * Styles
 * --------------------------------------------------------------------------------------------------------------------------------------------*/
// dark.css
function cssDark() {
    return src([
        paths.npm + "prismjs-vs/scss/prism-vs-dark.scss",
        paths.input + "css/dark.scss"])
        .pipe(sass().on('error', sass.logError))
        .pipe(concat("dark.css"))
        .pipe(cleanCss(cleanCssOptions))
        .pipe(dest(paths.outputCss));
}

// High contract.css
function cssHighContrast() {
    return src([
        paths.npm + "prismjs-vs/scss/prism-vs-dark.scss",
        paths.input + "css/high-contrast.scss"])
        .pipe(sass().on('error', sass.logError))
        .pipe(concat("high-contrast.css"))
        .pipe(cleanCss(cleanCssOptions))
        .pipe(dest(paths.outputCss));
}

// default.css
function cssDefault() {
    return src([
        paths.npm + "prismjs-vs/scss/prism-vs-light.scss",
        paths.input + "css/default.scss"])
        .pipe(sass().on('error', sass.logError))
        .pipe(concat("default.css"))
        .pipe(cleanCss(cleanCssOptions))
        .pipe(dest(paths.outputCss));
}

// Build all CSS
exports.buildCss = parallel(cssDark, cssDefault, cssHighContrast);

/*-------------------------------------------------------------------------------------------------------------------------------------------- 
 * Fonts 
 * --------------------------------------------------------------------------------------------------------------------------------------------*/
function buildFonts() {
    return src([
        paths.input + "fonts/*",
        paths.npm + "@fortawesome/fontawesome-free/webfonts/*"])
        .pipe(dest(paths.outputFonts));
}

exports.buildFonts = buildFonts;

/*--------------------------------------------------------------------------------------------------------------------------------------------
 * Dev tasks
 * --------------------------------------------------------------------------------------------------------------------------------------------*/

// Watch
exports.watch = function () {
    watch(paths.input + "js/Localizations/*.resx", function (cb) {
        exports.buildResx();
        cb();
    });
    watch(paths.input + "js/*.js", function (cb) {
        exports.buildScripts();
        cb();
    });
    watch(paths.input + "css/*", function (cb) {
        exports.buildCss();
        cb();
    });
};

// Build
exports.build = parallel(
    series(exports.buildResx, exports.buildScripts),
    exports.buildCss,
    exports.buildFonts);

// Clean
exports.clean = function () {
    return del([
        paths.outputCss,
        paths.outputJs,
        paths.outputFonts
    ]);
};