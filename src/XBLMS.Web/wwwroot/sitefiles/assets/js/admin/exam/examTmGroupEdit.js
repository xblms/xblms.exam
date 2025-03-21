var $url = '/exam/examTmGroup';
var $urlGet = $url + '/editGet';
var $urlPost = $url + '/editPost';

var data = utils.init({
  id: utils.getQueryInt("id"),
  groupTypeSelects: null,
  tmTree: null,
  checkdKeys: null,
  expandedKeys: null,
  checkStrictly: false,
  txList:null,
  filterText: '',
  selectOrgans: [],
  userGroups:[],
  form: {
    groupType:'All'
  }
});

var methods = {
  apiGet: function () {
    var $this = this;
    $api.get($urlGet, {
      params: {
        id: this.id,
      }
    }).then(function (response) {
      var res = response.data;

      $this.tmTree = res.tmTree;
      $this.groupTypeSelects = res.groupTypeSelects;
      $this.txList = res.txList;
      $this.userGroups = res.userGroups;

      $this.form = _.assign({}, res.group);
      if (res.group.id > 0) {
        $this.checkdKeys = $this.expandedKeys = res.group.treeIds;
      }

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },

  apiSubmit: function () {
    var $this = this;

    utils.loading(this, true);

    $api.post($urlPost, { group: $this.form  }).then(function (response) {
      utils.success('操作成功！');
      utils.closeLayer(false);
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },

  btnSubmitClick: function () {
    var $this = this;

    $this.form.treeIds = [];

    if ($this.form.groupType === 'Range') {
      var selectNodes = this.$refs.tmTree.getCheckedNodes();
      if (selectNodes && selectNodes.length > 0) {
        selectNodes.forEach((node) => {
          $this.form.treeIds.push(node.id);
        });
      }
    }

    this.$refs.form.validate(function (valid) {
      if (valid) {
        $this.apiSubmit();
      }
    });
  },

  btnCancelClick: function () {
    utils.closeLayer();
  },
  filterNode(value, data) {
    if (!value) return true;
    return data.label.indexOf(value) !== -1;
  }
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  watch: {
    filterText(val) {
      this.$refs.tmTree.filter(val);
    }
  },
  created: function () {
    this.apiGet();
  }
});
