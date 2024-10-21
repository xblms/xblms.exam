# XBLMES 在线考试系统

<img src="https://gitee.com/xblms/xblms/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/logo.png" height="180" align="center">

<br /><br />

## 介绍

基于 .NET Core 8

支持跨平台、国产化部署

支持国产人大金仓、达梦、OceanBase数据库 及 MySql、SqlServer、PostgreSql、SQLite 等数据库

## 演示地址

* 集团版本源码地址(https://gitee.com/xblms/xblmes-gc)，主要区别在于按公司和部门进行权限划分，各自管理和组织考试。

管理端：(http://8.131.91.222:5000/admin)

* 账号：admin，密码：123123

用户端：(http://8.131.91.222:5000/home)

* 账号：test1，密码：123123

移动端：(http://8.131.91.222:5000/app)

* 账号：test1，密码：123123

* 同一个账号不能同时登录，如果突然离线，有可能是其他用户登录后强制下线了

<img width="200" src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/demo/app/二维码.png"/>

## 系统展示

* 移动端

<img width="180" src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/demo/app/首页.jpg"/>
<img width="180"  src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/demo/app/考试中心.jpg"/>
<img width="180"  src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/demo/app/考试详细.jpg"/>
<img width="180"  src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/demo/app/考试中.jpg"/>
<img width="180"  src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/demo/app/交卷结果.jpg"/>
<img width="180"  src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/demo/app/考试成绩.jpg"/>
<img width="180"  src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/demo/app/查看答卷.jpg"/>
<img width="180"  src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/demo/app/问卷调查.jpg"/>
<img width="180"  src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/demo/app/刷题练习.jpg"/>
<img width="180"  src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/demo/app/刷题练习中.jpg"/>
<img width="180"  src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/demo/app/练习结果.jpg"/>
<img width="180"  src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/demo/app/刷题记录.jpg"/>
<img width="180"  src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/demo/app/我的.jpg"/>




## 发布手册
发布和部署手册：(https://gitee.com/xblms/xblms/tree/master/%E9%83%A8%E7%BD%B2%E6%89%8B%E5%86%8C)

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
```

> 进入文件夹 `./publish/xblms-win-x64` 获取部署文件

### Window(x32)：

```
npm install
npm run build-win-x32
dotnet build ./build-win-x32/build.sln -c Release
dotnet publish ./build-win-x32/src/XBLMS.Web/XBLMS.Web.csproj -r win-x32 -c Release -o ./publish/xblms-win-x32
```

> 进入文件夹 `./publish/xblms-win-x32` 获取部署文件

### Linux(x64)：

```
npm install
npm run build-linux-x64
dotnet build ./build-linux-x64/build.sln -c Release
dotnet publish ./build-linux-x64/src/XBLMS.Web/XBLMS.Web.csproj -r linux-x64 -c Release -o ./publish/xblms-linux-x64
```

> 进入文件夹 `./publish/xblms-linux-x64` 获取部署文件

### Linux(arm64)：

```
npm install
npm run build-linux-arm64
dotnet build ./build-linux-arm64/build.sln -c Release
dotnet publish ./build-linux-arm64/src/XBLMS.Web/XBLMS.Web.csproj -r linux-arm64 -c Release -o ./publish/xblms-linux-arm64
```

> 进入文件夹 `./publish/xblms-linux-arm64` 获取部署文件