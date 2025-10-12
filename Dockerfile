#引入镜像，低版本 docker 去掉  AS base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

MAINTAINER xblm

#配置工作目录
WORKDIR /app
  
#暴露容器端口，此端口与程序运行路径一致
EXPOSE 8888

#复制文件到工作目录
COPY . .
 
#ENV ：配置系统环境变量，比如程序环境环境等在这里配置（开发、预发、线上环境）
#这里是配置程序运行端口，如果程序不使用默认的80端口这里一定要设置（程序运行端口）
ENV ASPNETCORE_URLS http://+:8888

#设置时间为中国上海，默认为UTC时间
ENV TZ=Asia/Shanghai
#RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

#启动程序
ENTRYPOINT ["dotnet", "XBLMS.Web.dll"]




#docker操作

#1将该文件放入部署文件夹根目录

#2创建镜像
#docker build -t xblmselearning:8.0 .

#3启动容器
#docker run -d -p 8888:8888 xblmselearning:8.0

#4访问 http://localhost:8888/admin/install 安装系统

#5完成

#删除容器后数据会丢失，有情趣的朋友可以深入研究一下持久化