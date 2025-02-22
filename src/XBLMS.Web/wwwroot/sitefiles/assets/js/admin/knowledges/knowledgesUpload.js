var $url = 'knowledges';
var $urlUpload = $url + "/upload";
var $urlSubmit = $url + "/submit";

var data = utils.init({
  treeId: utils.getQueryInt("treeId"),
  drawerUpload: false,
  activeName: 'select',
  uploadFormpanel: false,
  uploadBtnLoad: false,
  uploadLoading:false,
  fileList: [],
  fileListNo: [],
  fileListYes: [],
  knowledgesList:[],
  errorOffset: 0,
});

var methods = {
  btnSubmitClick: function () {
    if (this.knowledgesList.length > 0) {
      this.apiSubmit();
    }
    else {
      utils.error("请上传文档", { layer: true });
    }
  },
  apiSubmit: function () {
    var $this = this;

    utils.loading(this, true);

    $api.post($urlSubmit, { treeId: this.treeId, list: this.knowledgesList }).then(function (response) {
      var res = response.data;
      utils.success("操作成功")
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
      utils.closeLayerSelf();
    });
  },
  btnUploadClick: function () {
    this.drawerUpload = true;
  },
  uploadFiles: function () {
    if (this.fileList.length > 0) {
      this.$refs.uploadFiles.submit();
    }
    else {
      utils.error("请选择文件");
    }
  },
  uploadBefore(file) {
    return true;
  },
  uploadChange: function (file, fileList) {
    var re = /(\.pdf)$/i;
    if (!re.exec(file.name)) {
      fileList.splice(fileList.indexOf(file), 1);

      var newFile = file;
      newFile.msg = '仅支持 pdf 文档';
      this.fileListNo.push(newFile);
    }
    this.fileList = fileList;
  },
  uploadExceed: function (files, fileList) {

  },
  uploadProgress: function (event, file, fileList) {
    this.uploadBtnLoad = true;
    if (file.percentage >= 99) {
      file.percentage = 99;
    }
  },
  uploadSuccess: function (response, file, fileList) {
    if (response.success) {
      this.fileListYes.push(file);
      this.knowledgesList.push({ treeId: this.treeId, name: response.fileName, url: response.filePath, coverImgUrl: response.coverImagePath, locked: false });
    }
    else {
      var newFile = file;
      newFile.msg = response.msg;
      this.fileListNo.push(newFile);
    }
    this.fileList.splice(fileList.indexOf(file), 1);
    if (this.fileList.length === 0) {
      this.uploadBtnLoad = false;
    }

  },

  uploadError: function (err, file, fileList) {
    var newFile = file;
    newFile.msg = JSON.parse(err.message).message;
    this.fileListNo.push(newFile);

    if (this.fileList.length === 0) {
      this.uploadBtnLoad = false;
    }
  },
  btnTableRemoveClick: function (row) {
    this.knowledgesList.splice(this.knowledgesList.indexOf(row), 1);
  },
  btnTableViewClick: function (row) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getKnowledgesUrl("knowledgesView", { url: row.url }),
      width: "88%",
      height: "99%"
    });
  },
  drawerClose: function () {
    this.fileList = [];
    this.fileListNo = [];
    this.fileListYes = [];
  },
  btnFileAbort: function (file) {
    this.$refs.uploadFiles.abort(file);
    this.fileList.splice(this.fileList.indexOf(file), 1);
    if (this.fileList.length <= 0) {
      this.uploadBtnLoad = false;
    }
  },
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.$urlUpload = $apiUrl + "/" + $urlUpload;
    utils.loading(this, false);
  }
});
