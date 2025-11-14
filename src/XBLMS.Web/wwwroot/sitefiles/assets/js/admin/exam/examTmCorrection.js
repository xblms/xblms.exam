var $url = 'exam/correction';

var data = utils.init({
  formInline: {
    keyWords: '',
    status: '',
    pageIndex: 1,
    pageSize: PER_PAGE,
  },
  list: [],
  total: 0,
  newsDrawerShow: false,
  form: {
    id: -1,
    title: '',
    subject: '',
    isTop: false,
    isEnable: true,
  },
  auditPanel: false,
  auditStatusList: null,
  auditForm: {
    id: 0,
    userId: 0,
    status: '',
    msg: ''
  }
});


var methods = {
  apiGet: function () {
    var $this = this;
    utils.loading(this, true);
    $api.get($url, { params: $this.formInline }).then(function (response) {
      var res = response.data;

      $this.list = res.list;
      $this.total = res.total;

      if ($this.auditStatusList === null) {
        $this.auditStatusList = res.statusList;
      }

    }).catch(function (error) {
      utils.loading($this, false);
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  searchList: function () {
    this.formInline.pageIndex = 1;
    this.apiGet();
  },
  btnUserViewClick: function (id) {
    utils.openUserView(id);
  },
  btnAdminViewClick: function (id) {
    utils.openAdminView(id);
  },
  btnTmViewClick: function (id) {
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getCommonUrl('examTmCorrectionLayerView', { id: id }),
      width: "88%",
      height: "88%"
    });
  },
  handleSizeChange(val) {
    this.formInline.pageIndex = 1;
    this.formInline.pageSize = val;
    this.searchList();
  },
  handleCurrentChange: function (val) {
    this.formInline.pageIndex = val;
    this.apiGet();
  },
  btnSearchClick: function () {
    this.formInline.pageIndex = 1;
    this.apiGet();
  },
  btnAuditClick: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examTmCorrectionAudit', { id: id }),
      width: "38%",
      height: "68%",
      end: function () {
        $this.apiGet();
      }
    });
  },
  btnEditClick: function (row) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examTmEdit', { id: row.tmId, examPaperId: row.examPaperId }),
      width: "78%",
      height: "98%",
      end: function () {
        $this.apiGet();
      }
    });
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

