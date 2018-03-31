var gulp = require('gulp');
var size = require('gulp-size');

gulp.task('fonts', function() {
    gulp.src(['./bower_components/font-awesome/fonts/**/*.{ttf,woff,eof,svg,otf,woff2}', './src/fonts/**/*.{ttf,eot,woff,woff2}'])
        .pipe(gulp.dest('./build/fonts'))
        .pipe(size({title: 'fonts'}));
});