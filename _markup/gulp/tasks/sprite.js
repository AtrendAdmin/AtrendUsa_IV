var gulp = require('gulp');
var config = require('../config');
var plumber = require('gulp-plumber');
var spritesmith = require('gulp.spritesmith');
var imagemin = require('gulp-imagemin');
var merge = require('merge-stream');
var buffer = require('vinyl-buffer');
var bs = require('browser-sync').get(config.bsName);
var chalk = require('chalk');
var svgSprite = require('gulp-svg-sprites');

// Png Sprites

gulp.task('sprite', function () {
    var spriteData = gulp.src('./src/sprite/*.png').pipe(plumber({
          errorHandler: function (error) {
            bs.notify("<strong>FAIL:</strong> Sprite");
            console.log(chalk.bold.bgRed('SPRITE ERROR: ' + error.message));
            this.emit('end');
        }}))
        .pipe(spritesmith({
            imgName: 'sprite.png',
            retinaImgName: 'sprite@2x.png',
            retinaSrcFilter: ['./src/sprite/*@2x.png'],
            cssName: '_sprite.scss',
            padding: 2,
            imgPath: '../images/sprite/sprite.png',
            retinaImgPath: '../images/sprite/sprite@2x.png'
        }));

    var imgStream = spriteData.img
        .pipe(buffer())
        .pipe(imagemin())
        .pipe(gulp.dest('./build/images/sprite'));

    var cssStream = spriteData.css
        .pipe(gulp.dest('./src/sass/'));

    return merge(imgStream, cssStream);
});

// Svg Sprites

gulp.task('svgSprites', function () {
    return gulp.src('./src/images/svg/*.svg')
        .pipe(svgSprite({
            padding: 10,
            cssFile: '../../src/sass/_svgSprite.scss',
            svgPath: '../images/svg/sprite.svg',
            preview: false
        }))
        .pipe(gulp.dest('./build/images/'))
});