var $url = 'exam/examTx';

var data = utils.init({
  list: null,
  name:'',
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($url, { params: { name: $this.name } }).then(function (response) {
      var res = response.data;

      $this.list = res.items;

    }).catch(function (error) {
      utils.loading($this, false);
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  apiDelete: function (id) {
    var $this = this;

    utils.loading(this, true);
    $api.post($url+'/del', { id: id }).then(function (response) {
      var res = response.data;
      utils.success('操作成功！');
    }).catch(function (error) {
      utils.loading($this, false);
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      $this.apiGet();
    });
  },

  handleCommand: function (type, row) {

    var $this = this;
    if (type === 'edit') {
      this.btnEditClick(row.id);
    }
    if (type === 'delete') {
      this.btnDeleteClick(row);
    }
  },
  btnSearchClick: function () {
    this.apiGet();
  },
  btnEditClick: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examTxEdit', { id: id }),
      width: "38%",
      height: "68%",
      end: function () {
        $this.apiGet();
      }
    });
  },

  btnDeleteClick: function (tx) {
    var $this = this;
    if (tx.tmCount > 0) {
      utils.error("不能删除被使用的题型");
    }
    else {
      top.utils.alertDelete({
        title: '删除题型',
        text: '此操作将删除题型 ' + tx.name + '，确定删除吗？',
        callback: function () {
          $this.apiDelete(tx.id);
        }
      });
    }

  }
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
  }
});
