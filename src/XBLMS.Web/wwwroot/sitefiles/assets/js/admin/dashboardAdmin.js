var $url = "/dashboardAdmin";
var $urlData = $url + "/data";

var data = utils.init({
  series: [],
  chartOptions: {},
  total: 0,
  list: [],
  pageIndex: 1,
  pageSize: PER_PAGE,
  loadMoreLoading: false
});

var methods = {
  apiGetLog: function () {
    var $this = this;

    if (this.total === 0) {
      utils.loading(this, true);
    }

    $api.get($url, { params: { pageIndex: this.pageIndex, pageSize: this.pageSize } }).then(function (response) {
      var res = response.data;
      $this.total = res.total;

      if (res.list && res.list.length > 0) {
        res.list.forEach(paper => {
          $this.list.push(paper);
        });
      }

    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      $this.loadMoreLoading = false;
    });
  },
  apiGetData: function () {
    var $this = this;
    utils.loading(this, true);

    $api.get($urlData).then(function (response) {
      var res = response.data;

      $this.series = res.dataList;
      $this.chartOptions = {
        chart: {
          type: 'bar',
          stacked: true,
        },
        title: {
          text: '',
          align: 'left',
        },
        stroke: {
          width: 1,
          colors: ['#fff']
        },
        plotOptions: {
          bar: {
            horizontal: true
          }
        },
        xaxis: {
          categories: res.dataTitleList
        },
        fill: {
          opacity: 1,
        },
        colors: ['#409EFF', '#FF7F50', '#F56C6C', '#FF0000', '#67C23A'],
        legend: {
          position: 'top',
          horizontalAlign: 'left'
        }
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnLoadMoreClick: function () {
    this.loadMoreLoading = true;
    this.pageIndex++;
    this.apiGetLog();
  },
  btnViewClick: function (log) {
    var $this = this;
    if (log.isAdmin) {
      utils.openAdminView(log.objectId);
    }
    else if (log.isUser) {
      utils.openUserView(log.objectId);
    }
    else if (log.isDeleteTm) {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getCommonUrl('examTmDeleteLayerView', { id: log.id }),
        width: "58%",
        height: "88%"
      });
    }
    else if (log.isTm) {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getCommonUrl('examTmLayerView', { id: log.objectId }),
        width: "58%",
        height: "88%"
      });
    }
    else if (log.isExam) {
      utils.openTopLeft(log.objectName + '-预览', utils.getCommonUrl("examPaperLayerView", { id: log.objectId }));
    }
    else if (log.isExamQ) {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getExamUrl('examQuestionnaireAnalysis', { id: log.objectId }),
        width: "98%",
        height: "98%"
      });
    }
    else if (log.isExamAss) {
      utils.openTopLeft(log.objectName + '-测评结果', utils.getExamUrl("examAssessmentUsers", { id: log.objectId }));
    }
    else if (log.isExamPk) {
      utils.openTopLeft(log.objectName + '-赛程', utils.getExamUrl("examPkRooms", { id: log.objectId }));
    }
    else if (log.isExamCer) {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getCommonUrl('examCerLayerView', { id: log.objectId }),
        width: "88%",
        height: "98%"
      });
    }
  },
  btnEditClick: function (log) {
    var $this = this;
    if (log.isAdmin) {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getSettingsUrl('administratorsLayerProfile', { userName: log.objectName }),
        width: "60%",
        height: "88%"
      });
    }
    else if (log.isUser) {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getSettingsUrl('usersLayerProfile', { userId: log.objectId }),
        width: "60%",
        height: "88%"
      });
    }
    else if (log.isTm) {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getExamUrl('examTmEdit', { id: log.objectId }),
        width: "78%",
        height: "98%"
      });
    }
    else if (log.isExam) {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getExamUrl('examPaperEdit', { id: log.objectId }),
        width: "98%",
        height: "98%",
      });
    }
    else if (log.isExamQ) {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getExamUrl('examQuestionnaireEdit', { id: log.objectId }),
        width: "98%",
        height: "98%"
      });
    }
    else if (log.isExamAss) {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getExamUrl('examAssessmentEdit', { id: log.objectId }),
        width: "98%",
        height: "98%"
      });
    }
    else if (log.isExamPk) {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getExamUrl('examPkEdit', { id: log.objectId }),
        width: "68%",
        height: "88%"
      });
    }
    else if (log.isExamCer) {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getExamUrl('examCerEdit', { id: log.objectId }),
        width: "99%",
        height: "99%"
      });
    }
  }
};
Vue.component("apexchart", {
  extends: VueApexCharts
});

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    this.apiGetLog();
    this.apiGetData();
  },
});
