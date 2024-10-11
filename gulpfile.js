const fs = require('fs-extra');
const del = require('del');
const gulp = require('gulp');
const through2 = require('through2');
const minifier = require('gulp-minifier');
const minify = require('gulp-minify');
const rename = require('gulp-rename');
const filter = require('gulp-filter');
const runSequence = require('gulp4-run-sequence');

let os = '';
const version = process.env.PRODUCTVERSION || '8.0';
const timestamp = (new Date()).getTime();
let publishDir = '';
let htmlDict = {};
fs.readdirSync('./src/XBLMS.Web/Pages/shared/').forEach(fileName => {
  let html = fs.readFileSync('./src/XBLMS.Web/Pages/shared/' + fileName, {
    encoding: "utf8",
  });
  htmlDict[fileName] = html;
  htmlDict[fileName.replace('.cshtml', '')] = html;
});

function transform(file, html) {
  let content = new String(file.contents);
  let result = html || '';

  let matches = [...content.matchAll(/@await Html.PartialAsync\("([\s\S]+?)"\)/gi)];
  if (matches) {
    for (let i = 0; i < matches.length; i++) {
      var match = matches[i];
      content = content.replace(match[0], htmlDict[match[1]]);
    }
  }

  let styles = '';
  matches = [...content.matchAll(/<style>([\s\S]+?)<\/style>/gi)];
  if (matches && matches[0]){
    content = content.replace(matches[0][0], '');
    styles = matches[0][0];
  }
  matches = [...content.matchAll(/@section Styles{([\s\S]+?)}/gi)];
  if (matches && matches[0]){
    content = content.replace(matches[0][0], '');
    styles = matches[0][1] + styles;
  }
  let scripts = '';
  matches = [...content.matchAll(/@section Scripts{([\s\S]+?)}/gi)];
  if (matches && matches[0]){
    content = content.replace(matches[0][0], '');
    scripts = matches[0][1];
  }

  result = result.replace('@RenderSection("Styles", required: false)', styles);
  result = result.replace('@RenderBody()', content);
  result = result.replace('@RenderSection("Scripts", required: false)', scripts);
  result = result.replace('@page', '');
  result = result.replace('@{ Layout = "_Layout"; }', '');
  result = result.replace('@{ Layout = "_LayoutHome"; }', '');
  result = result.replace('@{ Layout = "_LayoutApp"; }', '');
  result = result.replace(/\.css"/g, ".css?v=" + timestamp + '"');
  result = result.replace(/\.js"/g, ".js?v=" + timestamp + '"');

  file.contents = Buffer.from(result, 'utf8');
  return file;
}

// build tasks

gulp.task("build-src", function () {
  return gulp.src("./src/**/*").pipe(gulp.dest(`./build-${os}/src`));
});

gulp.task("build-sln", function () {
  return gulp.src("./build.sln").pipe(gulp.dest(`./build-${os}`));
});


gulp.task("build-admin", function () {
  return gulp
    .src("./src/XBLMS.Web/Pages/admin/**/*.cshtml")
    .pipe(through2.obj((file, enc, cb) => {
      cb(null, transform(file, htmlDict['_Layout']))
    }))
    .pipe(rename(function (path) {
      if (path.basename != 'index'){
        path.dirname += "/" + path.basename;
        path.basename = "index";
      }
      path.extname = ".html";
    }))
    .pipe(
      minifier({
        minify: true,
        minifyHTML: {
          collapseWhitespace: true,
          conservativeCollapse: true,
        },
      })
    )
    .pipe(gulp.dest(`./build-${os}/src/XBLMS.Web/wwwroot/admin`));
});

gulp.task("build-app", function () {
  return gulp
    .src("./src/XBLMS.Web/Pages/app/**/*.cshtml")
    .pipe(through2.obj((file, enc, cb) => {
      cb(null, transform(file, htmlDict['_LayoutApp']))
    }))
    .pipe(rename(function (path) {
      if (path.basename != 'index') {
        path.dirname += "/" + path.basename;
        path.basename = "index";
      }
      path.extname = ".html";
    }))
    .pipe(
      minifier({
        minify: true,
        minifyHTML: {
          collapseWhitespace: true,
          conservativeCollapse: true,
        },
      })
    )
    .pipe(gulp.dest(`./build-${os}/src/XBLMS.Web/wwwroot/app`));
});

gulp.task("build-home", function () {
  return gulp
    .src("./src/XBLMS.Web/Pages/home/**/*.cshtml")
    .pipe(through2.obj((file, enc, cb) => {
      cb(null, transform(file, htmlDict['_LayoutHome']))
    }))
    .pipe(rename(function (path) {
      if (path.basename != 'index'){
        path.dirname += "/" + path.basename;
        path.basename = "index";
      }
      path.extname = ".html";
    }))
    .pipe(
      minifier({
        minify: true,
        minifyHTML: {
          collapseWhitespace: true,
          conservativeCollapse: true,
        },
      })
    )
    .pipe(gulp.dest(`./build-${os}/src/XBLMS.Web/wwwroot/home`));
});

gulp.task('build-clean', function(){
  return del([`./build-${os}/src/XBLMS.Web/Pages/admin/**`, `./build-${os}/src/XBLMS.Web/Pages/home/**`, `./build-${os}/src/XBLMS.Web/Pages/app/**`], {force:true});
});

gulp.task("build-linux-x64", async function () {
  os = 'linux-x64';
  return runSequence(
      "build-src",
      "build-sln",
      "build-admin",
      "build-home",
      "build-app",
      "build-clean"
  );
});

gulp.task("build-linux-arm64", async function () {
  os = 'linux-arm64';
  return runSequence(
      "build-src",
      "build-sln",
      "build-admin",
      "build-home",
      "build-app",
      "build-clean"
  );
});

gulp.task("build-win-x64", async function () {
  os = 'win-x64';
  return runSequence(
      "build-src",
      "build-sln",
      "build-admin",
      "build-home",
      "build-app",
      "build-clean"
  );
});

gulp.task("build-win-x86", async function () {
  os = 'win-x86';
  return runSequence(
      "build-src",
      "build-sln",
      "build-admin",
      "build-home",
      "build-app",
      "build-clean"
  );
});

// copy tasks

gulp.task("copy-files", async function () {
  fs.copySync('./appsettings.json', publishDir + '/appsettings.json');
  fs.copySync('./web.config', publishDir + '/web.config');
  fs.removeSync(publishDir + '/appsettings.Development.json');
});

gulp.task("copy-css", function () {
  return gulp
    .src(["./src/XBLMS.Web/wwwroot/sitefiles/**/*.css"])
    .pipe(
      minifier({
        minify: true,
        collapseWhitespace: true,
        conservativeCollapse: true,
        minifyJS: false,
        minifyCSS: true,
        minifyHTML: false,
        ignoreFiles: ['.min.css']
      })
    )
    .pipe(gulp.dest(publishDir + "/wwwroot/sitefiles"));
});

gulp.task("copy-js", function () {
  const f = filter(['**/*-min.js']);
  return gulp
    .src(["./src/XBLMS.Web/wwwroot/sitefiles/**/*.js"])
    .pipe(minify())
    .pipe(f)
    .pipe(rename(function (path) {
      path.basename = path.basename.substring(0, path.basename.length - 4);
    }))
    .pipe(gulp.dest(publishDir + "/wwwroot/sitefiles"));
});

gulp.task("copy-linux-x64", async function (callback) {
  os = 'linux-x64';
  publishDir = `./publish/xblms-${version}-${os}`;

  return runSequence(
    "copy-files",
    "copy-css",
    "copy-js"
  );
});

gulp.task("copy-linux-arm64", async function (callback) {
  os = 'linux-arm64';
  publishDir = `./publish/xblms-${version}-${os}`;

  return runSequence(
    "copy-files",
    "copy-css",
    "copy-js"
  );
});

gulp.task("copy-win-x64", async function (callback) {
  os = 'win-x64';
  publishDir = `./publish/xblms-${version}-${os}`;

  return runSequence(
    "copy-files",
    "copy-css",
    "copy-js"
  );
});

gulp.task("copy-win-x86", async function (callback) {
  os = 'win-x86';
  publishDir = `./publish/xblms-${version}-${os}`;

  return runSequence(
    "copy-files",
    "copy-css",
    "copy-js"
  );
});
