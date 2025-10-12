var $url = '/study/studyPlanEdit';

var data = utils.init({
  id: utils.getQueryInt('id'),
  copyId: utils.getQueryInt('copyId'),
  form: null,
  courseList: [],
  courseSelectList: [],
  userGroupList: [],
  curGuid: '',
  curIsSelect: false,
  activeName: "1"
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($url, { params: { id: this.id, face: this.face } }).then(function (response) {
      var res = response.data;

      $this.userGroupList = res.userGroupList;
      $this.form = _.assign({}, res.item);

      if ($this.id > 0) {

        $this.courseList = res.courseList;
        $this.courseSelectList = res.courseSelectList;

        if ($this.copyId > 0) {
          $this.id = 0;
          $this.form.id = 0;
          $this.form.planName = $this.form.planName + "-复制";
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
    $api.post($url, { item: this.form, courseList: this.courseList, courseSelectList: this.courseSelectList }).then(function (response) {
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
  btnSaveClick: function () {
    this.apiSubmit();
  },
  btnSubmitClick: function () {
    var $this = this;
    this.form.submitType = 'Submit';
    this.$refs.form.validate(function (valid) {
      if (valid) {
        $this.apiSubmit();
      }
    });
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
  btnSelectCourseClick: function (isSelect) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getStudyUrl('studyCourseSelect', { pf: window.name, isSelect: isSelect }),
      width: "88%",
      height: "88%"
    });
  },
  selectCourseCallback: function (items, isSelect) {

    items.forEach(item => {

      var selectCourseInfo = {
        id: 0,
        guid: utils.uuid(),
        planId: this.id,
        courseId: item.id,
        courseName: item.name,
        isSelectCourse: isSelect,
        offLine: item.offLine,
        teacherId: item.teacherId,
        teacherName: item.teacherName,
        examId: item.examId,
        examName: item.examName,
        examQuestionnaireId: item.examQuestionnaireId,
        examQuestionnaireName: item.examQuestionnaireName,
        studyCourseEvaluationId: item.studyCourseEvaluationId,
        studyCourseEvaluationName: item.studyCourseEvaluationName,
        studyHour: item.studyHour,
        credit: item.credit,
        duration: item.duration,
        taxis: 1
      };

      let objExist = this.courseSelectList.find(itemExist => itemExist.courseId === selectCourseInfo.courseId);
      let objExist1 = this.courseList.find(itemExist => itemExist.courseId === selectCourseInfo.courseId);
      if (!objExist && !objExist1) {
        if (isSelect) {
          this.courseSelectList.push(selectCourseInfo);
          this.form.selectTotalDuration += selectCourseInfo.duration;
          this.form.selectTotalCount += 1;
          this.form.selectCourseOverCount += 1;
        }
        else {
          this.courseList.push(selectCourseInfo);
          this.form.totalDuration += selectCourseInfo.duration;
          this.form.totalCount += 1;
        }
      }

    })

  },
  btnRemoveCourse: function (row, isSelect) {
    var $this = this;
    top.utils.alertWarning({
      title: '提醒',
      text: '确定移除该课程吗？',
      callback: function () {
        if (isSelect) {
          $this.form.selectTotalDuration -= row.duration;
          $this.form.selectTotalCount -= 1;
          $this.form.selectCourseOverCount -= 1;
          $this.courseSelectList = $this.courseSelectList.filter(f => f.guid !== row.guid);
        }
        else {
          $this.form.totalDuration -= row.duration;
          $this.form.totalCount -= 1;
          $this.courseList = $this.courseList.filter(f => f.guid !== row.guid);
        }
      }
    });
  },
  btnDeleteExamClick: function (guid, isSelect) {
    var $this = this;
    top.utils.alertWarning({
      title: '提醒',
      text: '确定移除课后考试吗？',
      callback: function () {
        if (isSelect) {
          $this.courseSelectList.forEach(item => {
            if (item.guid === guid) {
              item.examId = 0;
              item.examName = '';
            }
          });
        }
        else {
          if (guid === "-1") {
            $this.form.examId = 0;
            $this.form.examName = "";
          }
          else {
            $this.courseList.forEach(item => {
              if (item.guid === guid) {
                item.examId = 0;
                item.examName = '';
              }
            });
          }

        }
      }
    });
  },
  btnSelectExamClick: function (guid, isSelect) {
    this.curGuid = guid;
    this.curIsSelect = isSelect;

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
    if (this.curIsSelect) {
      this.courseSelectList.forEach(item => {
        if (item.guid === this.curGuid) {
          item.examId = id;
          item.examName = name;
        }
      });
    }
    else {
      if (this.curGuid === "-1") {
        this.form.examId = id;
        this.form.examName = name;
      }
      else {
        this.courseList.forEach(item => {
          if (item.guid === this.curGuid) {
            item.examId = id;
            item.examName = name;
          }
        });
      }

    }
  },
  btnSelectTeacherClick: function (guid, isSelect) {
    this.curGuid = guid;
    this.curIsSelect = isSelect;

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
    if (this.curIsSelect) {
      this.courseSelectList.forEach(item => {
        if (item.guid === this.curGuid) {
          item.teacherId = id;
          item.teacherName = name;
        }
      });
    }
    else {
      this.courseList.forEach(item => {
        if (item.guid === this.curGuid) {
          item.teacherId = id;
          item.teacherName = name;
        }
      });
    }
  },
  btnDeleteTeacherClick: function (guid, isSelect) {
    var $this = this;
    top.utils.alertWarning({
      title: '提醒',
      text: '确定移除上课老师吗？',
      callback: function () {
        if (isSelect) {
          $this.courseSelectList.forEach(item => {
            if (item.guid === guid) {
              item.teacherId = 0;
              item.teacherName = '';
            }
          });
        }
        else {
          $this.courseList.forEach(item => {
            if (item.guid === guid) {
              item.teacherId = 0;
              item.teacherName = '';
            }
          });
        }
      }
    });
  },
  btnSelectQClick: function (guid, isSelect) {
    this.curGuid = guid;
    this.curIsSelect = isSelect;

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
    if (this.curIsSelect) {
      this.courseSelectList.forEach(item => {
        if (item.guid === this.curGuid) {
          item.examQuestionnaireId = id;
          item.examQuestionnaireName = name;
        }
      });
    }
    else {
      this.courseList.forEach(item => {
        if (item.guid === this.curGuid) {
          item.examQuestionnaireId = id;
          item.examQuestionnaireName = name;
        }
      });
    }
  },
  btnDeleteQClick: function (guid, isSelect) {
    var $this = this;
    top.utils.alertWarning({
      title: '提醒',
      text: '确定移除课后问卷吗？',
      callback: function () {
        if (isSelect) {
          $this.courseSelectList.forEach(item => {
            if (item.guid === guid) {
              item.examQuestionnaireId = 0;
              item.examQuestionnaireName = '';
            }
          });
        }
        else {
          $this.courseList.forEach(item => {
            if (item.guid === guid) {
              item.examQuestionnaireId = 0;
              item.examQuestionnaireName = '';
            }
          });
        }
      }
    });
  },
  btnSelectEvaluationClick: function (guid, isSelect) {
    this.curGuid = guid;
    this.curIsSelect = isSelect;

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
    if (this.curIsSelect) {
      this.courseSelectList.forEach(item => {
        if (item.guid === this.curGuid) {
          item.studyCourseEvaluationId = id;
          item.studyCourseEvaluationName = name;
        }
      });
    }
    else {
      this.courseList.forEach(item => {
        if (item.guid === this.curGuid) {
          item.studyCourseEvaluationId = id;
          item.studyCourseEvaluationName = name;
        }
      });
    }
  },
  btnDeleteEvaluationClick: function (guid, isSelect) {
    var $this = this;
    top.utils.alertWarning({
      title: '提醒',
      text: '确定移除课后评价吗？',
      callback: function () {
        if (isSelect) {
          $this.courseSelectList.forEach(item => {
            if (item.guid === guid) {
              item.studyCourseEvaluationId = 0;
              item.studyCourseEvaluationName = '';
            }
          });
        }
        else {
          $this.courseList.forEach(item => {
            if (item.guid === guid) {
              item.studyCourseEvaluationId = 0;
              item.studyCourseEvaluationName = '';
            }
          });
        }
      }
    });
  },
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
