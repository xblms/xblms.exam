var $url = "/exam/examPaperScore";

var data = utils.init({
  form: {
    keyWords: '',
    dateFrom: '',
    dateTo:'',
    pageIndex: 1,
    pageSize: PER_PAGE
  },
  list: [],
  total: 0,
  loadMoreLoading:false
});

var methods = {
  apiGet: function() {
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
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      $this.loadMoreLoading = false;
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
    if (paper.paper.secrecyPaperContent) {
      top.utils.openTopLeft(paper.paper.title, utils.getExamUrl("examPaperView", { id: paper.id }));
    }
    else {
      utils.error("不允许查看答卷");
    }

  },
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
  },
});
