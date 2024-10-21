var $url = 'exam/examPaperEdit';
var $urlGetConfig = $url + '/getConfig';

var data = utils.init({
  id: utils.getQueryInt('id'),
  copyId: utils.getQueryInt('copyId'),
  treeId: utils.getQueryInt('treeId'),
  item: null,
  userGroupList: null,
  tmGroupList: null,
  tmAllGroupList:null,
  tmFixedGroupList:null,
  paperTree: null,
  cerList: null,
  txList: null,
  tmRandomConfig: [],
  selectTms: null,
  form: null,
  txTotalScore: 0,
  submitDialogVisible: false,
  submitSubmitType: 'Save',
  submitSubmitIsClear: false,
  tmConfigDialogVisible: false,
  isUpdateDateTime: false,
  isUpdateExamTimes:false,
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($url, { params: { id: this.id } }).then(function (response) {
      var res = response.data;

      $this.tmAllGroupList = res.tmGroupList;
      $this.tmFixedGroupList = res.tmFixedGroupList;
      $this.txList = res.txList;
      $this.userGroupList = res.userGroupList;
      $this.paperTree = res.paperTree;
      $this.cerList = res.cerList;

      $this.tmGroupList = $this.tmAllGroupList;


      $this.form = _.assign({}, res.item);
      if ($this.id > 0) {
        if (res.configList != null && res.configList.length > 0) {
          $this.tmRandomConfig = res.configList;
        }
        if ($this.copyId > 0) {
          $this.id = 0;
          $this.form.id = 0;
          $this.form.title = $this.form.title + "-复制";
          $this.form.submitType = "Save";
        }
      }
      else
      {
        if ($this.treeId > 0) {
          $this.form.treeId = $this.treeId;
        }
        else {
          $this.form.treeId = null;
        }

        $this.$nextTick(() => {
          $this.apiGetConfig();
        })

      }


      if ($this.form.cerId === 0) {
        $this.form.cerId = null;
      }

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  apiGetConfig: function () {
    var $this = this;

    utils.loading(this, true, "正在加载题目配置数据");
    $api.post($urlGetConfig, { txIds: $this.form.txIds, tmGroupIds: $this.form.tmGroupIds }).then(function (response) {
      var res = response.data;

      $this.tmRandomConfig = res.items;

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnGetConfigClick: function () {
    if (this.form.tmRandomType !== 'RandomNone') {
      this.apiGetConfig();
    }
  },

  tmRandomTypeChange: function (value) {
    this.tmRandomTypeChange = null;
    this.form.txIds = null;
    this.form.tmGroupIds = null;
    this.tmGroupList = [];
    if (value == 'RandomNone') {
      this.tmGroupList = this.tmFixedGroupList;
    }
    else {
      this.tmGroupList = this.tmAllGroupList;
      this.btnGetConfigClick();
    }
  },
  btnOpenEditClick: function (ref, ptype) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getCommonUrl('editorTmOpenLayer', { pf: window.name, ptype: ptype, ref: ref }),
      width: "58%",
      height: "78%"
    });
  },
  btnSaveClick: function () {
    this.submitSubmitType = 'Save';
    this.submitSubmitIsClear = false;
    var $this = this;
    this.$refs.form.validate(function (valid) {
      if (valid) {
        var valido = $this.submitValid();
        if (!valido) return;

        $this.apiSubmit();
      }
    });

  },
  btnSubmitClick: function () {
    this.submitSubmitType = 'Submit';
    this.submitSubmitIsClear = false;

    if (this.form.userGroupIds && this.form.userGroupIds.length > 0) {
      var $this = this;
      this.$refs.form.validate(function (valid) {
        if (valid) {
          var valido = $this.submitValid();
          if (!valido) return;

          if ($this.id > 0 && $this.form.submitType === 'Submit') {
            $this.submitDialogVisible = true;
            $this.submitSubmitIsClear = true;
          }
          else {
            $this.apiSubmit();
          }
        }
      });
    }
    else {
      utils.error('请选择至少一个用户组', { layer: true });
    }
 
  },
  btnSubmit: function () {
    this.submitSubmitIsClear = false;
    this.apiSubmit();
  },
  btnSubmitClear: function () {
    var $this = this;
    top.utils.alertWarning({
      title: '重新发布提醒',
      text: '确定清空后发布吗？',
      confirmButtonText:'重新发布',
      showCancelButton:true,
      callback: function () {
        $this.submitSubmitIsClear = true;
        $this.apiSubmit();
      }
    });
  
  },
  apiSubmit: function () {

    var $this = this;
    utils.loading($this, true);
    $api.post($url, {
      isClear: this.submitSubmitIsClear,
      submitType: this.submitSubmitType,
      item: $this.form,
      configList: $this.tmRandomConfig,
      isUpdateDateTime: $this.isUpdateDateTime,
      isUpdateExamTimes: $this.isUpdateExamTimes
    }).then(function (response) {
      var res = response.data;
      if (res.value) {
        utils.success("操作成功");
      }

    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      utils.closeLayerSelf();
    });
  },
  submitValid: function () {
    if (this.form.tmRandomType === 'RandomNone') {
      if (this.form.tmGroupIds === null || this.form.tmGroupIds.length === 0) {
        utils.error('请选择至少一个题目组', { layer: true });
        return false;
      }
    }
    return true;
  },
  getSummaries(param) {
    if (this.tmRandomConfig && this.tmRandomConfig.length > 0) {
      const { columns, data } = param;
      const sums = [];
      columns.forEach((column, index) => {
        if (index === 0) {
          sums[index] = '合计';
          return;
        }
        if (index === 3) {
          sums[index] = '';
          return;
        }
        if (index === 1) {
          let totalCount = 0;
          data.forEach(item => {
            totalCount += (item.nandu1TmCount + item.nandu2TmCount + item.nandu3TmCount + item.nandu4TmCount + item.nandu5TmCount);
          })

          sums[index] = '共 ' + totalCount + ' 道题';
        }
        if (index === 2) {

          let totalScore = 0;
          data.forEach(item => {
            totalScore += (item.nandu1TmCount + item.nandu2TmCount + item.nandu3TmCount + item.nandu4TmCount + item.nandu5TmCount) * item.txScore;
          })

          if (this.tmConfigDialogVisible) {
            sums[6] = '题型总分 ' + totalScore + ' 分';
          }
          else {
            sums[index] = '题型总分 ' + totalScore + ' 分';
          }
        

          this.txTotalScore = totalScore;

          if (this.form.tmScoreType === 'ScoreTypeTx') {
            this.form.totalScore = this.txTotalScore;
          }
        }

      });
      return sums;
    }

  },
  scoreTypeChange: function (value) {
    if (value === 'ScoreTypeTx' && this.form.tmRandomType !== 'RandomNone') {
      this.form.totalScore = this.txTotalScore;
    }
    else {
      this.form.totalScore = 100;
    }
  },
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
  }
});
