var $url = 'exam/examTm/tmEdit/getSmall';
var $urlsubmit = 'exam/examTm/tmEdit/submitSmall';

var data = utils.init({
  pf: utils.getQueryString("pf"),
  id: utils.getQueryInt("id"),
  tmGuid: utils.getQueryString("guid"),
  isAdd: utils.getQueryBoolean("isAdd"),
  txList: [],
  form: null,
  btnMinusOptionsShow: false,
  curBaseTx: null,
  options: ['正确', '错误'],
  optionsValue: '',
  optionsEditShow: [],
  optionsEditReloaded: []
});

var methods = {
  apiGet: function () {
    var $this = this;
    utils.loading(this, true);
    $api.get($url, { params: { id: this.id } }).then(function (response) {
      var res = response.data;
      var tm = res.item;
      $this.txList = res.txList;


      if (tm.id > 0) {
        $this.form = _.assign({}, tm);
        $this.options = tm.options;
        $this.optionsValue = tm.answer;

        $this.curBaseTx = $this.txFormatter($this.form.txId);
        if ($this.curBaseTx === 'Duoxuanti') {
          $this.optionsValue = $this.form.optionsValues;
        }
      }
      else {
        if ($this.isAdd) {
          $this.form = _.assign({}, tm);
          $this.form.txId = null;
          $this.form.nandu = 1;
        }
        else {
          var parentLayer = top.frames[$this.pf];
          var smallList = parentLayer.$vue.tableTmSmallList;
          smallList.forEach(tmItem => {
            if (tmItem.guid === $this.tmGuid) {
              $this.form = _.assign({}, tmItem);

              $this.options = tmItem.options;
              $this.optionsValue = tmItem.answer;

              $this.curBaseTx = $this.txFormatter($this.form.txId);
              if ($this.curBaseTx === 'Duoxuanti') {
                $this.optionsValue = $this.form.optionsValues;
              }
            }
          })
        }

      }

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  apiEditSubmit: function () {
    var $this = this;
    $api.post($urlsubmit, { item: this.form }
    ).then(function (response) {
      var res = response.data;
    }).catch(function () {
    }).then(function () {
      utils.closeLayerSelf();
    });
  },
  btnAddOptions: function () {
    this.options.push('');
    this.btnMinusOptionsShow = true;
    this.optionsEditShow.push(false);
  },
  btnMinusOptions: function () {
    if (this.options != null) {
      var optionsCount = this.options.length;
      if (optionsCount > 4) {
        this.options.splice(optionsCount - 1, 1);
        this.optionsEditShow.splice(optionsCount - 1, 1);
      }
      var optionsCount = this.options.length;
      if (optionsCount <= 4) {
        this.btnMinusOptionsShow = false;
      }
    }
  },
  btnSubmitClick: function () {
    var $this = this;

    this.$refs.form.validate(function (valid) {
      if (valid) {

        if ($this.curBaseTx === 'Tiankongti') {
          if (!utils.contains($this.form.title, "___")) {
            utils.error('填空题必须包含占位符三个英文全角下划线”___“', { layer: true });
            return;
          }
        }
        if ($this.curBaseTx === 'Danxuanti' || $this.curBaseTx === 'Duoxuanti' || $this.curBaseTx === 'Panduanti') {
          if ($this.optionsValue === '' || $this.optionsValue === null || $this.optionsValue === []) {
            utils.error('请选择正确答案', { layer: true });
            return;
          }
          if ($this.options != null) {
            if ($this.options.length > 0) {
              var isTrue = true;
              for (var i = 0; i < $this.options.length; i++) {
                if ($this.options[i].trim() === '') {
                  isTrue = false;
                  break;
                }
              }
              if (!isTrue) {
                utils.error('请完善候选项内容', { layer: true });
                return;
              }
            }
            else {
              utils.error('请完善候选项内容', { layer: true });
              return;
            }
          }
          $this.form.answer = $this.optionsValue;
          $this.form.options = $this.options;
          $this.form.optionsValues = $this.optionsValue;
        }

        $this.form.guid = $this.tmGuid;

        var parentLayer = top.frames[$this.pf];
        parentLayer.$vue.smallEditCallBack($this.form, $this.isAdd);

        if ($this.form.id > 0) {
          $this.apiEditSubmit();
        }
        else {
          utils.closeLayerSelf();
        }
      }
    });
  },

  txFormatter: function (id) {
    let tx = this.txList.find(item => item.id === id);
    return tx.examTxBase;
  },
  txChange: function (value) {
    this.form.answer = "";
    this.form.jiexi = "";

    this.curBaseTx = this.txFormatter(value);
    this.options = ['', '', '', ''];
    this.optionsValue = 'A';
    if (this.curBaseTx === 'Danxuanti') { this.optionsValue = 'A'; this.optionsEditShow = [false, false, false, false]; }
    else if (this.curBaseTx === 'Duoxuanti') { this.optionsValue = ['A', 'B', 'C', 'D']; this.optionsEditShow = [false, false, false, false]; }
    else if (this.curBaseTx === 'Panduanti') { this.options = ['正确', '错误']; this.optionsValue = 'A'; }
    else { this.optionsValue = ""; }
    this.optionsEditReloaded = [];
  },
  btnOpenEditClick: function (ref, ptype) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getCommonUrl('editorTmOpenLayer', { pf: window.name, ptype: ptype, ref: ref }),
      width: "58%",
      height: "78%"
    });
  },
  btnOpenEditPlusClick: function (ref, ptype) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getCommonUrl('editorPlusOpenLayer', { pf: window.name, ptype: ptype, ref: ref }),
      width: "100%",
      height: "100%"
    });
  },
  setOptionsValue: function (index, value) {
    this.$set(this.options, index, value);
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
