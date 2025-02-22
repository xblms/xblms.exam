var $url = "/knowledges";
var $urlItem = $url + "/item";
var $urlTree = $url + "/tree";

var data = utils.init({
  form: {
    treeId:0,
    keywords: '',
    like: false,
    collect: false,
    state: '',
    orderby: '',
    pageIndex: 1,
    pageSize: 18
  },
  list1: [],
  list2: [],
  list3: [],
  list4: [],
  list5: [],
  list6: [],
  total: 0,
  pushTotal: 0,
  loadMoreLoading: false,
  treeSearchShow: false,
  treeSearchId: 0,
  treeSearchList: []
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
          if (pushIndex === 7) {
            pushIndex = 1;
          }
          if (pushIndex === 1) {
            $this.list1.push(item);
          }
          if (pushIndex === 2) {
            $this.list2.push(item);
          }
          if (pushIndex === 3) {
            $this.list3.push(item);
          }
          if (pushIndex === 4) {
            $this.list4.push(item);
          }
          if (pushIndex === 5) {
            $this.list5.push(item);
          }
          if (pushIndex === 6) {
            $this.list6.push(item);
          }
          pushIndex++;
          $this.pushTotal++;
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
  apiGetTree: function (deep) {
    var $this = this;

    utils.loading(this, true);
    $api.get($urlTree, { params: { id: this.treeSearchId } }).then(function (response) {
      var res = response.data;
      if (res.total > 0) {
        $this.treeSearchList.push({ selectId: 0, deep: deep, list: res.list });
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      $this.loadMoreLoading = false;
    });
  },
  btnTreeNodeClick: function (node, nodeItem) {
    if (node.selectId !== nodeItem.id) {
      this.treeSearchList = this.treeSearchList.filter(f => f.deep <= node.deep);
      node.selectId = nodeItem.id;
      if (node.selectId > 0) {
        this.treeSearchId = node.selectId;
        this.apiGetTree(node.deep + 1);
      }
      else {
        if (node.deep === 0) {
          this.treeSearchId = 0;
        }
        else {
          var parentNode = this.treeSearchList.find(item => item.deep === node.deep - 1);
          this.treeSearchId = parentNode.selectId;
        }
      }
      this.form.treeId = this.treeSearchId;
      this.btnSearchClick();
    }
  },
  btnSearchClick: function () {
    this.pushTotal = 0;
    this.form.pageIndex = 1;
    this.list1 = [];
    this.list2 = [];
    this.list3 = [];
    this.list4 = [];
    this.list5 = [];
    this.list6 = [];
    this.apiGet();
  },
  btnLoadMoreClick: function () {
    this.loadMoreLoading = true;
    this.form.pageIndex++;
    this.apiGet();
  },
  btnViewClick: function (row) {
    utils.openTopLeft(row.name, utils.getKnowledgesUrl("knowledgesView", { id: row.id }));
  }
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    this.apiGetTree(0);
    this.apiGet();
  },
});
