var gulp = require('gulp');
var config = require('../config');
var bs = require('browser-sync').get(config.bsName);
var size = require('gulp-size');
var plumber = require('gulp-plumber');
var jshint = require('gulp-jshint');
var chalk = require('chalk');

gulp.task('copyJs', function() {
    gulp.src(['./src/js/markup.js'])
        .pipe(plumber({
          errorHandler: function (error) {
            bs.notify("<strong>FAIL:</strong> COPY JS");
            console.log(chalk.bold.bgRed('COPY JS ERROR: ' + error.message));
            this.emit('end');
        }}))
        .pipe(jshint())
        .pipe(jshint.reporter('jshint-stylish'))
        .pipe(gulp.dest('./build/js'))
        .pipe(size({title: 'copyJs'}));
});