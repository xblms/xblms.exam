var $url = '/exam/examPaperSelect';

var data = utils.init({
  formInline: {
    keyword: '',
    pageIndex: 1,
    pageSize: PER_PAGE
  },
  paperList: null,
  paperTotal: 0,
  selectRow: null
});

var methods = {
  apiGet: function () {
    var $this = this;
    utils.loading(this, true);
    $api.get($url, { params: $this.formInline }).then(function (response) {
      var res = response.data;

      $this.paperList = res.items;
      $this.paperTotal = res.total;

    }).catch(function (error) {
      utils.loading($this, false);
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  handleCurrentChange: function (val) {
    this.formInline.pageIndex = val;
    this.apiGet();
  },
  btnSearchClick: function () {
    this.formInline.pageIndex = 1;
    this.apiGet();
  },

  btnEditClick: function (id) {

    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examPaperEdit', { id: id, isCourseUse: true }),
      width: "98%",
      height: "98%",
      end: function () {
        $this.btnSearchClick();
      }
    });

  },
  btnCopyClick: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examPaperEdit', { id: id, copyId: id }),
      width: "98%",
      height: "98%",
      end: function () {
        $this.btnSearchClick();
      }
    });
  },
  selectCurrentChange: function (row) {
    this.selectRow = row;
  },
  btnSelectConfirmClick: function () {

    if (this.selectRow && this.selectRow.id > 0) {
      var parentFrameName = utils.getQueryString("pf");
      var parentLayer = top.frames[parentFrameName];
      parentLayer.$vue.selectExamCallback(this.selectRow.id, this.selectRow.title);
      utils.closeLayerSelf();
    }
    else {
      utils.error("请选择一份试卷", { layer: true });
    }
  }
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
  }
});
