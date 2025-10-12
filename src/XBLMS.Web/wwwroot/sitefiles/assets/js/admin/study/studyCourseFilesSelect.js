var $url = 'study/studyCourseFilesSelect';

var data = utils.init({
  form: {
    fileType: utils.getQueryString("fileType"),
    keyword: '',
    groupId: 0
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

      $this.paths = res.paths;
      $this.list = res.list;

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
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
  btnView: function (row) {
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getStudyUrl('studyCourseFileLayerView', { id: row.id }),
      width: "68%",
      height: "99%",
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
  },
  fileTableBySelectable: function (row, index) {
    return row.type === 'File';
  },
  btnSelectClick: function () {
    var $this = this;

    var nodes = this.$refs.fileTable.selection;
    var ids = _.map(nodes, function (item) {
      return item.id;
    });
    var files = [];
    for (var i = 0; i < nodes.length; i++) {
      files.push(nodes[i])
    }
    if (files.length > 0) {
      var parentFrameName = utils.getQueryString("pf");
      var parentLayer = top.frames[parentFrameName];
      parentLayer.$vue.selectFilesCallback(files);
      utils.closeLayerSelf();
    }
    else {
      utils.error("请至少选中一个文件", { layer: true });
    }
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
