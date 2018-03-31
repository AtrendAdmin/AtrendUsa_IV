var gulp = require('gulp');
var config = require('../config');
var plumber = require('gulp-plumber');
var imagemin = require('gulp-imagemin');
var cache = require('gulp-cache');
var size = require('gulp-size');
var bs = require('browser-sync').get(config.bsName);
var chalk = require('chalk');

gulp.task('images', function() {
  bs.notify("Processing images...");
  console.log(chalk.bold.bgBlue('Processing images...'));
  // gulp.src(['./src/images/**/*', '!./src/images/svg/*.svg'])
  gulp.src('./src/images/**/*')
    .pipe(cache(imagemin({
        optimizationLevel: 3,
        progressive: true,
        interlaced: true
    })))
    .pipe(gulp.dest('./build/images'))
    .pipe(size({title: 'images'}));
});