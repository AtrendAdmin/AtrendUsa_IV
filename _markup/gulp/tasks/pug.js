var gulp = require('gulp');
var config = require('../config');
var plumber = require('gulp-plumber');
var pug = require('gulp-pug');
var sourcemaps = require('gulp-sourcemaps');
var size = require('gulp-size');
var bs = require('browser-sync').get(config.bsName);
var chalk = require('chalk');

gulp.task('pug', function() {
  bs.notify("<strong>Pug</strong> is changed");
  gulp.src(['./src/pug/*.pug', '!./src/pug/_*.pug'])
    .pipe(plumber({
      errorHandler: function (error) {
        bs.notify("<strong>FAIL:</strong> Pug");
        console.log(chalk.bold.bgRed('PUG ERROR: ' + error.message));
        this.emit('end');
    }}))
    .pipe(pug({
        pretty: true
    }))
    .pipe(size({title: 'html'}))
    .pipe(gulp.dest('./build/'));
});