database_structure_java
│
├─src
│  ├─db
│  │      JdbcDb.java
│  │      MySqlDb.java
│  │      OracleDb.java
│  │      PostgreSqlDb.java
│  │      SqlserverDb.java
│  │      
│  ├─service
│  │      MySqlService.java
│  │      OracleService.java
│  │      PostgresqlService.java
│  │      SqlserverService.java
│  │      
│  ├─servlet
│  │      FunctionServlet.java
│  │      
│  ├─util
│  │      FileUtil.java
│  │      
│  └─vo
│          ColumnVo.java
│          JsonVo.java
│          PostParamVo.java
│          TableVo.java
│          
└─WebContent
    │  common.css
    │  common.js
    │  index.html
    │  jquery-1.11.0.min.js
    │  README.md
    │  template-web-4.13.2.js
    │  
    ├─i18n
    │      en-us.js
    │      zh-cn.js
    │      
    ├─md
    │      README_EN_US.md
    │      README_ZH_CN.md
    │      
    ├─META-INF
    │      MANIFEST.MF
    │      
    ├─template
    │  ├─mysql
    │  │      mysql_generation.html
    │  │      mysql_save.html
    │  │      mysql_tables.html
    │  │      
    │  ├─oracle
    │  │      oracle_generation.html
    │  │      oracle_save.html
    │  │      oracle_tables.html
    │  │      
    │  ├─postgresql
    │  │      postgresql_generation.html
    │  │      postgresql_save.html
    │  │      postgresql_tables.html
    │  │      
    │  └─sqlserver
    │          sqlserver_generation.html
    │          sqlserver_save.html
    │          sqlserver_tables.html
    │          
    ├─view
    │      connect.html
    │      language.html
    │      
    └─WEB-INF
        │  web.xml
        │  
        └─lib
                fastjson-1.2.58.jar
                mysql-connector-java-5.1.47.jar
                ojdbc6.jar
                postgresql-42.2.6.jre7.jar
                sqljdbc4.jar

Read the WebContent/md/README_EN_US.md in English,using Google Translate from README_ZH_CN.md
中文请阅读WebContent/md/README_ZH_CN.md
