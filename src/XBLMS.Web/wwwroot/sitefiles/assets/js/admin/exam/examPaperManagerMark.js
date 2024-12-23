var $url = '/exam/examPaperManager';

var $urlMark = $url + '/mark';
var $urlMarkSelectMarker = $url + '/marker';

var data = utils.init({
  id: 0,
  formMark: {
    id: 0,
    keywords: '',
    dateFrom: '',
    dateTo: '',
    pageIndex: 1,
    pageSize: PER_PAGE
  },
  markList: null,
  markTotal: 0,
  markerList: null,
  markSelection: [],
  markerForm: {
    id: null,
    ids:null
  },
  markerSelectDialogVisible:false
});

var methods = {

  btnViewClick: function (id) {
    utils.openUserView(id);
  },
  btnAdminViewClick: function (id) {
    utils.openAdminView(id);
  },
  apiGet: function () {
    var $this = this;
    utils.loading(this, true);
    $api.get($urlMark, { params: $this.formMark }).then(function (response) {
      var res = response.data;
      $this.markList = res.list;
      $this.markTotal = res.total;
      $this.markerList = res.markerList;

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  markHandleSelectionChange(val) {
    this.markSelection = val;
  },
  btnMarkerArrange: function () {

    this.markerForm.id = null;
    this.markerForm.ids = null;

    if (this.markSelection && this.markSelection.length > 0) {
      this.markerSelectDialogVisible = true;
    }
    else {
      utils.error("请选择答卷", { layer: true });
    }
  },
  btnMarkerArrangeSubmit: function () {

    var startIds = [];
    this.markSelection.forEach(u => {
      startIds.push(u.id);
    });
    this.markerForm.ids = startIds;

    var $this = this;
    this.$refs.markerForm.validate(function (valid) {
      if (valid) {

        $this.apiSelectMarker();
      }
    });
  },
  apiSelectMarker: function () {
    var $this = this;
    utils.loading(this, true);
    $api.post($urlMarkSelectMarker, $this.markerForm).then(function (response) {
      var res = response.data;
      if (res.value) {
        utils.success("安排成功", { layer: true });
        $this.markSelection = null;
        $this.markerSelectDialogVisible = false;
        $this.apiGet();
      }

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnMarkSearchClick: function () {
    this.formMark.pageIndex = 1;
    this.apiGet();
  },
  btnPaperMarkView: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getCommonUrl('examPaperUserMark', { id: id }),
      width: "99%",
      height: "99%",
      end: function () {
        $this.btnMarkSearchClick();
        $this.btnScoreSearchClick();
      }
    });
  },
  markHandleCurrentChange: function (val) {
    this.formMark.pageIndex = val;
    this.apiGet();
  },
  btnMarkView: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getCommonUrl('examPaperUserMarkLayerView', { id: id }),
      width: "99%",
      height: "99%"
    });
  }
};
var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.id = this.formMark.id = utils.getQueryInt("id");
    this.apiGet();
  }
});
