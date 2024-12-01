var $url = '/exam/examPkRooms';

var data = utils.init({
  id: 0,
  title: '',
  list: null,
  activeNames: [],
  isEdit: false,
  editId: 0
});

var methods = {
  apiGet: function () {
    var $this = this;
    $api.get($url, { params: { id: this.id } }).then(function (response) {
      var res = response.data;
      $this.list = res.list;
      $this.title = res.title;

      if (!$this.isEdit) {
        if ($this.list.length > 0) {
          $this.list.forEach(item => {
            $this.activeNames.push(item.guid);
          })
        }
        $this.isEdit = false;
      }


    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnEditClick: function (pkId) {
    this.isEdit = true;
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examPkEditDate', { id: pkId }),
      width: "50%",
      height: "80%",
      end: function () {
        $this.apiGet();
      }
    });
  },
};
var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.id = utils.getQueryInt("id");
    this.apiGet();
  }
});
