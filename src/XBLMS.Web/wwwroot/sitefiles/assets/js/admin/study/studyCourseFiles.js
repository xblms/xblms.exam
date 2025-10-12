var $url = 'study/studyCourseFiles';

var $urlActionsDeleteFile = $url + '/file/del';
var $urlActionsDeleteGroup = $url + '/group/del';
var $urlActionsDeleteGroupAndFile = $url + '/delList';

var $urlActionsDownload = $url + '/file/download';

var data = utils.init({
  form: {
    keyword: '',
    groupId: 0,
    isFileServer: utils.getQueryBoolean("isFileServer"),
    token: utils.getQueryString("token")
  },
  list: null,
  groupId: 0,
  paths: null,
  curMouseoverId: 0,
  curFileType: ''
});

var methods = {
  apiList: function () {
    var $this = this;
    utils.loading(this, true);
    $api.get($url, {
      params: this.form
    }).then(function (response) {
      var res = response.data;

      if (res.redirectUrl !== '') {
        location.href = res.redirectUrl;
      }
      else if (res.sessionId !== '' && res.token !== '') {
        localStorage.removeItem(SESSION_ID_NAME);
        sessionStorage.removeItem(SESSION_ID_NAME);
        localStorage.removeItem(ACCESS_TOKEN_NAME);
        sessionStorage.removeItem(ACCESS_TOKEN_NAME);

        sessionStorage.setItem(SESSION_ID_NAME, res.sessionId);
        sessionStorage.setItem(ACCESS_TOKEN_NAME, res.token);
        localStorage.setItem(SESSION_ID_NAME, res.sessionId);
        localStorage.setItem(ACCESS_TOKEN_NAME, res.token);

        location.href = '/xblms-admin/study/studyCourseFiles?isFileServer=true';
      }
      else {

        $this.paths = res.paths;
        $this.list = res.list;
      }

    }).catch(function (error) {
      $vue.$message({
        message: error,
        type: 'error',
        showClose: true
      });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnGroupAddClick: function (id) {
    var $this = this;
    utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getStudyUrl('studyCourseFilesGroupEdit', { id: id, groupId: this.groupId }),
      width: "66%",
      height: "100%",
      offset: 'l',
      end: function () {
        $this.apiList();
      }
    });
  },
  btnEditClick: function (row) {
    this.btnGroupAddClick(row.id);
  },
  btnDeleteClick: function (row) {
    if (row.type === 'Group') {
      this.apiDeleteGroup(row.id);
    }
    else {
      this.apiDeleteFile(row.id);
    }
  },
  apiDeleteGroup: function (id) {
    var $this = this;

    utils.loading(this, true);
    $api.post($urlActionsDeleteGroup, {
      id: id
    }).then(function (response) {
      var res = response.data;
      $vue.$message({
        message: '操作成功',
        type: 'success',
        showClose: true
      });
    }).catch(function (error) {
      $vue.$message({
        message: error,
        type: 'error',
        showClose: true
      });
    }).then(function () {
      utils.loading($this, false);
      $this.apiList();
    });
  },
  btnDeleteMoreClick: function () {
    var $this = this;

    var nodes = this.$refs.fileTable.selection;
    var ids = _.map(nodes, function (item) {
      return item.id;
    });
    var files = [];
    for (var i = 0; i < nodes.length; i++) {
      files.push({ id: nodes[i].id, type: nodes[i].type })
    }
    if (files.length > 0) {
      $this.apiDeleteFileAndGroup(files);
    }
    else {
      $vue.$message({
        message: '请选择要删除的内容',
        type: 'error',
        showClose: true
      });
    }

  },
  apiDeleteFileAndGroup: function (files) {
    var $this = this;

    utils.loading(this, true);
    $api.post($urlActionsDeleteGroupAndFile, {
      files: files
    }).then(function (response) {
      var res = response.data;
      $vue.$message({
        message: '操作成功',
        type: 'success',
        showClose: true
      });
    }).catch(function (error) {
      $vue.$message({
        message: error,
        type: 'error',
        showClose: true
      });
    }).then(function () {
      utils.loading($this, false);
      $this.apiList();
    });
  },
  apiDeleteFile: function (id) {
    var $this = this;

    utils.loading(this, true);
    $api.post($urlActionsDeleteFile, {
      id: id
    }).then(function (response) {
      var res = response.data;
      $vue.$message({
        message: '操作成功',
        type: 'success',
        showClose: true
      });
    }).catch(function (error) {
      $vue.$message({
        message: error,
        type: 'error',
        showClose: true
      });
    }).then(function () {
      utils.loading($this, false);
      $this.apiList();
    });
  },
  btnTitleClick: function (material) {
    this.groupId = this.form.groupId = material.id;
    this.apiList();
  },
  breadcrumbPath: function (id) {
    this.groupId = this.form.groupId = id;
    this.apiList();
  },
  btnUploadMore: function () {
    var $this = this;
    utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getStudyUrl('studyCourseFilesUpload', { groupId: this.groupId }),
      width: "80%",
      height: "100%",
      offset: 'l',
      end: function () {
        $this.apiList();
      }
    });
  },

  btnDownloadClick: function (id) {
    window.open($apiUrl + "/" + $urlActionsDownload + '?id=' + id + '&access_token=' + $token);
  },
  btnView: function (id) {
    utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getStudyUrl('studyCourseFileLayerView', { id: id }),
      width: "66%",
      height: "100%",
      offset: 'l'
    });
  },

  mouseoverShowIn: function (row, column, cell, event) {
    this.curMouseoverId = row.id;
    this.curFileType = row.type;
  },
  mouseoverShowOut: function (row, column, cell, event) {
    this.curMouseoverId = 0;
    this.curFileType = '';
  },
  btnSearchClick: function () {
    this.apiList();
  }
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.apiList();
  }
});
