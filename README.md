
# 星期八在线考试系统（XBLMS.EXAM）

<br />

<img src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/logo.png" height="180" align="center">

<br />

## 演示环境

> 管理端演示环境

* 账号：admin，密码：123123

* [点击前往管理端演示](http://182.92.223.118:5000/admin)

> 用户端演示环境

* 账号：test1，密码：123123

* [点击前往用户端演示](http://182.92.223.118:5000/home)

> 移动端演示环境

* 账号：test1，密码：123123

* 同一个账号不能同时登录，如果突然离线，有可能是其他用户登录后强制下线了

* 防止踢来踢去，自己在后台创建管理员账号或者用户账号即可

* 扫码前往移动端演示

<img width="200" src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/demo/app/二维码.png"/>

> 部分功能截图

* 管理端

<img width="200" src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/demo/admin/登录.png"/>
<img width="200" src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/demo/admin/首页.png"/>
<img width="200" src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/demo/admin/考试管理.png"/>
<img width="200" src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/demo/admin/题目管理.png"/>
<img width="200" src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/demo/admin/题库统计.png"/>

* 用户端

<img width="200" src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/demo/home/首页.png"/>
<img width="200" src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/demo/home/考试中心.png"/>
<img width="200" src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/demo/home/准备考试.png"/>
<img width="200" src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/demo/home/考试中.png"/>
<img width="200" src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/demo/home/考试成绩.png"/>

* 移动端

<img width="200" src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/demo/app/首页.png"/>
<img width="200"  src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/demo/app/考试中心.jpg"/>
<img width="200"  src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/demo/app/考试详细.jpg"/>
<img width="200"  src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/demo/app/考试中.jpg"/>
<img width="200"  src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/demo/app/交卷结果.jpg"/>
<img width="200"  src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/demo/app/考试成绩.jpg"/>
<img width="200"  src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/demo/app/查看答卷.jpg"/>
<img width="200"  src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/demo/app/问卷调查.jpg"/>
<img width="200"  src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/demo/app/刷题练习.jpg"/>
<img width="200"  src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/demo/app/刷题练习中.jpg"/>
<img width="200"  src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/demo/app/练习结果.jpg"/>
<img width="200"  src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/demo/app/刷题记录.jpg"/>
<img width="200"  src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/demo/app/我的.jpg"/>


## 介绍

基于 .NET Core 8 + Vue

支持跨平台部署

支持人大金仓、达梦、OceanBase数据库 及 MySql、SqlServer、PostgreSql、SQLite 等数据库

### 优势

* 一码多库：一套代码，多种类型数据库随意搭配。

* NoSql：无数据库脚本，无sql语句，code first高级版，基于实体对象自动生成数据库结构。

* 一键切换：备份、还原、迁移、切换数据库分分钟搞定。

* 前后端分离：.netcore api restfull 提供轻量级的数据服务，搭配vue前端框架，开发部署简单高效。

* 一码多端：一套代码，适配pc端多端浏览器和移动端多端终端，响应式布局+element轻松实现耳目一新的ui用户体验。

* 跨平台：适配国内主流CPU、操作系统、国产数据库。

* 全方位安全机制

	1、完整且丰富的日志功能（管理员日志+用户日志+错误日志+数据日志）

    2、前后端安全（SQL注入、跨站脚本、非法文件上传、越权访问）

	3、轻松运维（无界面数据查询+审计）

	4、访问限制（白名单、黑名单、ip、ip段、区域）

	5、安全模式，自定义系统安全措施

* 技术支持+个性化定制（非常专业）

* 性能（.net 8 史无前例）

### 版本

* master 为开发版，这里会经常提交优化和更新，为下一个版本做准备。

* [发行版提供较稳定的生成环境部署包和源代码，前往下载](https://gitee.com/xblms/xblmes/releases)

### 系统升级

* 访问/admin/syncDatabase 进行数据库升级。发布最新的代码替换到原来的部署包即可升级部署文件，不要替换xblms.json文件。

## 发布手册

* [发布和部署手册](https://gitee.com/xblms/xblmes/tree/master/src/XBLMS.Web/wwwroot/sitefiles/assets/uploadtemplates/doc)

## 支持环境
### 支持的操作系统
#### Windows
|操作系统|版本|架构|
|:-|:-|:-|
|[Windows 10](https://www.microsoft.com/windows/)|Version 1607+|x64, x86, Arm64|
|[Windows 11](https://www.microsoft.com/windows/)|Version 22000+|x64, x86, Arm64|
|[Windows Server](https://learn.microsoft.com/windows-server/)|2012+|x64, x86|
|[Windows Server Core](https://learn.microsoft.com/windows-server/)|2012+|x64, x86|
|[Nano Server](https://learn.microsoft.com/windows-server/get-started/getting-started-with-nano-server)|Version 1809+|x64|

#### Linux
|操作系统|版本|架构|
|:-|:-|:-|
|[Alpine Linux](https://alpinelinux.org/)|3.15+|x64, Arm64, Arm32|
|[CentOS](https://www.centos.org/)|7+|x64|
|[Debian](https://www.debian.org/)|10+|x64, Arm64, Arm32|
|[Fedora](https://opensuse.org/)|33+|x64|
|[OpenSUSE](https://opensuse.org/)|15+|x64|
|[Oracle Linux](https://www.oracle.com/linux/)|7+|x64|
|[Red Hat Enterprise Linux](https://www.redhat.com/en/technologies/linux-platforms/enterprise-linux)|7+|x64, Arm64|
|[SUSE Enterprise Linux (SLES)](https://www.suse.com/products/server/)|12 SP2+|x64|
|[Ubuntu](https://ubuntu.com/)|18.04+|x64, Arm64, Arm32|
|[银河麒麟](https://kylinos.cn/)|10+|x64, Arm64|
|[中标麒麟](https://kylinos.cn/)|7+|x64, Arm64|

### 支持的数据库
|数据库|版本|
|:-|:-|
|[MySql](https://www.mysql.com/)|5.7+|
|[SqlServer](https://www.microsoft.com/en-us/sql-server)|2008+|
|[PostgreSql](https://www.postgresql.org/)|11+|
|[SQLite](https://sqlite.org/)|2.0+|
|[人大金仓](https://www.kingbase.com.cn/)|9.0+|
|[达梦](https://www.dameng.com/)|8.0+|
|[OceanBase](https://www.oceanbase.com/)|4.3+|

## 源码结构
```
├── src (源代码)
│   ├── Datory (数据库基础类)
│   ├── XBLMS (接口基础类)
│   ├── XBLMS.Core (核心代码)
│   ├── XBLMS.Web (UI)
│   │   ├── wwwroot (对外访问目录)
│   │   ├── Controllers (WebApi)
│   │   ├── log (运行日志)
│   │   ├── Pages (页面)
│   │   ├── appsettings.json (配置文件)
│   │   ├── web.config (配置文件，非IIS部署可以删除)
│   │   ├── xblms.json (配置文件)
├── appsettings.json (配置文件)
├── build.sln (解决方案，用于发布)
├── gulpfile.js (配置文件，用于发布)
├── xblms.sln (解决方案，用于开发)
```

## 功能介绍
### 管理端

#### 首页
- 管理员默认页面

|功能|说明|
|:-|:-|
|基本信息|显示当前账号信息和欢迎词。|
|预览信息|预览管理员信息。|
|修改信息|修改基本信息。|
|修改密码|修改登录密码。|
|退出登录|退出系统。|

#### 发布考试
- 支持 正式考试、模拟自测 等模式
- 支持 随机出题、手动选题、开考随机 等出题方式
- 支持 手动阅卷、自动阅卷 等判分方式
- 支持 证书绑定，考试通过即可获得证书

|功能|说明|
|:-|:-|
|试卷分类|试卷分类管理，支持无线层级，支持批量添加。|
|发布考试|发布考试，支持保存、发布、重新发布等操作。|
|复制|复制并发布试卷。|
|预览|预览试卷。|
|修改|修改试卷。|
|起停用|支持启用停用。|
|删除|删除试卷。|
|考试管理|考生管理、成绩管理、阅卷管理、统计图表等。|

#### 阅卷
- 管理员可以分配多个答卷给不同的阅卷老师进行阅卷，阅卷老师通过该功能进行判分。

|功能|说明|
|:-|:-|
|阅卷|对答卷中的主观题进行判分。|
|预览|预览阅卷。|

#### 考试管理
- 围绕考试的一些管理功能。

|功能|说明|
|:-|:-|
|题型管理|支持 单选、多选、判断、填空、简答 等基本题型，支持基于基本题型扩展。|
|题库管理|题目管理，支持题目批量导入、导出，支持预览。|
|证书管理|发布证书，支持证书内容拖拽定位，支持预览。|
|题目组|题目分组管理，可配置刷题，同时方便组卷。|

#### 答题竞赛
- 支持长连接，支持观战。

|功能|说明|
|:-|:-|
|发布竞赛|支持最少2人、最多512人进行答题淘汰赛知道决赛。|
|竞赛管理|支持排名，支持排位。|

#### 问卷调查
- 支持 内部问卷、外部问卷 等模式，外部问卷可以支持通过二维码进行填写，不需要登录系统。

|功能|说明|
|:-|:-|
|发布|发布问卷。|
|复制|复制问卷。|
|预览|预览问卷内容。|
|修改|修改问卷。|
|起停用|支持启用停用。|
|删除|删除问卷。|
|问卷统计|统计图表。|

#### 其他功能
|功能|说明|
|:-|:-|
| 企业管理  | 组织管理、管理员管理、角色管理、用户管理、用户组管理 等  |  
| 系统管理  | 管理员设置、用户设置、数据库管理、访问拦截管理 等  |  
| 日志管理  | 管理员日志、用户日志、系统错误日志，日志设置 等  |  
| 统计图表  | 用户登录统计、访问拦截统计 等  |  


## 编译

项目编译需要使用 Visual Studio 2022，你可以从这里下载 [Visual Studio Community 2022](https://www.visualstudio.com/downloads/)

[SDK 开发用](https://dotnet.microsoft.com/zh-cn/download/dotnet/thank-you/sdk-8.0.403-windows-x64-installer)

[运行时 部署用](https://dotnet.microsoft.com/zh-cn/download/dotnet/thank-you/runtime-aspnetcore-8.0.2-windows-hosting-bundle-installer)

## 发布跨平台版本

### Window(x64)：

```
npm install
npm run build-win-x64
dotnet build ./build-win-x64/build.sln -c Release
dotnet publish ./build-win-x64/src/XBLMS.Web/XBLMS.Web.csproj -r win-x64 -c Release -o ./publish/xblms-win-x64
npm run copy-win-x64
```

> 进入文件夹 `./publish/xblms-win-x64` 获取部署文件

### Window(x32)：

```
npm install
npm run build-win-x86
dotnet build ./build-win-x86/build.sln -c Release
dotnet publish ./build-win-x86/src/XBLMS.Web/XBLMS.Web.csproj -r win-x86 -c Release -o ./publish/xblms-win-x86
npm run copy-win-x86
```

> 进入文件夹 `./publish/xblms-win-x86` 获取部署文件

### Linux(x64)：

```
npm install
npm run build-linux-x64
dotnet build ./build-linux-x64/build.sln -c Release
dotnet publish ./build-linux-x64/src/XBLMS.Web/XBLMS.Web.csproj -r linux-x64 -c Release -o ./publish/xblms-linux-x64
npm run copy-linux-x64
```

> 进入文件夹 `./publish/xblms-linux-x64` 获取部署文件

### Linux(arm64)：

```
npm install
npm run build-linux-arm64
dotnet build ./build-linux-arm64/build.sln -c Release
dotnet publish ./build-linux-arm64/src/XBLMS.Web/XBLMS.Web.csproj -r linux-arm64 -c Release -o ./publish/xblms-linux-arm64
npm run copy-linux-arm64
```

> 进入文件夹 `./publish/xblms-linux-arm64` 获取部署文件

## 问题与建议

如发现问题或对产品有任何建议，请提交至 [Gitee Issues](https://gitee.com/xblms/xblmes/issues)。


## License

[GNU Affero General Public License v3.0](LICENSE)

Copyright (C) 2024 XBLMS.EXAM

## 打赏

开源无盈利，生活不容易，多少是心意，感谢支持与鼓励！

<img width="200" src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/ds.png"/>

## 你不一般

* 能看到这里，请帮忙点个star，当然没看到这里的朋友点了star那也是万分感谢（❤）。

* 除了上面提到的社区版（开源）之外，我们还准备了企业版（企业解决方案），介于你能力出众智慧超群万里挑一且品学兼优，恰好你的单位有这个需求，那么，从这里开始不再仅仅是代码的事情，是系统、架构、解决方案以及高效的持续跟踪服务。

* [欢迎垂询，点击前往](http://182.92.223.118)

* <img width="200" src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/examzj.jpg"/>