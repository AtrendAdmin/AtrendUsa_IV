var gulp = require('gulp');
var fs  = require('fs');
var config = require('../config');
var moment = require('moment');
var zip = require('gulp-zip');
var size = require('gulp-size');
var pkg = JSON.parse(fs.readFileSync('package.json'));
var filename = pkg.name + '_build_' + moment().format('MM.DD.YYYY-h.mm.ss.A');
var plumber = require('gulp-plumber');
var chalk = require('chalk');

gulp.task('zip', ['build'], function() {
  gulp.src('./build/*')
    .pipe(zip(filename + '.zip'))
    .on('error', function(err) {
        console.log(chalk.bold.bgRed('ZIP ERROR: ' + err));
    })
    .pipe(size({title: 'zip'}))
    .pipe(gulp.dest('./archive/'));
});