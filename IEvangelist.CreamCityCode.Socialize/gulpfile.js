/// <binding BeforeBuild='default' />
var gulp = require('gulp'),
    concat = require('gulp-concat'),
    cssmin = require('gulp-cssmin'),
    uglify = require('gulp-uglify'),
    merge = require('merge-stream'),
    del = require('del'),
    bundleconfig = require('./bundleconfig.json');

var regex = {
    css: /\.css$/,
    js: /\.js$/
};

gulp.task('min:js',
    () => merge(getBundles(regex.js).map(bundle => {
        return gulp.src(bundle.inputFiles, { base: '.' })
                   .pipe(concat(bundle.outputFileName))
                   .pipe(uglify())
                   .pipe(gulp.dest('.'));
    })));

gulp.task('min:css',
    () => merge(getBundles(regex.css).map(bundle => {
        return gulp.src(bundle.inputFiles, { base: '.' })
                   .pipe(concat(bundle.outputFileName))
                   .pipe(cssmin())
                   .pipe(gulp.dest('.'));
    })));

gulp.task('clean', () => {
    return del(bundleconfig.map(bundle => bundle.outputFileName));
});

gulp.task('watch', () => {
    getBundles(regex.js).forEach(bundle => gulp.watch(bundle.inputFiles, gulp.series(['min:js'])));
    getBundles(regex.css).forEach(bundle => gulp.watch(bundle.inputFiles, gulp.series(['min:css'])));
});

const getBundles =
    (regexPattern) => bundleconfig.filter(
        bundle => regexPattern.test(bundle.outputFileName));

gulp.task('default', gulp.series(['min:js', 'min:css']));