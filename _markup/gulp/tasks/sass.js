var gulp = require('gulp');
var config = require('../config');
var plumber = require('gulp-plumber');
var sass = require('gulp-sass');
var sourcemaps = require('gulp-sourcemaps');
var autoprefixer = require('gulp-autoprefixer');
var csso = require('gulp-csso');
var size = require('gulp-size');
var bs = require('browser-sync').get(config.bsName);
var chalk = require('chalk');

gulp.task('sass', function () {
    bs.notify("<strong>SASS/SCSS</strong> is changed");
    gulp.src('./src/sass/style.scss')
        .pipe(sourcemaps.init())
        .pipe(plumber({
          errorHandler: function (error) {
            bs.notify("<strong>FAIL:</strong> Sass");
            console.log(chalk.bold.bgRed('SASS ERROR: ' + error.message));
            this.emit('end');
        }}))
        .pipe(sass())
        .pipe(autoprefixer('last 4 versions'))
        .pipe(csso())
        .pipe(size({title: 'styles'}))
        .pipe(gulp.dest('./build/css'))
        .pipe(bs.stream());
});