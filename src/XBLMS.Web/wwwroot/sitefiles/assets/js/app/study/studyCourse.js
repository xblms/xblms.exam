var $url = "/study/studyCourse";
var $urlItem = $url + "/item";

var data = utils.init({
  form: {
    keyWords: '',
    mark: '',
    collection: false,
    state: '',
    orderby: '',
    pageIndex: 1,
    pageSize: 12
  },
  list1: [],
  list2: [],
  total: 0,
  pushTotal: 0,
  markTotal: 0,
  markList: [],
  markShowList: [],
  markShowIndex: 0,
  markMore: true,
  loadMoreLoading: false,
  appMenuActive: "studyCourse",
  courseDialogVisible: false
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
        var pushIndex = 1;
        res.list.forEach(item => {
          if (pushIndex === 3) {
            pushIndex = 1;
          }
          if (pushIndex === 1) {
            $this.list1.push(item);
          }
          if (pushIndex === 2) {
            $this.list2.push(item);
          }
          pushIndex++;
          $this.pushTotal++;
        });
      }
      $this.total = res.total;

      if ($this.markTotal === 0 && res.markTotal > 0) {
        $this.markList = res.markList;
        $this.markTotal = res.markTotal;
        $this.moreMarkList();
      }

    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      $this.loadMoreLoading = false;
    });
  },
  moreMarkList: function () {
    for (var i = 0; i < 5; i++) {
      if (this.markShowIndex < this.markList.length) {
        this.markShowList.push(this.markList[this.markShowIndex]);
        this.markShowIndex++;
      }
      else {
        this.markMore = false;
      }
    }
    if (this.markList.length === this.markShowList.length) {
      this.markMore = false;
    }
  },
  apiGetItem: function (id, planId) {
    var $this = this;

    $api.get($urlItem, { params: { id: id, planId: planId } }).then(function (response) {
      var res = response.data;

      let pIndex = $this.list1.findIndex(item => {
        return item.id === id && item.planId === planId;
      });

      if (pIndex >= 0) {
        $this.$set($this.list1, pIndex, res.item);
      }
      else {
        pIndex = $this.list2.findIndex(item => {
          return item.id === id && item.planId === planId;
        });
        if (pIndex >= 0) {
          $this.$set($this.list2, pIndex, res.item);
        }
        else {
          pIndex = $this.list3.findIndex(item => {
            return item.id === id && item.planId === planId;
          });
          if (pIndex >= 0) {
            $this.$set($this.list3, pIndex, res.item);
          }
        }
      }

    }).catch(function (error) {
    }).then(function () {
    });
  },
  btnSearchClick: function () {
    this.courseDialogVisible = false;
    this.form.pageIndex = 1;
    this.list1 = [];
    this.list2 = [];
    this.apiGet();
  },
  btnLoadMoreClick: function () {
    this.loadMoreLoading = true;
    this.form.pageIndex++;
    this.apiGet();
  },
  btnViewClick: function (row) {

    var curl = utils.getStudyUrl('studyCourseInfo', { id: row.id, planId: row.planId });
    if (row.offLine) {
      curl = utils.getStudyUrl('studyCourseOfflineInfo', { id: row.id, planId: row.planId });
    }

    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: curl,
      width: "100%",
      height: "100%",
      end: function () {
        $this.setDocumentTitel();
        $this.apiGetItem(row.id, row.planId);
      }
    });
  },
  marChangeClick: function (mark) {
    if (mark === this.form.mark) {
      this.form.mark = '';
    }
    else {
      this.form.mark = mark;
    }
  },
  setDocumentTitel: function () {
    top.document.title = "课程中心";
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
