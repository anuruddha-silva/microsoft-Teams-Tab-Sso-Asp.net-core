const gulp = require('gulp');
const zip = require('gulp-zip');
const del = require('del');

gulp.task('clean', function (done) {
    return del([
        'publish/**/*'
    ], done);
});

gulp.task('generate-manifest', function (done) {
    gulp.src(['Manifest/contoso*', 'Manifest/manifest.json'])
        .pipe(zip('TabSso.zip'))
        .pipe(gulp.dest('TeamsTabPublish'), done);
    done();
});

gulp.task('default', gulp.series('clean', 'generate-manifest'), function (done) {
    console.log('Build completed. Output in `publish` folder');
    done();
});