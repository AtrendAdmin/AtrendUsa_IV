var gulp = require('gulp');
var config = require('../config');
var concat = require('gulp-concat');
var rename = require('gulp-rename');
var uglify = require('gulp-uglify');
var size = require('gulp-size');
var fs = require('fs');
var source = require('../../scripts-list')(config.path.bower);
var chalk = require('chalk');

gulp.task('scripts', function() {
  if(source.length < 1) {
    console.log(chalk.bgRed('You have no scripts added to your list'));
    return false;
  } else {
    console.log(chalk.gray(' ------------------- '));
    console.log(' Scripts in list: ' + chalk.magenta(source.length));
    console.log(chalk.gray(' ------------------- '));

    for(var i=0; i < source.length; i++) {
      if (! fs.existsSync(source[i])) {
        var message = source[i] + ': FILE DOES NOT EXIST';

        console.log('   _____');
        console.log('  /     \\');
        console.log('  vvvvvvv  /|__/|');
        console.log('     I   /O,O   |');
        console.log('     I /_____   |');
        console.log('    J|/^ ^ ^ \\  |');
        console.log('     |^ ^ ^ ^ |W|');
        console.log('      \\m___m__|_|');
        console.log('     ');
        console.log('Error: ' + chalk.bgRed(message));

        throw new Error(message);
      }
    }

    return gulp.src(source)
      .pipe(concat('scripts.js'))
      .pipe(rename({suffix: '.min'}))
      .pipe(uglify({preserveComments: false}))
      .pipe(size({title: 'scripts'}))
      .pipe(gulp.dest('./build/js/'));
  }
});