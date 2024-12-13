var $url = '/exam/examTmAnalysis';
var $urlData = $url + '/data';
var $urlPaper = $url + '/paper';

var data = utils.init({
  form: {
    type: 'ByExamAll',
    paperId: null,
    reAnalysis: false,
    keyWords: '',
    orderType: 'rate',
    pageIndex: 1,
    pageSize: 100
  },
  typeList: null,
  paperList: null,
  paperLoading: false,
  list: null,
  total: 0,
  pageSizes: [PER_PAGE, 100, 500, 1000, 3000, 5000],
  pId: 0,
  pDate: null
});

var methods = {
  apiGetData: function () {
    var $this = this;
    utils.loading(this, true);
    $api.get($urlData).then(function (response) {
      var res = response.data;

      $this.typeList = res.typeList;

    }).catch(function (error) {
      utils.loading($this, false);
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  apiGetPaper: function (query) {
    var $this = this;

    if (query !== '') {
      this.paperLoading = true;
      $api.get($urlPaper, { params: { title: query } }).then(function (response) {
        var res = response.data;
        $this.paperList = res.list;
      }).catch(function (error) {
        $this.paperLoading = false;
        utils.error(error);
      }).then(function () {
        $this.paperLoading = false;
      });
    }
  },
  apiGet: function () {
    var $this = this;
    utils.loading(this, false);
    utils.loading(this, true, '首次加载或者重新统计的时候需要花点时间，请耐心等待...');
    $api.get($url, { params: $this.form }).then(function (response) {
      var res = response.data;

      $this.list = res.list;
      $this.total = res.total;
      $this.pId = res.pId;
      $this.pDate = res.pDate;

      $this.form.reAnalysis = false;;
    }).catch(function (error) {
      utils.loading($this, false);
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnReanalysisClick: function () {
    this.form.reAnalysis = true;
    this.btnSearchClick();
  },
  handleCurrentChange: function (val) {
    this.form.pageIndex = val;
    this.apiGet();
  },
  handleSizeChange: function (val) {
    this.form.pageIndex = 1;
    this.form.pageSize = val;

    this.btnSearchClick();
  },
  btnSearchClick: function () {
    this.form.pageIndex = 1;
    this.list = null;
    this.total = 0;
    if (this.form.type === 'ByExamOnlyOne') {
      if (this.form.paperId === null) {
        utils.error("请选择一份试卷");
      }
      else {
        this.apiGet();
      }
    }
    else {
      this.form.paperId = null;
      this.apiGet();
    }
  },
  btnViewClick: function (id) {
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getCommonUrl('examTmLayerView', { id: id }),
      width: "58%",
      height: "88%"
    });
  },
  btnEditClick: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examTmEdit', { id: id }),
      width: "78%",
      height: "98%"
    });
  },
  btnChatClick: function () {
    top.$vue.topFrameSrc = utils.getExamUrl("examTmAnalysisChat", { id: this.pId });
    top.$vue.topFrameTitle = "题库统计-错误分布图表";
    top.$vue.topFrameDrawer = true;
  }
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.apiGetData();
    this.apiGet();
  }
});
