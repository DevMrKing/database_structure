# 数据库文档生成工具：
## src
#### db：数据库操作类
#### service：数据库服务类
#### servlet：后台处理总入口
#### util：工具类
#### vo：封装的实体类

## WebContent
#### i18n：国际化语言包
#### md：不同语言的翻译说明
#### template：针对不同数据库写的模版处理文件
#### view：视图处理
#### WEB-INF/lib：第三方支持的jar

#### common.js 在整个项目中的公用js处理
#### common.css 在整个项目中公用的css处理
#### index.html 功能的入口
#### jquery-1.11.0.min.js jquery类库
#### template-web-4.13.2.js 模版渲染引擎

#### 需要用到的第三方支持：fastjson，oracle-jdbc，mysql-jdbc，sqlserver-jdbc,postgresql-jdbc

#### 作者使用jdk7编写本项目

# 二次开发：
#### 添加新的语言支持：
1. 在common.js里i18ns数组里加入浏览器对语言的支持的简码
2. 在common.js里lang_arr数组加上支持的语言选项
3. 在i18n文件夹下追加语言包js

#### 添加新的数据库支持：
1. 在common.js里support_database数组加上要支持的数据库
2. 在db,service包下追加支持数据库的处理java文件
3. 在servlet/FunctionServlet.java加上针对要支持数据库的if处理
4. 在common.js里,table_html(),generation_html(),里加上特别处理要跳转的模版路径
5. 在新的模版下template里要加上自定义处理的代码
