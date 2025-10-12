var $url = "/study/studyPlan";
var $urlItem = $url + "/item";

var data = utils.init({
  form: {
    year: 0,
    state: '',
    keyWords:'',
    pageIndex: 1,
    pageSize: PER_PAGE
  },
  yearList:null,
  list: [],
  total: 0,
  appMenuActive: "studyPlan",
  loadMoreLoading: false,
  planDialogVisible:false
});

var methods = {
  apiGet: function () {
    var $this = this;

    if (this.total === 0) {
      utils.loading(this, true);
    }

    $api.get($url, { params: this.form }).then(function (response) {
      var res = response.data;

      if (res.list && res.list.length > 0) {
        res.list.forEach(item => {
          $this.list.push(item);
        });
      }
      $this.total = res.total;
      $this.yearList = res.yearList;

    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      $this.loadMoreLoading = false;
    });
  },
  apiGetItem: function (id) {
    var $this = this;

    $api.get($urlItem, { params: { id: id } }).then(function (response) {
      var res = response.data;

      let pIndex = $this.list.findIndex(item => {
        return item.id === id;
      });

      $this.$set($this.list, pIndex, res.item);


    }).catch(function (error) {
    }).then(function () {
    });
  },
  btnSearchClick: function () {
    this.planDialogVisible = false;
    this.form.pageIndex = 1;
    this.list = [];
    this.apiGet();
  },
  btnLoadMoreClick: function () {
    this.loadMoreLoading = true;
    this.form.pageIndex++;
    this.apiGet();
  },
  btnViewClick: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getStudyUrl('studyPlanInfo', { id: id }),
      width: "100%",
      height: "100%",
      end: function () {
        $this.setDocumentTitel();
        $this.apiGetItem(id);
      }
    });
  },
  setDocumentTitel: function () {
    top.document.title = "学习任务";
  },
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    this.setDocumentTitel();
    this.apiGet();
  },
});
