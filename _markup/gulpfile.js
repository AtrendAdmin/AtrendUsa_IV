'use strict';

var gulp        = require('gulp');
var runSequence = require('run-sequence');
var requireDir  = require('require-dir');

requireDir('./gulp/tasks', {
    recurse: true
});

gulp.task('build', [ // Build task
    'pug',
    'sass',
    // 'svgSprites',
    'scripts',
    'fonts',
    'copyJs',
    'images'
]);

gulp.task('dev', function() { // Dev task with watch
    runSequence(
        'browserSync',
        'build',
        'watch'
    )
});

gulp.task('default', ['build']);