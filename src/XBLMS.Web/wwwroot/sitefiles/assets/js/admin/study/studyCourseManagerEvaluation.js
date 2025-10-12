var $url = '/study/studyCourseManager';

var $urlEvaluation = $url + '/evaluation';
var $urlEvaluationExport = $urlEvaluation + '/export';

var data = utils.init({
  id: 0,
  planId:0,
  formEvaluation: {
    id: 0,
    planId: 0,
    keyWords: '',
    pageIndex: 1,
    pageSize: PER_PAGE
  },
  evaluationList: null,
  evaluationTotal: 0,
  evaluationItems:null,
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
    $api.get($urlEvaluation, { params: $this.formEvaluation }).then(function (response) {
      var res = response.data;
      $this.evaluationList = res.list;
      $this.evaluationTotal = res.total;
      $this.evaluationItems = res.items;

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnEvaluationSearchClick: function () {
    this.formEvaluation.pageIndex = 1;
    this.apiGet();
  },
  btnEvaluationExportClick: function () {
    var $this = this;

    utils.loading(this, true);
    $api.post($urlEvaluationExport, this.formScore).then(function (response) {
      var res = response.data;

      window.open(res.value);
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  evaluationHandleCurrentChange: function (val) {
    this.formEvaluation.pageIndex = val;
    this.apiGet();
  },
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.id = this.formEvaluation.id = utils.getQueryInt("id");
    this.planId = this.formEvaluation.planId = utils.getQueryInt("planId");

    this.apiGet();
  }
});
