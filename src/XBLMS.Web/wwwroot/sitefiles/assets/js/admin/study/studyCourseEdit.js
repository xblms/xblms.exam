var $url = '/study/studyCourseEdit';

var data = utils.init({
  id: utils.getQueryInt('id'),
  copyId: utils.getQueryInt('copyId'),
  treeId: utils.getQueryInt('treeId'),
  face: utils.getQueryBoolean('face'),
  tree: null,
  form: null,
  markList: null,
  coursewareList: [],
  coverList: []
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($url, { params: { id: this.id, face: this.face } }).then(function (response) {
      var res = response.data;

      $this.markList = res.markList;
      $this.tree = res.tree;
      $this.form = _.assign({}, res.item);

      if ($this.id > 0) {
        $this.coursewareList = res.wareList;
        if ($this.copyId > 0) {
          $this.id = 0;
          $this.form.id = 0;
          $this.form.name = $this.form.name + "-复制";
        }
      }
      else {
        if ($this.treeId > 0) {
          $this.form.treeId = $this.treeId;
        }
        else {
          $this.form.treeId = null;
        }
      }
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },

  apiSubmit: function () {
    var $this = this;

    utils.loading(this, true);
    $api.post($url, { item: this.form, wareList: this.coursewareList }).then(function (response) {
      var res = response.data;
      if (res.value) {
        utils.success("操作成功");
        utils.closeLayerSelf();
      }
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },

  btnSubmitClick: function () {
    var $this = this;
    if (this.face || this.coursewareList.length > 0) {
      this.$refs.form.validate(function (valid) {
        if (valid) {
          $this.apiSubmit();
        }
      });
    }
    else {
      utils.error('请选择课件', { layer: true });
    }

  },
  btnOpenEditClick: function (ref, ptype) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getCommonUrl('editorOpenLayer', { pf: window.name, ptype: ptype, ref: ref }),
      width: "58%",
      height: "78%"
    });
  },
  uploadBefore(file) {
    var re = /(\.jpg|\.jpeg|\.bmp|\.gif|\.png|\.webp)$/i;
    if (!re.exec(file.name)) {
      utils.error('封面只能是图片格式，请选择有效的文件上传!', { layer: true });
      return false;
    }

    var isLt10M = file.size / 1024 / 1024 < 10;
    if (!isLt10M) {
      utils.error('封面大小不能超过 10MB!', { layer: true });
      return false;
    }
    return true;
  },

  uploadProgress: function () {
    utils.loading(this, true);
  },

  uploadSuccess: function (res, file, fileList) {
    this.form.coverImg = res.value;
    utils.loading(this, false);
    if (fileList.length > 1) fileList.splice(0, 1);
  },

  uploadError: function (err) {
    utils.loading(this, false);
    var error = JSON.parse(err.message);
    utils.error(error.message, { layer: true });
  },

  uploadRemove(file) {
    this.form.coverImg = null;
  },
  btnDeleteTeacherClick: function () {
    var $this = this;
    top.utils.alertWarning({
      title: '提醒',
      text: '确定移除上课老师吗？',
      callback: function () {
        $this.form.teacherId = 0;
        $this.form.teacherName = "";
        $this.form.offlineTeacher = "无";
      }
    });
  },
  btnSelectTeacherClick: function () {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getCommonUrl('selectAdministrators', { pf: window.name }),
      width: "88%",
      height: "98%"
    });
  },
  selectAdminCallback: function (id, name) {
    this.form.teacherId = id;
    this.form.teacherName = name;
    this.form.offlineTeacher = name;
  },

  btnDeleteExamClick: function () {
    var $this = this;
    top.utils.alertWarning({
      title: '提醒',
      text: '确定移除课后考试吗？',
      callback: function () {
        $this.form.examId = 0;
        $this.form.examName = "";
      }
    });
  },
  btnSelectExamClick: function () {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examPaperSelect', { pf: window.name }),
      width: "78%",
      height: "88%"
    });
  },
  selectExamCallback: function (id, name) {
    this.form.examId = id;
    this.form.examName = name;
  },
  btnSelectQClick: function () {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examQuestionnaireSelect', { pf: window.name }),
      width: "78%",
      height: "88%"
    });
  },
  selectQCallback: function (id, name) {
    this.form.examQuestionnaireId = id;
    this.form.examQuestionnaireName = name;
  },
  btnDeleteQClick: function () {
    var $this = this;
    top.utils.alertWarning({
      title: '提醒',
      text: '确定移除课后问卷吗？',
      callback: function () {
        $this.form.examQuestionnaireId = 0;
        $this.form.examQuestionnaireName = "";
      }
    });
  },
  btnSelectEvaluationClick: function () {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getStudyUrl('studyCourseEvaluationSelect', { pf: window.name }),
      width: "78%",
      height: "88%"
    });
  },
  selectEvaluationCallback: function (id, name) {
    this.form.studyCourseEvaluationId = id;
    this.form.studyCourseEvaluationName = name;
  },
  btnDeleteEvaluationClick: function () {
    var $this = this;
    top.utils.alertWarning({
      title: '提醒',
      text: '确定移除课后评价吗？',
      callback: function () {
        $this.form.studyCourseEvaluationId = 0;
        $this.form.studyCourseEvaluationName = "";
      }
    });
  },
  selectCourseware: function () {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getStudyUrl('studyCourseFilesSelect', { pf: window.name, fileType: 'MP4' }),
      width: "68%",
      height: "88%"
    });
  },
  selectFilesCallback: function (selectFiles) {
    if (selectFiles && selectFiles.length > 0) {
      selectFiles.forEach(file => {
        var newFile = { id: 0, courseId: 0, courseFileId: file.id, fileName: file.name, taxis: 1, duration: file.duration };
        this.coursewareList.push(newFile);
        this.form.duration += file.duration;
        this.coverList.push({ cover: file.cover, coverView: file.coverView });
      });
    }
  },
  btnRemoveCourseware: function (ware) {
    var $this = this;
    top.utils.alertWarning({
      title: '删除章节提醒',
      text: '确定删除吗？',
      callback: function () {
        $this.form.duration = 0;
        $this.coursewareList = $this.coursewareList.filter(f => f.courseFileId !== ware.courseFileId);
        if ($this.coursewareList.length > 0) {
          $this.coursewareList.forEach(ware => {
            $this.form.duration += ware.duration;
          })
        }
      }
    });
  }
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.uploadUrl = $apiUrl + $url + '/upload';
    this.apiGet();
  }
});
