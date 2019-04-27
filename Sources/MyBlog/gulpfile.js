/// <binding AfterBuild='default' />
"use strict";

var gulp = require("gulp");
var concat = require("gulp-concat");
var uglify = require("gulp-uglify");
var stylemin = require("gulp-cssmin");
var sass = require('gulp-sass');

var browserify = require("browserify");
var source = require("vinyl-source-stream");
var streamify = require("gulp-streamify");
var rename = require("gulp-rename");

var paths = {
    npm: "./node_modules/",
    sources: "./www/",
    www: "./wwwroot/",
    obj: "./obj/"
};
/* Scripts */
paths.scriptCommon = paths.www + "js/common.js";
paths.scriptLayout = paths.www + "js/layout.js";
paths.scriptPost = paths.www + "js/post.js";
paths.scriptPostDetails = paths.www + "js/post-details.js";
paths.scriptAccountEdit = paths.www + "js/account-edit.js";

/* Styles */
paths.styleDefault = paths.www + "css/default.css";
paths.styleDark = paths.www + "css/dark.css";
paths.styleHighContrast = paths.www + "css/high-contrast.css";

/* Fonts */
paths.fonts = paths.www + "fonts/";


/* Styles */
gulp.task("min-styles", function () {
    // dark.css
    gulp.src([
        paths.sources + "css/dark.scss",
        paths.npm + "highlight.js/styles/vs2015.css"])
        .pipe(sass().on('error', sass.logError))
        .pipe(concat(paths.styleDark))
        .pipe(stylemin())
        .pipe(gulp.dest("."));

    // High contract.css
    gulp.src([
        paths.sources + "css/high-contrast.scss",
        paths.npm + "highlight.js/styles/vs2015.css"])
        .pipe(sass().on('error', sass.logError))
        .pipe(concat(paths.styleHighContrast))
        .pipe(stylemin())
        .pipe(gulp.dest("."));

    // default.css
    return gulp.src([
        paths.sources + "css/default.scss",
        paths.npm + "highlight.js/styles/vs.css"])
        .pipe(sass().on('error', sass.logError))
        .pipe(concat(paths.styleDefault))
        .pipe(stylemin())
        .pipe(gulp.dest("."));
});

/* Scripts */
gulp.task("min-scripts", function () {
    // Default
    gulp.src([
        paths.npm + "jquery/dist/jquery.js",
        paths.npm + "bootstrap/dist/js/bootstrap.bundle.js",
        paths.sources + "js/_my.js"])
        .pipe(concat(paths.scriptCommon))
        .pipe(uglify())
        .pipe(gulp.dest("."));

    // Layout
    gulp.src([
        paths.sources + "js/App.js",
        paths.sources + "js/Get-More-Items.js",
        paths.sources + "js/Accessibility.js"])
        .pipe(concat(paths.scriptLayout))
        .pipe(uglify())
        .pipe(gulp.dest("."));

    // Post         .plugin(tsify)
    browserify(paths.sources + "js/Highlight.js")
        .bundle()
        .pipe(source("Post.js"))
        .pipe(streamify(uglify()))
        .pipe(rename(paths.scriptPost))
        .pipe(gulp.dest("."));

    // Post-Details
    gulp.src([
        paths.sources + "js/Post-Details.js"])
        .pipe(concat(paths.scriptPostDetails))
        .pipe(uglify())
        .pipe(gulp.dest("."));

    // Account edit
    return gulp.src([
        paths.sources + "js/Account-Edit.js"])
        .pipe(concat(paths.scriptAccountEdit))
        .pipe(uglify())
        .pipe(gulp.dest("."));
});

/* Copie des fonts */
gulp.task("copy-fonts", function () {
    return gulp.src([
        paths.sources + "fonts/*",
        paths.npm + "@fortawesome/fontawesome-free/webfonts/*"])
        .pipe(gulp.dest(paths.fonts));
});

gulp.task("default", gulp.series("min-styles", "min-scripts", "copy-fonts"));