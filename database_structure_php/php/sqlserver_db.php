<?php

/**
 * 获得sqlserver-pdo的连接字符串
 */
function get_sqlserver_dsn(){
    //数据库类型
    $dbms='sqlsrv';
    //数据库主机名
    $host=trim($_POST['host']);
    //使用端口
    $port=trim($_POST['port']);
    //使用的数据库
    $dbname=trim($_POST['dbname']);
    $dsn="$dbms:Server=$host,$port;Database=$dbname";
    return $dsn;
}

/**
 * 获取sqlserver-数据库连接
 */
function get_sqlserver_connect(){
    //初始化一个PDO对象
    $dsn=get_sqlserver_dsn();
    //数据库连接用户名
    $user=trim($_POST['user']);
    //对应的密码
    $pass=trim($_POST['pass']);
    //设置编码格式
    $connect = new PDO($dsn,$user,$pass);
    return $connect;
}

/**
 * 测试sqlserver数据库连接
 * @return true:连接成功，false:连接失败
 */
function test_sqlserver_connect(){
    //获取连接
    $connect=get_sqlserver_connect();
    //测试连接
    $select = $connect->query('select count(1)');
    //获取返回值
    $meta = $select->getColumnMeta(0);
    return $meta>0;
}

/**
 * 获得sqlserver数据库的所有表
 */
function sqlserver_tables(){
    //获取连接
    $connect=get_sqlserver_connect();
    //设置模式
    $connect->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
    //查询sql
    $sql="SELECT a.name AS table_name,CONVERT(NVARCHAR(100),isnull(g.[value],'-')) AS table_comment
FROM sys.tables a LEFT JOIN sys.extended_properties g ON (a.object_id = g.major_id AND g.minor_id = 0)";
    //预处理sql
    $select=$connect->prepare($sql);
    //查询变量
    $select->execute();
    //返回数据样式
    $data = $select->fetchAll(PDO::FETCH_OBJ);
    return $data;
}

/**
 * 获得sqlserver数据库的所有表结构
 */
function sqlserver_generation(){
    //获取连接
    $connect=get_sqlserver_connect();
    //设置模式
    $connect->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
    //查询sql
    $sql_tb="SELECT a.name AS table_name,CONVERT(NVARCHAR(100),isnull(g.[value],'-')) AS table_comment
    FROM sys.tables a 
    LEFT JOIN sys.extended_properties g ON (a.object_id = g.major_id AND g.minor_id = 0) 
    WHERE a.name IN (".$_POST["tb"].")";
    //预处理sql
    $select=$connect->prepare($sql_tb);
    //执行查询
    $select->execute();
    //返回所有表名有关的数据
    $tabls=$select->fetchAll(PDO::FETCH_OBJ);
    //循环处理所有表名，放入$tables总返回数据
    for ($i=0; $i<count($tabls); $i++) {
        //表的描述
        $sql_col="SELECT
        a.name AS column_name,
        COLUMNPROPERTY(a.id,a.name,'IsIdentity') AS 'extra',
        case when exists(
        SELECT xtype FROM sysobjects WHERE xtype='PK' AND name IN(
        SELECT name FROM sysindexes WHERE indid IN(
        SELECT indid FROM sysindexkeys WHERE id = a.id AND colid=a.colid
        ))) then 'true' else '' end AS 'column_key',
        b.name AS column_type,
        a.isnullable AS is_nullable,
        isnull(e.text,'') AS column_default,
        isnull(g.[value],'') AS column_comment
        FROM syscolumns a
        LEFT JOIN systypes b ON a.xusertype=b.xusertype
        INNER JOIN sysobjects d ON a.id=d.id
        LEFT JOIN syscomments e ON a.cdefault=e.id
        LEFT JOIN sys.extended_properties g ON a.id=g.major_id AND a.colid=g.minor_id
        LEFT JOIN sys.extended_properties f ON d.id=f.major_id AND f.minor_id=0
        WHERE d.name=?
        ORDER BY a.id,a.colorder";
        //预处理sql
        $select=$connect->prepare($sql_col);
        //执行查询
        $select->execute(array($tabls[$i]->table_name));
        $tabls[$i]->colums=$select->fetchAll(PDO::FETCH_OBJ);
    }
    return $tabls;
}

/**
 * 另存为html文件
 */
function sqlserver_save_html(){
    //表结构数据
    $tabls=sqlserver_generation();
    //数据序列化
    $tableData=json_encode($tabls);
    //文件名
    $dir=dirname(__FILE__);
    $fpath=$dir.'/../template/sqlserver/sqlserver_save.html';
    //文件内容
    $handle = fopen($fpath, "r");
    //通过filesize获得文件大小，将整个文件读到一个字符串中
    $file_content = fread($handle, filesize ($fpath));
    fclose($handle);
    //替换内容
    $file_content=str_replace('#tableDataJson#',$tableData,$file_content);
    return $file_content;
}
?>