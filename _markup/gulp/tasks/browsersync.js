var gulp = require('gulp');
var config = require('../config');
var bs = require('browser-sync').create(config.bsName);

function browserSync() {
    bs.init(config.browserSync);
};

gulp.task('browserSync', browserSync);

module.exports = browserSync;