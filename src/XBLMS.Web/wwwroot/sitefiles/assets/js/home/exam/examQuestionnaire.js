var $url = "/exam/examQuestionnaire";

var data = utils.init({
  form: {
    keyWords: '',
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
    if (paper.submitType === 'Submit') {
      utils.success("已提交")
    }
    else if (!paper.state) {
      utils.success("不在有效期内！")
    }
    else {
      var $this = this;
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getExamUrl('examQuestionnairing', { id: paper.id }),
        width: "100%",
        height: "100%",
        end: function () {

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
    this.apiGet();
  },
});
