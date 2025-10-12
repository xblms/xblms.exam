var $url = "/exam/examAssessment";
var $urlItem = $url + "/item";

var data = utils.init({
  form: {
    keyWords: '',
    pageIndex: 1,
    pageSize: PER_PAGE
  },
  list: [],
  total: 0,
  loadMoreLoading: false,
  appMenuActive: "ass"
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
        res.list.forEach(paper => {
          $this.list.push(paper);
        });
      }
      $this.total = res.total;

    }).catch(function (error) {
      utils.error(error, { layer: true });
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
      top.utils.pointNotice(res.pointNotice);

    }).catch(function (error) {
    }).then(function () {
    });
  },
  btnSearchClick: function () {
    this.form.pageIndex = 1;
    this.list = [];
    this.apiGet();
  },
  btnLoadMoreClick: function () {
    this.loadMoreLoading = true;
    this.form.pageIndex++;
    this.apiGet();
  },
  btnViewClick: function (paper) {

    if (paper.submitType === 'Submit') {
      utils.success("已提交", { layer: true })
    }
    else if (!paper.state) {
      utils.warning("不在有效期内！", { layer: true })
    }
    else {
      var $this = this;
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getExamUrl('examAssessmenting', { id: paper.id }),
        width: "100%",
        height: "100%",
        end: function () {
          $this.apiGetItem(paper.id);
        }
      });
    }
  },
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    top.document.title = "测评中心";
    this.apiGet();
  },
});
