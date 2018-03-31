var gulp = require('gulp');
var config = require('../config');
var bs = require('browser-sync').get(config.bsName);
var reload = bs.reload;
var chalk = require('chalk');

gulp.task('watch', function() {
    gulp.watch('./src/pug/**/*.pug', ['pug']);

    gulp.watch('./src/sass/**/*.scss', ['sass']);

    gulp.watch('./src/sprite/**/*.png', ['sprite', 'sass']);

    gulp.watch('src/images/**.*', ['images']);

    gulp.watch('./scripts.js', ['scripts']);

    gulp.watch('./build/*.html').on('change', reload);

    gulp.watch('./build/js/*.js').on('change', reload);

    gulp.watch('./scripts-list.js', function() {
        bs.notify("<strong>Scripts</strong> has been updated. You need to run gulp again.");
        console.log(chalk.bold.green('Scripts has been updated.'));
        console.log(chalk.bold.red('You need to rerun gulp.'));
    });

    gulp.watch('src/js/markup.js', ['copyJs']);

    gulp.watch('build/js/*.js').on('change', reload);
});