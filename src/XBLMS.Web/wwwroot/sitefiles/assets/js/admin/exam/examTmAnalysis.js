var $url = '/exam/examTmAnalysis';
var $urlData = $url + '/data';
var $urlPaper = $url + '/paper';
var $urlNewGroup = $url + '/newGroup';

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
  pDate: null,
  newTmGroupName: '',
  newTmGroupPopover:false
});

var methods = {
  apiGetData: function () {
    var $this = this;
    $api.get($urlData).then(function (response) {
      var res = response.data;

      $this.typeList = res.typeList;

    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
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
    utils.openTopLeft("题库统计-错误分布图表", utils.getExamUrl("examTmAnalysisChat", { id: this.pId }))
  },
  btnNewGroupClick: function () {
    var nodes = this.$refs.tmTable.selection;
    var ids = _.map(nodes, function (item) {
      return item.tmId;
    });
    if (ids.length > 0) {
      if (ids.length > 2000) {
        utils.error("不能超过2000道题");
      }
      else {
        this.newTmGroupPopover = true;
      }
    }
    else {
      utils.error("请选择一些题目");
    }
  },
  btnNewGroupSubmitClick: function () {
    if (this.newTmGroupName.length > 0) {
      this.apiSubmitTmGroup();
    }
    else {
      utils.error("请输入新题目组名称");
    }
  },
  apiSubmitTmGroup: function () {
    var $this = this;
    utils.loading(this, true);
    var nodes = this.$refs.tmTable.selection;
    var ids = _.map(nodes, function (item) {
      return item.tmId;
    });
    $api.post($urlNewGroup, { groupName: this.newTmGroupName, tmIdList: ids }).then(function (response) {
      var res = response.data;
      utils.success("操作成功");
      $this.newTmGroupPopover = false;
      $this.$refs.tmTable.clearSelection();

    }).catch(function (error) {
      utils.loading($this, false);
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    utils.loading(this, false);
    this.apiGetData();
    this.apiGet();
  }
});
